using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Interfaces.MES;
using SUNWODA_SEVB.Core.Models.MES;
using SUNWODA_SEVB.MES.Models;


namespace SUNWODA_SEVB.MES.Services
{
    /// <summary>
    /// 离线数据上传服务接口
    /// </summary>
    public interface IOfflineDataUploadService : IMesService
    {
        Task<OfflineDataUploadResponse> UploadAsync(
            string productSn,
            string testResult,
            List<OfflineTestData> testDatas,
            List<EnvironmentData>? environments = null,
            List<StepData>? stepDatas = null,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 离线数据上传服务实现
    /// </summary>
    public class OfflineDataUploadService : IOfflineDataUploadService
    {
        private readonly IMesApiClient _apiClient;
        private readonly IMesConfigurationProvider _configProvider;
        private readonly ILoggerService<OfflineDataUploadService> _logger;

        public string ServiceName => "OfflineDataUpload";
        public bool IsEnabled { get; private set; }

        public OfflineDataUploadService(
            IMesApiClient apiClient,
            IMesConfigurationProvider configProvider,
            ILoggerService<OfflineDataUploadService> logger)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> InitializeAsync()
        {
            try
            {
                var config = await _configProvider.GetConfigurationAsync();
                IsEnabled = config != null;

                if (IsEnabled)
                {
                    _logger.Info($"{ServiceName} 服务初始化成功");
                }
                else
                {
                    _logger.Info($"{ServiceName} 服务未启用");
                }

                return IsEnabled;
            }
            catch (Exception ex)
            {
                _logger.Error($"{ServiceName} 服务初始化失败", ex);
                IsEnabled = false;
                return false;
            }
        }

        public async Task<OfflineDataUploadResponse> UploadAsync(
            string productSn,
            string testResult,
            List<OfflineTestData> testDatas,
            List<EnvironmentData>? _environments = null,
            List<StepData>? _stepDatas = null,
            CancellationToken cancellationToken = default)
        {
            if (!IsEnabled)
            {
                _logger.Warn("离线数据上传服务未启用");
                return new OfflineDataUploadResponse
                {
                    Success = false,
                    Code = "SERVICE_DISABLED",
                    Message = "离线数据上传服务未启用"
                };
            }

            try
            {
                var config = await _configProvider.GetConfigurationAsync();
                if (config == null)
                {
                    throw new InvalidOperationException("MES配置不可用");
                }

                _logger.Info($"开始上传产品 {productSn} 的离线数据");

                var request = new OfflineDataUploadRequest
                {
                    operatorId = config.OperatorId,
                    productSn = productSn,
                    groupCode = config.GroupCode,
                    deviceSn = config.DeviceSn,
                    moNumber = config.MoNumber,
                    timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    testResult = testResult,
                    testData = testDatas,
                    environment = [],
                    stepData = []
                };

                var response = await _apiClient.PostAsync<OfflineDataUploadRequest, OfflineDataUploadResponse>(
                    "OfflineDataUpload",
                    request,
                    cancellationToken);

                if (response.Success)
                {
                    _logger.Info($"产品 {productSn} 的离线数据上传成功");
                }
                else
                {
                    _logger.Warn($"产品 {productSn} 的离线数据上传失败: {response.Message}");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error($"上传产品 {productSn} 的离线数据时发生错误", ex);

                return new OfflineDataUploadResponse
                {
                    Success = false,
                    Code = "EXCEPTION",
                    Message = ex.Message
                };
            }
        }
      

        public async Task ResetAsync()
        {
            IsEnabled = false;
            await Task.CompletedTask;
        }

        public string GetVersion() => "1.0.0";
    }
}
