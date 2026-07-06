using System.Collections.ObjectModel;
using System.Windows.Input;
using SUNWODA_SEVB.Core.Attributes;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Core.Common.Commands;

namespace SUNWODA_SEVB.ViewModels.Pages.SmartManagement
{
    [Module("HmiAlarmTopAnalysisPage", "HMI 报警 Top 分析", Category = "设备智慧管理", Order = 22)]
    public class VM_HmiAlarmTopAnalysisPage : ViewModelBase
    {
        private readonly List<HmiAlarmScenario> _scenarios = new();
        private string _selectedLine = "L1 密封钉线";
        private string _selectedDevice = "全部设备";
        private string _selectedShift = "白班";
        private string _selectedDate = "2026-07-02";
        private string _selectedAlarmName = "激光器通信异常";
        private string _selectedAlarmDetail = "激光焊接工位 | 10:42:11 - 11:00:23 | 影响 -320 pcs";
        private string _recommendation = "建议增加激光器通信心跳监控，通信丢失超过 3s 自动抓取 PLC 快照、网口状态和 HMI 操作记录。";

        public VM_HmiAlarmTopAnalysisPage()
        {
            RefreshCommand = new RelayCommand(LoadMockData);
            SelectAlarmCommand = new RelayCommand<object?>(SelectAlarm);
            LoadMockData();
        }

        public ObservableCollection<AlarmSummaryItem> SummaryItems { get; } = new();
        public ObservableCollection<AlarmEventItem> AlarmEvents { get; } = new();
        public ObservableCollection<AlarmTopItem> AlarmTopItems { get; } = new();
        public ObservableCollection<AlarmHeatRowItem> HeatRows { get; } = new();
        public ObservableCollection<AlarmEvidenceItem> EvidenceItems { get; } = new();
        public ObservableCollection<AlarmProcessStepItem> ProcessSteps { get; } = new();

        public ICommand RefreshCommand { get; }
        public ICommand SelectAlarmCommand { get; }

        public string SelectedLine { get => _selectedLine; private set => SetProperty(ref _selectedLine, value); }
        public string SelectedDevice { get => _selectedDevice; private set => SetProperty(ref _selectedDevice, value); }
        public string SelectedShift { get => _selectedShift; private set => SetProperty(ref _selectedShift, value); }
        public string SelectedDate { get => _selectedDate; private set => SetProperty(ref _selectedDate, value); }
        public string SelectedAlarmName { get => _selectedAlarmName; private set => SetProperty(ref _selectedAlarmName, value); }
        public string SelectedAlarmDetail { get => _selectedAlarmDetail; private set => SetProperty(ref _selectedAlarmDetail, value); }
        public string Recommendation { get => _recommendation; private set => SetProperty(ref _recommendation, value); }

        private void LoadMockData()
        {
            _scenarios.Clear();
            _scenarios.Add(CreateLaserScenario());
            _scenarios.Add(CreateBarcodeScenario());
            _scenarios.Add(CreateGlueScenario());
            _scenarios.Add(CreateTrayScenario());
            _scenarios.Add(CreateSensorScenario());
            _scenarios.Add(CreateMesScenario());
            LoadPeriodStatistics();
            SelectScenario("laser");
        }

        private void SelectAlarm(object? parameter)
        {
            if (parameter is AlarmEventItem item)
            {
                SelectScenario(item.ScenarioKey);
            }
        }

        private void LoadPeriodStatistics()
        {
            ReplaceCollection(SummaryItems, new[]
            {
                new AlarmSummaryItem("报警总数", "186", "较昨日 +12%"),
                new AlarmSummaryItem("高优先级", "18", "未闭环 5"),
                new AlarmSummaryItem("平均响应", "2m36s", "达标"),
                new AlarmSummaryItem("重复报警", "43", "需治理"),
                new AlarmSummaryItem("最长持续", "18m12s", "激光器通信异常")
            });

            ReplaceCollection(AlarmTopItems, new[]
            {
                new AlarmTopItem("1", "激光器通信异常", "32 次", 92, "#E94855"),
                new AlarmTopItem("2", "条码枪读取超时", "28 次", 79, "#F59E0B"),
                new AlarmTopItem("3", "胶压低于下限", "24 次", 68, "#F5B23D"),
                new AlarmTopItem("4", "MES 上传重试", "22 次", 62, "#2F66F6"),
                new AlarmTopItem("5", "安全门信号异常", "18 次", 51, "#7C3AED")
            });

            ReplaceCollection(HeatRows, CreatePeriodHeatRows());
        }

