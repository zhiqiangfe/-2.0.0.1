using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using SUNWODA_SEVB.Core.Attributes;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Core.Common.Commands;

namespace SUNWODA_SEVB.ViewModels.Pages.SmartManagement
{
    [Module("CpkProcessCapabilityPage", "CPK 过程能力", Category = "设备智慧管理", Order = 25)]
    public class VM_CpkProcessCapabilityPage : ViewModelBase
    {
        public VM_CpkProcessCapabilityPage()
        {
            RefreshCommand = new RelayCommand(LoadMockData);
            ExportCommand = new RelayCommand(() => { });
            LoadMockData();
        }

        public ObservableCollection<CpkSummaryItem> SummaryItems { get; } = new();
        public ObservableCollection<CpkMiniChartItem> MiniCharts { get; } = new();
        public ObservableCollection<CpkSpecItem> SpecItems { get; } = new();
        public ObservableCollection<CpkConclusionItem> Conclusions { get; } = new();
        public ObservableCollection<CpkHistogramBarItem> HistogramBars { get; } = new();

        public ICommand RefreshCommand { get; }
        public ICommand ExportCommand { get; }

        public string LineName { get; set; } = "L1";
        public string ProcessName { get; set; } = "密封钉焊接";
        public string ParameterName { get; set; } = "焊缝宽度";
        public string SampleCount { get; set; } = "最近 500 pcs";
        public string StartTime { get; set; } = "2026-07-02 08:00";
        public string EndTime { get; set; } = "2026-07-02 20:00";

        private void LoadMockData()
        {
            SummaryItems.Clear();
            SummaryItems.Add(new CpkSummaryItem("整体 CPK", "1.42", "低于目标 1.67", "#F59E0B"));
            SummaryItems.Add(new CpkSummaryItem("最佳工位", "内侧台阶2", "CPK 4.11", "#2FAE66"));
            SummaryItems.Add(new CpkSummaryItem("最低工位", "内侧台阶3", "CPK 0.49", "#E94855"));
            SummaryItems.Add(new CpkSummaryItem("异常点", "7 个", "需复核", "#E94855"));

            var names = new[]
            {
                "内侧台阶1", "内侧台阶2", "内侧台阶3", "内侧台阶4", "内侧台阶5",
                "内侧键隙1", "内侧键隙2", "内侧键隙3", "内侧键隙4", "内侧键隙5",
                "外侧台阶1", "外侧台阶2", "外侧台阶3", "外侧台阶4", "外侧台阶5"
            };
            var cpkValues = new[] { 3.32, 4.11, 0.49, 1.92, 0.72, 1.04, 3.06, 0.93, 1.34, 3.58, 3.32, 4.11, 0.49, 1.92, 0.72 };

            MiniCharts.Clear();
            for (var i = 0; i < names.Length; i++)
            {
                var trendPoints = new PointCollection();
                for (var j = 0; j < 18; j++)
                {
                    var wave = Math.Sin((j + i) * 0.72) * 0.16;
                    var drift = cpkValues[i] < 1.0 ? -0.18 + j * 0.012 : 0.10 - j * 0.006;
                    var trendValue = Math.Max(0.2, Math.Min(4.3, cpkValues[i] + wave + drift));
                    trendPoints.Add(new Point(j * 9.8, 58 - trendValue / 4.3 * 58));
                }

                MiniCharts.Add(new CpkMiniChartItem(
                    names[i],
                    $"CPK={cpkValues[i]:0.00}",
                    cpkValues[i] < 1.0 ? "能力不足" : cpkValues[i] < 1.33 ? "需关注" : "稳定",
                    cpkValues[i] < 1.0 ? "#E94855" : cpkValues[i] < 1.33 ? "#F59E0B" : "#2FAE66",
                    trendPoints));
            }

            SpecItems.Clear();
            SpecItems.Add(new CpkSpecItem("规格上限 USL", "2.95", "#E94855"));
            SpecItems.Add(new CpkSpecItem("目标值 Target", "2.80", "#2F66F6"));
            SpecItems.Add(new CpkSpecItem("规格下限 LSL", "2.65", "#E94855"));
            SpecItems.Add(new CpkSpecItem("均值 Mean", "2.84", "#243044"));
            SpecItems.Add(new CpkSpecItem("标准差 Sigma", "0.026", "#243044"));

            HistogramBars.Clear();
            foreach (var height in new[] { 18, 28, 44, 68, 92, 118, 104, 78, 52, 34, 20 })
            {
                HistogramBars.Add(new CpkHistogramBarItem(height));
            }

            Conclusions.Clear();
            Conclusions.Add(new CpkConclusionItem("1", "内侧台阶3 CPK=0.49，过程能力不足。"));
            Conclusions.Add(new CpkConclusionItem("2", "内侧台阶5 / 外侧台阶5 低于 1.0，建议优先排查。"));
            Conclusions.Add(new CpkConclusionItem("3", "CPK 页独立按数据库样本计算，不依赖产品追溯页。"));
        }
    }

    public record CpkSummaryItem(string Title, string Value, string Description, string Color);

    public record CpkMiniChartItem(string Name, string CpkText, string State, string Color, PointCollection TrendPoints);

    public record CpkTrendBarItem(double Height, string Color);

    public record CpkSpecItem(string Name, string Value, string Color);

    public record CpkHistogramBarItem(double Height);

    public record CpkConclusionItem(string Index, string Text);
}
