using NLog.Config;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Google.Protobuf.WellKnownTypes;
using System.Diagnostics;
using System.Reflection;

namespace HTHIUM.Logging
{
    /// <summary>
    /// NLog 日志帮助类
    /// </summary>
    public class LoggerHelper : ILogManagementService
    {
        private readonly NLog.ILogger _nlogLogger;

        /// <summary>
        /// 构造函数，通过 DI 注入 NLog.ILogger
        /// </summary>
        /// <param name="nlogLogger">NLog 的 ILogger 实例</param>
        public LoggerHelper(NLog.ILogger nlogLogger)
        {
            _nlogLogger = nlogLogger ?? throw new ArgumentNullException(nameof(nlogLogger));
        }

        #region ILoggerService 接口实现

        public void Trace(string message) => _nlogLogger.Trace(message);
        public void Trace(string message, Exception? exception = null) => _nlogLogger.Trace(exception, message);

        public void Debug(string message) => _nlogLogger.Debug(message);
        public void Debug(string message, Exception? exception = null) => _nlogLogger.Debug(exception, message);

        public void Info(string message) => _nlogLogger.Info(message);
        public void Info(string message, Exception? exception = null) => _nlogLogger.Info(exception, message);

        public void Warn(string message) => _nlogLogger.Warn(message);
        public void Warn(string message, Exception? exception = null) => _nlogLogger.Warn(exception, message);

        public void Error(string message) => _nlogLogger.Error(message);
        public void Error(string message, Exception? exception = null) => _nlogLogger.Error(exception, message);

        public void Fatal(string message) => _nlogLogger.Fatal(message);
        public void Fatal(string message, Exception? exception = null) => _nlogLogger.Fatal(exception, message);

        public void Log(LogLevel level, string message) => _nlogLogger.Log(ConvertToNLogLevel(level), message);
        public void Log(LogLevel level, string message, Exception? exception = null) => _nlogLogger.Log(ConvertToNLogLevel(level), exception, message);

        public void LogSpecial(LogLevel level, string message)
        {
            var logEvent = new LogEventInfo(ConvertToNLogLevel(level), _nlogLogger.Name, message);
            logEvent.Properties["LogToSpecialFile"] = true;
            _nlogLogger.Log(logEvent);
        }

        public void LogSpecial(LogLevel level, string message, Exception? exception)
        {
            var logEvent = new LogEventInfo(ConvertToNLogLevel(level), _nlogLogger.Name, message);
            logEvent.Exception = exception;
            logEvent.Properties["LogToSpecialFile"] = true;
            _nlogLogger.Log(logEvent);
        }
        #endregion
        #region ILogManagementService 接口实现
        public string GetLogFilePath(DateTime date)
        {
            var config = LogManager.Configuration;
            if (config == null)
            {
                // 如果 NLog 未配置，返回空或抛出异常
                return string.Empty; // 或 throw new InvalidOperationException("NLog 未配置");
            }

            // 查找第一个 FileTarget (可以根据需要调整查找逻辑)
            var fileTarget = config.AllTargets
                                   .OfType<NLog.Targets.FileTarget>()
                                   .FirstOrDefault(); // 或使用 .FirstOrDefault(t => t.Name == "logfile");

            if (fileTarget == null)
            {
                // 如果没有找到 FileTarget，返回空
                return string.Empty;
            }

            try
            {
                // 创建一个模拟的 LogEventInfo 来渲染 FileName 布局
                // 注意：这里需要设置一些上下文信息，特别是日期
                var logEventInfo = new LogEventInfo
                {
                    TimeStamp = date,
                    Level = NLog.LogLevel.Info, // Level 通常不影响文件名
                    LoggerName = "LogManagementService" // LoggerName 可能影响 ${logger} 变量
                };

                // 使用 FileTarget 的 FileName 布局渲染出实际路径
                string renderedFileName = fileTarget.FileName.Render(logEventInfo);

                // 如果路径是相对的，转换为绝对路径
                if (!Path.IsPathRooted(renderedFileName))
                {
                    renderedFileName = Path.GetFullPath(renderedFileName);
                }

                return renderedFileName;
            }
            catch (Exception ex)
            {
                // 如果渲染失败，记录内部日志（如果可能）或返回空
                // 注意：这里不能使用 _nlogLogger，因为可能造成循环或在配置时不可用
                System.Diagnostics.Debug.WriteLine($"Failed to render log file path: {ex.Message}");
                return string.Empty;
            }
        }


