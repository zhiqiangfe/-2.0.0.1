using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SUNWODA_SEVB.Core.Attributes;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Core.Common.Commands;

namespace SUNWODA_SEVB.ViewModels.Pages.SmartManagement
{
    [Module("DeviceOeeAnalysisPage", "设备 OEE", Category = "设备智慧管理", Order = 20)]
    public class VM_DeviceOeeAnalysisPage : ViewModelBase
    {
        private string _selectedLine = "L1 密封钉线";
        private string _selectedDevice = "密封钉设备 01";
        private string _selectedShift = "白班";
        private string _selectedGranularity = "小时";
        private int _refreshIndex;
        public ObservableCollection<string> Lines { get; } = new ObservableCollection<string> { "L1 密封钉线", "L2 焊接线", "L3 测试线" };
        public ObservableCollection<string> Devices { get; } = new ObservableCollection<string> { "密封钉设备 01", "激光焊接设备 02", "EOL 测试设备 03" };
        public ObservableCollection<string> Shifts { get; } = new ObservableCollection<string> { "白班", "夜班", "全天" };
        public ObservableCollection<string> Granularities { get; } = new ObservableCollection<string> { "小时", "班次", "日" };
        public ObservableCollection<MetricCard> MetricCards { get; } = new ObservableCollection<MetricCard>();
        public ObservableCollection<OeeComponentItem> OeeComponents { get; } = new ObservableCollection<OeeComponentItem>();
        public ObservableCollection<StatusSegmentItem> StatusSegments { get; } = new ObservableCollection<StatusSegmentItem>();
        public ObservableCollection<DowntimeDetailItem> DowntimeDetails { get; } = new ObservableCollection<DowntimeDetailItem>();
        public ObservableCollection<TrendLabelItem> TrendLabels { get; } = new ObservableCollection<TrendLabelItem>();
        public ObservableCollection<TrendMarkerItem> TrendMarkers { get; } = new ObservableCollection<TrendMarkerItem>();

        public PointCollection OeeTrendPoints { get; } = new PointCollection();
        public PointCollection AvailabilityTrendPoints { get; } = new PointCollection();
        public ICommand RefreshCommand { get; }
        public ICommand ExportCommand { get; }

        public string SelectedLine { get => _selectedLine; set => SetProperty(ref _selectedLine, value); }
        public string SelectedDevice { get => _selectedDevice; set => SetProperty(ref _selectedDevice, value); }
        public string SelectedShift { get => _selectedShift; set => SetProperty(ref _selectedShift, value); }
        public string SelectedGranularity { get => _selectedGranularity; set => SetProperty(ref _selectedGranularity, value); }

        public string TimeRange { get; private set; } = "2026-07-02 08:00 ~ 20:00";
        public string Summary { get; private set; } = "主要损失来自稼动率：故障停机累计 42min。最长停机 10:42，建议下钻报警快照。";

        public VM_DeviceOeeAnalysisPage()
        {
            RefreshCommand = new RelayCommand(LoadMockData);
            ExportCommand = new RelayCommand(() => { });
            LoadMockData();
        }

