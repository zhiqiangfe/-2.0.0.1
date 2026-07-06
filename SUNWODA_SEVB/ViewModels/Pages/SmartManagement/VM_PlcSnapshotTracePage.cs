using System.Collections.ObjectModel;
using System.Windows.Input;
using SUNWODA_SEVB.Core.Attributes;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Core.Common.Commands;

namespace SUNWODA_SEVB.ViewModels.Pages.SmartManagement
{
    [Module("PlcSnapshotTracePage", "PLC 快照追溯", Category = "设备智慧管理", Order = 23)]
    public class VM_PlcSnapshotTracePage : ViewModelBase
    {
        private readonly List<PlcAlarmScenario> _scenarios = new();
        private string _selectedLine = "L1 密封钉线";
        private string _selectedDevice = "密封钉设备 01";
        private string _alarmName = "激光器通信异常";
        private string _triggerTime = "2026-07-02 10:42:11";
        private string _snapshotWindow = "前后 60s";
        private string _rootCause = string.Empty;
        private string _confidenceText = "83%";
        private double _confidenceValue = 83;

        public VM_PlcSnapshotTracePage()
        {
            RefreshCommand = new RelayCommand(() => ApplyScenario(AlarmItems.FirstOrDefault(it => it.IsSelected)?.ScenarioKey ?? "laser"));
            SelectAlarmCommand = new RelayCommand<object?>(SelectAlarm);
            LoadMockData();
        }

        public ObservableCollection<PlcAlarmIndexItem> AlarmItems { get; } = new();
        public ObservableCollection<PlcReplayPointItem> ReplayPoints { get; } = new();
        public ObservableCollection<PlcProcessNodeItem> ProcessNodes { get; } = new();
        public ObservableCollection<PlcSnapshotPointItem> SnapshotPoints { get; } = new();
        public ObservableCollection<PlcEvidenceCardItem> EvidenceCards { get; } = new();
        public ObservableCollection<PlcActionItem> ActionItems { get; } = new();
        public ObservableCollection<PlcMetricItem> Metrics { get; } = new();

        public ICommand RefreshCommand { get; }
        public ICommand SelectAlarmCommand { get; }

        public string SelectedLine { get => _selectedLine; private set => SetProperty(ref _selectedLine, value); }
        public string SelectedDevice { get => _selectedDevice; private set => SetProperty(ref _selectedDevice, value); }
        public string AlarmName { get => _alarmName; private set => SetProperty(ref _alarmName, value); }
        public string TriggerTime { get => _triggerTime; private set => SetProperty(ref _triggerTime, value); }
        public string SnapshotWindow { get => _snapshotWindow; private set => SetProperty(ref _snapshotWindow, value); }
        public string RootCause { get => _rootCause; private set => SetProperty(ref _rootCause, value); }
        public string ConfidenceText { get => _confidenceText; private set => SetProperty(ref _confidenceText, value); }
        public double ConfidenceValue { get => _confidenceValue; private set => SetProperty(ref _confidenceValue, value); }

        private void LoadMockData()
        {
            _scenarios.Clear();
            _scenarios.Add(CreateLaserScenario());
            _scenarios.Add(CreateBarcodeScenario());
            _scenarios.Add(CreateGlueScenario());
            _scenarios.Add(CreateTrayScenario());
            _scenarios.Add(CreateMesScenario());
            ApplyScenario("laser");
        }

        private void SelectAlarm(object? parameter)
        {
            if (parameter is PlcAlarmIndexItem item)
            {
                ApplyScenario(item.ScenarioKey);
            }
        }

        private void ApplyScenario(string scenarioKey)
        {
            var scenario = _scenarios.FirstOrDefault(it => it.Key == scenarioKey) ?? _scenarios.First();

            SelectedLine = scenario.Line;
            SelectedDevice = scenario.Device;
            AlarmName = scenario.AlarmName;
            TriggerTime = scenario.TriggerTime;
            SnapshotWindow = scenario.SnapshotWindow;
            RootCause = scenario.RootCause;
            ConfidenceText = scenario.ConfidenceText;
            ConfidenceValue = scenario.ConfidenceValue;

            AlarmItems.Clear();
            foreach (var item in _scenarios)
            {
                AlarmItems.Add(new PlcAlarmIndexItem(
                    item.Key,
                    item.TriggerTime.Split(' ').Last(),
                    item.AlarmName,
                    item.Level,
                    item.AlarmDetail,
                    item.Color,
                    item.Key == scenario.Key));
            }

            ReplayPoints.ReplaceWith(scenario.ReplayPoints);
            ProcessNodes.ReplaceWith(scenario.ProcessNodes);
            SnapshotPoints.ReplaceWith(scenario.SnapshotPoints);
            EvidenceCards.ReplaceWith(scenario.EvidenceCards);
            Metrics.ReplaceWith(scenario.Metrics);
            ActionItems.ReplaceWith(scenario.ActionItems);
        }

