using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Data;
using SUNWODA_SEVB.Logging;
using SUNWODA_SEVB.Logging.Targets;
using SUNWODA_SEVB.PLC;

namespace SUNWODA_SEVB
{
    public partial class App : Application
    {
        private IHost? _host;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // 创建 Host，使用标准的配置加载方式
                _host = Host.CreateDefaultBuilder()
                    .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory)
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        config.Sources.Clear();
                        config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                        config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                        config.AddEnvironmentVariables();
                    })
                    .ConfigureServices((context, services) =>
                    {
                        ConfigurationHelper.SetConfiguration(context.Configuration);
                        ConfigureServices(services, context.Configuration);
                    })
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddNLog();
                    })
                    .Build();

                var appLogger = _host.Services.GetRequiredService<ILoggerService<App>>();
                // 正确加载 NLog 配置
                var env = _host.Services.GetRequiredService<IHostEnvironment>();
                var config = _host.Services.GetRequiredService<IConfiguration>();

                // 加载 NLog.config
                var nlogConfigPath = Path.Combine(env.ContentRootPath, "NLog.config");
                LogManager.Configuration = new XmlLoggingConfiguration(nlogConfigPath);

                // 安全设置配置变量 - 修复空引用警告
                var connectionString = config.GetConnectionString("DefaultConnection");

                // 验证连接字符串是否有效
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException("数据库连接字符串未配置或为空");
                }

                // 安全设置变量 - 使用非空值
                if (LogManager.Configuration.Variables.ContainsKey("dbConnection"))
                {
                    LogManager.Configuration.Variables["dbConnection"] = connectionString;
                }
                else
                {
                    LogManager.Configuration.Variables.Add("dbConnection", connectionString);
                }

                // 启动 Host
                await _host.StartAsync();

                // 初始化NLog的数据库目标
                InitializeNLogDatabaseTarget();

                // 测试数据库日志
                //await TestDatabaseLogging();

                // 初始化数据库
                var databaseService = _host.Services.GetRequiredService<IDatabaseService>();
                if (!databaseService.Initialize())
                {
                    appLogger.Error("数据库初始化失败，应用程序将退出");
                    MessageBox.Show("数据库初始化失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    Shutdown();
                    return;
                }

                appLogger.Info("数据库初始化成功");

                // 初始化PLC
                var plcService = _host.Services.GetRequiredService<IPLCService>();
                var isInitPlcs = await plcService.InitPlcs();
                if (!isInitPlcs)
                {
                    appLogger.Error("PLC初始化失败，应用程序将退出");
                    MessageBox.Show("PLC初始化失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    Shutdown();
                    return;
                }

                appLogger.Info("PLC初始化成功");

                // 记录应用启动

                appLogger.Info("========== 应用程序启动 ==========",true);
                appLogger.Info($"启动时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", true);
                appLogger.Info($"版本: {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}");
                appLogger.Info($"配置文件路径: {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json")}");
                appLogger.Info($"环境: {_host.Services.GetRequiredService<IHostEnvironment>().EnvironmentName}");



                // 设置全局异常处理
                SetupGlobalExceptionHandling();

                // 启动日志清理任务
                StartLogCleanupTask();

                // 创建并显示主窗口
                var mainWindow = _host.Services.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                // 启动失败时记录日志
                var logger = LogManager.GetLogger("AppStartup");
                logger.Fatal(ex, "应用程序启动失败", true);
                MessageBox.Show($"应用程序启动失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // 注册配置
            services.AddSingleton(configuration);

            // 添加日志服务
            services.AddNLogServices();

            // 内存缓存
            services.AddMemoryCache();

            // 使用新的方法注册 SqlSugar 和所有数据服务
            services.AddDataServices();

            // 添加数据库服务
            services.AddSingleton<IDatabaseService, DatabaseService>();

            services.AddSingleton<IPLCService, PLCService>();

            // 注册 ViewModels
            services.AddTransient<ViewModels.MainWindowViewModel>();

            // 注册 Views
            services.AddTransient<MainWindow>(serviceProvider =>
            {
                var viewModel = serviceProvider.GetRequiredService<ViewModels.MainWindowViewModel>();
                var window = new MainWindow
                {
                    DataContext = viewModel
                };
                return window;
            });
        }

        private void InitializeNLogDatabaseTarget()
        {
            // 初始化DatabaseLogTarget的服务提供者
            if (_host != null)
            {
                DatabaseLogTarget.Initialize(_host.Services);
            }
        }
        private async Task TestDatabaseLogging()
        {
            var logger = _host?.Services?.GetService<ILoggerService<App>>();
            if (logger != null)
            {
                logger.Info("测试数据库日志功能 - Info级别");
                logger.Warn("测试数据库日志功能 - Warn级别");
                logger.Error("测试数据库日志功能 - Error级别");

                // 强制刷新
                LogManager.Flush();

                // 等待一下让日志写入
                await Task.Delay(1000);
            }
        }

        private void SetupGlobalExceptionHandling()
        {
            // 应用程序域未处理异常
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var logger = LogManager.GetLogger("UnhandledException");
                if (args.ExceptionObject is Exception ex)
                {
                    logger.Fatal(ex, "发生未处理的域异常");
                }
                else
                {
                    logger.Fatal("发生未处理的域异常: {0}", args.ExceptionObject?.ToString() ?? "null");
                }
            };

            // WPF调度程序未处理异常
            DispatcherUnhandledException += (sender, args) =>
            {
                var logger = LogManager.GetLogger("DispatcherUnhandledException");
                logger.Fatal(args.Exception, "发生未处理的调度程序异常");

                // 标记为已处理，防止应用程序崩溃
                args.Handled = true;

                // 显示错误消息
                MessageBox.Show(
                    $"发生未处理的错误: {args.Exception.Message}",
                    "错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            };

            // Task未处理异常
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                var logger = LogManager.GetLogger("UnobservedTaskException");
                logger.Error(args.Exception, "发生未观察的任务异常");
                args.SetObserved();
            };
        }

        /// <summary>
        /// 启动日志清理任务
        /// </summary>
        private void StartLogCleanupTask()
        {
            _ = Task.Run(async () =>
            {
                var logger = _host?.Services?.GetService<ILoggerService<App>>();
                var logManagementService = _host?.Services?.GetService<ILogManagementService>();

                try
                {
                    // 程序启动时立即执行一次清理
                    logger?.Info("应用启动，开始执行初始日志清理任务");

                    // 清理启动时的文件日志 (90天)
                    logManagementService?.CleanupOldLogs(90);

                    // 清理启动时的数据库日志
                    await CleanupDatabaseLogs(logger);

                    logger?.Info("初始日志清理任务完成");
                }
                catch (Exception ex)
                {
                    logger?.Error("初始日志清理任务执行失败", ex);
                }

                // 定时清理任务循环
                while (_host != null)
                {
                    try
                    {
                        // 每天凌晨2点执行清理
                        var now = DateTime.Now;
                        var nextRun = now.Date.AddDays(1).AddHours(2);
                        var delay = nextRun - now;

                        await Task.Delay(delay);

                        logger?.Info("开始执行定时日志清理任务");

                        // 清理文件日志 (90天)
                        logManagementService?.CleanupOldLogs(90);

                        // 清理数据库日志
                        await CleanupDatabaseLogs(logger);

                        logger?.Info("定时日志清理任务完成");
                    }
                    catch (Exception ex)
                    {
                        logger?.Error("定时日志清理任务执行失败", ex);
                    }
                }
            });
        }

        /// <summary>
        /// 清理数据库日志
        /// </summary>
        private async Task CleanupDatabaseLogs(ILoggerService<App>? logger)
        {
            try
            {
                using (var scope = _host?.Services?.CreateScope())
                {
                    if (scope == null) return;

                    var appLogRepo = scope.ServiceProvider.GetRequiredService<IAppLogRepository>();
                    //后续可以根据需要添加其他日志仓储接口
                    //var mesLogRepo = scope.ServiceProvider.GetRequiredService<IMesInterfaceLogRepository>();
                    //var webLogRepo = scope.ServiceProvider.GetRequiredService<IWebInterfaceLogRepository>();

                    // 清理应用日志
                    var appLogResult = await CleanupAppLogs(appLogRepo, logger);

                    //// 清理MES接口日志 (30天)
                    //var mesLogCount = await mesLogRepo.DeleteOldLogsAsync(30);

                    //// 清理Web接口日志 (30天)  
                    //var webLogCount = await webLogRepo.DeleteOldLogsAsync(30);

                    logger?.Info($"数据库日志清理完成 - 应用日志: 按时间删除{appLogResult.TimeBasedCount}条,按大小删除{appLogResult.SizeBasedCount}条");
                }
            }
            catch (Exception ex)
            {
                logger?.Error("数据库日志清理失败", ex);
            }
        }

        /// <summary>
        /// 清理应用日志的结果
        /// </summary>
        private class AppLogCleanupResult
        {
            public int TimeBasedCount { get; set; }
            public int SizeBasedCount { get; set; }
        }

        /// <summary>
        /// 清理应用日志 - 支持按时间和大小两种方式
        /// </summary>
        private async Task<AppLogCleanupResult> CleanupAppLogs(IAppLogRepository appLogRepo, ILoggerService<App>? logger)
        {
            var result = new AppLogCleanupResult();

            try
            {
                // 1. 按时间清理：删除3天前的日志
                result.TimeBasedCount = await appLogRepo.DeleteOldLogsAsync(3);

                // 2. 按大小清理：检查数据库大小，如果超过100M则删除更多日志
                var databaseSizeMB = await appLogRepo.GetDatabaseSizeAsync();

                if (databaseSizeMB > 100)
                {
                    logger?.Info($"应用日志数据库大小已达到 {databaseSizeMB:F2}MB，开始按大小清理");

                    // 删除直到数据库小于100MB
                    result.SizeBasedCount = await appLogRepo.DeleteLogsBySize(100);

                    logger?.Info($"按大小清理完成，删除了 {result.SizeBasedCount} 条日志");
                }
            }
            catch (Exception ex)
            {
                logger?.Error("应用日志清理过程中发生错误", ex);
            }

            return result;
        }


        protected override async void OnExit(ExitEventArgs e)
        {
            try
            {
                // 记录应用退出
                var appLogger = _host?.Services?.GetService<ILoggerService<App>>();
                appLogger?.Info("========== 应用程序退出 ==========", true);
                appLogger?.Info($"退出时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", true);

                // 清理日志资源
                var logManagementService = _host?.Services?.GetService<ILogManagementService>();
                logManagementService?.Flush();

                // 停止 Host
                if (_host != null)
                {
                    await _host.StopAsync();
                    _host.Dispose();
                }
            }
            finally
            {
                // 最后关闭日志系统
                LogManager.Shutdown();
                base.OnExit(e);
            }
        }
    }
}