using Microsoft.Extensions.Configuration;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Interfaces.MES;
using SUNWODA_SEVB.Core.Models.MES;
using SUNWODA_SEVB.MES.Client;

namespace SUNWODA_SEVB.MES
{
    /// <summary>
    /// MES主服务实现
    /// </summary>
    public class MesService : IMesService
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerService<MesService> _logger;
        private readonly IMesConfigurationProvider _configProvider;
        private readonly IMesHttpClientManager _httpClientManager;
        private readonly IServiceProvider _serviceProvider;

        private readonly SemaphoreSlim _initLock = new(1, 1);
        private bool _isInitialized;
        private bool _isEnabled;

        public string ServiceName => "MesService";
        public bool IsEnabled => _isEnabled && _isInitialized;

        public MesService(
            IConfiguration configuration,
            ILoggerService<MesService> logger,
            IMesConfigurationProvider configProvider,
            IMesHttpClientManager httpClientManager,
            IServiceProvider serviceProvider)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
            _httpClientManager = httpClientManager ?? throw new ArgumentNullException(nameof(httpClientManager));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<bool> InitializeAsync()
        {
            await _initLock.WaitAsync();
            try
            {
                if (_isInitialized)
                {
                    _logger.Debug("MES服务已经初始化");
                    return IsEnabled;
                }

                _logger.Info("========== 开始初始化MES服务 ==========");

                // 1. 检查配置文件中的MES开关
                var mesEnabledInConfig = _configuration.GetValue<bool>("ProjectSettings:EnableMES", false);
                if (!mesEnabledInConfig)
                {
                    _logger.Info("MES服务在配置文件中已禁用");
                    _isEnabled = false;
                    _isInitialized = true;
                    return false;
                }

                // 2. 加载MES配置
                var mesConfig = await _configProvider.GetConfigurationAsync();
                if (mesConfig == null)
                {
                    _logger.Warn("未能加载MES配置，服务将被禁用");
                    _isEnabled = false;
                    _isInitialized = true;
                    return false;
                }

                // 3. 初始化HTTP客户端
                InitializeHttpClient(mesConfig);

                _isEnabled = true;
                _isInitialized = true;

                _logger.Info($"========== MES服务初始化完成 (启用状态: {IsEnabled}) ==========");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("MES服务初始化失败", ex);
                _isEnabled = false;
                _isInitialized = true;
                return false;
            }
            finally
            {
                _initLock.Release();
            }
        }

        private void InitializeHttpClient(MesApiConfiguration config)
        {
            _httpClientManager.ConfigureClient("MES", client =>
            {
                client.BaseAddress = new Uri(config.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "SUNWODA-SEVB-MES/1.0");

                // 添加自定义头
                foreach (var header in config.CustomHeaders)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            });

            _logger.Info($"HTTP客户端初始化完成: BaseUrl={config.BaseUrl}");
        }


        public async Task ResetAsync()
        {
            await _initLock.WaitAsync();
            try
            {
                _logger.Info("正在重置MES服务...");

                _isInitialized = false;
                _isEnabled = false;

                // 重新加载配置
                await _configProvider.ReloadConfigurationAsync();

                _logger.Info("MES服务重置完成");
            }
            finally
            {
                _initLock.Release();
            }
        }

        public string GetVersion()
        {
            return "1.0.0";
        }
    }
}
