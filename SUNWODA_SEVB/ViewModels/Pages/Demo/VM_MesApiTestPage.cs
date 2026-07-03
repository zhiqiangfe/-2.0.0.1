using Microsoft.Extensions.DependencyInjection;
using SUNWODA_SEVB.Core.Attributes;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Core.Common.Commands;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Interfaces.MES;
using SUNWODA_SEVB.Core.Models.MES;
using SUNWODA_SEVB.MES.Services;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;

namespace SUNWODA_SEVB.ViewModels.Pages.Demo
{
    /// <summary>
    /// MES 接口测试页面 ViewModel。
    /// 用于测试 AddMesServices 中已注入的 MES 业务接口。
    /// </summary>
    [Module("MesApiTestPage", "MES 接口测试", Category = "Demo", Order = 110)]
    public class VM_MesApiTestPage : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMesConfigurationProvider _configProvider;
        private readonly ILoggerService<VM_MesApiTestPage> _logger;

        private MesApiConfiguration? _configuration;
        private string _statusText = "就绪";
        private bool _isBusy;

        #region Marking 参数

        private string _markingProductSn = "TEST_SN_001";
        private string _markingDefectList = string.Empty;
        private string _markingResponse = string.Empty;

        #endregion

        #region Offline 参数

        private string _offlineProductSn = "TEST_SN_001";
        private string _offlineTestResult = "1";
        private string _offlineTestDataJson = string.Empty;
        private string _offlineResponse = string.Empty;

        #endregion

        #region PackProduce 参数

        private string _packProduceUrl = "http://10.98.177.17/api/Kanban/GetPackLineLatestData";
        private string _packProduceResponse = string.Empty;

        #endregion

        /// <summary>MES 配置信息。</summary>
        public MesApiConfiguration? Configuration
        {
            get => _configuration;
            set => SetProperty(ref _configuration, value);
        }

        /// <summary>状态栏文本。</summary>
        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        /// <summary>是否正在调用接口。</summary>
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #region Marking 属性

        public string MarkingProductSn
        {
            get => _markingProductSn;
            set => SetProperty(ref _markingProductSn, value);
        }

        public string MarkingDefectList
        {
            get => _markingDefectList;
            set => SetProperty(ref _markingDefectList, value);
        }

        public string MarkingResponse
        {
            get => _markingResponse;
            set => SetProperty(ref _markingResponse, value);
        }

        #endregion

        #region Offline 属性

        public string OfflineProductSn
        {
            get => _offlineProductSn;
            set => SetProperty(ref _offlineProductSn, value);
        }

        public string OfflineTestResult
        {
            get => _offlineTestResult;
            set => SetProperty(ref _offlineTestResult, value);
        }

        public string OfflineTestDataJson
        {
            get => _offlineTestDataJson;
            set => SetProperty(ref _offlineTestDataJson, value);
        }

        public string OfflineResponse
        {
            get => _offlineResponse;
            set => SetProperty(ref _offlineResponse, value);
        }

        #endregion

        #region PackProduce 属性

        public string PackProduceUrl
        {
            get => _packProduceUrl;
            set => SetProperty(ref _packProduceUrl, value);
        }

        public string PackProduceResponse
        {
            get => _packProduceResponse;
            set => SetProperty(ref _packProduceResponse, value);
        }

        #endregion

        public ICommand RefreshConfigCommand { get; }
        public ICommand CallMarkingCommand { get; }
        public ICommand CallOfflineCommand { get; }
        public ICommand CallPackProduceCommand { get; }
        public ICommand FillOfflineSampleCommand { get; }
        public ICommand ClearMarkingResponseCommand { get; }
        public ICommand ClearOfflineResponseCommand { get; }
        public ICommand ClearPackProduceResponseCommand { get; }

        public VM_MesApiTestPage(
            IServiceProvider serviceProvider,
            IMesConfigurationProvider configProvider,
            ILoggerService<VM_MesApiTestPage> logger)
        {
            _serviceProvider = serviceProvider;
            _configProvider = configProvider;
            _logger = logger;

            RefreshConfigCommand = new RelayCommand(async () => await LoadConfigurationAsync());
            CallMarkingCommand = new RelayCommand(async () => await CallMarkingAsync(), _ => !IsBusy);
            CallOfflineCommand = new RelayCommand(async () => await CallOfflineAsync(), _ => !IsBusy);
            CallPackProduceCommand = new RelayCommand(async () => await CallPackProduceAsync(), _ => !IsBusy);
            FillOfflineSampleCommand = new RelayCommand(() => OfflineTestDataJson = GetOfflineSampleJson());
            ClearMarkingResponseCommand = new RelayCommand(() => MarkingResponse = string.Empty);
            ClearOfflineResponseCommand = new RelayCommand(() => OfflineResponse = string.Empty);
            ClearPackProduceResponseCommand = new RelayCommand(() => PackProduceResponse = string.Empty);
        }

        protected override async Task OnNavigatedToAsync(object? parameter)
        {
            await base.OnNavigatedToAsync(parameter);
            await LoadConfigurationAsync();
            FillOfflineSampleCommand.Execute(null);
        }

        /// <summary>
        /// 加载 MES 配置。
        /// </summary>
        private async Task LoadConfigurationAsync()
        {
            try
            {
                StatusText = "正在加载 MES 配置...";
                Configuration = await _configProvider.GetConfigurationAsync();

                if (Configuration == null)
                {
                    StatusText = "MES 配置未加载（请检查 ProjectSettings:EnableMES 开关及数据库配置）";
                    return;
                }

                StatusText = $"MES 配置已加载: {Configuration.BaseUrl}";
            }
            catch (Exception ex)
            {
                _logger.Error("加载 MES 配置失败", ex);
                StatusText = $"加载失败: {ex.Message}";
            }
        }

