
using SUNWODA_SEVB.Core.Models.Web;

namespace SUNWODA_SEVB.Core.Interfaces.Web
{
    /// <summary>
    /// WEB配置接口
    /// </summary>
    public interface IWebConfiguration
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        WebSettings GetSettings();

        /// <summary>
        /// 重新加载配置
        /// </summary>
        Task ReloadAsync();

        /// <summary>
        /// 检查是否启用WEB服务
        /// </summary>
        Task<bool> IsWebEnabledAsync();
    }
}
