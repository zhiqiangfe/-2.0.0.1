using HTHIUM.Core.Common;

namespace HTHIUM.Core.Models.TcpDevices
{
    /// <summary>
    /// TCP 设备运行时状态模型。
    /// 配置数据来自数据库，连接状态、收发统计和错误信息由 TcpDeviceClient 在运行时更新。
    /// </summary>
    public class TcpDeviceRuntimeModel : ModelBase
    {
        private bool _isConnected;
        private string _statusText = "Disconnected";
        private string? _lastSent;
        private string? _lastReceived;
        private string? _lastError;
        private DateTime? _lastSentTime;
        private DateTime? _lastReceivedTime;
        private DateTime? _lastConnectedTime;
        private long _sentCount;
        private long _receivedCount;
        private long _errorCount;

        /// <summary>设备静态配置，来自 tcp_device_config 表。</summary>
        public TcpDeviceConfigModel Config { get; }

        /// <summary>设备 ID。</summary>
        public int ID => Config.ID;

        /// <summary>设备名称。</summary>
        public string Name => Config.Name;

        /// <summary>设备类型，例如 Scanner、Camera、Robot。</summary>
        public string DeviceType => Config.DeviceType;

        /// <summary>设备端点，格式为 IP:Port。</summary>
        public string Endpoint => $"{Config.IP}:{Config.Port}";

        /// <summary>当前是否已经建立 TCP 连接。</summary>
        public bool IsConnected { get => _isConnected; set => SetProperty(ref _isConnected, value); }

        /// <summary>当前状态文本，用于测试页面显示。</summary>
        public string StatusText { get => _statusText; set => SetProperty(ref _statusText, value); }

        /// <summary>最后一次发送给设备的文本。</summary>
        public string? LastSent { get => _lastSent; set => SetProperty(ref _lastSent, value); }

        /// <summary>最后一次从设备收到的文本。</summary>
        public string? LastReceived { get => _lastReceived; set => SetProperty(ref _lastReceived, value); }

        /// <summary>最后一次通讯错误信息。</summary>
        public string? LastError { get => _lastError; set => SetProperty(ref _lastError, value); }

        /// <summary>最后一次发送时间。</summary>
        public DateTime? LastSentTime { get => _lastSentTime; set => SetProperty(ref _lastSentTime, value); }

        /// <summary>最后一次接收时间。</summary>
        public DateTime? LastReceivedTime { get => _lastReceivedTime; set => SetProperty(ref _lastReceivedTime, value); }

        /// <summary>最后一次连接成功时间。</summary>
        public DateTime? LastConnectedTime { get => _lastConnectedTime; set => SetProperty(ref _lastConnectedTime, value); }

        /// <summary>累计发送次数。</summary>
        public long SentCount { get => _sentCount; set => SetProperty(ref _sentCount, value); }

        /// <summary>累计接收次数。</summary>
        public long ReceivedCount { get => _receivedCount; set => SetProperty(ref _receivedCount, value); }

        /// <summary>累计错误次数。</summary>
        public long ErrorCount { get => _errorCount; set => SetProperty(ref _errorCount, value); }

        /// <summary>
        /// 创建运行时对象。一个 TcpDeviceRuntimeModel 对应一条数据库设备配置。
        /// </summary>
        public TcpDeviceRuntimeModel(TcpDeviceConfigModel config)
        {
            Config = config;
        }
    }
}