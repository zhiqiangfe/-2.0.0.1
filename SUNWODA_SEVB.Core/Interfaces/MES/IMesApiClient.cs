
namespace SUNWODA_SEVB.Core.Interfaces.MES
{
    /// <summary>
    /// MES API客户端接口
    /// </summary>
    public interface IMesApiClient
    {
        /// <summary>
        /// 发送POST请求
        /// </summary>
        Task<TResponse> PostAsync<TRequest, TResponse>(
            string endpoint,
            TRequest request,
            CancellationToken cancellationToken = default)
            where TRequest : IMesRequest
            where TResponse : IMesResponse, new();

        /// <summary>
        /// 发送GET请求
        /// </summary>
        Task<TResponse> GetAsync<TResponse>(
            string endpoint,
            object? parameters = null,
            CancellationToken cancellationToken = default)
            where TResponse : IMesResponse, new();

        /// <summary>
        /// 设置超时时间
        /// </summary>
        void SetTimeout(TimeSpan timeout);
    }
}
