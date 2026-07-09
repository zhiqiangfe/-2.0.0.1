namespace HTHIUM.Core.Models.Data
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

        public long ConsumingTime { get; set; }

        public bool SuccessFlag { get; set; }
        public WebInterfaceLogModel() { }

    }
}
