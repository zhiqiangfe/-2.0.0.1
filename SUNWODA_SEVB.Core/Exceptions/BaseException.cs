
namespace SUNWODA_SEVB.Core.Exceptions
{
    /// <summary>
    /// 基础异常类
    /// </summary>
    public class BaseException : Exception
    {
        public string? ErrorCode { get; set; }
        public DateTime OccurredAt { get; set; }

        public BaseException() : base()
        {
            OccurredAt = DateTime.Now;
        }

        public BaseException(string message) : base(message)
        {
            OccurredAt = DateTime.Now;
        }

        public BaseException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
            OccurredAt = DateTime.Now;
        }

        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
            OccurredAt = DateTime.Now;
        }
    }
}
