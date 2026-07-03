using SqlSugar;

namespace SUNWODA_SEVB.Data.Models
{
    [SugarTable("tcp_device_config", "TCP device configuration")]
    public class TcpDeviceConfig
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }

        [SugarColumn(ColumnName = "name", ColumnDescription = "Device name")]
        public string Name { get; set; } = null!;

        [SugarColumn(ColumnName = "device_type", ColumnDescription = "Device type")]
        public string DeviceType { get; set; } = "Custom";

        [SugarColumn(ColumnName = "ip", ColumnDescription = "IP address")]
        public string IP { get; set; } = "127.0.0.1";

        [SugarColumn(ColumnName = "port", ColumnDescription = "Port")]
        public int Port { get; set; }

        [SugarColumn(ColumnName = "encoding_name", ColumnDescription = "Text encoding")]
        public string EncodingName { get; set; } = "UTF-8";

        [SugarColumn(ColumnName = "new_line", ColumnDescription = "None/CR/LF/CRLF")]
        public string NewLine { get; set; } = "CRLF";

        [SugarColumn(ColumnName = "connect_timeout_ms", ColumnDescription = "Connect timeout ms")]
        public int ConnectTimeoutMs { get; set; } = 3000;

        [SugarColumn(ColumnName = "receive_timeout_ms", ColumnDescription = "Receive timeout ms")]
        public int ReceiveTimeoutMs { get; set; } = 5000;

        [SugarColumn(ColumnName = "reconnect_interval_ms", ColumnDescription = "Reconnect interval ms")]
        public int ReconnectIntervalMs { get; set; } = 3000;

        [SugarColumn(ColumnName = "heartbeat_interval_ms", ColumnDescription = "Heartbeat interval ms")]
        public int HeartbeatIntervalMs { get; set; }

        [SugarColumn(ColumnName = "heartbeat_command", ColumnDescription = "Heartbeat command", IsNullable = true)]
        public string? HeartbeatCommand { get; set; }

        [SugarColumn(ColumnName = "is_auto_connect", ColumnDescription = "Auto connect")]
        public bool IsAutoConnect { get; set; } = true;

        [SugarColumn(ColumnName = "is_enable", ColumnDescription = "Enabled")]
        public bool IsEnable { get; set; } = true;

        [SugarColumn(ColumnName = "remark", ColumnDescription = "Remark", IsNullable = true)]
        public string? Remark { get; set; }
    }
}