        private static PlcAlarmScenario CreateLaserScenario()
        {
            return new PlcAlarmScenario(
                "laser",
                "L1 密封钉线",
                "密封钉设备 01",
                "激光器通信异常",
                "高",
                "快照 128 点",
                "#F05252",
                "2026-07-02 10:42:11",
                "前后 60s",
                "83%",
                83,
                "报警前 32s 出现激光就绪信号抖动，触发点通信丢失信号拉高，焊接电流归零。建议优先检查激光器网口、电源波动与交换机端口错误包。",
                new[]
                {
                    new PlcReplayPointItem("-60s", "PLC 通讯正常", "心跳稳定", "#2BB673"),
                    new PlcReplayPointItem("-32s", "就绪信号波动", "连续 3 次短暂断开", "#F5A524"),
                    new PlcReplayPointItem("-08s", "扫码过站完成", "批次 B20260702-08", "#4C82FF"),
                    new PlcReplayPointItem("0s", "报警触发", "通信丢失 = 是", "#F05252"),
                    new PlcReplayPointItem("+12s", "班组响应", "HMI 确认报警", "#8B5CF6"),
                    new PlcReplayPointItem("+48s", "工程介入", "检查网口与电源", "#7F92AB")
                },
                new[]
                {
                    new PlcProcessNodeItem("上料", "正常", "#2BB673"),
                    new PlcProcessNodeItem("扫码", "正常", "#2BB673"),
                    new PlcProcessNodeItem("定位", "正常", "#2BB673"),
                    new PlcProcessNodeItem("焊接", "异常", "#F05252"),
                    new PlcProcessNodeItem("检测", "等待", "#F5A524"),
                    new PlcProcessNodeItem("下料", "阻塞", "#F5A524")
                },
                new[]
                {
                    new PlcSnapshotPointItem("激光通信丢失", "否", "是", "是", "上升沿", "通信丢失", "#F05252"),
                    new PlcSnapshotPointItem("激光就绪信号", "是", "否", "否", "下降沿", "就绪抖动", "#F5A524"),
                    new PlcSnapshotPointItem("焊接电流", "86A", "0A", "0A", "归零", "焊接中断", "#F05252"),
                    new PlcSnapshotPointItem("工位忙碌信号", "是", "是", "否", "释放", "流程阻塞", "#F5A524"),
                    new PlcSnapshotPointItem("安全门信号", "正常", "正常", "正常", "无变化", "正常", "#2BB673"),
                    new PlcSnapshotPointItem("气源压力", "0.62", "0.61", "0.60", "轻微下降", "正常", "#2BB673")
                },
                CreateCommonEvidence("128 点位已冻结", "丢包率 18.6%", "10:44:19"),
                CreateCommonMetrics("强", "中", "弱"),
                CreateCommonActions());
        }

