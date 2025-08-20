using SUNWODA_SEVB.Core.Enumerations.MES;

namespace SUNWODA_SEVB.Core.Exceptions
{
    /// <summary>
    /// MES API异常
    /// </summary>
    public class MesApiException : BaseException
    {
        public string? Endpoint { get; set; }
        public object? RequestData { get; set; }
        public object? ResponseData { get; set; }
        public MesApiType ApiType { get; set; }
        public int? HttpStatusCode { get; set; }

        public MesApiException(string message) : base(message)
        {
        }

        public MesApiException(string message, string errorCode)
            : base(message, errorCode)
        {
        }

        public MesApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public MesApiException(string message, MesApiType apiType, string endpoint)
            : base(message)
        {
            ApiType = apiType;
            Endpoint = endpoint;
        }
    }
}
