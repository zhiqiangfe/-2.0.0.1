using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Core.Common.Commands;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Interfaces.MES;
using SUNWODA_SEVB.Core.Models.MES;
using SUNWODA_SEVB.Logging;
using SUNWODA_SEVB.MES.Services;
using SUNWODA_SEVB.MES.Models;
using Microsoft.Extensions.DependencyInjection;

namespace SUNWODA_SEVB.ViewModels
{
    public class MainWindowViewModel : ModelBase
    {
        #region Fields
        private readonly ILoggerService<MainWindowViewModel> _logger;
        private readonly IMesService _mesService;
        private readonly IMesConfigurationProvider _configProvider;
        private readonly IServiceProvider _serviceProvider;

        private string _statusMessage = "就绪";
        private string _logMessage = "";
        private string _productSn = "";
        private string _testResult = "PASS";
        private bool _isBusy = false;
        private string _mesConfigInfo = "";
        private ObservableCollection<string> _testDataLog;
        #endregion

        #region Constructor
        public MainWindowViewModel(
            ILoggerService<MainWindowViewModel> logger,
            IMesService mesService,
            IMesConfigurationProvider configProvider,
            IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mesService = mesService ?? throw new ArgumentNullException(nameof(mesService));
            _configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            _testDataLog = new ObservableCollection<string>();

            _logger.Info("MainWindowViewModel 已初始化");

            // 初始化命令
            TestLogCommand = new RelayCommand(ExecuteTestLog);
            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
            ShowWarningCommand = new RelayCommand(ExecuteShowWarning);
            ShowErrorCommand = new RelayCommand(ExecuteShowError);
            CheckMesStatusCommand = new RelayCommand(async () => await CheckMesStatusAsync());
            TestOfflineUploadCommand = new RelayCommand(async () => await TestOfflineUploadAsync());
            TestMarkingUploadCommand = new RelayCommand(async () => await TestMarkingUploadAsync());
            ReloadMesConfigCommand = new RelayCommand(async () => await ReloadMesConfigAsync());
            GenerateTestDataCommand = new RelayCommand(GenerateTestData);
            ClearLogsCommand = new RelayCommand(ClearLogs);
            BatchTestCommand = new RelayCommand(async () => await BatchTestAsync());

            // 初始加载MES配置信息
            _ = LoadMesConfigAsync();
        }
        #endregion

