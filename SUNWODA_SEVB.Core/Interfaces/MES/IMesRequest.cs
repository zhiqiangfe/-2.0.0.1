
namespace SUNWODA_SEVB.Core.Interfaces.MES
{
    /// <summary>
    /// MES请求接口
    /// </summary>
    public interface IMesRequest
    {
        ///// <summary>
        ///// 获取请求ID
        ///// </summary>
        //string GetRequestId();

        ///// <summary>
        ///// 获取时间戳
        ///// </summary>
        //DateTime GetTimestamp();

        /// <summary>
        /// 验证请求参数
        /// </summary>
        bool Validate(out string errorMessage);
    }
}
