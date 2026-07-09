using HTHIUM.Core.Models.TcpDevices;
using System.Net.Sockets;
using System.Text;

namespace HTHIUM.Services.TcpDevices
{
    /// <summary>
    /// 单台 TCP 设备客户端。
    /// 该类只负责一台设备的连接、断开、发送、接收和运行状态更新。
    /// </summary>
    internal sealed class TcpDeviceClient : IAsyncDisposable
    {
        private readonly TcpDeviceRuntimeModel _runtime;
        private readonly SemaphoreSlim _sendLock = new(1, 1);
        private TcpClient? _client;
        private NetworkStream? _stream;
        private CancellationTokenSource? _readCts;
        private bool _disposed;

        /// <summary>当前设备运行状态。</summary>
        public TcpDeviceRuntimeModel Runtime => _runtime;

        /// <summary>收到设备发来的文本时触发。这里不包含设备信息，设备信息由 TcpDeviceService 统一补充。</summary>
        public event EventHandler<string>? MessageReceived;

        /// <summary>连接状态、收发统计或错误变化时触发。</summary>
        public event EventHandler? StatusChanged;

        /// <summary>
        /// 创建单设备客户端。
        /// </summary>
        public TcpDeviceClient(TcpDeviceRuntimeModel runtime)
        {
            _runtime = runtime;
        }

        /// <summary>
        /// 建立 TCP 连接。连接成功后会启动独立读取循环，持续监听设备发来的数据。
        /// </summary>
        public async Task<bool> ConnectAsync(CancellationToken cancellationToken)
        {
            if (_disposed)
                return false;

            if (_runtime.IsConnected)
                return true;

            await CloseCurrentAsync();
            try
            {
                SetStatus(false, "Connecting", null);

                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(Math.Max(500, _runtime.Config.ConnectTimeoutMs));

                var client = new TcpClient();
                await client.ConnectAsync(_runtime.Config.IP, _runtime.Config.Port, timeoutCts.Token);

                _client = client;
                _stream = client.GetStream();
                _readCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

                _runtime.LastConnectedTime = DateTime.Now;
                SetStatus(true, "Connected", null);

                // 连接成功后开始监听设备数据；读取循环异常退出时会更新状态。
                _ = Task.Run(() => ReadLoopAsync(_readCts.Token));
                return true;
            }
            catch (Exception ex)
            {
                _runtime.ErrorCount++;
                SetStatus(false, "Connect failed", ex.Message);
                await CloseCurrentAsync();
                return false;
            }
        }

        /// <summary>
        /// 主动断开当前设备连接。
        /// </summary>
        public async Task DisconnectAsync()
        {
            await CloseCurrentAsync();
            SetStatus(false, "Disconnected", null);
        }

        /// <summary>
        /// 向设备发送文本命令。
        /// 发送时会按配置追加换行符，例如扫码枪/机器人常用 CRLF 结尾。
        /// </summary>
        public async Task<bool> SendAsync(string message, CancellationToken cancellationToken)
        {
            if (_stream == null || !_runtime.IsConnected)
                return false;

            await _sendLock.WaitAsync(cancellationToken);
            try
            {
                var encoding = GetEncoding(_runtime.Config.EncodingName);
                var payload = message + BuildNewLine(_runtime.Config.NewLine);
                var bytes = encoding.GetBytes(payload);

                await _stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
                await _stream.FlushAsync(cancellationToken);

                _runtime.LastSent = message;
                _runtime.LastSentTime = DateTime.Now;
                _runtime.SentCount++;
                StatusChanged?.Invoke(this, EventArgs.Empty);
                return true;
            }
            catch (Exception ex)
            {
                _runtime.ErrorCount++;
                SetStatus(false, "Send failed", ex.Message);
                await CloseCurrentAsync();
                return false;
            }
            finally
            {
                _sendLock.Release();
            }
        }

        /// <summary>
        /// 接收循环。只要连接存在，就持续从 NetworkStream 读取设备发送的数据。
        /// </summary>
        private async Task ReadLoopAsync(CancellationToken cancellationToken)
        {
            var buffer = new byte[4096];
            var encoding = GetEncoding(_runtime.Config.EncodingName);

            try
            {
                while (!cancellationToken.IsCancellationRequested && _stream != null)
                {
                    var count = await _stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    if (count <= 0)
                        break;

                    var text = encoding.GetString(buffer, 0, count).TrimEnd('\r', '\n');
                    _runtime.LastReceived = text;
                    _runtime.LastReceivedTime = DateTime.Now;
                    _runtime.ReceivedCount++;

                    MessageReceived?.Invoke(this, text);
                    StatusChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (OperationCanceledException)
            {
                // 主动断开或程序退出时会取消读取循环，这属于正常停止。
            }
            catch (Exception ex)
            {
                _runtime.ErrorCount++;
                SetStatus(false, "Receive failed", ex.Message);
            }
            finally
            {
                if (!_disposed)
                {
                    await CloseCurrentAsync();
                    SetStatus(false, "Disconnected", _runtime.LastError);
                }
            }
        }

        /// <summary>
        /// 更新运行状态并触发状态变化事件。
        /// </summary>
        private void SetStatus(bool connected, string status, string? error)
        {
            _runtime.IsConnected = connected;
            _runtime.StatusText = status;
            _runtime.LastError = error;
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 关闭当前连接并释放网络资源。
        /// </summary>
        private async Task CloseCurrentAsync()
        {
            try { _readCts?.Cancel(); } catch { }
            try { _stream?.Close(); } catch { }
            try { _client?.Close(); } catch { }

            _stream = null;
            _client = null;
            _readCts?.Dispose();
            _readCts = null;

            await Task.CompletedTask;
        }

        /// <summary>
        /// 根据配置获取文本编码。配置错误时回退到 UTF-8，避免因为编码名错误导致服务无法启动。
        /// </summary>
        private static Encoding GetEncoding(string? encodingName)
        {
            if (string.IsNullOrWhiteSpace(encodingName))
                return Encoding.UTF8;

            try
            {
                return Encoding.GetEncoding(encodingName);
            }
            catch
            {
                return Encoding.UTF8;
            }
        }

        /// <summary>
        /// 根据数据库配置生成发送命令结尾符。
        /// </summary>
        private static string BuildNewLine(string? newLine) => newLine?.ToUpperInvariant() switch
        {
            "CR" => "\r",
            "LF" => "\n",
            "CRLF" => "\r\n",
            _ => string.Empty
        };

        /// <summary>
        /// 释放单设备客户端。
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            _disposed = true;
            await CloseCurrentAsync();
            _sendLock.Dispose();
        }
    }
}