        public void CleanupOldLogs(int daysToKeep = 90)
        {
            try
            {
                // 尝试从配置推断日志根目录
                var config = LogManager.Configuration;
                string? logRootDirectory = null;

                if (config != null)
                {
                    // 查找 FileTarget 并尝试推断根目录
                    // 这部分逻辑可能需要根据你的具体 NLog 配置调整
                    // 例如，查找包含 ${basedir}/logs 或类似模式的路径
                    var fileTarget = config.AllTargets.OfType<NLog.Targets.FileTarget>().FirstOrDefault();
                    if (fileTarget != null)
                    {
                        // 简单示例：假设 FileName 包含 logs 目录
                        // 更健壮的方法是解析 Layout，但这很复杂
                        var dummyEvent = LogEventInfo.CreateNullEvent();
                        string renderedPath = fileTarget.FileName.Render(dummyEvent);
                        if (renderedPath.Contains("logs", StringComparison.OrdinalIgnoreCase))
                        {
                            // 粗略查找 logs 目录的父级
                            var parts = renderedPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                            int logsIndex = Array.FindIndex(parts, p => p.Equals("logs", StringComparison.OrdinalIgnoreCase));
                            if (logsIndex >= 0)
                            {
                                // 重新组合路径到 logs 目录
                                logRootDirectory = string.Join(Path.DirectorySeparatorChar.ToString(), parts.Take(logsIndex + 1));
                                if (!Path.IsPathRooted(logRootDirectory))
                                {
                                    logRootDirectory = Path.GetFullPath(logRootDirectory);
                                }
                            }
                        }
                    }
                }

                // 如果无法从配置推断，则使用默认路径 (风险在于可能与配置不符)
                if (string.IsNullOrEmpty(logRootDirectory))
                {
                    logRootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                }

                if (!Directory.Exists(logRootDirectory)) return;

                var cutoffDate = DateTime.Now.AddDays(-daysToKeep);
                var directories = Directory.GetDirectories(logRootDirectory);

                foreach (var dir in directories)
                {
                    var dirName = Path.GetFileName(dir);
                    // 假设目录名是日期格式，例如 yyyy-MM-dd
                    if (DateTime.TryParseExact(dirName, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var dirDate))
                    {
                        if (dirDate < cutoffDate)
                        {
                            try
                            {
                                Directory.Delete(dir, true);
                                // 注意：在管理服务中记录日志需谨慎，避免循环依赖
                                // 可以考虑使用 Trace 或 Debug 输出，或者记录到一个独立的管理日志文件
                                System.Diagnostics.Debug.WriteLine($"[LogManagement] 已删除过期日志目录: {dir}");
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"[LogManagement] 删除过期日志目录失败 {dir}: {ex.Message}");
                                // 或者，如果确定 LoggerHelper 已正确配置且不会因清理自身而受影响，可以记录
                                // _nlogLogger.Warn(ex, $"删除过期日志目录失败: {dir}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LogManagement] 清理过期日志时发生错误: {ex.Message}");
                // _nlogLogger.Error(ex, "清理过期日志时发生错误");
            }
        }

        public void Flush()
        {
            LogManager.Flush();
        }

        public void Shutdown()
        {
            LogManager.Shutdown();
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 转换日志级别
        /// </summary>
        private NLog.LogLevel ConvertToNLogLevel(LogLevel level)
        {
            return level switch
            {
                LogLevel.Trace => NLog.LogLevel.Trace,
                LogLevel.Debug => NLog.LogLevel.Debug,
                LogLevel.Info => NLog.LogLevel.Info,
                LogLevel.Warn => NLog.LogLevel.Warn,
                LogLevel.Error => NLog.LogLevel.Error,
                LogLevel.Fatal => NLog.LogLevel.Fatal,
                _ => NLog.LogLevel.Info
            };
        }

        #endregion
    }
}