        private static PlcAlarmScenario CreateBarcodeScenario()
        {
            return new PlcAlarmScenario(
                "barcode",
                "L1 密封钉线",
                "扫码工位 02",
                "条码读取超时",
                "中",
                "快照 86 点",
                "#F5A524",
                "2026-07-02 11:18:06",
                "前后 45s",
                "76%",
                76,
                "扫码触发后读码完成信号未在 3s 内返回，触发时扫码枪在线信号正常但读码结果为空。建议检查条码污损、扫码枪焦距和光源亮度。",
                new[]
                {
                    new PlcReplayPointItem("-45s", "扫码枪在线", "通讯正常", "#2BB673"),
                    new PlcReplayPointItem("-12s", "产品到位", "定位完成", "#4C82FF"),
                    new PlcReplayPointItem("0s", "读取超时", "读码完成 = 否", "#F5A524"),
                    new PlcReplayPointItem("+08s", "人工重扫", "HMI 手动触发", "#8B5CF6"),
                    new PlcReplayPointItem("+20s", "读码成功", "结果写入 MES", "#2BB673"),
                    new PlcReplayPointItem("+35s", "流程恢复", "放行完成", "#7F92AB")
                },
                new[]
                {
                    new PlcProcessNodeItem("上料", "正常", "#2BB673"),
                    new PlcProcessNodeItem("扫码", "超时", "#F5A524"),
                    new PlcProcessNodeItem("定位", "等待", "#F5A524"),
                    new PlcProcessNodeItem("焊接", "未开始", "#7F92AB"),
                    new PlcProcessNodeItem("检测", "未开始", "#7F92AB"),
                    new PlcProcessNodeItem("下料", "未开始", "#7F92AB")
                },
                new[]
                {
                    new PlcSnapshotPointItem("扫码触发信号", "否", "是", "否", "脉冲", "触发有效", "#2BB673"),
                    new PlcSnapshotPointItem("读码完成信号", "否", "否", "是", "延迟", "响应超时", "#F5A524"),
                    new PlcSnapshotPointItem("读码结果有效", "否", "否", "是", "延迟", "结果为空", "#F5A524"),
                    new PlcSnapshotPointItem("扫码枪在线", "是", "是", "是", "无变化", "通讯正常", "#2BB673"),
                    new PlcSnapshotPointItem("产品到位信号", "是", "是", "是", "无变化", "定位正常", "#2BB673"),
                    new PlcSnapshotPointItem("MES 放行信号", "否", "否", "是", "恢复", "重扫通过", "#2BB673")
                },
                CreateCommonEvidence("86 点位已冻结", "扫码光源偏暗", "11:19:02"),
                CreateCommonMetrics("中", "弱", "强"),
                CreateCommonActions());
        }

        private static PlcAlarmScenario CreateGlueScenario()
        {
            return new PlcAlarmScenario(
                "glue",
                "L2 点胶线",
                "点胶工位 03",
                "胶压低于下限",
                "中",
                "快照 94 点",
                "#F5A524",
                "2026-07-02 13:52:48",
                "前后 60s",
                "79%",
                79,
                "报警前胶压持续下滑，触发时胶压低限信号置位，点胶阀开度保持不变。建议检查胶桶余量、供胶泵和压力传感器漂移。",
                new[]
                {
                    new PlcReplayPointItem("-60s", "胶压正常", "0.48MPa", "#2BB673"),
                    new PlcReplayPointItem("-25s", "压力下降", "连续低于均值", "#F5A524"),
                    new PlcReplayPointItem("0s", "低限触发", "胶压低限 = 是", "#F05252"),
                    new PlcReplayPointItem("+10s", "暂停点胶", "阀门关闭", "#F5A524"),
                    new PlcReplayPointItem("+28s", "补压动作", "泵启动", "#4C82FF"),
                    new PlcReplayPointItem("+55s", "压力恢复", "0.46MPa", "#2BB673")
                },
                new[]
                {
                    new PlcProcessNodeItem("上料", "正常", "#2BB673"),
                    new PlcProcessNodeItem("扫码", "正常", "#2BB673"),
                    new PlcProcessNodeItem("定位", "正常", "#2BB673"),
                    new PlcProcessNodeItem("点胶", "低压", "#F05252"),
                    new PlcProcessNodeItem("检测", "等待", "#F5A524"),
                    new PlcProcessNodeItem("下料", "等待", "#F5A524")
                },
                new[]
                {
                    new PlcSnapshotPointItem("胶压低限信号", "否", "是", "否", "上升沿", "压力越限", "#F05252"),
                    new PlcSnapshotPointItem("胶压实际值", "0.42", "0.31", "0.38", "下降", "低于下限", "#F05252"),
                    new PlcSnapshotPointItem("点胶阀开度", "68%", "68%", "0%", "关闭", "暂停点胶", "#F5A524"),
                    new PlcSnapshotPointItem("供胶泵运行", "否", "否", "是", "启动", "补压动作", "#F5A524"),
                    new PlcSnapshotPointItem("产品到位信号", "是", "是", "是", "无变化", "定位正常", "#2BB673"),
                    new PlcSnapshotPointItem("安全互锁信号", "正常", "正常", "正常", "无变化", "正常", "#2BB673")
                },
                CreateCommonEvidence("94 点位已冻结", "压力曲线下滑", "13:53:22"),
                CreateCommonMetrics("强", "弱", "中"),
                CreateCommonActions());
        }

