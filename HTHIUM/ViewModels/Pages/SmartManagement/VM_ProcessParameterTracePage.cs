using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using HTHIUM.Core.Attributes;
using HTHIUM.Core.Common;
using HTHIUM.Core.Common.Commands;

namespace HTHIUM.ViewModels.Pages.SmartManagement
{
    [Module("ProcessParameterTracePage", "过程参数曲线追溯", Category = "设备智慧管理", Order = 28)]
    public class VM_ProcessParameterTracePage : ViewModelBase
    {
        private readonly List<WeldProcessScenario> _scenarios = new();
        private string _lineName = "L1 密封钉线";
        private string _stationName = "焊接工位 01";
        private string _selectedDevice = "激光焊接设备 01";
        private string _selectedParameter = "激光功率";
        private string _resultFilter = "全部";
        private DateTime? _startDateTime = new DateTime(2026, 7, 7, 8, 0, 0);
        private DateTime? _endDateTime = new DateTime(2026, 7, 7, 20, 0, 0);
        private string _productCode = "P20260707002";
        private string _result = "NG";
        private string _diagnosis = "稳定焊接段功率波动偏大，建议复核激光器输出稳定性、光路污染和焊接配方窗口。";
        private string _avgPower = "1482 W";
        private string _peakPower = "1588 W";
        private string _powerRange = "±7.4%";
        private string _overLimit = "12 点";
        private string _chartTitle = "激光功率过程曲线";
        private string _chartSubtitle = "蓝线为实际功率，灰线为设定功率，红色虚线为上下限窗口";
        private string _rangeLabel = "功率范围：1200W - 1700W";

        public VM_ProcessParameterTracePage()
        {
            RefreshCommand = new RelayCommand(LoadMockData);
            SelectRecordCommand = new RelayCommand<object?>(SelectRecord);
            ExportCommand = new RelayCommand(() => { });

            ReplaceCollection(DeviceOptions, new[]
            {
                "激光焊接设备 01",
                "点胶设备 01",
                "热压设备 01",
                "拧紧设备 01"
            });
            ReplaceCollection(ResultOptions, new[] { "全部", "OK", "NG" });
            ResetParameterOptions();
            LoadMockData();
        }

        public ObservableCollection<string> DeviceOptions { get; } = new();
        public ObservableCollection<string> ParameterOptions { get; } = new();
        public ObservableCollection<string> ResultOptions { get; } = new();
        public ObservableCollection<WeldProcessRecordItem> Records { get; } = new();
        public ObservableCollection<ProcessMetricItem> Metrics { get; } = new();
        public ObservableCollection<ProcessStageItem> Stages { get; } = new();
        public ObservableCollection<ProcessPointMarkerItem> Markers { get; } = new();
        public ObservableCollection<ProcessLegendItem> Legends { get; } = new();

        public ICommand RefreshCommand { get; }
        public ICommand SelectRecordCommand { get; }
        public ICommand ExportCommand { get; }

        public string LineName { get => _lineName; private set => SetProperty(ref _lineName, value); }
        public string StationName { get => _stationName; private set => SetProperty(ref _stationName, value); }
        public string ResultFilter { get => _resultFilter; set => SetProperty(ref _resultFilter, value); }
        public DateTime? StartDateTime { get => _startDateTime; set => SetProperty(ref _startDateTime, value); }
        public DateTime? EndDateTime { get => _endDateTime; set => SetProperty(ref _endDateTime, value); }
        public string ProductCode { get => _productCode; private set => SetProperty(ref _productCode, value); }
        public string Result { get => _result; private set => SetProperty(ref _result, value); }
        public string Diagnosis { get => _diagnosis; private set => SetProperty(ref _diagnosis, value); }
        public string AvgPower { get => _avgPower; private set => SetProperty(ref _avgPower, value); }
        public string PeakPower { get => _peakPower; private set => SetProperty(ref _peakPower, value); }
        public string PowerRange { get => _powerRange; private set => SetProperty(ref _powerRange, value); }
        public string OverLimit { get => _overLimit; private set => SetProperty(ref _overLimit, value); }
        public string ChartTitle { get => _chartTitle; private set => SetProperty(ref _chartTitle, value); }
        public string ChartSubtitle { get => _chartSubtitle; private set => SetProperty(ref _chartSubtitle, value); }
        public string RangeLabel { get => _rangeLabel; private set => SetProperty(ref _rangeLabel, value); }

