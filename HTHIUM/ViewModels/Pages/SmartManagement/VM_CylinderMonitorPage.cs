using System.Collections.ObjectModel;
using System.Windows.Input;
using HTHIUM.Core.Attributes;
using HTHIUM.Core.Common;
using HTHIUM.Core.Common.Commands;

namespace HTHIUM.ViewModels.Pages.SmartManagement
{
    [Module("CylinderMonitorPage", "气缸监控", Category = "设备智慧管理", Order = 27)]
    public class VM_CylinderMonitorPage : ViewModelBase
    {
        public VM_CylinderMonitorPage()
        {
            RefreshCommand = new RelayCommand(LoadMockData);
            ExportCommand = new RelayCommand(() => { });
            LoadMockData();
        }

        public ObservableCollection<CylinderSummaryItem> SummaryItems { get; } = new();
        public ObservableCollection<CylinderRiskRankItem> RiskRanks { get; } = new();
        public ObservableCollection<CylinderRealtimeItem> RealtimeItems { get; } = new();
        public ObservableCollection<CylinderAlarmRecordItem> AlarmRecords { get; } = new();
        public ObservableCollection<CylinderActionItem> ActionItems { get; } = new();

        public ICommand RefreshCommand { get; }
        public ICommand ExportCommand { get; }

        public string LineName { get; set; } = "L1 密封钉线";
        public string DeviceName { get; set; } = "焊接设备 01";
        public string StationGroup { get; set; } = "全部气缸";
        public string StateFilter { get; set; } = "全部状态";
        public string TimeRange { get; set; } = "2026-07-07 08:00 ~ 20:00";

        private void LoadMockData()
        {
            ReplaceCollection(SummaryItems, new[]
            {
                new CylinderSummaryItem("在线气缸", "42", "个", "覆盖 8 个工位", "#2F66F6"),
                new CylinderSummaryItem("动作超时", "6", "次", "较昨日 +2", "#E94855"),
                new CylinderSummaryItem("到位异常", "4", "个", "伸出/缩回信号不一致", "#E94855"),
                new CylinderSummaryItem("平均气压", "0.58", "MPa", "低于建议值 0.02", "#F59E0B"),
                new CylinderSummaryItem("疑似漏气", "3", "处", "保压下降过快", "#F59E0B"),
                new CylinderSummaryItem("待处理", "5", "项", "需点检确认", "#2F66F6")
            });

            ReplaceCollection(RiskRanks, new[]
            {
                new CylinderRiskRankItem("1", "CY-08 焊接压紧气缸", "12 次", "缩回超时", 90, "#E94855"),
                new CylinderRiskRankItem("2", "CY-15 下料夹爪气缸", "9 次", "到位信号抖动", 76, "#E94855"),
                new CylinderRiskRankItem("3", "CY-03 上料定位气缸", "7 次", "伸出慢", 62, "#F59E0B"),
                new CylinderRiskRankItem("4", "CY-21 顶升气缸", "5 次", "压力不足", 48, "#F59E0B"),
                new CylinderRiskRankItem("5", "CY-11 扫码挡停气缸", "3 次", "动作次数偏高", 34, "#2F66F6")
            });

            ReplaceCollection(RealtimeItems, new[]
            {
                new CylinderRealtimeItem("CY-08", "焊接压紧气缸", "报警", "缩回超时", "ON", "OFF", "0.51MPa", "1.28s", "18,620", "10:42:11", "#E94855"),
                new CylinderRealtimeItem("CY-15", "下料夹爪气缸", "报警", "到位信号抖动", "ON", "ON", "0.55MPa", "0.92s", "12,406", "11:18:06", "#E94855"),
                new CylinderRealtimeItem("CY-03", "上料定位气缸", "预警", "伸出动作偏慢", "ON", "OFF", "0.56MPa", "0.84s", "20,331", "13:52:48", "#F59E0B"),
                new CylinderRealtimeItem("CY-21", "顶升气缸", "预警", "压力不足", "OFF", "ON", "0.49MPa", "1.05s", "9,845", "14:16:33", "#F59E0B"),
                new CylinderRealtimeItem("CY-11", "扫码挡停气缸", "运行", "动作正常", "OFF", "ON", "0.60MPa", "0.36s", "25,108", "15:09:27", "#2F66F6"),
                new CylinderRealtimeItem("CY-02", "上料推送气缸", "在线", "待命", "OFF", "ON", "0.61MPa", "0.42s", "18,442", "15:21:10", "#23A862"),
                new CylinderRealtimeItem("CY-06", "产品压紧气缸", "在线", "动作正常", "ON", "OFF", "0.59MPa", "0.58s", "16,903", "15:28:42", "#23A862"),
                new CylinderRealtimeItem("CY-12", "扫码升降气缸", "在线", "待命", "OFF", "ON", "0.60MPa", "0.47s", "13,254", "15:34:19", "#23A862"),
                new CylinderRealtimeItem("CY-18", "缓存放行气缸", "运行", "动作正常", "ON", "OFF", "0.57MPa", "0.63s", "8,734", "15:40:06", "#2F66F6"),
                new CylinderRealtimeItem("CY-24", "检测挡停气缸", "在线", "待命", "OFF", "ON", "0.62MPa", "0.39s", "11,608", "15:44:51", "#23A862")
            });

            ReplaceCollection(AlarmRecords, new[]
            {
                new CylinderAlarmRecordItem("10:42:11", "CY-08", "缩回超时", "1.28s", "焊接压紧", "待确认", "#E94855"),
                new CylinderAlarmRecordItem("11:18:06", "CY-15", "到位信号抖动", "0.92s", "下料夹爪", "处理中", "#F59E0B"),
                new CylinderAlarmRecordItem("13:52:48", "CY-03", "伸出动作偏慢", "0.84s", "上料定位", "待确认", "#E94855"),
                new CylinderAlarmRecordItem("14:16:33", "CY-21", "压力不足", "1.05s", "顶升机构", "已派单", "#2F66F6"),
                new CylinderAlarmRecordItem("15:09:27", "CY-11", "动作次数偏高", "0.36s", "扫码挡停", "观察", "#23A862")
            });

            ReplaceCollection(ActionItems, new[]
            {
                new CylinderActionItem("1", "CY-08：优先检查节流阀、导轨阻力和缩回磁性开关位置。"),
                new CylinderActionItem("2", "CY-15：复核伸出/缩回双到位信号，排查传感器抖动和接线松动。"),
                new CylinderActionItem("3", "CY-21：检查供气压力、调压阀和局部管路漏气。"),
                new CylinderActionItem("4", "将动作超时、信号冲突、压力不足纳入后续规则配置。"),
                new CylinderActionItem("5", "对频繁异常气缸建立点检记录，并关联备件更换周期。")
            });
        }

        private static void ReplaceCollection<T>(ObservableCollection<T> collection, IEnumerable<T> items)
        {
            collection.Clear();
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }

    public record CylinderSummaryItem(string Title, string Value, string Unit, string Description, string Color);

    public record CylinderRiskRankItem(string Rank, string CylinderName, string AlarmCount, string Description, double Percent, string Color);

    public record CylinderRealtimeItem(
        string CylinderNo,
        string CylinderName,
        string State,
        string Message,
        string ExtendSignal,
        string RetractSignal,
        string Pressure,
        string ActionTime,
        string CycleCount,
        string LastActionTime,
        string Color);

    public record CylinderAlarmRecordItem(string Time, string CylinderNo, string AlarmName, string ActionTime, string Station, string Status, string Color);

    public record CylinderActionItem(string Index, string Text);
}
