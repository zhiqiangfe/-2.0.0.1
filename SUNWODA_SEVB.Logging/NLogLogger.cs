using NLog;
using System;
using System.Runtime.CompilerServices;
using ILogger = NLog.ILogger;

namespace SUNWODA_SEVB.Logging
{
    /// <summary>
    /// 使用 NLog 实现的泛型日志记录器
    /// </summary>
    /// <typeparam name="T">日志上下文类型</typeparam>
    public class NLogLogger<T> : ILoggerService<T>
    {
        // 从 NLog 获取与泛型类型 T 关联的 logger 实例
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(T).FullName!);

        #region 基础日志方法（仅消息）

        public void Trace(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Trace, message, null, memberName, filePath, lineNumber);
        }

        public void Debug(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Debug, message, null, memberName, filePath, lineNumber);
        }

        public void Info(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Info, message, null, memberName, filePath, lineNumber);
        }

        public void Warn(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Warn, message, null, memberName, filePath, lineNumber);
        }

        public void Error(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Error, message, null, memberName, filePath, lineNumber);
        }

        public void Fatal(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Fatal, message, null, memberName, filePath, lineNumber);
        }

        #endregion

        #region 带异常的日志方法

        public void TraceException(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Trace, message, exception, memberName, filePath, lineNumber);
        }

        public void DebugException(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Debug, message, exception, memberName, filePath, lineNumber);
        }

        public void InfoException(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Info, message, exception, memberName, filePath, lineNumber);
        }

        public void WarnException(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Warn, message, exception, memberName, filePath, lineNumber);
        }

        public void ErrorException(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Error, message, exception, memberName, filePath, lineNumber);
        }

        public void FatalException(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Fatal, message, exception, memberName, filePath, lineNumber);
        }

        #endregion

        #region 通用日志方法

        public void Log(LogLevel level, string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(ConvertToNLogLevel(level), message, null, memberName, filePath, lineNumber);
        }

        public void LogException(LogLevel level, string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(ConvertToNLogLevel(level), message, exception, memberName, filePath, lineNumber);
        }

        #endregion

        #region 特殊日志方法

        public void LogSpecial(LogLevel level, string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            var logEvent = CreateLogEventWithCallSite(ConvertToNLogLevel(level), message, null, memberName, filePath, lineNumber);
            logEvent.Properties["LogToSpecialFile"] = true;
            Logger.Log(typeof(NLogLogger<T>), logEvent);
        }

        public void LogSpecialException(LogLevel level, string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            var logEvent = CreateLogEventWithCallSite(ConvertToNLogLevel(level), message, exception, memberName, filePath, lineNumber);
            logEvent.Properties["LogToSpecialFile"] = true;
            Logger.Log(typeof(NLogLogger<T>), logEvent);
        }

        #endregion

        #region 私有辅助方法

        /// <summary>
        /// 核心日志记录方法，包含调用位置信息
        /// </summary>
        private void LogWithCallSite(NLog.LogLevel level, string message, Exception? exception,
            string memberName, string filePath, int lineNumber)
        {
            var logEvent = CreateLogEventWithCallSite(level, message, exception, memberName, filePath, lineNumber);
            Logger.Log(typeof(NLogLogger<T>), logEvent);
        }

        /// <summary>
        /// 创建包含调用位置信息的 LogEventInfo
        /// </summary>
        private LogEventInfo CreateLogEventWithCallSite(NLog.LogLevel level, string message, Exception? exception,
            string memberName, string filePath, int lineNumber)
        {
            var logEvent = new LogEventInfo(level, Logger.Name, message);
            logEvent.Exception = exception;

            // 设置调用位置信息
            if (!string.IsNullOrEmpty(filePath))
            {
                // 使用泛型类型 T 的名称作为类名
                string className = typeof(T).Name;

                // 设置 CallSite 信息
                logEvent.SetCallerInfo(className, memberName, filePath, lineNumber);
            }

            return logEvent;
        }

        /// <summary>
        /// 将自定义的 LogLevel 转换为 NLog 的 LogLevel
        /// </summary>
        private static NLog.LogLevel ConvertToNLogLevel(LogLevel level)
        {
            return level switch
            {
                LogLevel.Trace => NLog.LogLevel.Trace,
                LogLevel.Debug => NLog.LogLevel.Debug,
                LogLevel.Info => NLog.LogLevel.Info,
                LogLevel.Warn => NLog.LogLevel.Warn,
                LogLevel.Error => NLog.LogLevel.Error,
                LogLevel.Fatal => NLog.LogLevel.Fatal,
                _ => NLog.LogLevel.Debug, // 默认级别
            };
        }

        #endregion
    }
}
