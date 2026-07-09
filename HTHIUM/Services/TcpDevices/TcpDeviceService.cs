using Mapster;
using SqlSugar;
using HTHIUM.Core.Interfaces;
using HTHIUM.Core.Interfaces.TcpDevices;
using HTHIUM.Core.Models.TcpDevices;
using HTHIUM.Data.Models;

namespace HTHIUM.Services.TcpDevices
{
    /// <summary>
    /// TCP 设备管理服务。
    /// 负责从数据库加载设备配置、创建单设备客户端、统一转发收发消息和维护自动连接。
    /// </summary>
    public class TcpDeviceService : ITcpDeviceService, IAsyncDisposable
    {
        private readonly ISqlSugarClient _db;
        private readonly ILoggerService<TcpDeviceService> _logger;

        // Key 为设备 ID，Value 为具体 TCP 客户端；一个设备对应一个 TcpDeviceClient。
        private readonly Dictionary<int, TcpDeviceClient> _clients = new();

        // Key 为设备 ID，Value 为运行时状态；测试页面和业务层通过该集合查看状态。
        private readonly Dictionary<int, TcpDeviceRuntimeModel> _devices = new();

        // 防止 Reload、Connect 等操作并发修改客户端集合。
        private readonly SemaphoreSlim _syncLock = new(1, 1);
        private CancellationTokenSource? _serviceCts;
        private bool _initialized;

        /// <summary>当前已加载的设备运行状态集合。</summary>
        public IReadOnlyDictionary<int, TcpDeviceRuntimeModel> Devices => _devices;

        /// <summary>统一消息事件。RX 表示收到设备消息，TX 表示软件发送消息。</summary>
        public event EventHandler<TcpDeviceMessageEventArgs>? MessageReceived;

        /// <summary>设备状态变化事件。</summary>
        public event EventHandler<TcpDeviceRuntimeModel>? StatusChanged;

        /// <summary>
        /// 注入数据库客户端和日志服务。
        /// </summary>
        public TcpDeviceService(ISqlSugarClient db, ILoggerService<TcpDeviceService> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// 初始化 TCP 模块。
        /// 先加载数据库配置，再启动自动连接和心跳维护循环。
        /// </summary>
        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            if (_initialized)
                return;

            await ReloadAsync(cancellationToken);
            _serviceCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // 后台循环会根据 IsAutoConnect 自动连接设备，并按配置发送心跳。
            _ = Task.Run(() => MaintainConnectionsAsync(_serviceCts.Token));
            _initialized = true;
        }

        /// <summary>
        /// 从 tcp_device_config 表重新加载启用设备。
        /// 重新加载时会释放旧客户端，避免旧 IP/端口继续占用连接。
        /// </summary>
        public async Task ReloadAsync(CancellationToken cancellationToken = default)
        {
            await _syncLock.WaitAsync(cancellationToken);
            try
            {
                var configs = await _db.Queryable<TcpDeviceConfig>()
                    .Where(x => x.IsEnable)
                    .OrderBy(x => x.ID)
                    .ToListAsync(cancellationToken);

                foreach (var client in _clients.Values)
                {
                    await client.DisposeAsync();
                }
                _clients.Clear();
                _devices.Clear();

                foreach (var config in configs)
                {
                    // Data 层实体转换为 Core 层配置模型，通讯层只使用业务模型。
                    var model = config.Adapt<TcpDeviceConfigModel>();
                    var runtime = new TcpDeviceRuntimeModel(model);
                    var client = new TcpDeviceClient(runtime);

                    // 单设备客户端收到文本后，服务层统一补充设备信息和 RX 方向再向外转发。
                    client.MessageReceived += (_, message) =>
                    {
                        MessageReceived?.Invoke(this, new TcpDeviceMessageEventArgs(runtime.ID, runtime.Name, "RX", message));
                    };

                    // 单设备状态变化后，服务层统一通知界面或业务层刷新状态。
                    client.StatusChanged += (_, _) => StatusChanged?.Invoke(this, runtime);

                    _devices[model.ID] = runtime;
                    _clients[model.ID] = client;
                }

                _logger.Info($"TCP device configs loaded, count: {_clients.Count}");
            }
            finally
            {
                _syncLock.Release();
            }
        }

        /// <summary>
        /// 手动连接指定设备，通常由测试页面 Connect 按钮或业务流程调用。
        /// </summary>
        public async Task<bool> ConnectAsync(int deviceId, CancellationToken cancellationToken = default)
        {
            return _clients.TryGetValue(deviceId, out var client)
                && await client.ConnectAsync(cancellationToken);
        }

        /// <summary>
        /// 手动断开指定设备。
        /// </summary>
        public async Task DisconnectAsync(int deviceId)
        {
            if (_clients.TryGetValue(deviceId, out var client))
            {
                await client.DisconnectAsync();
            }
        }

        /// <summary>
        /// 向指定设备发送文本。
        /// 如果设备尚未连接，会先尝试连接；发送成功后会产生 TX 消息事件用于日志展示。
        /// </summary>
        public async Task<bool> SendAsync(int deviceId, string message, CancellationToken cancellationToken = default)
        {
            if (!_clients.TryGetValue(deviceId, out var client))
                return false;

            if (!client.Runtime.IsConnected && !await client.ConnectAsync(cancellationToken))
                return false;

            var success = await client.SendAsync(message, cancellationToken);
            if (success)
            {
                MessageReceived?.Invoke(this, new TcpDeviceMessageEventArgs(client.Runtime.ID, client.Runtime.Name, "TX", message));
            }
            return success;
        }

        /// <summary>
        /// 后台维护循环。
        /// 对 IsAutoConnect=true 的设备进行自动连接；对配置了心跳的设备定时发送心跳命令。
        /// </summary>
        private async Task MaintainConnectionsAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var client in _clients.Values.ToArray())
                {
                    // 数据库没有开启自动连接时，只加载配置，不主动连接设备。
                    if (!client.Runtime.Config.IsAutoConnect)
                        continue;

                    if (!client.Runtime.IsConnected)
                    {
                        await client.ConnectAsync(cancellationToken);
                    }
                    else if (client.Runtime.Config.HeartbeatIntervalMs > 0
                        && !string.IsNullOrWhiteSpace(client.Runtime.Config.HeartbeatCommand))
                    {
                        var last = client.Runtime.LastSentTime ?? DateTime.MinValue;
                        if ((DateTime.Now - last).TotalMilliseconds >= client.Runtime.Config.HeartbeatIntervalMs)
                        {
                            await client.SendAsync(client.Runtime.Config.HeartbeatCommand!, cancellationToken);
                        }
                    }
                }

                // 使用所有设备中最小的重连间隔作为下一次扫描间隔，最低 500ms，避免空转。
                var delay = _devices.Values.Select(x => x.Config.ReconnectIntervalMs).DefaultIfEmpty(3000).Min();
                await Task.Delay(Math.Max(500, delay), cancellationToken);
            }
        }

        /// <summary>
        /// 释放 TCP 服务，停止后台循环并释放所有设备客户端。
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            try { _serviceCts?.Cancel(); } catch { }
            foreach (var client in _clients.Values)
            {
                await client.DisposeAsync();
            }
            _clients.Clear();
            _devices.Clear();
            _serviceCts?.Dispose();
            _syncLock.Dispose();
        }
    }
}