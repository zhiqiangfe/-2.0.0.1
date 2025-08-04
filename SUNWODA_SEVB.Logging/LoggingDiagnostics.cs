using NLog;
using NLog.Config;
using System.Text;
using SUNWODA_SEVB.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace SUNWODA_SEVB.Logging
{
    public static class LoggingDiagnostics
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 诊断日志配置
        /// </summary>
        public static void DiagnoseLoggingConfiguration(IServiceProvider serviceProvider)
        {
            var sb = new StringBuilder();
            sb.AppendLine("========== NLog 配置诊断 ==========");

            try
            {
                // 检查 NLog 配置
                var config = LogManager.Configuration;
                if (config == null)
                {
                    sb.AppendLine("❌ NLog 配置为 null");
                }
                else
                {
                    sb.AppendLine("✅ NLog 配置已加载");

                    // 列出所有目标
                    sb.AppendLine("\n已注册的目标:");
                    foreach (var target in config.AllTargets)
                    {
                        sb.AppendLine($"  - {target.Name} ({target.GetType().Name})");
                    }

                    // 列出所有规则
                    sb.AppendLine("\n日志规则:");
                    foreach (var rule in config.LoggingRules)
                    {
                        sb.AppendLine($"  - Logger: {rule.LoggerNamePattern}, MinLevel: {rule.Levels.FirstOrDefault()}, Targets: {string.Join(", ", rule.Targets.Select(t => t.Name))}");
                    }
                }

                // 检查数据库连接
                sb.AppendLine("\n数据库连接测试:");
                using (var scope = serviceProvider.CreateScope())
                {
                    var appLogRepo = scope.ServiceProvider.GetService<IAppLogRepository>();
                    if (appLogRepo == null)
                    {
                        sb.AppendLine("❌ IAppLogRepository 未注册");
                    }
                    else
                    {
                        sb.AppendLine("✅ IAppLogRepository 已注册");

                        // 尝试写入测试日志
                        try
                        {
                            var testLog = new Core.Models.AppLogModel
                            {
                                LogLevel = "TEST",
                                Logger = "DiagnosticsTest",
                                Message = "诊断测试日志",
                                LogTime = DateTime.Now
                            };

                            var result = appLogRepo.AddAsync(testLog).GetAwaiter().GetResult();
                            if (result)
                            {
                                sb.AppendLine("✅ 测试日志写入成功");
                            }
                            else
                            {
                                sb.AppendLine("❌ 测试日志写入失败");
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine($"❌ 测试日志写入异常: {ex.Message}");
                        }
                    }
                }

                // 输出诊断结果
                var diagnosticResult = sb.ToString();
                Logger.Info(diagnosticResult);
                System.Diagnostics.Debug.WriteLine(diagnosticResult);

                // 也写入到文件
                var diagnosticFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "diagnostics.log");
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(diagnosticFile)!);
                System.IO.File.WriteAllText(diagnosticFile, diagnosticResult);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "诊断过程中发生错误");
            }
        }

        /// <summary>
        /// 强制刷新所有日志目标
        /// </summary>
        public static void ForceFlushAllTargets()
        {
            try
            {
                LogManager.Flush(TimeSpan.FromSeconds(5));
                Logger.Info("所有日志目标已刷新");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "刷新日志目标时发生错误");
            }
        }
    }
}