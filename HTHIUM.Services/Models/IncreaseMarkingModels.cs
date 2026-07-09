using HTHIUM.Core.Models.MES;

namespace HTHIUM.MES.Models
{
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
                errorMessage = "SerialNumber is required";
                return false;
            }

            if (string.IsNullOrWhiteSpace(DeviceSn))
            {
                errorMessage = "DeviceSn is required";
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
    }
}
