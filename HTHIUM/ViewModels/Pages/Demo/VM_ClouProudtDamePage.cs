using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using HTHIUM.Core.Attributes;
using HTHIUM.Core.Common;
using HTHIUM.Core.Common.Commands;

namespace HTHIUM.ViewModels.Pages.Demo
{
    [Module("ClouProudtDamePage", "工业生产监控")]
    public class VM_ClouProudtDamePage : ViewModelBase
    {
        private readonly DispatcherTimer _refreshTimer;
        private readonly Random _random = new Random();

        private bool _plcConnected = true;
        private bool _mesConnected = true;
        private string _plcStatusText = "Connected";
        private string _mesStatusText = "Connected";
        private Brush _plcStatusBrush = new SolidColorBrush(Color.FromRgb(34, 197, 94));
        private Brush _mesStatusBrush = new SolidColorBrush(Color.FromRgb(34, 197, 94));
        private string _recipeName = "PACK-ALPHA-01";
        private string _currentBarcode = "BC202604280001";
        private int _todayOutput = 1280;
        private int _okCount = 1252;
        private int _ngCount = 28;
        private double _yieldRate = 97.81;
        private DateTime _lastUpdateTime = DateTime.Now;
        private ProductionRealtimeItem? _selectedRecord;

        public VM_ClouProudtDamePage()
        {
            ProductionRecords = new ObservableCollection<ProductionRealtimeItem>();
            OperationLogs = new ObservableCollection<ProductionLogItem>();

            RefreshCommand = new RelayCommand(RefreshDashboard);
            ClearLogCommand = new RelayCommand(ClearLogs);
            ToggleAutoRefreshCommand = new RelayCommand(ToggleAutoRefresh);

            _refreshTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            _refreshTimer.Tick += (_, _) => RefreshDashboard();

            SeedData();
        }

        public ObservableCollection<ProductionRealtimeItem> ProductionRecords { get; }

        public ObservableCollection<ProductionLogItem> OperationLogs { get; }

        public ICommand RefreshCommand { get; }

        public ICommand ClearLogCommand { get; }

        public ICommand ToggleAutoRefreshCommand { get; }

        public bool PlcConnected
        {
            get => _plcConnected;
            set => SetProperty(ref _plcConnected, value);
        }

        public bool MesConnected
        {
            get => _mesConnected;
            set => SetProperty(ref _mesConnected, value);
        }

        public string PlcStatusText
        {
            get => _plcStatusText;
            set => SetProperty(ref _plcStatusText, value);
        }

        public string MesStatusText
        {
            get => _mesStatusText;
            set => SetProperty(ref _mesStatusText, value);
        }

        public Brush PlcStatusBrush
        {
            get => _plcStatusBrush;
            set => SetProperty(ref _plcStatusBrush, value);
        }

        public Brush MesStatusBrush
        {
            get => _mesStatusBrush;
            set => SetProperty(ref _mesStatusBrush, value);
        }

        public string RecipeName
        {
            get => _recipeName;
            set => SetProperty(ref _recipeName, value);
        }

        public string CurrentBarcode
        {
            get => _currentBarcode;
            set => SetProperty(ref _currentBarcode, value);
        }

        public int TodayOutput
        {
            get => _todayOutput;
            set => SetProperty(ref _todayOutput, value);
        }

        public int OkCount
        {
            get => _okCount;
            set => SetProperty(ref _okCount, value);
        }

        public int NgCount
        {
            get => _ngCount;
            set => SetProperty(ref _ngCount, value);
        }

        public double YieldRate
        {
            get => _yieldRate;
            set => SetProperty(ref _yieldRate, value);
        }

        public DateTime LastUpdateTime
        {
            get => _lastUpdateTime;
            set => SetProperty(ref _lastUpdateTime, value);
        }

        public ProductionRealtimeItem? SelectedRecord
        {
            get => _selectedRecord;
            set => SetProperty(ref _selectedRecord, value);
        }

        public bool IsAutoRefreshEnabled => _refreshTimer.IsEnabled;

        protected override async Task OnNavigatedToAsync(object? parameter)
        {
            if (!_refreshTimer.IsEnabled)
            {
                _refreshTimer.Start();
                OnPropertyChanged(nameof(IsAutoRefreshEnabled));
            }

            await base.OnNavigatedToAsync(parameter);
        }

        protected override async Task OnNavigatedFromAsync()
        {
            _refreshTimer.Stop();
            OnPropertyChanged(nameof(IsAutoRefreshEnabled));
            await base.OnNavigatedFromAsync();
        }

        private void SeedData()
        {
            ProductionRecords.Clear();
            OperationLogs.Clear();

            for (int i = 0; i < 10; i++)
            {
                ProductionRecords.Add(CreateRecord(10 - i));
            }

            SelectedRecord = ProductionRecords.FirstOrDefault();

            AddLog("INFO", "System boot completed.");
            AddLog("INFO", "Recipe loaded successfully.");
            AddLog("INFO", "PLC and MES links are healthy.");
        }

        private void RefreshDashboard()
        {
            SimulateConnectivity();
            SimulateProductionSummary();
            SimulateProductionRecord();
            LastUpdateTime = DateTime.Now;
        }

