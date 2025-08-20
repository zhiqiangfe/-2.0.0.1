
namespace SUNWODA_SEVB.Core.Interfaces.MES
{
    /// <summary>
    /// MES服务基础接口
    /// </summary>
    public interface IMesService
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// 检查服务健康状态
        /// </summary>
        Task<bool> CheckHealthAsync();

        /// <summary>
        /// 获取服务版本
        /// </summary>
        string GetVersion();
    }
}