        private void SelectScenario(string key)
        {
            var scenario = _scenarios.FirstOrDefault(it => it.Key == key) ?? _scenarios.First();

            SelectedAlarmName = scenario.AlarmName;
            SelectedAlarmDetail = $"{scenario.Station} | {scenario.StartTime} - {scenario.EndTime} | 影响 {scenario.Impact}";
            Recommendation = scenario.Recommendation;

            ReplaceCollection(EvidenceItems, scenario.EvidenceItems);
            ReplaceCollection(ProcessSteps, scenario.ProcessSteps);

            AlarmEvents.Clear();
            foreach (var item in _scenarios)
            {
                AlarmEvents.Add(new AlarmEventItem(
                    item.Key,
                    item.StartTime,
                    item.Level,
                    item.AlarmName,
                    item.Station,
                    item.Duration,
                    item.Color,
                    item.Key == scenario.Key));
            }
        }

        private static HmiAlarmScenario CreateLaserScenario()
        {
            return CreateScenario(
                "laser",
                "激光器通信异常",
                "高",
                "激光焊接",
                "10:42:11",
                "11:00:23",
                "18m12s",
                "-320 pcs",
                "#E94855",
                "建议增加激光器通信心跳监控，通信丢失超过 3s 自动抓取 PLC 快照、网口状态和 HMI 操作记录。",
                "HMI-LASER-203",
                "LaserCommLost",
                new[] { "激光器通信异常", "真空压力低", "条码枪读取超时", "MES 上传重试", "安全门信号异常" },
                new[] { 32, 26, 21, 18, 15 });
        }

        private static HmiAlarmScenario CreateBarcodeScenario()
        {
            return CreateScenario(
                "barcode",
                "条码枪读取超时",
                "中",
                "扫码工位",
                "11:18:06",
                "11:22:41",
                "4m35s",
                "-54 pcs",
                "#F59E0B",
                "建议检查扫码枪焦距、光源亮度和条码污损情况；连续超时超过 3 次时推送班组复扫提醒。",
                "HMI-SCAN-104",
                "ScannerReadDone",
                new[] { "条码枪读取超时", "扫码结果为空", "产品码重复绑定", "MES 上传重试", "定位未完成" },
                new[] { 28, 19, 16, 14, 10 });
        }

        private static HmiAlarmScenario CreateGlueScenario()
        {
            return CreateScenario(
                "glue",
                "胶压低于下限",
                "中",
                "点胶工位",
                "13:52:48",
                "13:57:09",
                "4m21s",
                "-62 pcs",
                "#F59E0B",
                "建议联动胶压曲线和供胶泵状态，低压持续超过 10s 自动暂停点胶并提示检查胶桶余量。",
                "HMI-GLUE-087",
                "GluePressureLow",
                new[] { "胶压低于下限", "点胶阀未打开", "胶桶余量低", "压力传感器漂移", "产品到位异常" },
                new[] { 24, 18, 13, 11, 8 });
        }

        private static HmiAlarmScenario CreateTrayScenario()
        {
            return CreateScenario(
                "tray",
                "料盘满料未取走",
                "高",
                "下料工位",
                "16:31:12",
                "16:43:18",
                "12m06s",
                "-145 pcs",
                "#E94855",
                "建议将满料信号与 AGV 取料任务绑定，超过 60s 未取走时升级通知并打开缓存预警。",
                "HMI-TRAY-311",
                "TrayFull",
                new[] { "料盘满料未取走", "AGV 响应超时", "下料缓存满", "检测放行等待", "安全门信号异常" },
                new[] { 30, 24, 22, 12, 9 });
        }

