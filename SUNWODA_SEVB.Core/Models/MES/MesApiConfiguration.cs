using SUNWODA_SEVB.Core.Common;

namespace SUNWODA_SEVB.Core.Models.MES
{
    /// <summary>
    /// MES API配置
    /// </summary>
    public class MesApiConfiguration : ModelBase
    {
        private string baseUrl = string.Empty;
        private int timeoutSeconds = 30;
        private bool enableRetry = true;
        private int maxRetryCount = 3;
        private Dictionary<string, string> endpoints = new();

        /// <summary>
        /// 基础URL
        /// </summary>
        public string BaseUrl
        {
            get => baseUrl;
            set => SetProperty(ref baseUrl, value);
        }

        /// <summary>
        /// 超时时间（秒）
        /// </summary>
        public int TimeoutSeconds
        {
            get => timeoutSeconds;
            set => SetProperty(ref timeoutSeconds, value);
        }

        /// <summary>
        /// 是否启用重试
        /// </summary>
        public bool EnableRetry
        {
            get => enableRetry;
            set => SetProperty(ref enableRetry, value);
        }

        /// <summary>
        /// 最大重试次数
        /// </summary>
        public int MaxRetryCount
        {
            get => maxRetryCount;
            set => SetProperty(ref maxRetryCount, value);
        }

        /// <summary>
        /// 端点映射
        /// </summary>
        public Dictionary<string, string> Endpoints
        {
            get => endpoints;
            set => SetProperty(ref endpoints, value);
        }

        /// <summary>
        /// 从配置文件加载
        /// </summary>
        public static MesApiConfiguration LoadFromConfiguration()
        {
            return ConfigurationHelper.GetSection<MesApiConfiguration>("MesApi");
        }
    }
}
