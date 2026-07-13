using SqlSugar;

namespace HTHIUM.Data.Models
{
    [SugarTable("hmi_alarm_code_map", "HMI报警代码关联表")]
    [SugarIndex("idx_alarm_code", nameof(AlarmCode), OrderByType.Asc)]
    [SugarIndex("idx_alarm_name", nameof(AlarmName), OrderByType.Asc)]
    public class HmiAlarmCodeMap
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public long ID { get; set; }

        [SugarColumn(ColumnName = "alarm_code", ColumnDescription = "报警代码", Length = 100)]
        public string AlarmCode { get; set; } = null!;

        [SugarColumn(ColumnName = "alarm_name", ColumnDescription = "报警名称", Length = 200)]
        public string AlarmName { get; set; } = null!;

        [SugarColumn(ColumnName = "alarm_level", ColumnDescription = "报警等级：高/中/低", Length = 20, IsNullable = true)]
        public string? AlarmLevel { get; set; }

        [SugarColumn(ColumnName = "possible_reason", ColumnDescription = "报警可能原因", ColumnDataType = "text", IsNullable = true)]
        public string? PossibleReason { get; set; }

        [SugarColumn(ColumnName = "handle_suggestion", ColumnDescription = "处理建议", ColumnDataType = "text", IsNullable = true)]
        public string? HandleSuggestion { get; set; }

        [SugarColumn(ColumnName = "device_name", ColumnDescription = "适用设备名称", Length = 100, IsNullable = true)]
        public string? DeviceName { get; set; }

        [SugarColumn(ColumnName = "station_name", ColumnDescription = "适用工位名称", Length = 100, IsNullable = true)]
        public string? StationName { get; set; }

        [SugarColumn(ColumnName = "process_name", ColumnDescription = "适用工序名称", Length = 100, IsNullable = true)]
        public string? ProcessName { get; set; }

        [SugarColumn(ColumnName = "is_enable", ColumnDescription = "是否启用")]
        public bool IsEnable { get; set; } = true;

        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", Length = 500, IsNullable = true)]
        public string? Remark { get; set; }

        [SugarColumn(ColumnName = "created_time", ColumnDescription = "创建时间")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "updated_time", ColumnDescription = "更新时间", IsNullable = true)]
        public DateTime? UpdatedTime { get; set; }
    }
}
