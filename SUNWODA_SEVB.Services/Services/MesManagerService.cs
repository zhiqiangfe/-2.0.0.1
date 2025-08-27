using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Interfaces.Data;
using SUNWODA_SEVB.Core.Models.MES;
using SUNWODA_SEVB.MES.Client;
using System.Collections.Concurrent;

namespace SUNWODA_SEVB.MES.Services
{
    /// <summary>
    /// MES服务管理器接口
    /// </summary>
    public interface IMesManagerService
    {
        /// <summary>
        /// 获取离线数据上传服务
        /// </summary>
        IOfflineDataUploadService GetOfflineDataUploadService();

        /// <summary>
        /// 获取Marking数据上传服务
        /// </summary>
        IMarkingDataUploadService GetMarkingDataUploadService();

        /// <summary>
        /// 获取指定类型的MES服务
        /// </summary>
        T GetService<T>() where T : IMesService;

        /// <summary>
        /// 检查MES服务是否可用
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// 初始化MES服务管理器
        /// </summary>
        /// <returns></returns>
        Task<bool> InitializeAsync();

        /// <summary>
        /// 检查所有服务的健康状态
        /// </summary>
        Task<Dictionary<string, bool>> CheckAllServicesHealthAsync();

        /// <summary>
        /// 获取所有已注册的服务
        /// </summary>
        IEnumerable<IMesService> GetAllServices();
    }

    /// <summary>
    /// MES服务管理器实现
    /// </summary>
    public class MesManagerService : IMesManagerService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILoggerService<MesManagerService> _logger;
        private readonly ConcurrentDictionary<Type, IMesService> _serviceCache;
        private readonly SemaphoreSlim _initSemaphore = new(1, 1);

        private bool _isInitialized;
        private bool _mesEnabledInConfig;
        private bool _mesEnabledInDatabase;
        private MesApiConfiguration? _mesConfiguration;
        private IMesHttpClientManager? _httpClientManager;

        public bool IsEnabled => _mesEnabledInConfig && _mesEnabledInDatabase && _isInitialized;

        public MesManagerService(
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            ILoggerService<MesManagerService> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceCache = new ConcurrentDictionary<Type, IMesService>();

            // 从配置文件读取是否启用MES（总开关）
            _mesEnabledInConfig = _configuration.GetValue<bool>("ProjectSettings:EnableMES", false);
            _mesEnabledInDatabase = false;
            _isInitialized = false;

            _logger.Info($"MES管理器创建完成. 配置文件MES开关: {_mesEnabledInConfig}");
        }

        /// <summary>
        /// 初始化MES服务
        /// </summary>
        public async Task<bool> InitializeAsync()
        {
            await _initSemaphore.WaitAsync();
            try
            {
                if (_isInitialized)
                {
                    return IsEnabled;
                }

                _logger.Info("========== MES服务初始化开始 ==========");

                // 1. 检查配置文件总开关
                if (!_mesEnabledInConfig)
                {
                    _logger.Info("MES服务在配置文件中已禁用（总开关关闭）");
                    _isInitialized = true;
                    return false;
                }

                // 2. 从配置文件获取项目名称
                var projectName = _configuration.GetValue<string>("ProjectSettings:ProjectName");
                if (string.IsNullOrEmpty(projectName))
                {
                    _logger.Error("未配置项目名称");
                    _isInitialized = true;
                    return false;
                }

                // 3. 从数据库加载MES配置
                var loadResult = await LoadMesConfigurationFromDatabaseAsync(projectName);
                if (!loadResult)
                {
                    _logger.Info($"项目 {projectName} 在数据库中未配置MES或配置无效");
                    _isInitialized = true;
                    return false;
                }

                // 4. 初始化HTTP客户端管理器
                InitializeHttpClientManager();

                // 5. 创建MES API客户端
                InitializeMesApiClient();

                _isInitialized = true;
                _logger.Info($"========== MES服务初始化完成 (启用状态: {IsEnabled}) ==========");

                return IsEnabled;
            }
            catch (Exception ex)
            {
                _logger.Error("MES服务初始化失败", ex);
                _mesEnabledInDatabase = false;
                _isInitialized = true;
                return false;
            }
            finally
            {
                _initSemaphore.Release();
            }
        }