        private static PlcAlarmScenario CreateTrayScenario()
        {
            return new PlcAlarmScenario(
                "tray",
                "L1 密封钉线",
                "下料工位 05",
                "料盘满料未取走",
                "高",
                "快照 112 点",
                "#F05252",
                "2026-07-02 16:31:12",
                "前后 60s",
                "81%",
                81,
                "下料满料信号持续保持，AGV 呼叫已发出但取料完成信号未返回，导致后续工位阻塞。建议检查 AGV 任务派发、料盘传感器和下料缓存策略。",
                new[]
                {
                    new PlcReplayPointItem("-60s", "下料正常", "缓存未满", "#2BB673"),
                    new PlcReplayPointItem("-18s", "缓存接近满", "剩余 1 格", "#F5A524"),
                    new PlcReplayPointItem("0s", "满料报警", "满料信号 = 是", "#F05252"),
                    new PlcReplayPointItem("+15s", "AGV 呼叫", "任务已发送", "#4C82FF"),
                    new PlcReplayPointItem("+38s", "未取走", "完成信号未返回", "#F5A524"),
                    new PlcReplayPointItem("+60s", "人工介入", "清空缓存", "#8B5CF6")
                },
                new[]
                {
                    new PlcProcessNodeItem("上料", "正常", "#2BB673"),
                    new PlcProcessNodeItem("扫码", "正常", "#2BB673"),
                    new PlcProcessNodeItem("定位", "正常", "#2BB673"),
                    new PlcProcessNodeItem("焊接", "正常", "#2BB673"),
                    new PlcProcessNodeItem("检测", "等待", "#F5A524"),
                    new PlcProcessNodeItem("下料", "满料", "#F05252")
                },
                new[]
                {
                    new PlcSnapshotPointItem("料盘满料信号", "否", "是", "是", "上升沿", "缓存满料", "#F05252"),
                    new PlcSnapshotPointItem("AGV 呼叫信号", "否", "是", "是", "置位", "任务已发", "#4C82FF"),
                    new PlcSnapshotPointItem("取料完成信号", "否", "否", "否", "未变化", "未取走", "#F05252"),
                    new PlcSnapshotPointItem("下料允许信号", "是", "否", "否", "关闭", "阻塞保护", "#F5A524"),
                    new PlcSnapshotPointItem("检测放行信号", "是", "是", "否", "关闭", "上游等待", "#F5A524"),
                    new PlcSnapshotPointItem("安全互锁信号", "正常", "正常", "正常", "无变化", "正常", "#2BB673")
                },
                CreateCommonEvidence("112 点位已冻结", "AGV 响应超时", "16:32:04"),
                CreateCommonMetrics("强", "中", "中"),
                CreateCommonActions());
        }

        private static PlcAlarmScenario CreateMesScenario()
        {
            return new PlcAlarmScenario(
                "mes",
                "L1 密封钉线",
                "MES 通讯模块",
                "MES 上传重试",
                "中",
                "快照 76 点",
                "#4C82FF",
                "2026-07-02 19:04:10",
                "前后 45s",
                "72%",
                72,
                "PLC 过站完成后 MES 上传确认信号延迟返回，期间重试计数增加。建议检查接口响应时间、网络延迟和服务端队列积压。",
                new[]
                {
                    new PlcReplayPointItem("-45s", "通讯正常", "响应 180ms", "#2BB673"),
                    new PlcReplayPointItem("-10s", "过站完成", "等待上传", "#4C82FF"),
                    new PlcReplayPointItem("0s", "上传重试", "重试计数 +1", "#F5A524"),
                    new PlcReplayPointItem("+12s", "二次重试", "响应延迟", "#F5A524"),
                    new PlcReplayPointItem("+25s", "MES 确认", "上传成功", "#2BB673"),
                    new PlcReplayPointItem("+40s", "流程放行", "继续生产", "#7F92AB")
                },
                new[]
                {
                    new PlcProcessNodeItem("上料", "正常", "#2BB673"),
                    new PlcProcessNodeItem("扫码", "正常", "#2BB673"),
                    new PlcProcessNodeItem("定位", "正常", "#2BB673"),
                    new PlcProcessNodeItem("焊接", "完成", "#2BB673"),
                    new PlcProcessNodeItem("检测", "完成", "#2BB673"),
                    new PlcProcessNodeItem("MES", "延迟", "#F5A524")
                },
                new[]
                {
                    new PlcSnapshotPointItem("过站完成信号", "否", "是", "是", "置位", "等待上传", "#4C82FF"),
                    new PlcSnapshotPointItem("MES 上传请求", "否", "是", "否", "脉冲", "请求已发", "#4C82FF"),
                    new PlcSnapshotPointItem("MES 上传确认", "否", "否", "是", "延迟", "响应慢", "#F5A524"),
                    new PlcSnapshotPointItem("上传重试计数", "0", "1", "2", "增加", "重试发生", "#F5A524"),
                    new PlcSnapshotPointItem("网络连接状态", "正常", "正常", "正常", "无变化", "链路正常", "#2BB673"),
                    new PlcSnapshotPointItem("流程放行信号", "否", "否", "是", "恢复", "上传成功", "#2BB673")
                },
                CreateCommonEvidence("76 点位已冻结", "接口响应 4.8s", "19:04:38"),
                CreateCommonMetrics("中", "强", "弱"),
                CreateCommonActions());
        }

