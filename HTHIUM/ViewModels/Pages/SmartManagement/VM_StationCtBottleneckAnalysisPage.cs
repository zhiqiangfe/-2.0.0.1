using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using HTHIUM.Core.Attributes;
using HTHIUM.Core.Common;
using HTHIUM.Core.Common.Commands;

namespace HTHIUM.ViewModels.Pages.SmartManagement
{
    [Module("StationCtBottleneckAnalysisPage", "工位 CT 瓶颈分析", Category = "设备智慧管理", Order = 21)]
    public class VM_StationCtBottleneckAnalysisPage : ViewModelBase
    {
        private string _selectedLine = "L1 密封钉线";
        private string _selectedStationGroup = "焊接 / 封装段";
        private string _selectedShift = "白班";

        public VM_StationCtBottleneckAnalysisPage()
        {
            RefreshCommand = new RelayCommand(LoadMockData);
            LoadMockData();
        }

        public ObservableCollection<string> Lines { get; } = new ObservableCollection<string> { "L1 密封钉线", "L2 焊接线", "L3 测试线" };
        public ObservableCollection<string> StationGroups { get; } = new ObservableCollection<string> { "焊接 / 封装段", "测试段", "包装段" };
        public ObservableCollection<string> Shifts { get; } = new ObservableCollection<string> { "白班", "夜班", "全天" };
        public ObservableCollection<StationCtItem> Stations { get; } = new ObservableCollection<StationCtItem>();
        public ObservableCollection<ActionGanttItem> ActionSteps { get; } = new ObservableCollection<ActionGanttItem>();
        public ObservableCollection<CauseTagItem> CauseTags { get; } = new ObservableCollection<CauseTagItem>();
        public ObservableCollection<RecommendationItem> Recommendations { get; } = new ObservableCollection<RecommendationItem>();
        public ICommand RefreshCommand { get; }

        public string SelectedLine { get => _selectedLine; set => SetProperty(ref _selectedLine, value); }
        public string SelectedStationGroup { get => _selectedStationGroup; set => SetProperty(ref _selectedStationGroup, value); }
        public string SelectedShift { get => _selectedShift; set => SetProperty(ref _selectedShift, value); }
        public string TimeRange { get; private set; } = "2026-07-02 08:00 ~ 20:00";
        public string TargetCt { get; private set; } = "4.50s";
        public string BottleneckStation { get; private set; } = "激光焊接";
        public string BottleneckCt { get; private set; } = "5.86s";
        public string BottleneckSummary { get; private set; } = "当前瓶颈来自激光焊接工位，主要损失集中在 PLC 握手等待与焊接动作本体耗时。";

        private void LoadMockData()
        {
            Stations.Clear();
            Stations.Add(new StationCtItem("扫码上料", "4.31s", "等待 0.18s", "正常", 267, "#2FAE66"));
            Stations.Add(new StationCtItem("夹具定位", "4.72s", "等待 0.41s", "轻微", 293, "#F5B23D"));
            Stations.Add(new StationCtItem("激光焊接", "5.86s", "等待 0.79s", "瓶颈", 363, "#E94855"));
            Stations.Add(new StationCtItem("视觉检测", "4.96s", "等待 0.32s", "关注", 308, "#F59E0B"));
            Stations.Add(new StationCtItem("下料复位", "4.18s", "等待 0.12s", "正常", 259, "#2FAE66"));
            Stations.Add(new StationCtItem("MES 过站", "4.54s", "通信 0.48s", "关注", 281, "#F59E0B"));

            ActionSteps.Clear();
            ActionSteps.Add(new ActionGanttItem("扫码确认", "0.42s", 0, 42, "#2FAE66"));
            ActionSteps.Add(new ActionGanttItem("夹具定位", "0.76s", 42, 76, "#10A6A6"));
            ActionSteps.Add(new ActionGanttItem("等待握手", "0.79s", 118, 79, "#F59E0B"));
            ActionSteps.Add(new ActionGanttItem("激光焊接", "2.38s", 197, 238, "#E94855"));
            ActionSteps.Add(new ActionGanttItem("视觉检测", "0.88s", 435, 88, "#2F66F6"));
            ActionSteps.Add(new ActionGanttItem("下料复位", "0.63s", 523, 63, "#F5B23D"));

            CauseTags.Clear();
            CauseTags.Add(new CauseTagItem("设备动作本体长", "#E94855"));
            CauseTags.Add(new CauseTagItem("PLC 握手等待", "#F59E0B"));
            CauseTags.Add(new CauseTagItem("夹具定位串行", "#F5B23D"));
            CauseTags.Add(new CauseTagItem("MES 上传波动", "#2F66F6"));

            Recommendations.Clear();
            Recommendations.Add(new RecommendationItem("1", "采集焊接开始/结束、使能、完成信号，拆分设备动作本体与等待。"));
            Recommendations.Add(new RecommendationItem("2", "将扫码完成后的夹具预定位提前，减少焊接前串行等待。"));
            Recommendations.Add(new RecommendationItem("3", "MES 过站改异步队列，避免通信抖动进入主节拍。"));
            Recommendations.Add(new RecommendationItem("4", "对 13:00 后 CT 抬升区间自动关联报警和参数快照。"));

            OnPropertyChanged(nameof(TimeRange));
            OnPropertyChanged(nameof(TargetCt));
            OnPropertyChanged(nameof(BottleneckStation));
            OnPropertyChanged(nameof(BottleneckCt));
            OnPropertyChanged(nameof(BottleneckSummary));
        }
    }

    public class StationCtItem
    {
        public StationCtItem(string name, string ct, string waitText, string state, double barWidth, string color)
        {
            Name = name;
            Ct = ct;
            WaitText = waitText;
            State = state;
            BarWidth = barWidth;
            AccentBrush = (Brush)new BrushConverter().ConvertFromString(color)!;
        }

        public string Name { get; }
        public string Ct { get; }
        public string WaitText { get; }
        public string State { get; }
        public double BarWidth { get; }
        public Brush AccentBrush { get; }
    }

    public class ActionGanttItem
    {
        public ActionGanttItem(string name, string duration, double left, double width, string color)
        {
            Name = name;
            Duration = duration;
            Left = left;
            Width = width;
            FillBrush = (Brush)new BrushConverter().ConvertFromString(color)!;
        }

        public string Name { get; }
        public string Duration { get; }
        public double Left { get; }
        public double Width { get; }
        public Brush FillBrush { get; }
    }

    public class CauseTagItem
    {
        public CauseTagItem(string text, string color)
        {
            Text = text;
            FillBrush = (Brush)new BrushConverter().ConvertFromString(color)!;
        }

        public string Text { get; }
        public Brush FillBrush { get; }
    }

    public class RecommendationItem
    {
        public RecommendationItem(string no, string text)
        {
            No = no;
            Text = text;
        }

        public string No { get; }
        public string Text { get; }
    }
}
