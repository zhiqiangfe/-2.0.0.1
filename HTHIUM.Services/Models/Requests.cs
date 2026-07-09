using HTHIUM.Core.Models.MES;

namespace HTHIUM.MES.Models
{
    /// <summary>
    /// 离线数据上传请求
    /// </summary>
    public class OfflineDataUploadRequest : BaseRequest
    {
        public string operatorId { get; set; } = string.Empty;
        public string productSn { get; set; } = string.Empty;
        public string groupCode { get; set; } = string.Empty;
        public string deviceSn { get; set; } = string.Empty;
        public string moNumber { get; set; } = string.Empty;
        public string timeStamp { get; set; } = string.Empty;
        public string testResult { get; set; } = string.Empty;
        public List<OfflineTestData> testData { get; set; } = new();
        public List<EnvironmentData>? environment { get; set; }
        public List<StepData>? stepData { get; set; }

        /// <summary>
        /// 验证请求数据
        /// </summary>
        public override bool Validate(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(productSn))
            {
                errorMessage = "ProductSn is required";
                return false;
            }

            if (string.IsNullOrWhiteSpace(testResult))
            {
                errorMessage = "TestResult is required";
                return false;
            }

            // 验证TestResult值
            if (!testResult.Equals("0", StringComparison.OrdinalIgnoreCase) &&
                !testResult.Equals("1", StringComparison.OrdinalIgnoreCase))
            {
                errorMessage = "TestResult must be '0' or '1'";
                return false;
            }

            if (testData == null || testData.Count == 0)
            {
                errorMessage = "TestData is required and must not be empty";
                return false;
            }

            // 验证时间戳格式
            if (string.IsNullOrWhiteSpace(timeStamp))
            {
                errorMessage = "TimeStamp is required";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }


    /// <summary>
    /// 离线数据上传响应
    /// </summary>
    public class OfflineDataUploadResponse : BaseResponse
    {
        public string Status { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string TraceId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Marking数据上传请求
    /// </summary>
    public class IncreaseMarkingRequest : BaseRequest
    {
        public string DeviceSn { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string GroupCode { get; set; } = string.Empty;
        public string ControlGroup { get; set; } = string.Empty;
        public string TimeStamp { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;

        public override bool Validate(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(SerialNumber))
            {
                errorMessage = "序列号不能为空";
                return false;
            }

            if (string.IsNullOrWhiteSpace(DeviceSn))
            {
                errorMessage = "设备序列号不能为空";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Marking数据上传响应
    /// </summary>
    public class IncreaseMarkingResponse : BaseResponse
    {
        public string Status { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string TraceId { get; set; } = string.Empty;
    }

}