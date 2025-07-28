using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using SUNWODA_SEVB.Core.Services;
using SUNWODA_SEVB.Data;
using SUNWODA_SEVB.Data.Services;
using SUNWODA_SEVB.Logging;

namespace SUNWODA_SEVB
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? _host;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // 创建 Host，它会自动配置 DI 和日志
                _host = Host.CreateDefaultBuilder()
                            .ConfigureServices((context, services) =>
                            {
                                ConfigureServices(services);
                            })
                            .Build();

                // 启动 Host
                await _host.StartAsync();

                // 记录应用启动
                var appLogger = _host.Services.GetRequiredService<ILoggerService<App>>();
                appLogger.Info("========== 应用程序启动 ==========");
                appLogger.Info($"启动时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                appLogger.Info($"版本: {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}");

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

                // 设置全局异常处理
                SetupGlobalExceptionHandling();


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

        private void ConfigureServices(IServiceCollection services)
        {
            // 添加日志服务
            services.AddNLogServices();

            // 添加内存缓存服务
            services.AddMemoryCache();

            // 数据库 / 数据访问
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

        private void SetupGlobalExceptionHandling()
        {
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

            DispatcherUnhandledException += (sender, args) =>
            {
                var logger = LogManager.GetLogger("DispatcherUnhandledException");
                logger.Fatal(args.Exception, "发生未处理的调度程序异常");
                args.Handled = true;
            };
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            try
            {
                // 记录应用退出
                var appLogger = _host?.Services?.GetService<ILoggerService<App>>();
                appLogger?.Info("========== 应用程序退出 ==========");

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
