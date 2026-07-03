using System.Net.Http;
using System.Text.Json;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Interfaces.MES;

namespace SUNWODA_SEVB.MES.Services
{
    public interface IPackProduceQueryService : IMesService
    {
        Task<List<PackProduceDto>> GetPackProduceAsync(
            string? requestUrl = null,
            CancellationToken cancellationToken = default
        );

        string LastRawJson { get; }

        int LastStatusCode { get; }

        string LastErrorMessage { get; }
    }

    public class PackProduceQueryService : IPackProduceQueryService
    {
        private const string DefaultRequestUrl =
            "http://10.98.177.17/api/Kanban/GetPackLineLatestData";

        private readonly HttpClient _httpClient;
        private readonly IMesConfigurationProvider _configProvider;
        private readonly ILoggerService<PackProduceQueryService> _logger;

        public string ServiceName => "PackProduceQuery";
        public bool IsEnabled { get; private set; }
        public string LastRawJson { get; private set; } = string.Empty;
        public int LastStatusCode { get; private set; }
        public string LastErrorMessage { get; private set; } = string.Empty;

        public PackProduceQueryService(
            HttpClient httpClient,
            IMesConfigurationProvider configProvider,
            ILoggerService<PackProduceQueryService> logger
        )
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
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
                    _logger.Info($"{ServiceName} 服务未启用，接口测试页仍可使用默认地址");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"{ServiceName} 服务初始化失败", ex);
                IsEnabled = false;
                return false;
            }
        }

        public async Task<List<PackProduceDto>> GetPackProduceAsync(
            string? requestUrl = null,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                LastErrorMessage = string.Empty;
                LastRawJson = string.Empty;

                var url = string.IsNullOrWhiteSpace(requestUrl) ? DefaultRequestUrl : requestUrl.Trim();
                _logger.Info($"开始请求 PackProduce 接口: {url}");

                var response = await _httpClient.GetAsync(url, cancellationToken);
                LastStatusCode = (int)response.StatusCode;

                if (!response.IsSuccessStatusCode)
                {
                    LastErrorMessage = $"请求失败: {response.StatusCode}";
                    _logger.Warn($"PackProduce 接口请求失败: {response.StatusCode}", true);
                    return new List<PackProduceDto>();
                }

                var json = await response.Content.ReadAsStringAsync(cancellationToken);

                if (json.StartsWith("\"", StringComparison.Ordinal))
                {
                    json = JsonSerializer.Deserialize<string>(json) ?? string.Empty;
                }

                LastRawJson = json;

                var wrapper = JsonSerializer.Deserialize<PackProduceResponse>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                var result = wrapper?.ds ?? new List<PackProduceDto>();
                _logger.Info($"PackProduce 接口请求成功，返回 {result.Count} 条记录");
                return result;
            }
            catch (Exception ex)
            {
                LastErrorMessage = ex.Message;
                _logger.Error($"PackProduce 接口调用异常: {ex.Message}", ex, true);
                return new List<PackProduceDto>();
            }
        }

        public async Task<bool> CheckHealthAsync()
        {
            try
            {
                var data = await GetPackProduceAsync(DefaultRequestUrl, CancellationToken.None);
                return data.Count >= 0 && string.IsNullOrWhiteSpace(LastErrorMessage);
            }
            catch
            {
                return false;
            }
        }

        public async Task ResetAsync()
        {
            IsEnabled = false;
            LastRawJson = string.Empty;
            LastErrorMessage = string.Empty;
            LastStatusCode = 0;
            await Task.CompletedTask;
        }

        public string GetVersion() => "1.0.0";
    }

    public class PackProduceResponse
    {
        public List<PackProduceDto> ds { get; set; } = new();
    }

    public class PackProduceDto
    {
        public string SpecificationId { get; set; } = string.Empty;
        public string SpecificationDescription { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string CollectSN { get; set; } = string.Empty;
        public string LotSN { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
    }

    public class StatusItem
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
