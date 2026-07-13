using System.Collections.ObjectModel;
using System.Windows.Input;
using HTHIUM.Core.Attributes;
using HTHIUM.Core.Common;
using HTHIUM.Core.Common.Commands;
using HTHIUM.Data.Models;
using SqlSugar;

namespace HTHIUM.ViewModels.Pages.SmartManagement
{
    [Module("HmiAlarmTopAnalysisPage", "HMI 报警 Top 分析", Category = "设备智慧管理", Order = 22)]
    public class VM_HmiAlarmTopAnalysisPage : ViewModelBase
    {
        private readonly ISqlSugarClient? _db;
        private readonly List<HmiAlarmScenario> _scenarios = new();
        private DateTime? _startDateTime = new DateTime(2026, 7, 2, 8, 0, 0);
        private DateTime? _endDateTime = new DateTime(2026, 7, 2, 20, 0, 0);
        private string _selectedLine = "L1 密封钉线";
        private string _selectedDevice = "全部设备";
        private string _selectedShift = "白班";
        private string _selectedDate = "2026-07-02";
        private string _selectedAlarmName = "激光器通信异常";
        private string _selectedAlarmDetail = "激光焊接 | 10:42:11 - 11:00:23 | 影响 -320 pcs";
        private string _recommendation = "建议增加激光器通信心跳监控，通信丢失超过3秒自动抓取 PLC 快照、网口状态和 HMI 操作记录。";

        public VM_HmiAlarmTopAnalysisPage()
            : this(null)
        {
        }

        public VM_HmiAlarmTopAnalysisPage(ISqlSugarClient? db)
        {
            _db = db;
            RefreshCommand = new RelayCommand(LoadData);
            SelectAlarmCommand = new RelayCommand<object?>(SelectAlarm);
            LoadData();
        }

        public ObservableCollection<AlarmSummaryItem> SummaryItems { get; } = new();
        public ObservableCollection<AlarmEventItem> AlarmEvents { get; } = new();
        public ObservableCollection<AlarmTopItem> AlarmTopItems { get; } = new();
        public ObservableCollection<AlarmHeatRowItem> HeatRows { get; } = new();
        public ObservableCollection<AlarmEvidenceItem> EvidenceItems { get; } = new();
        public ObservableCollection<AlarmProcessStepItem> ProcessSteps { get; } = new();

        public ICommand RefreshCommand { get; }
        public ICommand SelectAlarmCommand { get; }

        public DateTime? StartDateTime { get => _startDateTime; set => SetProperty(ref _startDateTime, value); }
        public DateTime? EndDateTime { get => _endDateTime; set => SetProperty(ref _endDateTime, value); }
        public string SelectedLine { get => _selectedLine; private set => SetProperty(ref _selectedLine, value); }
        public string SelectedDevice { get => _selectedDevice; private set => SetProperty(ref _selectedDevice, value); }
        public string SelectedShift { get => _selectedShift; private set => SetProperty(ref _selectedShift, value); }
        public string SelectedDate { get => _selectedDate; private set => SetProperty(ref _selectedDate, value); }
        public string SelectedAlarmName { get => _selectedAlarmName; private set => SetProperty(ref _selectedAlarmName, value); }
        public string SelectedAlarmDetail { get => _selectedAlarmDetail; private set => SetProperty(ref _selectedAlarmDetail, value); }
        public string Recommendation { get => _recommendation; private set => SetProperty(ref _recommendation, value); }

        private void LoadData()
        {
            if (!TryLoadDatabaseData())
            {
                LoadFallbackData();
            }
        }

