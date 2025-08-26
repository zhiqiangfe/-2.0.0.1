namespace SUNWODA_SEVB.Core.Models.Data
{
    /// <summary>
    /// MES接口日志表
    /// </summary>

    public partial class MesInterfaceLogModel
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

        /// <summary>
        /// API类型
        /// </summary>
        public string? ApiType { get; set; }

        /// <summary>
        /// 端点URL
        /// </summary>
        public string? Endpoint { get; set; }

        /// <summary>
        /// HTTP状态码
        /// </summary>
        public int? HttpStatusCode { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// 操作员工号
        /// </summary>
        public string? OperatorId { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string? DeviceNumber { get; set; }

        public MesInterfaceLogModel() { }
        public MesInterfaceLogModel(string method, string inputJson = null!)
        {
            Method = method;
            InputJson = inputJson;
            StartTime = DateTime.Now;
        }
        /// <summary>
        /// 扩展的构造函数，支持新增字段
        /// </summary>
        public MesInterfaceLogModel(string method, string inputJson = null!, string? apiType = null,
            string? endpoint = null, string? operatorId = null, string? deviceNumber = null)
        {
            Method = method;
            InputJson = inputJson;
            StartTime = DateTime.Now;
            ApiType = apiType;
            Endpoint = endpoint;
            OperatorId = operatorId;
            DeviceNumber = deviceNumber;
        }

        public void MESLog(bool success, string outputJson = null!)
        {
            SuccessFlag = success;
            OutputJson = outputJson;
            EndTime = DateTime.Now;
            LogDate = EndTime; // 将日志日期设置为结束时间
            ConsumingTime = (long)(EndTime - StartTime).TotalMilliseconds; // 自动计算耗时
        }

        /// <summary>
        /// 扩展的MESLog方法，支持设置HTTP状态码和错误代码
        /// </summary>
        public void MESLog(bool success, string outputJson = null!, int? httpStatusCode = null, string? errorCode = null)
        {
            SuccessFlag = success;
            OutputJson = outputJson;
            EndTime = DateTime.Now;
            LogDate = EndTime;
            ConsumingTime = (long)(EndTime - StartTime).TotalMilliseconds;
            HttpStatusCode = httpStatusCode;
            ErrorCode = errorCode;
        }

    }

}
