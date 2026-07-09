using HTHIUM.Core.Common;
using HTHIUM.Core.Interfaces.MES;

namespace HTHIUM.Core.Models.MES
{
    /// <summary>
    /// MES请求基类
    /// </summary>
    public abstract class BaseRequest : ModelBase, IMesRequest
    {
        private string requestId;
        private DateTime timestamp;

        protected BaseRequest()
        {
            requestId = Guid.NewGuid().ToString("N");
            timestamp = DateTime.Now;
        }

        ///// <summary>
        ///// 请求ID
        ///// </summary>
        //public string RequestId
        //{
        //    get => requestId;
        //    set => SetProperty(ref requestId, value);
        //}

        ///// <summary>
        ///// 时间戳
        ///// </summary>
        //public DateTime Timestamp
        //{
        //    get => timestamp;
        //    set => SetProperty(ref timestamp, value);
        //}

        //public virtual string GetRequestId() => RequestId;
        //public virtual DateTime GetTimestamp() => Timestamp;

        /// <summary>
        /// 验证请求参数
        /// </summary>
        public abstract bool Validate(out string errorMessage);
    }
}
