using HTHIUM.Core.Common;
using HTHIUM.Core.Interfaces.MES;

namespace HTHIUM.Core.Models.MES
{
    /// <summary>
    /// MES响应基类
    /// </summary>
    public class BaseResponse : ModelBase, IMesResponse
    {
        private bool success;
        private string code = string.Empty;
        private string message = string.Empty;

        public bool Success
        {
            get => success;
            set => SetProperty(ref success, value);
        }

        public string Code
        {
            get => code;
            set => SetProperty(ref code, value ?? string.Empty);
        }

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value ?? string.Empty);
        }
    }

    /// <summary>
    /// MES泛型响应基类
    /// </summary>
    public class BaseResponse<T> : BaseResponse, IMesResponse<T>
    {
        private T? data;

        public T Data
        {
            get => data!;
            set => SetProperty(ref data, value);
        }
    }
}
