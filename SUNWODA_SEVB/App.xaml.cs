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

                // 记录应用启动

                appLogger.Info("========== 应用程序启动 ==========");
                appLogger.Info($"启动时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
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
                logger.Fatal(ex, "应用程序启动失败");
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

        private void StartLogCleanupTask()
        {
            _ = Task.Run(async () =>
            {
                var logger = _host?.Services?.GetService<ILoggerService<App>>();
                var logManagementService = _host?.Services?.GetService<ILogManagementService>();

                while (_host != null)
                {
                    try
                    {
                        // 每天凌晨2点执行清理
                        var now = DateTime.Now;
                        var nextRun = now.Date.AddDays(1).AddHours(2);
                        var delay = nextRun - now;

                        await Task.Delay(delay);

                        logger?.Info("开始执行日志清理任务");

                        // 清理文件日志
                        logManagementService?.CleanupOldLogs(90);

                        // 清理数据库日志
                        using (var scope = _host.Services.CreateScope())
                        {
                            var appLogRepo = scope.ServiceProvider.GetRequiredService<IAppLogRepository>();
                            var mesLogRepo = scope.ServiceProvider.GetRequiredService<IMesInterfaceLogRepository>();
                            var webLogRepo = scope.ServiceProvider.GetRequiredService<IWebInterfaceLogRepository>();

                            var appLogCount = await appLogRepo.DeleteOldLogsAsync(90);
                            var mesLogCount = await mesLogRepo.DeleteOldLogsAsync(90);
                            var webLogCount = await webLogRepo.DeleteOldLogsAsync(90);

                            logger?.Info($"已清理日志 - 应用日志: {appLogCount} 条, MES日志: {mesLogCount} 条, Web日志: {webLogCount} 条");
                        }

                        logger?.Info("日志清理任务完成");
                    }
                    catch (Exception ex)
                    {
                        logger?.Error("日志清理任务执行失败", ex);
                    }
                }
            });
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            try
            {
                // 记录应用退出
                var appLogger = _host?.Services?.GetService<ILoggerService<App>>();
                appLogger?.Info("========== 应用程序退出 ==========");
                appLogger?.Info($"退出时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

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