        /// <summary>
        /// 调用 Marking 数据上传接口。
        /// </summary>
        private async Task CallMarkingAsync()
        {
            if (string.IsNullOrWhiteSpace(MarkingProductSn))
            {
                StatusText = "请输入产品序列号";
                return;
            }

            IsBusy = true;
            StatusText = "正在调用 Marking 数据上传接口...";
            MarkingResponse = string.Empty;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IMarkingDataUploadService>();

                var initialized = await service.InitializeAsync();
                if (!initialized)
                {
                    StatusText = "Marking 服务初始化失败，请检查 MES 配置";
                    return;
                }

                var defectList = string.IsNullOrWhiteSpace(MarkingDefectList)
                    ? null
                    : MarkingDefectList.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

                var response = await service.UploadAsync(MarkingProductSn, defectList);
                MarkingResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
                StatusText = $"Marking 接口调用完成: Success={response.Success}, Code={response.Code}";
            }
            catch (Exception ex)
            {
                MarkingResponse = ex.ToString();
                StatusText = $"Marking 接口调用异常: {ex.Message}";
                _logger.Error("MES Marking 接口测试调用失败", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 调用离线数据上传接口。
        /// </summary>
        private async Task CallOfflineAsync()
        {
            if (string.IsNullOrWhiteSpace(OfflineProductSn))
            {
                StatusText = "请输入产品序列号";
                return;
            }

            if (string.IsNullOrWhiteSpace(OfflineTestResult))
            {
                StatusText = "请输入测试结果";
                return;
            }

            List<OfflineTestData>? testDatas = null;
            try
            {
                testDatas = string.IsNullOrWhiteSpace(OfflineTestDataJson)
                    ? new List<OfflineTestData>()
                    : JsonSerializer.Deserialize<List<OfflineTestData>>(OfflineTestDataJson);
            }
            catch (Exception ex)
            {
                StatusText = $"测试数据 JSON 格式错误: {ex.Message}";
                return;
            }

            IsBusy = true;
            StatusText = "正在调用离线数据上传接口...";
            OfflineResponse = string.Empty;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IOfflineDataUploadService>();

                var initialized = await service.InitializeAsync();
                if (!initialized)
                {
                    StatusText = "离线数据服务初始化失败，请检查 MES 配置";
                    return;
                }

                var response = await service.UploadAsync(OfflineProductSn, OfflineTestResult, testDatas ?? new List<OfflineTestData>());
                OfflineResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
                StatusText = $"离线数据接口调用完成: Success={response.Success}, Code={response.Code}";
            }
            catch (Exception ex)
            {
                OfflineResponse = ex.ToString();
                StatusText = $"离线数据接口调用异常: {ex.Message}";
                _logger.Error("MES 离线数据接口测试调用失败", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 调用 PackProduce 查询接口。
        /// </summary>
        private async Task CallPackProduceAsync()
        {
            if (string.IsNullOrWhiteSpace(PackProduceUrl))
            {
                StatusText = "请输入请求地址";
                return;
            }

            IsBusy = true;
            StatusText = "正在调用 PackProduce 查询接口...";
            PackProduceResponse = string.Empty;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IPackProduceQueryService>();

                var initialized = await service.InitializeAsync();
                if (!initialized)
                {
                    StatusText = "PackProduce 服务初始化失败，请检查 MES 配置";
                    return;
                }

                var data = await service.GetPackProduceAsync(PackProduceUrl);
                var rawJson = service.LastRawJson;
                var statusCode = service.LastStatusCode;
                var errorMessage = service.LastErrorMessage;

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    PackProduceResponse = $"状态码: {statusCode}\r\n错误: {errorMessage}\r\n原始响应:\r\n{TryFormatJson(rawJson)}";
                    StatusText = $"PackProduce 接口调用失败: {errorMessage}";
                }
                else
                {
                    PackProduceResponse = $"状态码: {statusCode}\r\n返回记录数: {data.Count}\r\n原始响应:\r\n{TryFormatJson(rawJson)}";
                    StatusText = $"PackProduce 接口调用完成: 返回 {data.Count} 条记录";
                }
            }
            catch (Exception ex)
            {
                PackProduceResponse = ex.ToString();
                StatusText = $"PackProduce 接口调用异常: {ex.Message}";
                _logger.Error("MES PackProduce 接口测试调用失败", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 获取离线数据上传示例 JSON。
        /// </summary>
        private static string GetOfflineSampleJson()
        {
            var sample = new List<OfflineTestData>
            {
                new()
                {
                    paramCode = "VOLTAGE",
                    paramName = "电压",
                    paramValue = "3.85",
                    paramResult = "PASS",
                    paramUnit = "V"
                },
                new()
                {
                    paramCode = "IMPEDANCE",
                    paramName = "内阻",
                    paramValue = "12.5",
                    paramResult = "PASS",
                    paramUnit = "mΩ"
                }
            };

            return JsonSerializer.Serialize(sample, new JsonSerializerOptions { WriteIndented = true });
        }

        /// <summary>
        /// 尝试格式化 JSON，失败则返回原字符串。
        /// </summary>
        private static string TryFormatJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return json;

            try
            {
                var document = JsonDocument.Parse(json);
                return JsonSerializer.Serialize(document, new JsonSerializerOptions { WriteIndented = true });
            }
            catch
            {
                return json;
            }
        }
    }
}