        public string SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                if (SetProperty(ref _selectedDevice, value))
                {
                    ResetParameterOptions();
                }
            }
        }

        public string SelectedParameter
        {
            get => _selectedParameter;
            set => SetProperty(ref _selectedParameter, value);
        }

        public PointCollection SetPowerPoints { get; } = new();
        public PointCollection ActualPowerPoints { get; } = new();
        public PointCollection UpperLimitPoints { get; } = new();
        public PointCollection LowerLimitPoints { get; } = new();

        private void ResetParameterOptions()
        {
            var options = SelectedDevice switch
            {
                "点胶设备 01" => new[] { "点胶压力", "胶量流速", "针头高度" },
                "热压设备 01" => new[] { "热压温度", "热压压力", "保压位移" },
                "拧紧设备 01" => new[] { "拧紧扭矩", "拧紧转速", "角度曲线" },
                _ => new[] { "激光功率", "焊接速度", "保护气流量" }
            };

            ReplaceCollection(ParameterOptions, options);
            SelectedParameter = options.Contains(SelectedParameter) ? SelectedParameter : options[0];
        }

        private void LoadMockData()
        {
            var profile = BuildProfile(SelectedDevice, SelectedParameter);
            LineName = profile.LineName;
            StationName = profile.StationName;
            ChartTitle = profile.ChartTitle;
            ChartSubtitle = profile.ChartSubtitle;
            RangeLabel = profile.RangeLabel;

            _scenarios.Clear();
            foreach (var scenario in CreateScenarios(profile))
            {
                _scenarios.Add(scenario);
            }

            ReplaceCollection(Legends, new[]
            {
                new ProcessLegendItem($"设定{profile.ShortName}", "#7F92AB"),
                new ProcessLegendItem($"实际{profile.ShortName}", "#2F66F6"),
                new ProcessLegendItem("上下限", "#E94855"),
                new ProcessLegendItem("异常点", "#E94855")
            });

            SelectScenario(_scenarios.First().Key);
        }

        private void SelectRecord(object? parameter)
        {
            if (parameter is WeldProcessRecordItem item)
            {
                SelectScenario(item.Key);
            }
        }

        private void SelectScenario(string key)
        {
            var scenario = _scenarios.FirstOrDefault(it => it.Key == key) ?? _scenarios.First();
            ProductCode = scenario.ProductCode;
            Result = scenario.Result;
            Diagnosis = scenario.Diagnosis;
            AvgPower = $"{scenario.AvgPower} {scenario.Unit}";
            PeakPower = $"{scenario.PeakPower} {scenario.Unit}";
            PowerRange = scenario.PowerRange;
            OverLimit = scenario.OverLimit;

            ReplacePoints(SetPowerPoints, scenario.SetPoints);
            ReplacePoints(ActualPowerPoints, scenario.ActualPoints);
            ReplacePoints(UpperLimitPoints, scenario.UpperLimitPoints);
            ReplacePoints(LowerLimitPoints, scenario.LowerLimitPoints);
            ReplaceCollection(Markers, scenario.Markers);
            ReplaceCollection(Metrics, new[]
            {
                new ProcessMetricItem(scenario.DurationName, scenario.Duration, "#2F66F6"),
                new ProcessMetricItem($"{scenario.ShortName}均值", AvgPower, "#172033"),
                new ProcessMetricItem($"{scenario.ShortName}峰值", PeakPower, "#172033"),
                new ProcessMetricItem("波动范围", PowerRange, scenario.Result == "OK" ? "#23A862" : "#E94855"),
                new ProcessMetricItem("超限点数", OverLimit, scenario.OverLimit.StartsWith("0") ? "#23A862" : "#E94855"),
                new ProcessMetricItem("判定结果", Result, scenario.Color)
            });
            ReplaceCollection(Stages, scenario.Stages);

            Records.Clear();
            foreach (var item in _scenarios)
            {
                Records.Add(new WeldProcessRecordItem(item.Key, item.Time, item.ProductCode, item.BatchNo, item.Result, item.State, item.Color, item.Key == scenario.Key));
            }
        }

        private static DeviceProcessProfile BuildProfile(string deviceName, string parameterName)
        {
            return deviceName switch
            {
                "点胶设备 01" => new DeviceProcessProfile(
                    "L2 顶盖点胶线",
                    "点胶工位 02",
                    parameterName,
                    parameterName.Replace("点胶", string.Empty),
                    "点胶压力过程曲线",
                    "蓝线为实际压力，灰线为设定压力，红色虚线为工艺上下限",
                    "压力范围：180kPa - 360kPa",
                    "kPa",
                    "点胶时长",
                    "2.4s",
                    180,
                    360,
                    260,
                    22,
                    "胶线",
                    "供胶段",
                    "稳定点胶段",
                    "断胶段",
                    "P"),
                "热压设备 01" => new DeviceProcessProfile(
                    "L3 模组热压线",
                    "热压工位 01",
                    parameterName,
                    parameterName.Replace("热压", string.Empty),
                    "热压温度过程曲线",
                    "蓝线为实际温度，灰线为设定温度，红色虚线为上下限窗口",
                    "温度范围：120℃ - 210℃",
                    "℃",
                    "热压时长",
                    "6.0s",
                    120,
                    210,
                    175,
                    8,
                    "产品",
                    "升温段",
                    "保温段",
                    "降温段",
                    "H"),
                "拧紧设备 01" => new DeviceProcessProfile(
                    "L4 PACK 装配线",
                    "拧紧工位 03",
                    parameterName,
                    parameterName.Replace("拧紧", string.Empty),
                    "拧紧扭矩过程曲线",
                    "蓝线为实际扭矩，灰线为设定扭矩，红色虚线为工艺窗口",
                    "扭矩范围：0N·m - 12N·m",
                    "N·m",
                    "拧紧时长",
                    "1.2s",
                    0,
                    12,
                    8,
                    0.8,
                    "螺栓",
                    "寻帽段",
                    "升扭段",
                    "锁付段",
                    "T"),
                _ => new DeviceProcessProfile(
                    "L1 密封钉线",
                    "焊接工位 01",
                    parameterName,
                    parameterName.Replace("激光", string.Empty),
                    "激光功率过程曲线",
                    "蓝线为实际功率，灰线为设定功率，红色虚线为上下限窗口",
                    "功率范围：1200W - 1700W",
                    "W",
                    "焊接时长",
                    "1.8s",
                    1200,
                    1700,
                    1500,
                    70,
                    "焊缝",
                    "预热段",
                    "稳定焊接段",
                    "收尾段",
                    "P")
            };
        }

        private static IReadOnlyList<WeldProcessScenario> CreateScenarios(DeviceProcessProfile profile)
        {
            var abnormalA = profile.ShortName switch
            {
                "压力" => "压力波动",
                "温度" => "温度波动",
                "扭矩" => "扭矩波动",
                _ => "功率波动"
            };
            var abnormalB = profile.ShortName switch
            {
                "压力" => "压力偏低",
                "温度" => "温度偏低",
                "扭矩" => "扭矩偏高",
                _ => "功率偏低"
            };
            var abnormalC = profile.ShortName switch
            {
                "压力" => "断胶异常",
                "温度" => "降温异常",
                "扭矩" => "角度异常",
                _ => "收尾异常"
            };

            return new[]
            {
                CreateScenario(profile, "10:42:11", $"{profile.CodePrefix}20260707002", "B20260707-01", "NG", abnormalA, "#E94855", -18, 72, "±7.4%", "12 点", BuildDiagnosis(profile, abnormalA)),
                CreateScenario(profile, "10:44:18", $"{profile.CodePrefix}20260707003", "B20260707-01", "OK", "稳定", "#23A862", 0, 28, "±1.8%", "0 点", $"{profile.ShortName}曲线与设定值贴合，关键工艺阶段均正常。"),
                CreateScenario(profile, "10:46:36", $"{profile.CodePrefix}20260707004", "B20260707-01", "NG", abnormalB, "#F59E0B", -92, -40, "低于下限", "18 点", BuildDiagnosis(profile, abnormalB)),
                CreateScenario(profile, "10:49:02", $"{profile.CodePrefix}20260707005", "B20260707-01", "OK", "稳定", "#23A862", -2, 31, "±2.1%", "0 点", $"{profile.ShortName}过程稳定，波动处于工艺窗口内。"),
                CreateScenario(profile, "10:51:24", $"{profile.CodePrefix}20260707006", "B20260707-01", "NG", abnormalC, "#E94855", -30, 56, "末段异常", "7 点", BuildDiagnosis(profile, abnormalC))
            };
        }

        private static string BuildDiagnosis(DeviceProcessProfile profile, string state)
        {
            if (state.Contains("波动"))
            {
                return $"{profile.Stage2}{profile.ShortName}波动偏大，建议复核设备输出稳定性、工艺配方窗口和传感器采集链路。";
            }

            if (state.Contains("偏低"))
            {
                return $"{profile.Stage2}实际{profile.ShortName}持续低于下限，建议检查供给单元、执行机构和参数配方。";
            }

            if (state.Contains("偏高"))
            {
                return $"{profile.Stage2}实际{profile.ShortName}超过上限，建议检查夹具定位、锁付阻力和参数配方。";
            }

            return $"{profile.Stage3}出现异常变化，建议回放过程曲线并对照 PLC 状态与报警记录。";
        }

        private static WeldProcessScenario CreateScenario(DeviceProcessProfile profile, string time, string productCode, string batchNo, string result, string state, string color, double avgOffset, double peakOffset, string range, string overLimit, string diagnosis)
        {
            var key = $"{profile.ChartTitle}-{productCode}";
            var set = new List<Point>();
            var actual = new List<Point>();
            var upper = new List<Point>();
            var lower = new List<Point>();
            var markers = new List<ProcessPointMarkerItem>();
            const double left = 0;
            const double width = 620;
            const double height = 250;

            double ScaleY(double value) => height - (value - profile.MinValue) / (profile.MaxValue - profile.MinValue) * height;

            for (var i = 0; i < 46; i++)
            {
                var t = i / 45.0;
                var x = left + t * width;
                var setValue = t < 0.16
                    ? profile.NominalValue - profile.LimitOffset * 3 + t / 0.16 * profile.LimitOffset * 3
                    : t > 0.84
                        ? profile.NominalValue - (t - 0.84) / 0.16 * profile.LimitOffset * 2.8
                        : profile.NominalValue;
                var actualValue = setValue + Math.Sin(i * 0.72) * profile.LimitOffset * 0.18;

                if (state.Contains("波动") && t > 0.34 && t < 0.7)
                {
                    actualValue += Math.Sin(i * 1.55) * profile.LimitOffset * 1.25;
                }
                else if (state.Contains("偏低") && t > 0.28 && t < 0.76)
                {
                    actualValue -= profile.LimitOffset * 1.65 + Math.Sin(i) * profile.LimitOffset * 0.22;
                }
                else if (state.Contains("偏高") && t > 0.28 && t < 0.76)
                {
                    actualValue += profile.LimitOffset * 1.65 + Math.Sin(i) * profile.LimitOffset * 0.22;
                }
                else if ((state.Contains("异常") || state.Contains("断胶")) && t > 0.78)
                {
                    actualValue -= (t - 0.78) * profile.LimitOffset * 7.3;
                }

                set.Add(new Point(x, ScaleY(setValue)));
                actual.Add(new Point(x, ScaleY(actualValue)));
                upper.Add(new Point(x, ScaleY(setValue + profile.LimitOffset)));
                lower.Add(new Point(x, ScaleY(setValue - profile.LimitOffset)));

                if (actualValue > setValue + profile.LimitOffset || actualValue < setValue - profile.LimitOffset)
                {
                    markers.Add(new ProcessPointMarkerItem(x - 4, ScaleY(actualValue) - 4, "#E94855"));
                }
            }

            var stages = BuildStages(profile, state);
            var avg = Math.Round(profile.NominalValue + avgOffset, 1);
            var peak = Math.Round(profile.NominalValue + peakOffset, 1);

            return new WeldProcessScenario(key, time, productCode, batchNo, result, state, color, avg, peak, range, overLimit, diagnosis, profile.Unit, profile.ShortName, profile.DurationName, profile.Duration, new PointCollection(set), new PointCollection(actual), new PointCollection(upper), new PointCollection(lower), markers, stages);
        }

        private static IReadOnlyList<ProcessStageItem> BuildStages(DeviceProcessProfile profile, string state)
        {
            var stage1 = new ProcessStageItem(profile.Stage1, "0% - 16%", "正常", "#23A862");
            var stage2 = new ProcessStageItem(profile.Stage2, "16% - 84%", "正常", "#23A862");
            var stage3 = new ProcessStageItem(profile.Stage3, "84% - 100%", "正常", "#23A862");

            if (state.Contains("波动") || state.Contains("偏低") || state.Contains("偏高"))
            {
                stage2 = stage2 with { State = state, Color = "#E94855" };
            }
            else if (state.Contains("异常") || state.Contains("断胶"))
            {
                stage3 = stage3 with { State = state, Color = "#E94855" };
            }

            return new[] { stage1, stage2, stage3 };
        }

        private static void ReplacePoints(PointCollection target, PointCollection source)
        {
            target.Clear();
            foreach (var point in source)
            {
                target.Add(point);
            }
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

    public record DeviceProcessProfile(
        string LineName,
        string StationName,
        string ParameterName,
        string ShortName,
        string ChartTitle,
        string ChartSubtitle,
        string RangeLabel,
        string Unit,
        string DurationName,
        string Duration,
        double MinValue,
        double MaxValue,
        double NominalValue,
        double LimitOffset,
        string ProductName,
        string Stage1,
        string Stage2,
        string Stage3,
        string CodePrefix);

    public record WeldProcessScenario(
        string Key,
        string Time,
        string ProductCode,
        string BatchNo,
        string Result,
        string State,
        string Color,
        double AvgPower,
        double PeakPower,
        string PowerRange,
        string OverLimit,
        string Diagnosis,
        string Unit,
        string ShortName,
        string DurationName,
        string Duration,
        PointCollection SetPoints,
        PointCollection ActualPoints,
        PointCollection UpperLimitPoints,
        PointCollection LowerLimitPoints,
        IReadOnlyList<ProcessPointMarkerItem> Markers,
        IReadOnlyList<ProcessStageItem> Stages);

    public record WeldProcessRecordItem(string Key, string Time, string ProductCode, string BatchNo, string Result, string State, string Color, bool IsSelected)
    {
        public string Background => IsSelected ? "#EEF4FF" : "White";
        public string BorderColor => IsSelected ? Color : "#E3EAF4";
    }

    public record ProcessMetricItem(string Name, string Value, string Color);

    public record ProcessStageItem(string Name, string TimeRange, string State, string Color);

    public record ProcessLegendItem(string Name, string Color);

    public record ProcessPointMarkerItem(double X, double Y, string Color);
}
