namespace SUNWODA_SEVB.Core.Models.TcpDevices
{
    /// <summary>
    /// TCP 设备配置业务模型。
    /// 该模型由数据库表 tcp_device_config 映射而来，供通讯服务和业务层使用。
    /// </summary>
    public class TcpDeviceConfigModel
    {
        /// <summary>数据库主键，也是运行时设备 ID。</summary>
        public int ID { get; set; }

        /// <summary>设备名称，用于界面显示和业务区分，例如 Scanner01、Camera01。</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>设备类型，用于业务分流，例如 Scanner、Camera、Robot。</summary>
        public string DeviceType { get; set; } = "Scanner";

        /// <summary>设备 IP 地址。</summary>
        public string IP { get; set; } = "127.0.0.1";

        /// <summary>设备 TCP 端口。</summary>
        public int Port { get; set; }

        /// <summary>收发文本使用的编码名称，例如 UTF-8、GBK。</summary>
        public string EncodingName { get; set; } = "UTF-8";

        /// <summary>发送命令时追加的换行符类型：None、CR、LF、CRLF。</summary>
        public string NewLine { get; set; } = "CRLF";

        /// <summary>连接超时时间，单位毫秒。</summary>
        public int ConnectTimeoutMs { get; set; } = 3000;

        /// <summary>接收超时时间，单位毫秒。</summary>
        public int ReceiveTimeoutMs { get; set; } = 5000;

        /// <summary>自动重连检查间隔，单位毫秒。</summary>
        public int ReconnectIntervalMs { get; set; } = 3000;

        /// <summary>心跳发送间隔，0 表示不启用心跳。</summary>
        public int HeartbeatIntervalMs { get; set; } = 0;

        /// <summary>心跳命令内容，只有 HeartbeatIntervalMs 大于 0 时才会发送。</summary>
        public string? HeartbeatCommand { get; set; }

        /// <summary>是否在后台服务启动后自动连接设备。</summary>
        public bool IsAutoConnect { get; set; } = true;

        /// <summary>是否启用该设备，未启用的设备不会加载到 TCP 服务。</summary>
        public bool IsEnable { get; set; } = true;

        /// <summary>备注信息。</summary>
        public string? Remark { get; set; }
    }
}