        private void LoadMockData()
        {
            _refreshIndex++;
            var variation = _refreshIndex % 2 == 0 ? 0.0 : 0.4;
            TimeRange = SelectedShift == "夜班" ? "2026-07-02 20:00 ~ 2026-07-03 08:00" : "2026-07-02 08:00 ~ 20:00";
            Summary = $"主要损失来自稼动率：{SelectedDevice} 故障停机累计 42min。最长停机 10:42，建议下钻报警快照。";

            MetricCards.Clear();
            MetricCards.Add(new MetricCard("OEE", $"{82.4 + variation:0.0}%", "低于目标 2.6%", "#E02020"));
            MetricCards.Add(new MetricCard("稼动率", $"{88.1 + variation:0.0}%", "故障停机偏高", "#F59E0B"));
            MetricCards.Add(new MetricCard("性能效率", "93.6%", "节拍基本稳定", "#16A34A"));
            MetricCards.Add(new MetricCard("良率", "99.2%", "NG 103 pcs", "#16A34A"));
            MetricCards.Add(new MetricCard("总产量", "12,860", "计划达成 96.3%", "#667085"));
            MetricCards.Add(new MetricCard("故障时长", "42min", "最长停机 18min", "#E02020"));

            OeeComponents.Clear();
            OeeComponents.Add(new OeeComponentItem("稼动率", "88.1%", 220, "#F5B23D"));
            OeeComponents.Add(new OeeComponentItem("性能效率", "93.6%", 235, "#4F83F8"));
            OeeComponents.Add(new OeeComponentItem("良率", "99.2%", 250, "#58B870"));

            StatusSegments.Clear();
            StatusSegments.Add(new StatusSegmentItem(300, "#2FAE66", "08:00-10:42 运行"));
            StatusSegments.Add(new StatusSegmentItem(70, "#4F83F8", "10:42-11:00 待机/切换"));
            StatusSegments.Add(new StatusSegmentItem(90, "#E94855", "10:42-11:00 故障"));
            StatusSegments.Add(new StatusSegmentItem(330, "#2FAE66", "11:00-16:30 运行"));
            StatusSegments.Add(new StatusSegmentItem(75, "#F59E0B", "16:30-17:12 报警待确认"));
            StatusSegments.Add(new StatusSegmentItem(110, "#2FAE66", "17:12-20:00 运行"));

            DowntimeDetails.Clear();
            DowntimeDetails.Add(new DowntimeDetailItem("10:42:11", "11:00:23", "故障", "18min 12s", "激光焊接", "激光器通信异常", "-320 pcs", "待确认"));
            DowntimeDetails.Add(new DowntimeDetailItem("11:18:06", "11:22:41", "报警", "4min 35s", "扫码工位", "条码枪读取超时", "-54 pcs", "已复位"));
            DowntimeDetails.Add(new DowntimeDetailItem("12:36:20", "12:43:55", "停机", "7min 35s", "治具回流", "治具到位信号丢失", "-108 pcs", "已处理"));
            DowntimeDetails.Add(new DowntimeDetailItem("13:52:48", "13:57:09", "报警", "4min 21s", "点胶工位", "胶压低于下限", "-62 pcs", "已确认"));
            DowntimeDetails.Add(new DowntimeDetailItem("15:26:40", "15:39:05", "停机", "12min 25s", "上料工位", "来料等待", "-180 pcs", "已处理"));
            DowntimeDetails.Add(new DowntimeDetailItem("16:31:12", "16:43:18", "报警", "12min 06s", "下料工位", "料盘满料未取走", "-145 pcs", "待确认"));
            DowntimeDetails.Add(new DowntimeDetailItem("17:08:19", "17:19:30", "报警", "11min 11s", "检测工位", "传感器波动", "-96 pcs", "已复位"));
            DowntimeDetails.Add(new DowntimeDetailItem("18:22:33", "18:28:14", "故障", "5min 41s", "视觉检测", "相机取图失败", "-71 pcs", "已处理"));
            DowntimeDetails.Add(new DowntimeDetailItem("19:04:10", "19:08:36", "报警", "4min 26s", "MES 通讯", "过站上传重试", "-38 pcs", "自动恢复"));
            DowntimeDetails.Add(new DowntimeDetailItem("19:16:28", "19:20:02", "报警", "3min 34s", "扫码工位", "产品码重复绑定", "-42 pcs", "待确认"));
            DowntimeDetails.Add(new DowntimeDetailItem("19:27:45", "19:31:10", "报警", "3min 25s", "夹爪机构", "夹爪闭合不到位", "-39 pcs", "已复位"));
            DowntimeDetails.Add(new DowntimeDetailItem("19:38:06", "19:42:58", "报警", "4min 52s", "温控模块", "加热温度超上限", "-58 pcs", "已确认"));
            DowntimeDetails.Add(new DowntimeDetailItem("19:49:32", "19:53:26", "报警", "3min 54s", "安全门", "安全门开关信号异常", "-44 pcs", "已处理"));

            TrendLabels.Clear();
            TrendLabels.Add(new TrendLabelItem(46, "08:00"));
            TrendLabels.Add(new TrendLabelItem(312, "12:00"));
            TrendLabels.Add(new TrendLabelItem(642, "16:00"));
            TrendLabels.Add(new TrendLabelItem(708, "20:00"));

            TrendMarkers.Clear();
            TrendMarkers.Add(new TrendMarkerItem(158, 74));
            TrendMarkers.Add(new TrendMarkerItem(506, 36));

            OeeTrendPoints.Clear();
            OeeTrendPoints.Add(new Point(26, 74));
            OeeTrendPoints.Add(new Point(96, 58));
            OeeTrendPoints.Add(new Point(166, 78));
            OeeTrendPoints.Add(new Point(236, 34));
            OeeTrendPoints.Add(new Point(312, 48));
            OeeTrendPoints.Add(new Point(382, 8));
            OeeTrendPoints.Add(new Point(456, 30));
            OeeTrendPoints.Add(new Point(516, 38));
            OeeTrendPoints.Add(new Point(586, 16));
            OeeTrendPoints.Add(new Point(656, 26));
            OeeTrendPoints.Add(new Point(744, 0));

            AvailabilityTrendPoints.Clear();
            AvailabilityTrendPoints.Add(new Point(26, 88));
            AvailabilityTrendPoints.Add(new Point(96, 78));
            AvailabilityTrendPoints.Add(new Point(166, 98));
            AvailabilityTrendPoints.Add(new Point(236, 64));
            AvailabilityTrendPoints.Add(new Point(312, 70));
            AvailabilityTrendPoints.Add(new Point(382, 34));
            AvailabilityTrendPoints.Add(new Point(456, 56));
            AvailabilityTrendPoints.Add(new Point(516, 68));
            AvailabilityTrendPoints.Add(new Point(586, 44));
            AvailabilityTrendPoints.Add(new Point(656, 50));
            AvailabilityTrendPoints.Add(new Point(744, 30));

            OnPropertyChanged(nameof(TimeRange));
            OnPropertyChanged(nameof(Summary));
            OnPropertyChanged(nameof(OeeTrendPoints));
            OnPropertyChanged(nameof(AvailabilityTrendPoints));
        }
    }