        private bool TryLoadDatabaseData()
        {
            if (_db == null)
            {
                return false;
            }

            try
            {
                var start = StartDateTime ?? new DateTime(2026, 7, 2, 8, 0, 0);
                var end = EndDateTime ?? start.AddHours(12);
                if (end <= start)
                {
                    end = start.AddMinutes(1);
                    EndDateTime = end;
                }

                var records = _db.Queryable<HmiAlarmRecord>()
                    .Where(it => it.TriggerTime >= start && it.TriggerTime <= end)
                    .OrderBy(it => it.TriggerTime, OrderByType.Desc)
                    .ToList();

                if (records.Count == 0)
                {
                    LoadEmptyData(start, end);
                    return true;
                }

                var maps = _db.Queryable<HmiAlarmCodeMap>()
                    .Where(it => it.IsEnable)
                    .ToList()
                    .GroupBy(it => it.AlarmCode)
                    .ToDictionary(it => it.Key, it => it.First());

                _scenarios.Clear();
                foreach (var record in records)
                {
                    maps.TryGetValue(record.AlarmCode, out var map);
                    _scenarios.Add(CreateScenarioFromRecord(record, map));
                }

                LoadPeriodStatistics(records);
                SelectScenario(_scenarios.First().Key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SelectAlarm(object? parameter)
        {
            if (parameter is AlarmEventItem item)
            {
                SelectScenario(item.ScenarioKey);
            }
        }

        private void LoadPeriodStatistics(IReadOnlyList<HmiAlarmRecord> records)
        {
            var highCount = records.Count(it => it.AlarmLevel == "高");
            var responseRecords = records.Where(it => it.ResponseSeconds.HasValue).ToList();
            var avgResponseSeconds = responseRecords.Count == 0 ? 0 : (int)responseRecords.Average(it => it.ResponseSeconds!.Value);
            var repeatCount = records
                .GroupBy(it => new { it.DeviceName, it.AlarmCode })
                .Where(it => it.Count() > 1)
                .Sum(it => it.Count());
            var longest = records
                .OrderByDescending(it => it.DurationSeconds ?? 0)
                .FirstOrDefault();

            ReplaceCollection(SummaryItems, new[]
            {
                new AlarmSummaryItem("报警总数", records.Count.ToString(), "当前查询范围"),
                new AlarmSummaryItem("高优先级", highCount.ToString(), $"占比 {GetPercent(highCount, records.Count):0}%"),
                new AlarmSummaryItem("平均响应", FormatDuration(avgResponseSeconds), "首次响应"),
                new AlarmSummaryItem("重复报警", repeatCount.ToString(), "按设备+代码"),
                new AlarmSummaryItem("最长持续", FormatDuration(longest?.DurationSeconds ?? 0), longest?.AlarmName ?? "-")
            });

            var topGroups = records
                .GroupBy(it => new
                {
                    it.AlarmCode,
                    AlarmName = it.AlarmName ?? it.AlarmCode
                })
                .Select(group => new
                {
                    group.Key.AlarmCode,
                    group.Key.AlarmName,
                    Count = group.Count(),
                    LastTriggerTime = group.Max(it => it.TriggerTime)
                })
                .OrderByDescending(it => it.Count)
                .ThenByDescending(it => it.LastTriggerTime)
                .ToList();

            var maxTopCount = topGroups.Count == 0 ? 1 : topGroups.Max(it => it.Count);
            var topItems = topGroups
                .Select((group, index) =>
                {
                    var percent = GetPercent(group.Count, maxTopCount);
                    return new AlarmTopItem((index + 1).ToString(), group.AlarmName, $"{group.Count} 次", Math.Max(12, percent), TopColors[index % TopColors.Length]);
                })
                .ToArray();

            ReplaceCollection(AlarmTopItems, topItems);
            ReplaceCollection(HeatRows, CreateHeatRows(records));
        }

        private void LoadEmptyData(DateTime start, DateTime end)
        {
            _scenarios.Clear();
            AlarmEvents.Clear();
            AlarmTopItems.Clear();
            HeatRows.Clear();
            EvidenceItems.Clear();
            ProcessSteps.Clear();

            SelectedLine = "-";
            SelectedDevice = "-";
            SelectedShift = "-";
            SelectedDate = $"{start:yyyy-MM-dd HH:mm} - {end:yyyy-MM-dd HH:mm}";
            SelectedAlarmName = "暂无报警数据";
            SelectedAlarmDetail = "当前时间范围内没有查询到报警记录";
            Recommendation = "请调整时间范围后重新查询，或确认报警采集服务是否已写入 hmi_alarm_record。";

            ReplaceCollection(SummaryItems, new[]
            {
                new AlarmSummaryItem("报警总数", "0", "当前查询范围"),
                new AlarmSummaryItem("高优先级", "0", "占比 0%"),
                new AlarmSummaryItem("平均响应", "-", "首次响应"),
                new AlarmSummaryItem("重复报警", "0", "按设备+代码"),
                new AlarmSummaryItem("最长持续", "-", "-")
            });
        }

        private void SelectScenario(string key)
        {
            var scenario = _scenarios.FirstOrDefault(it => it.Key == key) ?? _scenarios.First();

            SelectedLine = scenario.Line;
            SelectedDevice = scenario.Device;
            SelectedShift = scenario.Shift;
            SelectedDate = $"{StartDateTime:yyyy-MM-dd HH:mm} - {EndDateTime:yyyy-MM-dd HH:mm}";
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

        private static HmiAlarmScenario CreateScenarioFromRecord(HmiAlarmRecord record, HmiAlarmCodeMap? map)
        {
            var level = record.AlarmLevel ?? map?.AlarmLevel ?? "中";
            var alarmName = record.AlarmName ?? map?.AlarmName ?? record.AlarmCode;
            var color = level switch
            {
                "高" => "#E94855",
                "中" => "#F59E0B",
                "低" => "#F5B23D",
                _ => "#2F66F6"
            };

            var reason = string.IsNullOrWhiteSpace(map?.PossibleReason) ? "未维护报警可能原因。" : map.PossibleReason!;
            var suggestion = string.IsNullOrWhiteSpace(map?.HandleSuggestion) ? "请根据现场现象补充处理建议。" : map.HandleSuggestion!;
            var durationSeconds = record.DurationSeconds ?? GetDurationSeconds(record.TriggerTime, record.RecoverTime);
            var responseSeconds = record.ResponseSeconds ?? 0;

            return new HmiAlarmScenario(
                record.ID.ToString(),
                record.LineName ?? "-",
                record.DeviceName,
                "白班",
                record.TriggerTime.ToString("yyyy-MM-dd"),
                alarmName,
                level,
                record.StationName ?? "-",
                record.TriggerTime.ToString("HH:mm:ss"),
                record.RecoverTime?.ToString("HH:mm:ss") ?? "未恢复",
                FormatDuration(durationSeconds),
                $"{record.ImpactQty ?? 0} pcs",
                color,
                $"报警可能原因：{reason}\r\n处理建议：{suggestion}",
                new[]
                {
                    new AlarmEvidenceItem("报警代码", record.AlarmCode),
                    new AlarmEvidenceItem("报警等级", level),
                    new AlarmEvidenceItem("持续时长", FormatDuration(durationSeconds)),
                    new AlarmEvidenceItem("首次响应", responseSeconds > 0 ? FormatDuration(responseSeconds) : "-"),
                    new AlarmEvidenceItem("关联点位", record.RawValue ?? "-"),
                    new AlarmEvidenceItem("关联设备", record.DeviceName)
                },
                new[]
                {
                    new AlarmProcessStepItem("报警触发", record.TriggerTime.ToString("HH:mm"), "已完成", "#2FAE66"),
                    new AlarmProcessStepItem("班组响应", responseSeconds > 0 ? FormatDuration(responseSeconds) : "待定", responseSeconds > 0 ? "已完成" : "待处理", responseSeconds > 0 ? "#2FAE66" : "#F59E0B"),
                    new AlarmProcessStepItem("报警恢复", record.RecoverTime?.ToString("HH:mm") ?? "未恢复", record.RecoverTime.HasValue ? "已完成" : "进行中", record.RecoverTime.HasValue ? "#2FAE66" : "#F59E0B"),
                    new AlarmProcessStepItem("原因确认", "待定", "待处理", "#637083"),
                    new AlarmProcessStepItem("改善验证", "未开始", "待处理", "#637083")
                });
        }

        private static AlarmHeatRowItem[] CreateHeatRows(IReadOnlyList<HmiAlarmRecord> records)
        {
            var stations = records
                .Select(it => it.StationName)
                .Where(it => !string.IsNullOrWhiteSpace(it))
                .Distinct()
                .Take(6)
                .ToList();

            if (stations.Count == 0)
            {
                stations.Add("未分配");
            }

            return stations.Select(station =>
            {
                var values = new int[7];
                foreach (var record in records.Where(it => it.StationName == station))
                {
                    var hour = record.TriggerTime.Hour;
                    var index = hour switch
                    {
                        < 10 => 0,
                        < 12 => 1,
                        < 14 => 2,
                        < 16 => 3,
                        < 18 => 4,
                        < 20 => 5,
                        _ => 6
                    };
                    values[index]++;
                }

                return new AlarmHeatRowItem(station!, values);
            }).ToArray();
        }

        private void LoadFallbackData()
        {
            _scenarios.Clear();
            var fallback = new[]
            {
                new HmiAlarmRecord { ID = 1, LineName = "L1 密封钉线", DeviceName = "密封钉设备 01", StationName = "激光焊接", ProcessName = "密封钉焊接", AlarmCode = "HMI-LASER-203", AlarmName = "激光器通信异常", AlarmLevel = "高", TriggerTime = new DateTime(2026, 7, 2, 10, 42, 11), RecoverTime = new DateTime(2026, 7, 2, 11, 0, 23), DurationSeconds = 1092, ImpactQty = -320, ResponseSeconds = 128, RawValue = "LaserCommLost" },
                new HmiAlarmRecord { ID = 2, LineName = "L1 密封钉线", DeviceName = "密封钉设备 01", StationName = "扫码工位", ProcessName = "扫码上料", AlarmCode = "HMI-SCAN-104", AlarmName = "条码枪读取超时", AlarmLevel = "中", TriggerTime = new DateTime(2026, 7, 2, 11, 18, 6), RecoverTime = new DateTime(2026, 7, 2, 11, 22, 41), DurationSeconds = 275, ImpactQty = -54, ResponseSeconds = 76, RawValue = "ScannerReadDone" },
                new HmiAlarmRecord { ID = 3, LineName = "L1 密封钉线", DeviceName = "密封钉设备 01", StationName = "点胶工位", ProcessName = "点胶", AlarmCode = "HMI-GLUE-087", AlarmName = "胶压低于下限", AlarmLevel = "中", TriggerTime = new DateTime(2026, 7, 2, 13, 52, 48), RecoverTime = new DateTime(2026, 7, 2, 13, 57, 9), DurationSeconds = 261, ImpactQty = -62, ResponseSeconds = 94, RawValue = "GluePressureLow" }
            };

            foreach (var record in fallback.OrderByDescending(it => it.TriggerTime))
            {
                _scenarios.Add(CreateScenarioFromRecord(record, null));
            }

            LoadPeriodStatistics(fallback);
            SelectScenario(_scenarios.First().Key);
        }

        private static int GetDurationSeconds(DateTime triggerTime, DateTime? recoverTime)
        {
            return recoverTime.HasValue ? Math.Max(0, (int)(recoverTime.Value - triggerTime).TotalSeconds) : 0;
        }

        private static double GetPercent(int value, int total)
        {
            return total <= 0 ? 0 : value * 100.0 / total;
        }

        private static string FormatDuration(int seconds)
        {
            if (seconds <= 0)
            {
                return "-";
            }

            return seconds >= 60 ? $"{seconds / 60}m{seconds % 60:00}s" : $"{seconds}s";
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
