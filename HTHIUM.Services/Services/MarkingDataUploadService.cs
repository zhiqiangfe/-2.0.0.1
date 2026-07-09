using HTHIUM.Core.Interfaces;
using HTHIUM.Core.Interfaces.MES;
using HTHIUM.MES.Models;

namespace HTHIUM.MES.Services
{
    /// <summary>
    /// Marking数据上传服务接口
    /// </summary>
    public interface IMarkingDataUploadService : IMesService
    {
        Task<IncreaseMarkingResponse> UploadAsync(
            string productSn,
            List<string>? defectList = null,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Marking数据上传服务实现
    /// </summary>
    public class MarkingDataUploadService : IMarkingDataUploadService
    {
        private readonly IMesApiClient _apiClient;
        private readonly IMesConfigurationProvider _configProvider;
        private readonly ILoggerService<MarkingDataUploadService> _logger;

        public string ServiceName => "IncreaseMarking";
        public bool IsEnabled { get; private set; }

        public MarkingDataUploadService(
            IMesApiClient apiClient,
            IMesConfigurationProvider configProvider,
            ILoggerService<MarkingDataUploadService> logger)
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

        public async Task<IncreaseMarkingResponse> UploadAsync(
            string productSn,
            List<string>? defectList = null,
            CancellationToken cancellationToken = default)
        {
            if (!IsEnabled)
            {
                _logger.Warn("Marking数据上传服务未启用");
                return new IncreaseMarkingResponse
                {
                    Success = false,
                    Code = "SERVICE_DISABLED",
                    Message = "Marking数据上传服务未启用"
                };
            }

            try
            {
                var config = await _configProvider.GetConfigurationAsync();
                if (config == null)
                {
                    throw new InvalidOperationException("MES配置不可用");
                }

                _logger.Info($"开始上传产品 {productSn} 的Marking数据");

                var request = new IncreaseMarkingRequest
                {
                    DeviceSn = config.DeviceSn,
                    SerialNumber = productSn,
                    GroupCode = config.GroupCode,
                    ControlGroup = config.ControlGroup,
                    TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Remark = defectList != null && defectList.Count > 0
                        ? string.Join(",", defectList)
                        : string.Empty
                };

                var response = await _apiClient.PostAsync<IncreaseMarkingRequest, IncreaseMarkingResponse>(
                    "IncreaseMarking",
                    request,
                    cancellationToken);

                if (response.Success)
                {
                    _logger.Info($"产品 {productSn} 的Marking数据上传成功");
                }
                else
                {
                    _logger.Warn($"产品 {productSn} 的Marking数据上传失败: {response.Message}");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error($"上传产品 {productSn} 的Marking数据时发生错误", ex);

                return new IncreaseMarkingResponse
                {
                    Success = false,
                    Code = "EXCEPTION",
                    Message = ex.Message
                };
            }
        }

        public async Task<bool> CheckHealthAsync()
        {
            if (!IsEnabled)
                return false;

            try
            {
                var testRequest = new IncreaseMarkingRequest
                {
                    DeviceSn = "HEALTH_CHECK",
                    SerialNumber = "HEALTH_CHECK",
                    GroupCode = "HEALTH_CHECK",
                    ControlGroup = "HEALTH_CHECK",
                    TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Remark = "Health Check"
                };

                var response = await _apiClient.PostAsync<IncreaseMarkingRequest, IncreaseMarkingResponse>(
                    "HealthCheck",
                    testRequest,
                    CancellationToken.None);

                return response?.Success ?? false;
            }
            catch
            {
                return false;
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