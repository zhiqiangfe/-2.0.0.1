using SqlSugar;

namespace SUNWODA_SEVB.Data.Models
{
    /// <summary>
    /// Web接口日志表
    /// </summary>
    //[SugarIndex("web_interface_logs_logdate", nameof(LogDate), OrderByType.Desc)]
    [SugarTable("web_interface_logs", "Web接口日志表")]
    public class WebInterfaceLog
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public int ID { get; set; }

        [SugarColumn(ColumnName = "log_date", ColumnDescription = "日志记录日期")]
        public DateTime LogDate { get; set; }

        [SugarColumn(ColumnName = "method", ColumnDescription = "调用方法名")]
        public string Method { get; set; } = null!;

        [SugarColumn(ColumnName = "input_json", ColumnDescription = "输入参数(JSON)", IsNullable = true, ColumnDataType = "TEXT")]
        public string InputJson { get; set; } = null!;

        [SugarColumn(ColumnName = "output_json", ColumnDescription = "输出结果(JSON)", IsNullable = true, ColumnDataType = "TEXT")]
        public string OutputJson { get; set; } = null!;

        [SugarColumn(ColumnName = "consuming_time", ColumnDescription = "总耗时(毫秒)")]
        public long ConsumingTime { get; set; }

        [SugarColumn(ColumnName = "success_flag", ColumnDescription = "是否成功")]
        public bool SuccessFlag { get; set; }

    }
}