        #region Properties
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                    _logger.Debug($"状态消息已更新: {value}");
                }
            }
        }

        public string LogMessage
        {
            get => _logMessage;
            set
            {
                if (_logMessage != value)
                {
                    _logMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ProductSn
        {
            get => _productSn;
            set
            {
                if (_productSn != value)
                {
                    _productSn = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TestResult
        {
            get => _testResult;
            set
            {
                if (_testResult != value)
                {
                    _testResult = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsNotBusy));
                }
            }
        }

        public bool IsNotBusy => !IsBusy;

        public string MesConfigInfo
        {
            get => _mesConfigInfo;
            set
            {
                if (_mesConfigInfo != value)
                {
                    _mesConfigInfo = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> TestDataLog
        {
            get => _testDataLog;
            set
            {
                _testDataLog = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand TestLogCommand { get; private set; }
        public ICommand ShowInfoCommand { get; private set; }
        public ICommand ShowWarningCommand { get; private set; }
        public ICommand ShowErrorCommand { get; private set; }

        // MES相关命令
        public ICommand CheckMesStatusCommand { get; private set; }
        public ICommand TestOfflineUploadCommand { get; private set; }
        public ICommand TestMarkingUploadCommand { get; private set; }
        public ICommand ReloadMesConfigCommand { get; private set; }
        public ICommand GenerateTestDataCommand { get; private set; }
        public ICommand ClearLogsCommand { get; private set; }
        public ICommand BatchTestCommand { get; private set; }
        #endregion

        #region Command Initialization
        //private void InitializeCommands()
        //{
        //    // 原有命令
        //    TestLogCommand = new RelayCommand(ExecuteTestLog);
        //    ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
        //    ShowWarningCommand = new RelayCommand(ExecuteShowWarning);
        //    ShowErrorCommand = new RelayCommand(ExecuteShowError);

        //    // MES相关命令
        //    CheckMesStatusCommand = new RelayCommand(async () => await CheckMesStatusAsync());
        //    TestOfflineUploadCommand = new RelayCommand(async () => await TestOfflineUploadAsync());
        //    TestMarkingUploadCommand = new RelayCommand(async () => await TestMarkingUploadAsync());
        //    ReloadMesConfigCommand = new RelayCommand(async () => await ReloadMesConfigAsync());
        //    GenerateTestDataCommand = new RelayCommand(GenerateTestData);
        //    ClearLogsCommand = new RelayCommand(ClearLogs);
        //    BatchTestCommand = new RelayCommand(async () => await BatchTestAsync());
        //}
        #endregion

        #region Original Methods
        private void ExecuteTestLog()
        {
            try
            {
                _logger.Info("用户点击了测试日志按钮");

                if (_mesService.IsEnabled)
                {
                    _logger.Info("MES服务已启用");
                    AddLogEntry("MES服务状态: 已启用", "INFO");
                    StatusMessage = "MES服务已启用";
                }
                else
                {
                    _logger.Warn("MES服务未启用");
                    AddLogEntry("MES服务状态: 未启用", "WARN");
                    StatusMessage = "MES服务未启用";
                }
            }
            catch (Exception ex)
            {
                _logger.Error("执行测试日志时发生错误", ex);
                AddLogEntry($"错误: {ex.Message}", "ERROR");
                StatusMessage = "测试操作失败";
            }
        }

        private void ExecuteShowInfo()
        {
            _logger.Info($"用户查看信息 - {DateTime.Now}");
            AddLogEntry($"查看信息 - {DateTime.Now:yyyy-MM-dd HH:mm:ss}", "INFO");
            StatusMessage = "显示信息完成";
        }

        private void ExecuteShowWarning()
        {
            _logger.Warn("用户触发了警告操作");
            AddLogEntry("警告操作已触发", "WARN");
            StatusMessage = "警告操作已记录";
        }

        private void ExecuteShowError()
        {
            try
            {
                throw new InvalidOperationException("这是一个示例错误");
            }
            catch (Exception ex)
            {
                _logger.Error("模拟错误操作", ex);
                AddLogEntry($"模拟错误: {ex.Message}", "ERROR");
                StatusMessage = "错误已记录到日志";
            }
        }
        #endregion

        #region MES Test Methods

        /// <summary>
        /// 加载MES配置信息
        /// </summary>
        private async Task LoadMesConfigAsync()
        {
            try
            {
                var config = await _configProvider.GetConfigurationAsync();
                if (config != null)
                {
                    MesConfigInfo = $"Profile: {config.ProfileName}\n" +
                                   $"服务器: {config.BaseUrl}\n" +
                                   $"超时: {config.TimeoutSeconds}秒\n" +
                                   $"重试: {(config.EnableRetry ? $"启用({config.MaxRetryCount}次)" : "禁用")}\n" +
                                   $"设备SN: {config.DeviceSn}";
                }
                else
                {
                    MesConfigInfo = "MES未配置或已禁用";
                }
            }
            catch (Exception ex)
            {
                MesConfigInfo = $"加载配置失败: {ex.Message}";
                _logger.Error("加载MES配置失败", ex);
            }
        }

        /// <summary>
        /// 检查MES状态
        /// </summary>
        private async Task CheckMesStatusAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                StatusMessage = "正在检查MES状态...";
                AddLogEntry("开始检查MES状态", "INFO");

                // 检查主服务状态
                var mesEnabled = _mesService.IsEnabled;
                AddLogEntry($"MES主服务: {(mesEnabled ? "已启用" : "未启用")}", mesEnabled ? "SUCCESS" : "WARN");

                if (!mesEnabled)
                {
                    StatusMessage = "MES服务未启用";
                    return;
                }

                // 获取配置信息
                var config = await _configProvider.GetConfigurationAsync();
                if (config != null)
                {
                    AddLogEntry($"配置Profile: {config.ProfileName}", "INFO");
                    AddLogEntry($"服务器地址: {config.BaseUrl}", "INFO");
                }
                else
                {
                    AddLogEntry("无法获取MES配置", "ERROR");
                    StatusMessage = "MES配置不可用";
                }

                // 重新加载配置信息显示
                await LoadMesConfigAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("检查MES状态失败", ex);
                AddLogEntry($"检查失败: {ex.Message}", "ERROR");
                StatusMessage = "检查MES状态失败";
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 测试离线数据上传
        /// </summary>
        private async Task TestOfflineUploadAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                StatusMessage = "正在测试离线数据上传...";
                AddLogEntry("========== 开始离线数据上传测试 ==========", "INFO");

                // 获取服务
                using var scope = _serviceProvider.CreateScope();
                var offlineService = scope.ServiceProvider.GetService<IOfflineDataUploadService>();

                if (offlineService == null)
                {
                    AddLogEntry("离线数据上传服务不可用", "ERROR");
                    StatusMessage = "服务不可用";
                    return;
                }

                // 初始化服务
                if (!offlineService.IsEnabled)
                {
                    var initialized = await offlineService.InitializeAsync();
                    if (!initialized)
                    {
                        AddLogEntry("服务初始化失败", "ERROR");
                        StatusMessage = "服务初始化失败";
                        return;
                    }
                }

                // 准备测试数据
                var sn = string.IsNullOrWhiteSpace(ProductSn)
                    ? $"TEST_{DateTime.Now:yyyyMMddHHmmss}"
                    : ProductSn;

                AddLogEntry($"产品SN: {sn}", "INFO");
                AddLogEntry($"测试结果: {TestResult}", "INFO");

                var testData = GenerateTestDataList();
                

                // 上传数据
                AddLogEntry("正在上传数据...", "INFO");
                var response = await offlineService.UploadAsync(
                    productSn: sn,
                    testResult: TestResult,
                    testDatas: testData,
                    environments: []
                );

                // 处理响应
                if (response.Success)
                {
                    AddLogEntry($"上传成功! TraceId: {response.TraceId}", "SUCCESS");
                    StatusMessage = "离线数据上传成功";
                }
                else
                {
                    AddLogEntry($"上传失败! 错误: {response.Message}", "ERROR");
                    AddLogEntry($"错误代码: {response.Code}", "ERROR");
                    StatusMessage = "离线数据上传失败";
                }

                AddLogEntry("========== 离线数据上传测试完成 ==========", "INFO");
            }
            catch (Exception ex)
            {
                _logger.Error("离线数据上传测试失败", ex);
                AddLogEntry($"测试异常: {ex.Message}", "ERROR");
                StatusMessage = "测试失败";
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 测试Marking上传
        /// </summary>
        private async Task TestMarkingUploadAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                StatusMessage = "正在测试Marking数据上传...";
                AddLogEntry("========== 开始Marking数据上传测试 ==========", "INFO");

                // 获取服务
                using var scope = _serviceProvider.CreateScope();
                var markingService = scope.ServiceProvider.GetService<IMarkingDataUploadService>();

                if (markingService == null)
                {
                    AddLogEntry("Marking数据上传服务不可用", "ERROR");
                    StatusMessage = "服务不可用";
                    return;
                }

                // 初始化服务
                if (!markingService.IsEnabled)
                {
                    var initialized = await markingService.InitializeAsync();
                    if (!initialized)
                    {
                        AddLogEntry("服务初始化失败", "ERROR");
                        StatusMessage = "服务初始化失败";
                        return;
                    }
                }

                // 准备测试数据
                var sn = string.IsNullOrWhiteSpace(ProductSn)
                    ? $"MARK_{DateTime.Now:yyyyMMddHHmmss}"
                    : ProductSn;

                var defectList = TestResult.ToUpper() == "FAIL"
                    ? new List<string> { "外观缺陷", "尺寸超差", "功能异常" }
                    : null;

                AddLogEntry($"产品SN: {sn}", "INFO");
                AddLogEntry($"缺陷数量: {defectList?.Count ?? 0}", "INFO");

                // 上传数据
                AddLogEntry("正在上传Marking数据...", "INFO");
                var response = await markingService.UploadAsync(
                    productSn: sn,
                    defectList: defectList
                );

                // 处理响应
                if (response.Success)
                {
                    AddLogEntry($"Marking上传成功! TraceId: {response.TraceId}", "SUCCESS");
                    StatusMessage = "Marking数据上传成功";
                }
                else
                {
                    AddLogEntry($"Marking上传失败! 错误: {response.Message}", "ERROR");
                    AddLogEntry($"错误代码: {response.Code}", "ERROR");
                    StatusMessage = "Marking数据上传失败";
                }

                AddLogEntry("========== Marking数据上传测试完成 ==========", "INFO");
            }
            catch (Exception ex)
            {
                _logger.Error("Marking数据上传测试失败", ex);
                AddLogEntry($"测试异常: {ex.Message}", "ERROR");
                StatusMessage = "测试失败";
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 重新加载MES配置
        /// </summary>
        private async Task ReloadMesConfigAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                StatusMessage = "正在重新加载配置...";
                AddLogEntry("开始重新加载MES配置", "INFO");

                var result = await _configProvider.ReloadConfigurationAsync();

                if (result)
                {
                    AddLogEntry("配置重新加载成功", "SUCCESS");

                    // 重新初始化MES服务
                    await _mesService.ResetAsync();
                    await _mesService.InitializeAsync();

                    // 重新加载配置信息显示
                    await LoadMesConfigAsync();

                    AddLogEntry("MES服务重新初始化完成", "SUCCESS");
                    StatusMessage = "配置重新加载成功";
                }
                else
                {
                    AddLogEntry("配置重新加载失败", "ERROR");
                    StatusMessage = "配置重新加载失败";
                }
            }
            catch (Exception ex)
            {
                _logger.Error("重新加载配置失败", ex);
                AddLogEntry($"重新加载失败: {ex.Message}", "ERROR");
                StatusMessage = "重新加载失败";
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 批量测试
        /// </summary>
        private async Task BatchTestAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                StatusMessage = "正在执行批量测试...";
                AddLogEntry("========== 开始批量测试 ==========", "INFO");

                int successCount = 0;
                int failCount = 0;
                int testCount = 5; // 测试5个产品

                using var scope = _serviceProvider.CreateScope();
                var offlineService = scope.ServiceProvider.GetService<IOfflineDataUploadService>();

                if (offlineService == null)
                {
                    AddLogEntry("服务不可用", "ERROR");
                    return;
                }

                // 初始化服务
                if (!offlineService.IsEnabled)
                {
                    await offlineService.InitializeAsync();
                }

                for (int i = 1; i <= testCount; i++)
                {
                    var sn = $"BATCH_{DateTime.Now:yyyyMMddHHmmss}_{i:D3}";
                    var result = i % 3 == 0 ? "1" : "0"; // 每3个有1个失败

                    AddLogEntry($"测试产品 {i}/{testCount}: {sn}", "INFO");

                    var response = await offlineService.UploadAsync(
                        productSn: sn,
                        testResult: result,
                        testDatas: GenerateTestDataList(),
                        environments: GenerateEnvironmentData()
                    );

                    if (response.Success)
                    {
                        successCount++;
                        AddLogEntry($"  ✓ 上传成功", "SUCCESS");
                    }
                    else
                    {
                        failCount++;
                        AddLogEntry($"  ✗ 上传失败: {response.Message}", "ERROR");
                    }

                    // 延迟一下，避免太快
                    await Task.Delay(500);
                }

                AddLogEntry($"批量测试完成: 成功={successCount}, 失败={failCount}", "INFO");
                AddLogEntry("========== 批量测试结束 ==========", "INFO");
                StatusMessage = $"批量测试完成: 成功{successCount}/{testCount}";
            }
            catch (Exception ex)
            {
                _logger.Error("批量测试失败", ex);
                AddLogEntry($"批量测试异常: {ex.Message}", "ERROR");
                StatusMessage = "批量测试失败";
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// 生成测试数据
        /// </summary>
        private void GenerateTestData()
        {
            ProductSn = "190124C16971700";
            TestResult = DateTime.Now.Second % 2 == 0 ? "0" : "1";  // OK为0，NG为1
            AddLogEntry($"已生成测试数据 - SN: {ProductSn}, 结果: {TestResult}", "INFO");
            StatusMessage = "测试数据已生成";
        }

        /// <summary>
        /// 清空日志
        /// </summary>
        private void ClearLogs()
        {
            TestDataLog.Clear();
            LogMessage = "";
            StatusMessage = "日志已清空";
        }

        /// <summary>
        /// 添加日志条目
        /// </summary>
        private void AddLogEntry(string message, string level = "INFO")
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            var logEntry = $"[{timestamp}] [{level}] {message}";

            // 添加到集合
            
                TestDataLog.Add(logEntry);

                // 保持最多100条记录
                if (TestDataLog.Count > 100)
                {
                    TestDataLog.RemoveAt(0);
                }
           

            // 更新最后的日志消息
            LogMessage = message;

            // 根据级别记录到日志系统
            switch (level.ToUpper())
            {
                case "ERROR":
                    _logger.Error(message);
                    break;
                case "WARN":
                    _logger.Warn(message);
                    break;
                case "SUCCESS":
                    _logger.Info($"[SUCCESS] {message}");
                    break;
                default:
                    _logger.Info(message);
                    break;
            }
        }

        /// <summary>
        /// 生成测试数据列表
        /// </summary>
        private List<OfflineTestData> GenerateTestDataList()
        {
            var random = new Random();
            return new List<OfflineTestData>
            {
                new OfflineTestData
                {
                    paramCode = "1026",
                    paramName = "AI检测翻折",
                    paramValue = "正极翻折",
                    paramResult = "0",
                    paramUnit = ""
                }
              
            };
        }

        /// <summary>
        /// 生成环境数据
        /// </summary>
        private List<EnvironmentData> GenerateEnvironmentData()
        {
            var random = new Random();
            return new List<EnvironmentData>
            {
                //new EnvironmentData
                //{
                //    Parameter = "环境温度",
                //    Value = (22 + random.NextDouble() * 5).ToString("F1"),
                //    Unit = "℃"
                //},
                //new EnvironmentData
                //{
                //    Parameter = "环境湿度",
                //    Value = (50 + random.NextDouble() * 20).ToString("F0"),
                //    Unit = "%"
                //},
                //new EnvironmentData
                //{
                //    Parameter = "大气压力",
                //    Value = (1010 + random.NextDouble() * 10).ToString("F0"),
                //    Unit = "hPa"
                //}
            };
        }
        #endregion
    }
}