        private static PlcEvidenceCardItem[] CreateCommonEvidence(string plcValue, string networkValue, string hmiValue)
        {
            return new[]
            {
                new PlcEvidenceCardItem("PLC 快照", plcValue, "#4C82FF"),
                new PlcEvidenceCardItem("关联证据", networkValue, "#F05252"),
                new PlcEvidenceCardItem("HMI 确认", hmiValue, "#8B5CF6"),
                new PlcEvidenceCardItem("MES 批次", "B20260702-08", "#2BB673")
            };
        }

        private static PlcMetricItem[] CreateCommonMetrics(string plc, string network, string operation)
        {
            return new[]
            {
                new PlcMetricItem("PLC 证据", plc, "#F05252"),
                new PlcMetricItem("关联日志", network, "#F5A524"),
                new PlcMetricItem("操作记录", operation, "#4C82FF")
            };
        }

        private static PlcActionItem[] CreateCommonActions()
        {
            return new[]
            {
                new PlcActionItem("1", "保存快照为报警证据包"),
                new PlcActionItem("2", "自动抓取关联日志和状态曲线"),
                new PlcActionItem("3", "生成维修工单并绑定批次"),
                new PlcActionItem("4", "沉淀为同类报警诊断规则")
            };
        }
    }

    internal static class PlcCollectionExtensions
    {
        public static void ReplaceWith<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            collection.Clear();
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }

    public record PlcAlarmScenario(
        string Key,
        string Line,
        string Device,
        string AlarmName,
        string Level,
        string AlarmDetail,
        string Color,
        string TriggerTime,
        string SnapshotWindow,
        string ConfidenceText,
        double ConfidenceValue,
        string RootCause,
        IReadOnlyList<PlcReplayPointItem> ReplayPoints,
        IReadOnlyList<PlcProcessNodeItem> ProcessNodes,
        IReadOnlyList<PlcSnapshotPointItem> SnapshotPoints,
        IReadOnlyList<PlcEvidenceCardItem> EvidenceCards,
        IReadOnlyList<PlcMetricItem> Metrics,
        IReadOnlyList<PlcActionItem> ActionItems);

    public record PlcAlarmIndexItem(string ScenarioKey, string Time, string Name, string Level, string Detail, string Color, bool IsSelected)
    {
        public string Background => IsSelected ? "#17243A" : "#101A2C";
        public string BorderColor => IsSelected ? Color : "#25364F";
    }

    public record PlcReplayPointItem(string Offset, string Title, string Description, string Color);

    public record PlcProcessNodeItem(string Name, string State, string Color);

    public record PlcSnapshotPointItem(string Name, string BeforeValue, string TriggerValue, string AfterValue, string ChangeType, string Evidence, string Color)
    {
        public string TriggerBackground => Color == "#F05252" ? "#2A1720" : Color == "#F5A524" ? "#2A2315" : "#142619";
    }

    public record PlcEvidenceCardItem(string Name, string Value, string Color);

    public record PlcMetricItem(string Name, string Value, string Color);

    public record PlcActionItem(string Index, string Text);
}
