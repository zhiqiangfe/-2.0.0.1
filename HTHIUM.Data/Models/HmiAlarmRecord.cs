using SqlSugar;

namespace HTHIUM.Data.Models
{
    [SugarTable("hmi_alarm_record", "HMI报警记录表")]
    [SugarIndex("idx_alarm_time", nameof(TriggerTime), OrderByType.Desc)]
    [SugarIndex("idx_alarm_code", nameof(AlarmCode), OrderByType.Asc)]
    [SugarIndex("idx_device_time", nameof(DeviceName), OrderByType.Asc, nameof(TriggerTime), OrderByType.Desc)]
    [SugarIndex("idx_station_time", nameof(StationName), OrderByType.Asc, nameof(TriggerTime), OrderByType.Desc)]
    public class HmiAlarmRecord
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public long ID { get; set; }

        [SugarColumn(ColumnName = "line_name", ColumnDescription = "产线名称", Length = 100, IsNullable = true)]
        public string? LineName { get; set; }

        [SugarColumn(ColumnName = "device_name", ColumnDescription = "设备名称", Length = 100)]
        public string DeviceName { get; set; } = null!;

        [SugarColumn(ColumnName = "station_name", ColumnDescription = "工位名称", Length = 100, IsNullable = true)]
        public string? StationName { get; set; }

        [SugarColumn(ColumnName = "process_name", ColumnDescription = "工序名称", Length = 100, IsNullable = true)]
        public string? ProcessName { get; set; }

        [SugarColumn(ColumnName = "alarm_code", ColumnDescription = "报警代码", Length = 100)]
        public string AlarmCode { get; set; } = null!;

        [SugarColumn(ColumnName = "alarm_name", ColumnDescription = "报警名称", Length = 200, IsNullable = true)]
        public string? AlarmName { get; set; }

        [SugarColumn(ColumnName = "alarm_level", ColumnDescription = "报警等级：高/中/低", Length = 20, IsNullable = true)]
        public string? AlarmLevel { get; set; }

        [SugarColumn(ColumnName = "trigger_time", ColumnDescription = "报警触发时间")]
        public DateTime TriggerTime { get; set; }

        [SugarColumn(ColumnName = "recover_time", ColumnDescription = "报警恢复时间", IsNullable = true)]
        public DateTime? RecoverTime { get; set; }

        [SugarColumn(ColumnName = "duration_seconds", ColumnDescription = "持续时长，单位秒", IsNullable = true)]
        public int? DurationSeconds { get; set; }

        [SugarColumn(ColumnName = "alarm_status", ColumnDescription = "报警状态", Length = 20)]
        public string AlarmStatus { get; set; } = "触发中";

        [SugarColumn(ColumnName = "source", ColumnDescription = "报警来源", Length = 50, IsNullable = true)]
        public string? Source { get; set; } = "HMI";

        [SugarColumn(ColumnName = "raw_value", ColumnDescription = "原始报警值或原始报文", Length = 200, IsNullable = true)]
        public string? RawValue { get; set; }

        [SugarColumn(ColumnName = "impact_qty", ColumnDescription = "影响产量", IsNullable = true)]
        public int? ImpactQty { get; set; }

        [SugarColumn(ColumnName = "response_seconds", ColumnDescription = "首次响应耗时，单位秒", IsNullable = true)]
        public int? ResponseSeconds { get; set; }

        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", Length = 500, IsNullable = true)]
        public string? Remark { get; set; }

        [SugarColumn(ColumnName = "created_time", ColumnDescription = "创建时间")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "updated_time", ColumnDescription = "更新时间", IsNullable = true)]
        public DateTime? UpdatedTime { get; set; }
    }
}
