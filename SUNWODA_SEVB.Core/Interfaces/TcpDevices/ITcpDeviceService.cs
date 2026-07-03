using SUNWODA_SEVB.Core.Models.TcpDevices;

namespace SUNWODA_SEVB.Core.Interfaces.TcpDevices
{
    /// <summary>
    /// TCP 设备通讯服务接口。
    /// 业务层、测试页面只依赖该接口，不直接依赖具体 Socket 客户端实现。
    /// </summary>
    public interface ITcpDeviceService
    {
        /// <summary>当前已加载的设备运行状态集合，Key 为 tcp_device_config.id。</summary>
        IReadOnlyDictionary<int, TcpDeviceRuntimeModel> Devices { get; }

        /// <summary>设备消息事件。Direction=RX 表示收到仪器数据，Direction=TX 表示软件发送数据。</summary>
        event EventHandler<TcpDeviceMessageEventArgs>? MessageReceived;

        /// <summary>设备连接状态或运行状态变化事件。</summary>
        event EventHandler<TcpDeviceRuntimeModel>? StatusChanged;

        /// <summary>初始化 TCP 服务：加载数据库设备配置，并启动自动连接/心跳维护循环。</summary>
        Task InitializeAsync(CancellationToken cancellationToken = default);

        /// <summary>重新从数据库加载启用的 TCP 设备配置。</summary>
        Task ReloadAsync(CancellationToken cancellationToken = default);

        /// <summary>手动连接指定设备。</summary>
        Task<bool> ConnectAsync(int deviceId, CancellationToken cancellationToken = default);

        /// <summary>手动断开指定设备。</summary>
        Task DisconnectAsync(int deviceId);

        /// <summary>向指定设备发送文本命令。</summary>
        Task<bool> SendAsync(int deviceId, string message, CancellationToken cancellationToken = default);
    }
}