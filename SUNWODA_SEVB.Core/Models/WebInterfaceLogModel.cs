

namespace SUNWODA_SEVB.Core.Entities
{
    /// <summary>
    /// Web接口日志表
    /// </summary>

    public class WebInterfaceLogModel
    {
        public int ID { get; set; }

        public DateTime LogDate { get; set; }

        public string Method { get; set; } = null!;

        public string InputJson { get; set; } = null!;

        public string OutputJson { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public long ConsumingTime { get; set; }

        public bool SuccessFlag { get; set; }
        public WebInterfaceLogModel() { }

        public WebInterfaceLogModel(string method, string inputJson = null!)
        {
            Method = method;
            InputJson = inputJson;
            StartTime = DateTime.Now;
        }
        public void WebLog(bool success, string outputJson = null!)
        {
            SuccessFlag = success;
            OutputJson = outputJson;
            EndTime = DateTime.Now;
            LogDate = EndTime; // 将日志日期设置为结束时间
            ConsumingTime = (long)(EndTime - StartTime).TotalMilliseconds; // 自动计算耗时
        }

    }
}