        private static HmiAlarmScenario CreateSensorScenario()
        {
            return CreateScenario(
                "sensor",
                "传感器波动",
                "低",
                "检测工位",
                "17:08:19",
                "17:19:30",
                "11m11s",
                "-96 pcs",
                "#F5B23D",
                "建议记录传感器波动频次与温度、振动数据，超过阈值时安排点检并更换易漂移传感器。",
                "HMI-SENSOR-066",
                "SensorFluctuation",
                new[] { "传感器波动", "检测结果抖动", "相机取图失败", "定位偏移", "温度超上限" },
                new[] { 18, 16, 13, 12, 9 });
        }

        private static HmiAlarmScenario CreateMesScenario()
        {
            return CreateScenario(
                "mes",
                "MES 上传重试",
                "中",
                "MES 通讯",
                "19:04:10",
                "19:08:36",
                "4m26s",
                "-38 pcs",
                "#2F66F6",
                "建议统计接口响应时间和重试次数，连续重试时先缓存过站数据，恢复后自动补传。",
                "HMI-MES-502",
                "MesUploadRetry",
                new[] { "MES 上传重试", "接口响应超时", "过站数据缓存", "批次校验失败", "网络延迟" },
                new[] { 22, 17, 14, 11, 9 });
        }

        private static HmiAlarmScenario CreateScenario(
            string key,
            string alarmName,
            string level,
            string station,
            string startTime,
            string endTime,
            string duration,
            string impact,
            string color,
            string recommendation,
            string alarmCode,
            string plcTag,
            string[] topNames,
            int[] topCounts)
        {
            var topItems = topNames.Select((name, index) =>
            {
                var percent = Math.Max(35, 92 - index * 13);
                return new AlarmTopItem((index + 1).ToString(), name, $"{topCounts[index]} 次", percent, index == 0 ? color : TopColors[index % TopColors.Length]);
            }).ToArray();

            return new HmiAlarmScenario(
                key,
                "L1 密封钉线",
                "全部设备",
                "白班",
                "2026-07-02",
                alarmName,
                level,
                station,
                startTime,
                endTime,
                duration,
                impact,
                color,
                recommendation,
                new[]
                {
                    new AlarmSummaryItem("报警总数", "186", "较昨日 +12%"),
                    new AlarmSummaryItem("高优先级", "18", "未闭环 5"),
                    new AlarmSummaryItem("平均响应", "2m36s", "达标"),
                    new AlarmSummaryItem("重复报警", "43", "需治理"),
                    new AlarmSummaryItem("最长持续", duration, alarmName)
                },
                topItems,
                CreateHeatRows(station),
                new[]
                {
                    new AlarmEvidenceItem("报警等级", level),
                    new AlarmEvidenceItem("报警代码", alarmCode),
                    new AlarmEvidenceItem("持续时长", duration),
                    new AlarmEvidenceItem("首次响应", "2m08s"),
                    new AlarmEvidenceItem("关联 PLC", plcTag),
                    new AlarmEvidenceItem("关联批次", "B20260702-08")
                },
                new[]
                {
                    new AlarmProcessStepItem("报警触发", startTime[..5], "已完成", "#2FAE66"),
                    new AlarmProcessStepItem("班组响应", "2m后", "已完成", "#2FAE66"),
                    new AlarmProcessStepItem("工程确认", "待定", level == "高" ? "进行中" : "已完成", level == "高" ? "#F59E0B" : "#2FAE66"),
                    new AlarmProcessStepItem("原因归类", "待定", "进行中", "#F59E0B"),
                    new AlarmProcessStepItem("改善验证", "未开始", "待处理", "#637083")
                });
        }

