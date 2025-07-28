using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SUNWODA_SEVB.Logging
{
    /// <summary>
    /// 使用 NLog 实现的日志管理服务
    /// </summary>
    public class NLogManagementService : ILogManagementService
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public string GetLogFilePath(DateTime date)
        {
            var config = LogManager.Configuration;
            if (config == null) return string.Empty;

            var fileTarget = config.AllTargets
                                  .OfType<NLog.Targets.FileTarget>()
                                  .FirstOrDefault();

            if (fileTarget == null) return string.Empty;

            try
            {
                var logEventInfo = new LogEventInfo
                {
                    TimeStamp = date,
                    Level = NLog.LogLevel.Info,
                    LoggerName = "LogManagementService"
                };

                string renderedFileName = fileTarget.FileName.Render(logEventInfo);

                if (!Path.IsPathRooted(renderedFileName))
                {
                    renderedFileName = Path.GetFullPath(renderedFileName);
                }

                return renderedFileName;
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, "获取日志文件路径失败");
                return string.Empty;
            }
        }
        public void CleanupOldLogs(int daysToKeep = 90)
        {
            try
            {
                var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                if (!Directory.Exists(logDir)) return;

                var cutoffDate = DateTime.Now.AddDays(-daysToKeep);
                var directories = Directory.GetDirectories(logDir);

                foreach (var dir in directories)
                {
                    var dirName = Path.GetFileName(dir);
                    if (DateTime.TryParseExact(dirName, "yyyy-MM-dd", null,
                        System.Globalization.DateTimeStyles.None, out var dirDate))
                    {
                        if (dirDate < cutoffDate)
                        {
                            try
                            {
                                Directory.Delete(dir, true);
                                Logger.Info($"已删除过期日志目录: {dir}");
                            }
                            catch (Exception ex)
                            {
                                Logger.Warn(ex, $"删除过期日志目录失败: {dir}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "清理过期日志时发生错误");
            }
        }

        public void Flush()
        {
            LogManager.Flush();
        }
       
        /// <summary>
        /// 关闭日志系统，释放资源
        /// </summary>
        public void Shutdown()
        {
            try
            {
                LogManager.Flush();
                LogManager.Shutdown();
                Logger.Info("日志系统已关闭");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"关闭日志系统时发生错误: {ex.Message}");
            }
        }
    }
}
