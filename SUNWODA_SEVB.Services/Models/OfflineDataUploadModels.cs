using SUNWODA_SEVB.Core.Models.MES;

namespace SUNWODA_SEVB.MES.Models
{
    /// <summary>
    /// 离线数据上传请求
    /// </summary>
    public class OfflineDataUploadRequest : BaseRequest
    {
        public string OperatorId { get; set; } = string.Empty;
        public string ProductSn { get; set; } = string.Empty;
        public string GroupCode { get; set; } = string.Empty;
        public string DeviceSn { get; set; } = string.Empty;
        public string MoNumber { get; set; } = string.Empty;
        public string TimeStamp { get; set; } = string.Empty;
        public string TestResult { get; set; } = string.Empty;
        public List<TestData> TestData { get; set; } = new();
        public List<EnvironmentData>? Environment { get; set; }
        public object? StepData { get; set; }

        public override bool Validate(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(ProductSn))
            {
                errorMessage = "ProductSn is required";
                return false;
            }

            if (string.IsNullOrWhiteSpace(TestResult))
            {
                errorMessage = "TestResult is required";
                return false;
            }

            if (TestData == null || TestData.Count == 0)
            {
                errorMessage = "TestData is required and must not be empty";
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
    }
}