using System.Collections.ObjectModel;
using System.Windows.Input;
using SUNWODA_SEVB.Core.Attributes;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Core.Common.Commands;

namespace SUNWODA_SEVB.ViewModels.Pages.SmartManagement
{
    [Module("ServoFrequentAlarmMonitorPage", "伺服报警监控", Category = "设备智慧管理", Order = 26)]
    public class VM_ServoFrequentAlarmMonitorPage : ViewModelBase
    {
        public VM_ServoFrequentAlarmMonitorPage()
        {
            RefreshCommand = new RelayCommand(LoadMockData);
            ExportCommand = new RelayCommand(() => { });
            LoadMockData();
        }

        public ObservableCollection<ServoAlarmSummaryItem> SummaryItems { get; } = new();
        public ObservableCollection<ServoAxisAlarmRankItem> AxisRanks { get; } = new();
        public ObservableCollection<ServoOnlineAxisItem> OnlineAxes { get; } = new();
        public ObservableCollection<ServoAlarmRuleItem> RuleItems { get; } = new();
        public ObservableCollection<ServoAlarmRecordItem> AlarmRecords { get; } = new();
        public ObservableCollection<ServoActionItem> ActionItems { get; } = new();

        public ICommand RefreshCommand { get; }
        public ICommand ExportCommand { get; }

        public string LineName { get; set; } = "L1 密封钉线";
        public string DeviceName { get; set; } = "焊接设备 01";
        public string ServoGroup { get; set; } = "全部伺服轴";
        public string AlarmLevel { get; set; } = "全部";
        public string TimeRange { get; set; } = "2026-07-02 08:00 ~ 20:00";
        public string RuleVersion { get; set; } = "当前标准 v1.0";