        /// <summary>
        /// 从数据库加载MES配置
        /// </summary>
        private async Task<bool> LoadMesConfigurationFromDatabaseAsync(string projectName)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var globalSettingRepo = scope.ServiceProvider.GetRequiredService<IGlobalSettingRepository>();
                var mesSettingRepo = scope.ServiceProvider.GetRequiredService<IMESSettingRepository>();

                // 1. 查询项目是否配置了MES Profile
                _logger.Debug($"查询项目 {projectName} 的MES配置");
                var mesProfileSettings = await globalSettingRepo.GetByNameAsync( "MesProfile");
              

                if (mesProfileSettings as bool == true )
                {
                    _logger.Info($"项目 {projectName} 未配置MES Profile");
                    _mesEnabledInDatabase = false;
                    return false;
                }

                _logger.Info($"项目 {projectName} 使用MES配置: {mesProfileSettings}");

                // 2. 加载MES详细配置
                var mesSettings = await mesSettingRepo.GetByProfileNameAsync(mesProfileSettings);
                if (mesSettings == null || !mesSettings.Any())
                {
                    _logger.Warn($"未找到Profile 的MES配置");
                    _mesEnabledInDatabase = false;
                    return false;
                }

                // 3. 构建配置对象
                _mesConfiguration = BuildMesConfiguration(profileName, mesSettings);

                // 验证必要配置
                if (string.IsNullOrWhiteSpace(_mesConfiguration.BaseUrl))
                {
                    _logger.Error("MES BaseUrl未配置");
                    _mesEnabledInDatabase = false;
                    return false;
                }

