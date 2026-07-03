using SUNWODA_SEVB.Component.CustomControls;
using SUNWODA_SEVB.Component.UserControls;
using SUNWODA_SEVB.Core.Attributes;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Core.Common.Commands;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Interfaces.Data;
using SUNWODA_SEVB.Core.Interfaces.PLC;
using SUNWODA_SEVB.Core.Models.PLC;
using SUNWODA_SEVB.PLC;
using SUNWODA_SEVB.ViewModels.Windows.Common;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace SUNWODA_SEVB.ViewModels.Pages.Demo
{
    [Module("AIMarkdownViewerTestPage", "AI Markdown控件演示")]
    public class VM_AIMarkdownViewerTestPage : ViewModelBase
    {
        private readonly ILoggerService<VM_MainWindow> _logger;
        private readonly INavigationService _navigationService;
        private readonly IModuleManager _moduleManager;
        private readonly IPLCService _plcService;
        private readonly IPLCAddressConfigRepository _plcAddressConfigRepository;      
        private string? _markdownContent;
        private string? _thinkingContent;
        private string? _pendingWriteValue;
        private bool _isStreaming;
        private bool _initializedView = false;
        private ConnectInfo? _selectedConnection;
        private PLCRWAddress? _selectedAddress;
        private readonly DispatcherTimer _plcRefreshTimer;
        private readonly Queue<double> _selectedAddressHistory = new();
        private PointCollection _trendPoints = new();
        private string _trendStatusText = "请选择数值型地址参数以查看实时曲线";
        private double _trendMinValue;
        private double _trendMaxValue;
        private double _trendCurrentValue;
        private AiMarkdownViewer? _markdownViewer;

        private const int TrendCapacity = 60;
        private const double TrendWidth = 360;
        private const double TrendHeight = 180;

        #region 属性

        public string? MarkdownContent
        {
            get => _markdownContent;
            set => SetProperty(ref _markdownContent, value);
        }

        public string? ThinkingContent
        {
            get => _thinkingContent;
            set => SetProperty(ref _thinkingContent, value);
        }

        public bool IsStreaming
        {
            get => _isStreaming;
            set => SetProperty(ref _isStreaming, value);
        }

        public string? PendingWriteValue
        {
            get => _pendingWriteValue;
            set => SetProperty(ref _pendingWriteValue, value);
        }

        public ConnectInfo? SelectedConnection
        {
            get => _selectedConnection;
            set => SetProperty(ref _selectedConnection, value);
        }

        public PLCRWAddress? SelectedAddress
        {
            get => _selectedAddress;
            set
            {
                if (!ReferenceEquals(_selectedAddress, value))
                {
                    if (_selectedAddress != null)
                    {
                        _selectedAddress.PropertyChanged -= SelectedAddress_PropertyChanged;
                    }

                    if (SetProperty(ref _selectedAddress, value) && value != null)
                    {
                        _selectedAddress.PropertyChanged += SelectedAddress_PropertyChanged;
                        PendingWriteValue = value.MonitorValue?.ToString();                    
                     
                    }
                }
            }
        }

        public ObservableCollection<ConnectInfo> ConnectionItems { get; } = new();

        public ObservableCollection<PLCRWAddress> AddressItems { get; } = new();

        public PointCollection TrendPoints
        {
            get => _trendPoints;
            set => SetProperty(ref _trendPoints, value);
        }

        public string TrendStatusText
        {
            get => _trendStatusText;
            set => SetProperty(ref _trendStatusText, value);
        }

        public double TrendMinValue
        {
            get => _trendMinValue;
            set => SetProperty(ref _trendMinValue, value);
        }

        public double TrendMaxValue
        {
            get => _trendMaxValue;
            set => SetProperty(ref _trendMaxValue, value);
        }

        public double TrendCurrentValue
        {
            get => _trendCurrentValue;
            set => SetProperty(ref _trendCurrentValue, value);
        }

        #endregion

        #region 命令

        public ICommand SimulateAiResponseCommand { get; }
        public ICommand SimulateThinkingCommand { get; }
        public ICommand TestMarkdownCommand { get; }
        public ICommand ClearAllCommand { get; }
        public ICommand RefreshPlcDataCommand { get; }
        public ICommand WriteSelectedValueCommand { get; }
        public ICommand WriteTestValueCommand { get; }

        #endregion

        public VM_AIMarkdownViewerTestPage(
            ILoggerService<VM_MainWindow> logger,
            INavigationService navigationService,
            IModuleManager moduleManager, IPLCService plcService,
            IPLCAddressConfigRepository plcAddressConfigRepository)
        {
            _logger = logger;
             _plcService = plcService;
            _navigationService = navigationService;
            _moduleManager = moduleManager;
            // 初始化命令
            SimulateAiResponseCommand = new RelayCommand(async () => await SimulateAiResponse());
            SimulateThinkingCommand = new RelayCommand(async () => await SimulateThinking());
            TestMarkdownCommand = new RelayCommand(TestMarkdownFeatures);
            ClearAllCommand = new RelayCommand(ClearAll);
            RefreshPlcDataCommand = new RelayCommand(RefreshPlcData);
            WriteSelectedValueCommand = new RelayCommand(async () => await WriteSelectedValueAsync());
            WriteTestValueCommand = new RelayCommand(async () => await WriteSelectedTestValueAsync());
            _plcAddressConfigRepository =
               plcAddressConfigRepository
               ?? throw new ArgumentNullException(nameof(plcAddressConfigRepository));

            _plcRefreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _plcRefreshTimer.Tick += (_, _) => RefreshPlcData();
        }    
        protected override async Task OnNavigatedToAsync(object? parameter)
        {
            if (!_initializedView)
            {
                var moduleName = typeof(VM_AIMarkdownViewerTestPage).GetCustomAttribute<ModuleAttribute>()?.Name;
                if (moduleName != null)
                {
                    var view = _moduleManager.GetViewFromService(moduleName);

                    await RunOnUIThreadAsync(() =>
                    {
                        _markdownViewer = view?.FindName("MarkdownViewer") as AiMarkdownViewer;
                    });
                }
                _initializedView = true;
            }

            await base.OnNavigatedToAsync(parameter);
            RefreshPlcData();
            if (!_plcRefreshTimer.IsEnabled)
            {
                _plcRefreshTimer.Start();
            }
        }

        protected override async Task OnNavigatedFromAsync()
        {
            if (_plcRefreshTimer.IsEnabled)
            {
                _plcRefreshTimer.Stop();
            }

            await base.OnNavigatedFromAsync();
        }

        #region 方法

        /// <summary>
        /// 模拟AI流式响应
        /// </summary>
        private async Task SimulateAiResponse()
        {
            var connectedPlc = _plcService.ConnectionStatus.FirstOrDefault(x => x.Value.Status);
            if (connectedPlc.Value == null)
            {
                _logger.Warn("当前没有已连接的PLC，跳过PLC读写测试", true);
            }
            else
            {
                _logger.Info($"PLC {connectedPlc.Key} 已连接，开始执行读写测试");
            }

            var runtimeAddress = _plcService.RWAddresses.Values.FirstOrDefault();
            if (runtimeAddress == null)
            {
                _logger.Warn("当前没有已加载的PLC地址参数，跳过PLC测试", true);
            }
            else
            {
                _logger.Info(
                    $"读取PLC地址参数: {runtimeAddress.ParameterName}({runtimeAddress.Address}) = {runtimeAddress.MonitorValue}"
                );

                var addressConfig = (
                    await _plcAddressConfigRepository.GetMonitorAddressesAsync(runtimeAddress.PlcId)
                ).FirstOrDefault(x => x.ID == runtimeAddress.ID);
                if (addressConfig != null)
                {
                    _logger.Info(
                        $"地址配置已命中: ID={addressConfig.ID}, 类型={addressConfig.Type}, 地址={addressConfig.Address}"
                    );
                }

                var testValue = BuildTestValue(runtimeAddress);
                var success = await _plcService.WriteValueAsync(runtimeAddress.ID, testValue);
                if (success)
                {
                    PendingWriteValue = testValue.ToString();
                    _logger.Info(
                        $"PLC测试写入成功: {runtimeAddress.ParameterName}({runtimeAddress.Address}) <= {testValue}"
                    );
                }
            }

            RefreshPlcData();

            IsStreaming = true;
            string aiResponse =
                @"# AI响应示例

我正在为您生成一个**详细的响应**。这个响应包含了多种Markdown元素。

## 主要特性

1. **实时渲染** - 支持Markdown的实时渲染
2. *流式输出* - 模拟AI的逐字输出效果
3. `代码高亮` - 支持内联代码和代码块

### 代码示例

```csharp
public class Example
{
    public void HelloWorld()
    {
        Console.WriteLine(""Hello, World!"");
    }
}
```

## 引用示例

> 这是一个引用块，通常用于引用其他来源的内容。
> 可以包含多行文本。

## 列表示例

### 无序列表
- 第一项
- 第二项
  - 子项目1
  - 子项目2
- 第三项

### 有序列表
1. 步骤一
2. 步骤二
3. 步骤三

## 链接和图片

访问 [百度](https://www.baidu.com) 获取更多信息。";

            // 模拟流式输出
            var helper = new StreamingHelper(
                _markdownViewer ?? throw new ArgumentNullException(nameof(_markdownViewer))
            );
            foreach (char c in aiResponse)
            {
                helper.AppendToken(c.ToString());
                await Task.Delay(10); // 模拟网络延迟
            }

            helper.Complete();
            IsStreaming = false;
        }

        /// <summary>
        /// 模拟深度思考
        /// </summary>
        private async Task SimulateThinking()
        {
            // 清空之前的思考内容
            ClearAll();

            // 逐步添加思考内容
            ThinkingContent = "🤔 正在分析您的问题...\n";
            await Task.Delay(800);

            ThinkingContent += "📚 搜索相关知识库...\n";
            await Task.Delay(800);

            ThinkingContent += "🔗 构建逻辑链条...\n";
            await Task.Delay(800);

            ThinkingContent += "💡 生成响应方案...\n";
            await Task.Delay(800);

            ThinkingContent += "✨ 优化输出内容...\n";
            await Task.Delay(1000);

            ThinkingContent += "\n✅ 思考完成！开始生成响应...\n";

            // 开始输出响应，但保留思考内容
            await SimulateAiResponse();
        }

        /// <summary>
        /// 清空思考内容
        /// </summary>
        private void ClearAll()
        {
            IsStreaming = false;
            // 清空思考内容
            ThinkingContent = "";
            MarkdownContent = "";
            _markdownViewer?.ClearMarkdownContent();
        }

        private void RefreshPlcData()
        {
            var selectedConnectionName = SelectedConnection?.Name;
            var selectedAddressId = SelectedAddress?.ID;

            ConnectionItems.Clear();
            foreach (var connection in _plcService.ConnectionStatus.Values.OrderBy(x => x.Name))
            {
                ConnectionItems.Add(connection);
            }

            AddressItems.Clear();
            foreach (var address in _plcService.RWAddresses.Values.OrderBy(x => x.PlcId).ThenBy(x => x.Address))
            {
                AddressItems.Add(address);
            }

            SelectedConnection =
                ConnectionItems.FirstOrDefault(x => x.Name == selectedConnectionName)
                ?? ConnectionItems.FirstOrDefault();

            var currentSelectedAddress =
                AddressItems.FirstOrDefault(x => x.ID == selectedAddressId)
                ?? AddressItems.FirstOrDefault();

            if (!ReferenceEquals(SelectedAddress, currentSelectedAddress))
            {
                SelectedAddress = currentSelectedAddress;
            }
            else if (SelectedAddress != null)
            {
               
            }
        }

        private async Task WriteSelectedValueAsync()
        {
            if (SelectedAddress == null)
            {
                _logger.Warn("请先选择一个PLC地址", true);
                return;
            }

            try
            {
                var value = ConvertInputToAddressValue(SelectedAddress, PendingWriteValue);
                var success = await _plcService.WriteValueAsync(SelectedAddress.ID, value);
                if (success)
                {
                    _logger.Info(
                        $"手动写入成功: {SelectedAddress.ParameterName}({SelectedAddress.Address}) <= {value}"
                    );
                    PendingWriteValue = value.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("手动写入PLC失败", ex, true);
            }
        }

        private async Task WriteSelectedTestValueAsync()
        {
            if (SelectedAddress == null)
            {
                _logger.Warn("请先选择一个PLC地址", true);
                return;
            }

            var testValue = BuildTestValue(SelectedAddress);
            var success = await _plcService.WriteValueAsync(SelectedAddress.ID, testValue);
            if (success)
            {
                PendingWriteValue = testValue.ToString();
                _logger.Info(
                    $"测试值写入成功: {SelectedAddress.ParameterName}({SelectedAddress.Address}) <= {testValue}"
                );
            }
        }

        /// <summary>
        /// 测试Markdown功能
        /// </summary>
        private void TestMarkdownFeatures()
        {
            //ClearAll();

            // 等待一帧，确保UI更新
            //await Task.Delay(50);

            MarkdownContent =
                @"# Markdown功能测试

## 文本格式化

这是**粗体文本**，这是*斜体文本*。

这是`内联代码`，适合标记变量名或短代码。

## 代码块

```csharp
public class Example
{
    public void HelloWorld()
    {
        Console.WriteLine(""Hello, World!"");
    }
}
```

## 任务列表（扩展功能）

- [x] 实现基础Markdown渲染
- [x] 支持流式输出
- [x] 添加深度思考显示
- [ ] 支持数学公式
- [ ] 支持表格渲染
- [ ] 支持Mermaid图表

> 这是一个引用";
        }

        private static object BuildTestValue(PLCRWAddress address)
        {
            return address.Type?.ToUpperInvariant() switch
            {
                "BOOL" => true,
                "BYTE" => (byte)1,
                "SHORT" => (short)12,
                "USHORT" => (ushort)12,
                "INT" => 123,
                "UINT" => (uint)123,
                "LONG" => (long)123,
                "ULONG" => (ulong)123,
                "DOUBLE" => 123.12d,
                "FLOAT" => 123.12f,
                _ => "TEST",
            };
        }

        private static object ConvertInputToAddressValue(PLCRWAddress address, string? rawValue)
        {
            var valueText = rawValue?.Trim() ?? string.Empty;

            return address.Type?.ToUpperInvariant() switch
            {
                "BOOL" => bool.Parse(valueText),
                "BYTE" => byte.Parse(valueText),
                "SHORT" => short.Parse(valueText),
                "USHORT" => ushort.Parse(valueText),
                "INT" => int.Parse(valueText),
                "UINT" => uint.Parse(valueText),
                "LONG" => long.Parse(valueText),
                "ULONG" => ulong.Parse(valueText),
                "DOUBLE" => double.Parse(valueText),
                "FLOAT" => float.Parse(valueText),
                _ => valueText,
            };
        }

        private void ResetTrendHistory()
        {
            _selectedAddressHistory.Clear();
            TrendPoints = new PointCollection();
            TrendMinValue = 0;
            TrendMaxValue = 0;
            TrendCurrentValue = 0;
            TrendStatusText = SelectedAddress == null
                ? "请选择数值型地址参数以查看实时曲线"
                : $"等待采样: {SelectedAddress.ParameterName}";
        }
      
        private void RebuildTrendPoints()
        {
            if (_selectedAddressHistory.Count == 0)
            {
                TrendPoints = new PointCollection();
                return;
            }

            var values = _selectedAddressHistory.ToArray();
            var min = values.Min();
            var max = values.Max();
            var range = Math.Abs(max - min) < 0.000001 ? 1 : max - min;
            var stepX = values.Length <= 1 ? 0 : TrendWidth / (values.Length - 1);
            var points = new PointCollection();

            for (var i = 0; i < values.Length; i++)
            {
                var normalized = (values[i] - min) / range;
                var x = i * stepX;
                var y = TrendHeight - normalized * TrendHeight;
                points.Add(new Point(x, y));
            }

            TrendPoints = points;
        }

        private static bool TryConvertToDouble(object? rawValue, out double value)
        {
            value = 0;
            if (rawValue == null)
            {
                return false;
            }

            try
            {
                value = rawValue switch
                {
                    bool boolValue => boolValue ? 1 : 0,
                    byte byteValue => byteValue,
                    short shortValue => shortValue,
                    ushort ushortValue => ushortValue,
                    int intValue => intValue,
                    uint uintValue => uintValue,
                    long longValue => longValue,
                    ulong ulongValue => ulongValue,
                    float floatValue => floatValue,
                    double doubleValue => doubleValue,
                    decimal decimalValue => (double)decimalValue,
                    _ => Convert.ToDouble(rawValue),
                };
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SelectedAddress_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PLCRWAddress.MonitorValue) && sender is PLCRWAddress address)
            {
                PendingWriteValue = address.MonitorValue?.ToString();
            }
        }
        #endregion
    }
}
