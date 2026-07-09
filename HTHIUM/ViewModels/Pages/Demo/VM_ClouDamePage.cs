using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using HTHIUM.Core.Attributes;
using HTHIUM.Core.Common;
using HTHIUM.Core.Common.Commands;
using HTHIUM.Core.Interfaces;
using HTHIUM.MES.Services;
using HTHIUM.ViewModels.Windows.Common;

namespace HTHIUM.ViewModels.Pages.Demo
{
    [Module("ClouDamePage", "Clou 接口测试")]
    public class VM_ClouDamePage : ViewModelBase
    {
        private readonly ILoggerService<VM_MainWindow> _logger;
        private readonly IPackProduceQueryService _packProduceQueryService;

        private string _requestUrl =
            "http://10.98.177.17/api/Kanban/GetPackLineLatestData";
        private bool _isLoading;
        private string _requestStatus = "尚未发起请求";
        private string _responseRawJson = string.Empty;
        private string _lastRefreshTime = "-";
        private string _errorMessage = string.Empty;
        private PackProduceDto? _selectedItem;

        public VM_ClouDamePage(
            ILoggerService<VM_MainWindow> logger,
            IPackProduceQueryService packProduceQueryService
        )
        {
            _logger = logger;
            _packProduceQueryService =
                packProduceQueryService ?? throw new ArgumentNullException(nameof(packProduceQueryService));

            QueryCommand = new RelayCommand(async () => await QueryAsync(), _ => !IsLoading);
            ClearCommand = new RelayCommand(ClearResult);
            LoadSampleCommand = new RelayCommand(LoadSampleUrl);

            RebuildStatusItems(0, "-", "-");
        }

        public string RequestUrl
        {
            get => _requestUrl;
            set => SetProperty(ref _requestUrl, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string RequestStatus
        {
            get => _requestStatus;
            set => SetProperty(ref _requestStatus, value);
        }

        public string ResponseRawJson
        {
            get => _responseRawJson;
            set => SetProperty(ref _responseRawJson, value);
        }

        public string LastRefreshTime
        {
            get => _lastRefreshTime;
            set => SetProperty(ref _lastRefreshTime, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public PackProduceDto? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ObservableCollection<PackProduceDto> PackProduceItems { get; } = new();

        public ObservableCollection<StatusItem> StatusItems { get; } = new();

        public ICommand QueryCommand { get; }

        public ICommand ClearCommand { get; }

        public ICommand LoadSampleCommand { get; }

        protected override async Task OnNavigatedToAsync(object? parameter)
        {
            await base.OnNavigatedToAsync(parameter);
            if (PackProduceItems.Count == 0 && !IsLoading)
            {
                await QueryAsync();
            }
        }

        private async Task QueryAsync()
        {
            if (string.IsNullOrWhiteSpace(RequestUrl))
            {
                RequestStatus = "请求地址不能为空";
                ErrorMessage = "请输入有效接口地址";
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                RequestStatus = "请求中...";

                await _packProduceQueryService.InitializeAsync();
                var items = await _packProduceQueryService.GetPackProduceAsync(RequestUrl.Trim());

                PackProduceItems.Clear();
                foreach (var item in items)
                {
                    PackProduceItems.Add(item);
                }

                SelectedItem = PackProduceItems.FirstOrDefault();
                ResponseRawJson = FormatJson(_packProduceQueryService.LastRawJson);
                LastRefreshTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                var latestCreateDate = PackProduceItems
                    .OrderByDescending(x => x.CreateDate)
                    .Select(x => x.CreateDate == default ? "-" : x.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"))
                    .FirstOrDefault() ?? "-";

                var latestLotSn = PackProduceItems.FirstOrDefault()?.LotSN ?? "-";
                RebuildStatusItems(PackProduceItems.Count, latestCreateDate, latestLotSn);

                if (string.IsNullOrWhiteSpace(_packProduceQueryService.LastErrorMessage))
                {
                    RequestStatus = $"请求成功，返回 {PackProduceItems.Count} 条记录";
                    _logger.Info(RequestStatus);
                }
                else
                {
                    RequestStatus = "请求失败";
                    ErrorMessage = _packProduceQueryService.LastErrorMessage;
                    _logger.Warn($"Clou 接口请求失败: {_packProduceQueryService.LastErrorMessage}", true);
                }
            }
            catch (Exception ex)
            {
                RequestStatus = "请求异常";
                ErrorMessage = ex.Message;
                _logger.Error("Clou 接口测试页请求异常", ex, true);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ClearResult()
        {
            PackProduceItems.Clear();
            StatusItems.Clear();
            ResponseRawJson = string.Empty;
            ErrorMessage = string.Empty;
            RequestStatus = "结果已清空";
            LastRefreshTime = "-";
            SelectedItem = null;
            RebuildStatusItems(0, "-", "-");
        }

        private void LoadSampleUrl()
        {
            RequestUrl = "http://10.98.177.17/api/Kanban/GetPackLineLatestData";
        }

        private void RebuildStatusItems(int count, string latestCreateDate, string latestLotSn)
        {
            StatusItems.Clear();
            StatusItems.Add(new StatusItem { Name = "返回数量", Value = count.ToString() });
            StatusItems.Add(new StatusItem { Name = "最后刷新", Value = LastRefreshTime });
            StatusItems.Add(new StatusItem { Name = "最新过站", Value = latestCreateDate });
            StatusItems.Add(new StatusItem { Name = "最新批次", Value = latestLotSn });
        }

        private static string FormatJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return string.Empty;
            }

            try
            {
                using var document = System.Text.Json.JsonDocument.Parse(json);
                return System.Text.Json.JsonSerializer.Serialize(
                    document,
                    new System.Text.Json.JsonSerializerOptions { WriteIndented = true }
                );
            }
            catch
            {
                return json;
            }
        }
    }
}