                _mesEnabledInDatabase = true;
                _logger.Info($"成功加载MES配置: BaseUrl={_mesConfiguration.BaseUrl}, Timeout={_mesConfiguration.TimeoutSeconds}s");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("从数据库加载MES配置时发生错误", ex);
                _mesEnabledInDatabase = false;
                return false;
            }
        }

        /// <summary>
        /// 构建MES配置对象
        /// </summary>
        private MesApiConfiguration BuildMesConfiguration(string profileName, IEnumerable<dynamic> settings)
        {
            var config = new MesApiConfiguration
            {
                ProfileName = profileName,
                TimeoutSeconds = 30,  // 默认值
                EnableRetry = true,    // 默认值
                MaxRetryCount = 3      // 默认值
            };

            foreach (var setting in settings)
            {
                if (setting?.Key == null || setting?.Value == null) continue;

                switch (setting.Key.ToLower())
                {
                    case "baseurl":
                        config.BaseUrl = setting.Value;
                        break;
                    case "timeout":
                    case "timeoutseconds":
                        if (int.TryParse(setting.Value, out var timeout))
                            config.TimeoutSeconds = timeout;
                        break;
                    case "enableretry":
                        if (bool.TryParse(setting.Value, out var retry))
                            config.EnableRetry = retry;
                        break;
                    case "maxretrycount":
                        if (int.TryParse(setting.Value, out var retryCount))
                            config.MaxRetryCount = retryCount;
                        break;
                    case "operatorid":
                        config.OperatorId = setting.Value;
                        break;
                    case "groupcode":
                        config.GroupCode = setting.Value;
                        break;
                    case "devicesn":
                        config.DeviceSn = setting.Value;
                        break;
                    case "monumber":
                        config.MoNumber = setting.Value;
                        break;
                    case "controlgroup":
                        config.ControlGroup = setting.Value;
                        break;
                    default:
                        // 存储自定义设置
                        config.CustomSettings[setting.Key] = setting.Value;
                        break;
                }
            }

            return config;
        }

        /// <summary>
        /// 初始化HTTP客户端管理器
        /// </summary>
        private void InitializeHttpClientManager()
        {
            if (_mesConfiguration == null)
            {
                _logger.Warn("MES配置为空，无法初始化HTTP客户端");
                return;
            }

            // 尝试从DI容器获取，如果没有则创建新实例
            _httpClientManager = _serviceProvider.GetService<IMesHttpClientManager>();

            if (_httpClientManager == null)
            {
                _httpClientManager = new MesHttpClientManager(
                    _serviceProvider.GetRequiredService<ILoggerService<MesHttpClientManager>>());
            }

            // 配置默认客户端
            _httpClientManager.ConfigureClient("MesApiClient", client =>
            {
                client.BaseAddress = new Uri(_mesConfiguration.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(_mesConfiguration.TimeoutSeconds);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "SUNWODA-SEVB-MES/1.0");

                // 添加自定义头
                if (!string.IsNullOrEmpty(_mesConfiguration.OperatorId))
                    client.DefaultRequestHeaders.Add("X-Operator-Id", _mesConfiguration.OperatorId);
                if (!string.IsNullOrEmpty(_mesConfiguration.GroupCode))
                    client.DefaultRequestHeaders.Add("X-Group-Code", _mesConfiguration.GroupCode);
                if (!string.IsNullOrEmpty(_mesConfiguration.DeviceSn))
                    client.DefaultRequestHeaders.Add("X-Device-SN", _mesConfiguration.DeviceSn);
            });

            _logger.Info("HTTP客户端管理器初始化完成");
        }

        /// <summary>
        /// 初始化MES API客户端
        /// </summary>
        private void InitializeMesApiClient()
        {
            // 这里可以预创建一些常用的API客户端
            // 但主要还是按需创建
            _logger.Info("MES API客户端准备就绪");
        }

        /// <summary>
        /// 获取离线数据上传服务
        /// </summary>
        public IOfflineDataUploadService? GetOfflineDataUploadService()
        {
            return GetService<IOfflineDataUploadService>();
        }

        /// <summary>
        /// 获取Marking数据上传服务
        /// </summary>
        public IMarkingDataUploadService? GetMarkingDataUploadService()
        {
            return GetService<IMarkingDataUploadService>();
        }

        /// <summary>
        /// 获取指定类型的服务
        /// </summary>
        public T? GetService<T>() where T : class, IMesService
        {
            if (!IsEnabled)
            {
                _logger.Warn($"MES服务未启用，无法获取服务 {typeof(T).Name}");
                return null;
            }

            var serviceType = typeof(T);

            // 尝试从缓存获取
            if (_serviceCache.TryGetValue(serviceType, out var cachedService))
            {
                return cachedService as T;
            }

            // 从DI容器创建新实例
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetService<T>();

                if (service == null)
                {
                    _logger.Error($"服务 {serviceType.Name} 未在DI容器中注册");
                    return null;
                }

                // 初始化服务
                if (_mesConfiguration != null)
                {
                    var initResult = service.InitializeAsync(_mesConfiguration).GetAwaiter().GetResult();
                    if (!initResult)
                    {
                        _logger.Error($"服务 {service.ServiceName} 初始化失败");
                        return null;
                    }
                }

                // 添加到缓存
                _serviceCache.TryAdd(serviceType, service);

                _logger.Info($"创建并缓存MES服务: {service.ServiceName}");
                return service;
            }
            catch (Exception ex)
            {
                _logger.Error($"创建服务 {serviceType.Name} 失败", ex);
                return null;
            }
        }

        /// <summary>
        /// 检查所有服务的健康状态
        /// </summary>
        public async Task<Dictionary<string, bool>> CheckAllServicesHealthAsync()
        {
            var results = new Dictionary<string, bool>();

            if (!IsEnabled)
            {
                _logger.Info("MES服务未启用，跳过健康检查");
                return results;
            }

            // 检查已缓存的服务
            foreach (var service in _serviceCache.Values)
            {
                try
                {
                    var isHealthy = await service.CheckHealthAsync();
                    results[service.ServiceName] = isHealthy;

                    if (!isHealthy)
                    {
                        _logger.Warn($"服务 {service.ServiceName} 健康检查失败");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"检查服务 {service.ServiceName} 健康状态时发生错误", ex);
                    results[service.ServiceName] = false;
                }
            }

            return results;
        }

        /// <summary>
        /// 获取所有已创建的服务
        /// </summary>
        public IEnumerable<IMesService> GetAllServices()
        {
            return _serviceCache.Values.ToList();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _initSemaphore?.Dispose();
            _httpClientManager?.Dispose();

            // 重置所有服务
            foreach (var service in _serviceCache.Values)
            {
                try
                {
                    service.ResetAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    _logger.Error($"重置服务 {service.ServiceName} 时发生错误", ex);
                }
            }

            _serviceCache.Clear();
            _logger.Info("MES管理器已释放所有资源");
        }
    }
}