        private static AlarmHeatRowItem[] CreateHeatRows(string activeStation)
        {
            var rows = new[]
            {
                ("扫码", new[] { 1, 2, 1, 0, 1, 2, 1 }),
                ("定位", new[] { 0, 1, 2, 2, 1, 1, 0 }),
                ("焊接", new[] { 2, 3, 4, 5, 3, 4, 2 }),
                ("点胶", new[] { 1, 1, 2, 4, 2, 1, 1 }),
                ("检测", new[] { 0, 1, 1, 2, 3, 2, 1 }),
                ("下料", new[] { 1, 1, 2, 2, 5, 3, 2 })
            };

            return rows.Select(row =>
            {
                var values = row.Item2.ToArray();
                if (activeStation.Contains(row.Item1))
                {
                    values[3] = 5;
                    values[4] = Math.Max(values[4], 4);
                }
                return new AlarmHeatRowItem(row.Item1, values);
            }).ToArray();
        }

        private static AlarmHeatRowItem[] CreatePeriodHeatRows()
        {
            return new[]
            {
                new AlarmHeatRowItem("扫码", new[] { 1, 2, 1, 0, 1, 2, 1 }),
                new AlarmHeatRowItem("定位", new[] { 0, 1, 2, 2, 1, 1, 0 }),
                new AlarmHeatRowItem("焊接", new[] { 2, 3, 4, 5, 3, 4, 2 }),
                new AlarmHeatRowItem("点胶", new[] { 1, 1, 2, 4, 2, 1, 1 }),
                new AlarmHeatRowItem("检测", new[] { 0, 1, 1, 2, 3, 2, 1 }),
                new AlarmHeatRowItem("下料", new[] { 1, 1, 2, 2, 5, 3, 2 })
            };
        }

        private static readonly string[] TopColors = { "#E94855", "#F59E0B", "#F5B23D", "#2F66F6", "#7C3AED" };

        private static void ReplaceCollection<T>(ObservableCollection<T> collection, IEnumerable<T> items)
        {
            collection.Clear();
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }

    public record HmiAlarmScenario(
        string Key,
        string Line,
        string Device,
        string Shift,
        string Date,
        string AlarmName,
        string Level,
        string Station,
        string StartTime,
        string EndTime,
        string Duration,
        string Impact,
        string Color,
        string Recommendation,
        IReadOnlyList<AlarmSummaryItem> SummaryItems,
        IReadOnlyList<AlarmTopItem> TopItems,
        IReadOnlyList<AlarmHeatRowItem> HeatRows,
        IReadOnlyList<AlarmEvidenceItem> EvidenceItems,
        IReadOnlyList<AlarmProcessStepItem> ProcessSteps);

    public record AlarmSummaryItem(string Name, string Value, string Description);

    public record AlarmEventItem(string ScenarioKey, string Time, string Level, string Name, string Station, string Duration, string AccentColor, bool IsSelected)
    {
        public string Background => IsSelected ? "#FFF7F7" : "White";
        public string BorderColor => IsSelected ? AccentColor : "#E2E8F0";
    }

    public record AlarmTopItem(string Rank, string Name, string Count, double Percent, string FillColor)
    {
        public string PercentText => $"{Percent:0}%";
    }

    public class AlarmHeatRowItem
    {
        public AlarmHeatRowItem(string station, int[] values)
        {
            Station = station;
            foreach (var value in values)
            {
                Cells.Add(new AlarmHeatCellItem(value));
            }
        }

        public string Station { get; }
        public ObservableCollection<AlarmHeatCellItem> Cells { get; } = new();
    }

    public class AlarmHeatCellItem
    {
        public AlarmHeatCellItem(int value)
        {
            Value = value == 0 ? string.Empty : value.ToString();
            FillColor = value switch
            {
                >= 5 => "#E94855",
                4 => "#F59E0B",
                3 => "#F7C45F",
                2 => "#7CD99A",
                1 => "#BFE8CE",
                _ => "#EAF7EF"
            };
            TextColor = value >= 4 ? "White" : "#243044";
        }

        public string Value { get; }
        public string FillColor { get; }
        public string TextColor { get; }
    }

    public record AlarmEvidenceItem(string Name, string Value);

    public record AlarmProcessStepItem(string Name, string Time, string State, string FillColor);
}
