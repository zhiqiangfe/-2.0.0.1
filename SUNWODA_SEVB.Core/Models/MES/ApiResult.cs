using SUNWODA_SEVB.Core.Common;

namespace SUNWODA_SEVB.Core.Models.MES
{
    /// <summary>
    /// API统一返回结果
    /// </summary>
    public class ApiResult<T> : ModelBase
    {
        private bool isSuccess;
        private T? result;
        private string? errorMessage;
        private string? errorCode;
        private DateTime responseTime;

        public bool IsSuccess
        {
            get => isSuccess;
            set => SetProperty(ref isSuccess, value);
        }

        public T? Result
        {
            get => result;
            set => SetProperty(ref result, value);
        }

        public string? ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        public string? ErrorCode
        {
            get => errorCode;
            set => SetProperty(ref errorCode, value);
        }

        public DateTime ResponseTime
        {
            get => responseTime;
            set => SetProperty(ref responseTime, value);
        }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static ApiResult<T> Ok(T result)
        {
            return new ApiResult<T>
            {
                IsSuccess = true,
                Result = result,
                ResponseTime = DateTime.Now
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static ApiResult<T> Fail(string errorMessage, string? errorCode = null)
        {
            return new ApiResult<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode,
                ResponseTime = DateTime.Now
            };
        }
    }
}
