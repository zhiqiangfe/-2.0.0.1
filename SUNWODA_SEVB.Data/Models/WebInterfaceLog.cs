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

        [SugarColumn(ColumnName = "input_json", ColumnDescription = "输入参数(JSON)", IsNullable = true)]
        public string InputJson { get; set; } = null!;

        [SugarColumn(ColumnName = "output_json", ColumnDescription = "输出结果(JSON)", IsNullable = true)]
        public string OutputJson { get; set; } = null!;

        [SugarColumn(ColumnName = "start_time", ColumnDescription = "开始时间")]
        public DateTime StartTime { get; set; }

        [SugarColumn(ColumnName = "end_time", ColumnDescription = "结束时间")]
        public DateTime EndTime { get; set; }

        [SugarColumn(ColumnName = "consuming_time", ColumnDescription = "总耗时(毫秒)")]
        public long ConsumingTime { get; set; }

        [SugarColumn(ColumnName = "success_flag", ColumnDescription = "是否成功")]
        public bool SuccessFlag { get; set; }

     
        //public WebInterfaceLog() { }

        //public WebInterfaceLog(string method, string inputJson = null!)
        //{
        //    Method = method;
        //    InputJson = inputJson;
        //    StartTime = DateTime.Now;
        //}
        //public void WebLog(bool success, string outputJson = null!)
        //{
        //    SuccessFlag = success;
        //    OutputJson = outputJson;
        //    EndTime = DateTime.Now;
        //    LogDate = EndTime; // 将日志日期设置为结束时间
        //    ConsumingTime = (long)(EndTime - StartTime).TotalMilliseconds; // 自动计算耗时
        //}
    }
}