    public class MetricCard
    {
        public MetricCard(string title, string value, string description, string accent)
        {
            Title = title;
            Value = value;
            Description = description;
            AccentBrush = (Brush)new BrushConverter().ConvertFromString(accent)!;
        }

        public string Title { get; }
        public string Value { get; }
        public string Description { get; }
        public Brush AccentBrush { get; }
    }

    public class OeeComponentItem
    {
        public OeeComponentItem(string name, string value, double width, string fill)
        {
            Name = name;
            Value = value;
            Width = width;
            FillBrush = (Brush)new BrushConverter().ConvertFromString(fill)!;
        }

        public string Name { get; }
        public string Value { get; }
        public double Width { get; }
        public Brush FillBrush { get; }
    }

    public class StatusSegmentItem
    {
        public StatusSegmentItem(double width, string fill, string tooltip)
        {
            Width = width;
            FillBrush = (Brush)new BrushConverter().ConvertFromString(fill)!;
            Tooltip = tooltip;
        }

        public double Width { get; }
        public Brush FillBrush { get; }
        public string Tooltip { get; }
    }

    public class DowntimeDetailItem
    {
        public DowntimeDetailItem(string startTime, string endTime, string status, string duration, string station, string alarmName, string impact, string processState)
        {
            StartTime = startTime;
            EndTime = endTime;
            Status = status;
            Duration = duration;
            Station = station;
            AlarmName = alarmName;
            Impact = impact;
            ProcessState = processState;
        }

        public string StartTime { get; }
        public string EndTime { get; }
        public string Status { get; }
        public string Duration { get; }
        public string Station { get; }
        public string AlarmName { get; }
        public string Impact { get; }
        public string ProcessState { get; }
    }

    public class TrendLabelItem
    {
        public TrendLabelItem(double x, string label)
        {
            X = x;
            Label = label;
        }

        public double X { get; }
        public string Label { get; }
    }

    public class TrendMarkerItem
    {
        public TrendMarkerItem(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }
    }
}