        private void ToggleAutoRefresh()
        {
            if (_refreshTimer.IsEnabled)
            {
                _refreshTimer.Stop();
                AddLog("WARN", "Auto refresh paused by operator.");
            }
            else
            {
                _refreshTimer.Start();
                AddLog("INFO", "Auto refresh resumed.");
            }

            OnPropertyChanged(nameof(IsAutoRefreshEnabled));
        }

        private void ClearLogs()
        {
            OperationLogs.Clear();
            AddLog("INFO", "Logs cleared.");
        }

        private void SimulateConnectivity()
        {
            if (_random.NextDouble() < 0.08)
            {
                PlcConnected = !PlcConnected;
            }

            if (_random.NextDouble() < 0.06)
            {
                MesConnected = !MesConnected;
            }

            PlcStatusText = PlcConnected ? "Connected" : "Disconnected";
            MesStatusText = MesConnected ? "Connected" : "Disconnected";
            PlcStatusBrush = PlcConnected
                ? new SolidColorBrush(Color.FromRgb(34, 197, 94))
                : new SolidColorBrush(Color.FromRgb(239, 68, 68));
            MesStatusBrush = MesConnected
                ? new SolidColorBrush(Color.FromRgb(34, 197, 94))
                : new SolidColorBrush(Color.FromRgb(239, 68, 68));

            if (!PlcConnected)
            {
                AddLog("ERROR", "PLC connection lost.");
            }

            if (!MesConnected)
            {
                AddLog("ERROR", "MES interface timeout.");
            }
        }

        private void SimulateProductionSummary()
        {
            var produced = _random.Next(1, 4);
            TodayOutput += produced;
            OkCount += produced;

            if (_random.NextDouble() < 0.2)
            {
                NgCount += 1;
                OkCount = Math.Max(0, OkCount - 1);
                AddLog("WARN", "NG detected on latest unit.");
            }

            YieldRate = TodayOutput == 0 ? 0 : Math.Round((double)OkCount / Math.Max(1, TodayOutput) * 100, 2);
            CurrentBarcode = $"BC{DateTime.Now:yyyyMMddHHmmss}";

            if (_random.NextDouble() < 0.15)
            {
                RecipeName = _random.Next(0, 2) == 0 ? "PACK-ALPHA-01" : "PACK-BETA-02";
                AddLog("INFO", $"Recipe switched to {RecipeName}.");
            }
        }

        private void SimulateProductionRecord()
        {
            var record = CreateRecord(0);
            ProductionRecords.Insert(0, record);

            while (ProductionRecords.Count > 30)
            {
                ProductionRecords.RemoveAt(ProductionRecords.Count - 1);
            }

            SelectedRecord = ProductionRecords.FirstOrDefault();
            AddLog("INFO", $"New barcode scanned: {record.Barcode}");
        }

        private ProductionRealtimeItem CreateRecord(int secondsAgo)
        {
            var timestamp = DateTime.Now.AddSeconds(-secondsAgo);
            var p1 = Math.Round(12 + _random.NextDouble() * 3, 2);
            var p2 = Math.Round(36 + _random.NextDouble() * 6, 2);
            var p3 = Math.Round(0.85 + _random.NextDouble() * 0.2, 3);
            var result = _random.NextDouble() < 0.85 ? "OK" : "NG";

            return new ProductionRealtimeItem
            {
                Barcode = $"BC{timestamp:yyyyMMddHHmmss}",
                Parameter1 = p1,
                Parameter2 = p2,
                Parameter3 = p3,
                Result = result,
                Station = "PACK-01",
                Recipe = RecipeName,
                Timestamp = timestamp
            };
        }

        private void AddLog(string level, string message)
        {
            OperationLogs.Insert(0, new ProductionLogItem
            {
                Time = DateTime.Now,
                Level = level,
                Message = message
            });

            while (OperationLogs.Count > 80)
            {
                OperationLogs.RemoveAt(OperationLogs.Count - 1);
            }
        }
    }

    public class ProductionRealtimeItem : ModelBase
    {
        private string _barcode = string.Empty;
        private double _parameter1;
        private double _parameter2;
        private double _parameter3;
        private string _result = string.Empty;
        private string _station = string.Empty;
        private string _recipe = string.Empty;
        private DateTime _timestamp;

        public string Barcode
        {
            get => _barcode;
            set => SetProperty(ref _barcode, value);
        }

        public double Parameter1
        {
            get => _parameter1;
            set => SetProperty(ref _parameter1, value);
        }

        public double Parameter2
        {
            get => _parameter2;
            set => SetProperty(ref _parameter2, value);
        }

        public double Parameter3
        {
            get => _parameter3;
            set => SetProperty(ref _parameter3, value);
        }

        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public string Station
        {
            get => _station;
            set => SetProperty(ref _station, value);
        }

        public string Recipe
        {
            get => _recipe;
            set => SetProperty(ref _recipe, value);
        }

        public DateTime Timestamp
        {
            get => _timestamp;
            set => SetProperty(ref _timestamp, value);
        }
    }

    public class ProductionLogItem : ModelBase
    {
        private DateTime _time;
        private string _level = string.Empty;
        private string _message = string.Empty;

        public DateTime Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        public string Level
        {
            get => _level;
            set => SetProperty(ref _level, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
    }
}
