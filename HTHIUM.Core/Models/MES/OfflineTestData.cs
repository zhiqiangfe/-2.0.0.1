
namespace HTHIUM.Core.Models.MES
{
    /// <summary>
    /// 测试数据模型
    /// </summary>
    public class OfflineTestData
    {
        public string paramCode { get; set; } = string.Empty;
        public string paramName { get; set; } = string.Empty;
        public string paramValue { get; set; } = string.Empty;
        public string paramResult { get; set; } = string.Empty;
        public string paramUnit { get; set; } = string.Empty;
    }

    /// <summary>
    /// 环境数据模型
    /// </summary>
    public class EnvironmentData
    {
        /// <summary>
        /// 参数代码
        /// </summary>
        public string paramCode { get; set; }= string.Empty;
        /// <summary>
        /// 参数项名称
        /// </summary>
        public string paramName { get; set; } = string.Empty;
        /// <summary>
        /// 参数项值
        /// </summary>
        public string paramValue { get; set; } = string.Empty;
        /// <summary>
        /// 参数项结果
        /// </summary>
        public string paramResult { get; set; } = string.Empty;
    }
    public class StepData
    {
        public string StepName { get; set; } = string.Empty;
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
       
    }
}