        private void LoadMockData()
        {
            ReplaceCollection(SummaryItems, new[]
            {
                new ServoAlarmSummaryItem("报警总数", "126", "次", "较昨日 +18%", "#F59E0B"),
                new ServoAlarmSummaryItem("频繁报警轴", "5", "个", "超过规则阈值", "#E94855"),
                new ServoAlarmSummaryItem("重复报警", "38", "次", "同代码反复触发", "#E94855"),
                new ServoAlarmSummaryItem("最长未恢复", "22", "min", "Z1 轴过载", "#E94855"),
                new ServoAlarmSummaryItem("抖动复发", "11", "次", "5 分钟内复发", "#F59E0B"),
                new ServoAlarmSummaryItem("待处理工单", "6", "单", "需设备确认", "#2F66F6")
            });

            ReplaceCollection(AxisRanks, new[]
            {
                new ServoAxisAlarmRankItem("1", "X3 焊接横移轴", "32 次", "复发 7 次", 92, "#E94855"),
                new ServoAxisAlarmRankItem("2", "Z1 下压轴", "26 次", "最长 22min", 82, "#E94855"),
                new ServoAxisAlarmRankItem("3", "Y2 上料移载轴", "18 次", "温度报警", 64, "#F59E0B"),
                new ServoAxisAlarmRankItem("4", "R1 旋转轴", "14 次", "跟随误差", 54, "#F59E0B"),
                new ServoAxisAlarmRankItem("5", "X2 扫码移载轴", "11 次", "通讯抖动", 44, "#2F66F6")
            });

            ReplaceCollection(OnlineAxes, new[]
            {
                new ServoOnlineAxisItem("X3", "焊接横移轴", "报警", "SV-203 跟随误差", "48.2℃", "18.6%", "0.036mm", 86, "#E94855"),
                new ServoOnlineAxisItem("Z1", "下压轴", "报警", "SV-118 伺服过载", "45.1℃", "15.2%", "0.041mm", 78, "#E94855"),
                new ServoOnlineAxisItem("Y2", "上料移载轴", "预警", "温度偏高", "51.8℃", "9.8%", "0.018mm", 64, "#F59E0B"),
                new ServoOnlineAxisItem("R1", "旋转轴", "运行", "轻微波动", "42.6℃", "8.4%", "0.012mm", 42, "#2F66F6"),
                new ServoOnlineAxisItem("X2", "扫码移载轴", "在线", "通讯正常", "39.4℃", "4.2%", "0.006mm", 24, "#23A862"),
                new ServoOnlineAxisItem("Z2", "顶升轴", "在线", "负载正常", "40.1℃", "5.1%", "0.008mm", 28, "#23A862")
            });

            ReplaceCollection(RuleItems, new[]
            {
                new ServoAlarmRuleItem("频繁报警", "同一轴 1 小时 >= 5 次", "高风险", "#E94855"),
                new ServoAlarmRuleItem("重复报警", "同代码 1 天 >= 10 次", "重复治理", "#E94855"),
                new ServoAlarmRuleItem("抖动复发", "恢复后 5 分钟内再次触发", "复发", "#F59E0B"),
                new ServoAlarmRuleItem("长时间未恢复", "持续时间 > 10 分钟", "超时", "#F59E0B"),
                new ServoAlarmRuleItem("报警升级", "高风险连续 2 小时未关闭", "工单升级", "#2F66F6")
            });

            ReplaceCollection(AlarmRecords, new[]
            {
                new ServoAlarmRecordItem("10:42:11", "X3", "SV-203", "跟随误差过大", "8m12s", "是", "待确认", "#E94855"),
                new ServoAlarmRecordItem("11:18:06", "Z1", "SV-118", "伺服过载", "22min", "否", "处理中", "#F59E0B"),
                new ServoAlarmRecordItem("13:52:48", "Y2", "SV-087", "电机温度高", "4m21s", "是", "待确认", "#E94855"),
                new ServoAlarmRecordItem("16:31:12", "R1", "SV-311", "编码器通讯异常", "12m06s", "是", "已派单", "#2F66F6"),
                new ServoAlarmRecordItem("19:04:10", "X2", "SV-502", "驱动器通讯抖动", "4m26s", "否", "观察", "#23A862"),
                new ServoAlarmRecordItem("19:36:22", "X3", "SV-203", "跟随误差过大", "3m18s", "是", "待确认", "#E94855")
            });

            ReplaceCollection(ActionItems, new[]
            {
                new ServoActionItem("1", "X3：优先检查丝杆/导轨润滑、负载变化和伺服增益参数"),
                new ServoActionItem("2", "Z1：核查过载报警前后的电流曲线与机械卡滞"),
                new ServoActionItem("3", "对 SV-203 / SV-118 建立重复报警专项治理任务"),
                new ServoActionItem("4", "频繁报警轴自动推送点检工单，并记录处理结果"),
                new ServoActionItem("5", "同一报警连续复发时自动升级到设备工程师")
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

    public record ServoAlarmSummaryItem(string Title, string Value, string Unit, string Description, string Color);

    public record ServoAxisAlarmRankItem(string Rank, string AxisName, string AlarmCount, string Description, double Percent, string Color);

    public record ServoOnlineAxisItem(
        string Axis,
        string AxisName,
        string State,
        string Message,
        string Temperature,
        string CurrentWave,
        string FollowError,
        double LoadPercent,
        string Color);

    public class ServoAlarmHeatRowItem
    {
        public ServoAlarmHeatRowItem(string axisName, int[] values)
        {
            AxisName = axisName;
            foreach (var value in values)
            {
                Cells.Add(new ServoAlarmHeatCellItem(value));
            }
        }

        public string AxisName { get; }
        public ObservableCollection<ServoAlarmHeatCellItem> Cells { get; } = new();
    }

    public class ServoAlarmHeatCellItem
    {
        public ServoAlarmHeatCellItem(int value)
        {
            Value = value == 0 ? string.Empty : value.ToString();
            FillColor = value switch
            {
                >= 5 => "#E94855",
                4 => "#F59E0B",
                3 => "#F8B7BE",
                2 => "#FBEBCB",
                1 => "#FBE3E6",
                _ => "#EAF6EF"
            };
            TextColor = value >= 4 ? "White" : "#172033";
        }

        public string Value { get; }
        public string FillColor { get; }
        public string TextColor { get; }
    }

    public record ServoAlarmTrendPointItem(string Time, int Count, double Height);

    public record ServoAlarmRuleItem(string Name, string Rule, string State, string Color);

    public record ServoAlarmRecordItem(string Time, string Axis, string Code, string Name, string Duration, string Repeated, string Status, string Color);

    public record ServoActionItem(string Index, string Text);

}
