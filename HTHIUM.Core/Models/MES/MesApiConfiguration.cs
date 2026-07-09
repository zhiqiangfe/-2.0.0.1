using HTHIUM.Core.Common;

namespace HTHIUM.Core.Models.MES
{
    /// <summary>
    /// MES API配置
    /// </summary>
    public class MesApiConfiguration : ModelBase
    {
        private string profileName = string.Empty;
        private string baseUrl = string.Empty;
        private int timeoutSeconds = 30;
        private bool enableRetry = true;
        private int maxRetryCount = 3;

        // 业务相关配置
        private string operatorId = string.Empty;
        private string groupCode = string.Empty;
        private string deviceSn = string.Empty;
        private string moNumber = string.Empty;
        private string controlGroup = string.Empty;

        private Dictionary<string, string> endpoints = new();
        private Dictionary<string, string> customHeaders = new();
        private Dictionary<string, string> customSettings = new();

        /// <summary>
        /// 配置文件名称
        /// </summary>
        public string ProfileName
        {
            get => profileName;
            set => SetProperty(ref profileName, value);
        }

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
        /// 操作员ID
        /// </summary>
        public string OperatorId
        {
            get => operatorId;
            set => SetProperty(ref operatorId, value);
        }

        /// <summary>
        /// 组代码
        /// </summary>
        public string GroupCode
        {
            get => groupCode;
            set => SetProperty(ref groupCode, value);
        }

        /// <summary>
        /// 设备序列号
        /// </summary>
        public string DeviceSn
        {
            get => deviceSn;
            set => SetProperty(ref deviceSn, value);
        }

        /// <summary>
        /// 工单号
        /// </summary>
        public string MoNumber
        {
            get => moNumber;
            set => SetProperty(ref moNumber, value);
        }

        /// <summary>
        /// 控制组
        /// </summary>
        public string ControlGroup
        {
            get => controlGroup;
            set => SetProperty(ref controlGroup, value);
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
        /// 自定义请求头
        /// </summary>
        public Dictionary<string, string> CustomHeaders
        {
            get => customHeaders;
            set => SetProperty(ref customHeaders, value);
        }

        /// <summary>
        /// 自定义设置
        /// </summary>
        public Dictionary<string, string> CustomSettings
        {
            get => customSettings;
            set => SetProperty(ref customSettings, value);
        }

        /// <summary>
        /// 获取完整的端点URL
        /// </summary>
        public string GetEndpointUrl(string endpointKey)
        {
            if (Endpoints.TryGetValue(endpointKey, out var endpoint))
            {
                return $"{BaseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";
            }

            // 如果没有映射，直接使用key作为端点
            return $"{BaseUrl.TrimEnd('/')}/{endpointKey.TrimStart('/')}";
        }

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        public bool IsValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(BaseUrl))
            {
                errorMessage = "BaseUrl未配置";
                return false;
            }

            if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out _))
            {
                errorMessage = "BaseUrl格式无效";
                return false;
            }

            if (TimeoutSeconds <= 0)
            {
                errorMessage = "超时时间必须大于0";
                return false;
            }

            if (MaxRetryCount < 0)
            {
                errorMessage = "重试次数不能小于0";
                return false;
            }

            return true;
        }
    }
}
