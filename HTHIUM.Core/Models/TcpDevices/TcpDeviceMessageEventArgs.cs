namespace HTHIUM.Core.Models.TcpDevices
{
    /// <summary>
    /// TCP 设备收发消息事件参数。
    /// TcpDeviceService 会把单设备客户端收到/发送的文本统一包装成该对象再通知业务层。
    /// </summary>
    public class TcpDeviceMessageEventArgs : EventArgs
    {
        /// <summary>设备 ID，对应 tcp_device_config.id。</summary>
        public int DeviceId { get; }

        /// <summary>设备名称，便于业务层按设备区分消息来源。</summary>
        public string DeviceName { get; }

        /// <summary>消息方向：RX 表示仪器发给软件；TX 表示软件发给仪器。</summary>
        public string Direction { get; }

        /// <summary>消息正文。</summary>
        public string Message { get; }

        /// <summary>事件产生时间。</summary>
        public DateTime Time { get; }

        /// <summary>
        /// 创建一条 TCP 消息事件。
        /// </summary>
        public TcpDeviceMessageEventArgs(int deviceId, string deviceName, string direction, string message)
        {
            DeviceId = deviceId;
            DeviceName = deviceName;
            Direction = direction;
            Message = message;
            Time = DateTime.Now;
        }
    }
}