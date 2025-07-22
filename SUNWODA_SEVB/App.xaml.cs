using System.Configuration;
using System.Data;
using System.Windows;
using SUNWODA_SEVB.Logging;

namespace SUNWODA_SEVB
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 记录应用启动
            LoggerHelper.Instance.Info("========== 应用程序启动 ==========");
            LoggerHelper.Instance.Info($"启动时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            LoggerHelper.Instance.Info($"版本: {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}");

            // 设置全局异常处理
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception;

            if (e.ExceptionObject is Exception ex)
            {
                exception = ex;
            }
            else
            {
                // 创建一个包装异常来包含非 Exception 类型的错误信息
                exception = new Exception($"Non-exception object: {e.ExceptionObject?.ToString() ?? "null"}");
            }

            LoggerHelper.Instance.Fatal("发生未处理的域异常", exception);
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LoggerHelper.Instance.Fatal("发生未处理的调度程序异常", e.Exception);
            e.Handled = true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            LoggerHelper.Instance.Info("========== 应用程序退出 ==========");
            LoggerHelper.Instance.Flush();
            LoggerHelper.Instance.Shutdown();
            base.OnExit(e);
        }
    }

}
