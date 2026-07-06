using System.Collections.ObjectModel;
using System.Windows.Input;
using SUNWODA_SEVB.Core.Attributes;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Core.Common.Commands;

namespace SUNWODA_SEVB.ViewModels.Pages.SmartManagement
{
    [Module("ProductQualityTracePage", "产品品质追溯", Category = "设备智慧管理", Order = 24)]
    public class VM_ProductQualityTracePage : ViewModelBase
    {
        private QualityTraceRowItem? _selectedRecord;

        public VM_ProductQualityTracePage()
        {
            SearchCommand = new RelayCommand(LoadMockData);
            ExportCommand = new RelayCommand(() => { });
            LoadMockData();
        }

        public ObservableCollection<QualitySummaryItem> SummaryItems { get; } = new();
        public ObservableCollection<QualityTraceRowItem> Records { get; } = new();
        public ObservableCollection<QualityTraceStepItem> TraceSteps { get; } = new();
        public ObservableCollection<QualityDetailItem> DetailItems { get; } = new();

        public ICommand SearchCommand { get; }
        public ICommand ExportCommand { get; }

        public string ProductCode { get; set; } = "P20260702-104211-0038";
        public string BatchNo { get; set; } = "B20260702-08";
        public string ProcessName { get; set; } = "密封钉焊接";
        public string VisualResult { get; set; } = "不合格";
        public string StartTime { get; set; } = "2026-07-02 08:00";
        public string EndTime { get; set; } = "2026-07-02 20:00";

        public QualityTraceRowItem? SelectedRecord
        {
            get => _selectedRecord;
            set
            {
                if (SetProperty(ref _selectedRecord, value))
                {
                    RefreshSelectedDetail();
                }
            }
        }

        private void LoadMockData()
        {
            SummaryItems.Clear();
            SummaryItems.Add(new QualitySummaryItem("查询结果", "1,286 条", "#2F66F6"));
            SummaryItems.Add(new QualitySummaryItem("NG 产品", "36 条", "#E94855"));
            SummaryItems.Add(new QualitySummaryItem("待复判", "12 条", "#F59E0B"));
            SummaryItems.Add(new QualitySummaryItem("关联设备", "密封钉设备 01", "#7C3AED"));

            Records.Clear();
            Records.Add(new QualityTraceRowItem("P20260702-0038", "10:42:11", "10:43:02", "激光焊接", "NG", "OK", "NG", "电流 86A / 宽度 2.93", "B08"));
            Records.Add(new QualityTraceRowItem("P20260702-0039", "10:43:08", "10:43:55", "激光焊接", "OK", "OK", "OK", "电流 82A / 宽度 2.82", "B08"));
            Records.Add(new QualityTraceRowItem("P20260702-0040", "10:44:19", "10:45:01", "视觉检测", "NG", "OK", "NG", "焊缝宽度 2.93", "B08"));
            Records.Add(new QualityTraceRowItem("P20260702-0041", "10:45:12", "10:46:00", "扫码工位", "OK", "OK", "OK", "气源压力 0.61", "B08"));
            Records.Add(new QualityTraceRowItem("P20260702-0042", "10:46:06", "10:46:52", "点胶工位", "OK", "OK", "OK", "胶压 0.42", "B08"));
            Records.Add(new QualityTraceRowItem("P20260702-0043", "10:47:10", "10:47:59", "激光焊接", "NG", "OK", "NG", "电流 89A / 宽度 2.96", "B08"));
            Records.Add(new QualityTraceRowItem("P20260702-0044", "10:48:12", "10:49:05", "复检工位", "OK", "OK", "OK", "拉力 128N", "B08"));
            Records.Add(new QualityTraceRowItem("P20260702-0045", "10:49:20", "10:50:10", "激光焊接", "OK", "OK", "OK", "焊缝宽度 2.84", "B08"));
            Records.Add(new QualityTraceRowItem("P20260702-0046", "10:50:22", "10:51:12", "下料工位", "OK", "OK", "OK", "节拍 7.8s", "B08"));
            Records.Add(new QualityTraceRowItem("P20260702-0047", "10:52:01", "10:52:49", "视觉检测", "NG", "OK", "NG", "外观缺陷 A2", "B08"));

            SelectedRecord = Records.FirstOrDefault();
        }

        private void RefreshSelectedDetail()
        {
            var record = SelectedRecord;
            TraceSteps.Clear();
            DetailItems.Clear();

            if (record is null)
            {
                return;
            }

            TraceSteps.Add(new QualityTraceStepItem("扫码", "OK", "#2FAE66"));
            TraceSteps.Add(new QualityTraceStepItem("定位", "OK", "#2FAE66"));
            TraceSteps.Add(new QualityTraceStepItem("焊接", record.VisualResult == "NG" ? "异常" : "OK", record.VisualResult == "NG" ? "#E94855" : "#2FAE66"));
            TraceSteps.Add(new QualityTraceStepItem("检测", record.VisualResult, record.VisualResult == "NG" ? "#E94855" : "#2FAE66"));
            TraceSteps.Add(new QualityTraceStepItem("复判", record.VisualResult == "NG" ? "待定" : "通过", record.VisualResult == "NG" ? "#F59E0B" : "#2FAE66"));

            DetailItems.Add(new QualityDetailItem("产品编号", record.ProductCode));
            DetailItems.Add(new QualityDetailItem("生产批次", record.BatchNo));
            DetailItems.Add(new QualityDetailItem("当前工位", record.Station));
            DetailItems.Add(new QualityDetailItem("视觉结果", record.VisualResult == "NG" ? "不合格" : "合格"));
            DetailItems.Add(new QualityDetailItem("关键参数", record.KeyParameter));
            DetailItems.Add(new QualityDetailItem("处理建议", record.VisualResult == "NG" ? "进入复判并追踪同批次" : "正常放行"));
        }
    }

    public record QualitySummaryItem(string Title, string Value, string Color);

    public record QualityTraceRowItem(
        string ProductCode,
        string InTime,
        string OutTime,
        string Station,
        string VisualResult,
        string InResult,
        string OutResult,
        string KeyParameter,
        string BatchNo);

    public record QualityTraceStepItem(string Name, string State, string Color);

    public record QualityDetailItem(string Name, string Value);
}
