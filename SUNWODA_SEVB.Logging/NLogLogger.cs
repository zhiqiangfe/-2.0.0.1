using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Services;
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
        private static readonly ILogger Logger = NLog.LogManager.GetLogger(typeof(T).FullName!);

        #region ILoggerService<T> 实现

        /// <summary>
        /// 获取日志上下文类型
        /// </summary>
        public Type ContextType => typeof(T);

        #endregion

        #region 基础日志方法（仅消息）

        public void Trace(string message,bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Trace, message, null, isToDatabase, memberName, filePath, lineNumber);
        }

        public void Debug(string message, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Debug, message, null, isToDatabase, memberName, filePath, lineNumber);
        }

        public void Info(string message, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Info, message, null, isToDatabase, memberName, filePath, lineNumber);
        }

        public void Warn(string message, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Warn, message, null, isToDatabase, memberName, filePath, lineNumber);
        }

        public void Error(string message, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Error, message, null, isToDatabase, memberName, filePath, lineNumber);
        }

        public void Fatal(string message, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Fatal, message, null, isToDatabase, memberName, filePath, lineNumber);
        }

        #endregion

        #region 带异常的日志方法

        public void Trace(string message, Exception exception, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Trace, message, exception, isToDatabase, memberName, filePath, lineNumber);
        }

        public void Debug(string message, Exception exception, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Debug, message, exception, isToDatabase, memberName, filePath, lineNumber);
        }

        public void Info(string message, Exception exception, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Info, message, exception, isToDatabase, memberName, filePath, lineNumber);
        }

        public void Warn(string message, Exception exception, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Warn, message, exception, isToDatabase, memberName, filePath, lineNumber);
        }

        public void Error(string message, Exception exception, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Error, message, exception, isToDatabase, memberName, filePath, lineNumber);
        }

        public void Fatal(string message, Exception exception, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(NLog.LogLevel.Fatal, message, exception, isToDatabase, memberName, filePath, lineNumber);
        }

        #endregion

        #region 通用日志方法

        public void Log(CoreLogLevel level, string message, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(ConvertToNLogLevel(level), message, null, isToDatabase, memberName, filePath, lineNumber);
        }

        public void Log(CoreLogLevel level, string message, Exception exception, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            LogWithCallSite(ConvertToNLogLevel(level), message, exception, isToDatabase, memberName, filePath, lineNumber);
        }

        #endregion

        #region 特殊日志方法
        /// <summary>
        /// 记录到特殊日志文件
        /// </summary>
        public void LogToSpecialFile(string fileName, string message, CoreLogLevel level = CoreLogLevel.Info, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            // 使用文件名创建特定的logger
            var specialLogger = NLog.LogManager.GetLogger($"SpecialFile.{fileName}");
            var nlogLevel = ConvertToNLogLevel(level);

            var logEvent = new NLog.LogEventInfo(nlogLevel, specialLogger.Name, message);

            // 设置特殊文件标记
            logEvent.Properties["SpecialFileName"] = fileName;

            // 设置是否记录到数据库
            logEvent.Properties["IsToDatabase"] = isToDatabase;

            // 设置调用位置信息
            if (!string.IsNullOrEmpty(filePath))
            {
                string className = typeof(T).Name;
                logEvent.SetCallerInfo(className, memberName, filePath, lineNumber);
            }

            specialLogger.Log(typeof(NLogLogger<T>), logEvent);
        }

        public void LogSpecial(CoreLogLevel level, string message, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            var logEvent = CreateLogEventWithCallSite(ConvertToNLogLevel(level), message, null, isToDatabase, memberName, filePath, lineNumber);
            logEvent.Properties["LogToSpecialFile"] = true;
            Logger.Log(typeof(NLogLogger<T>), logEvent);
        }

        public void LogSpecial(CoreLogLevel level, string message, Exception exception, bool isToDatabase = false,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            var logEvent = CreateLogEventWithCallSite(ConvertToNLogLevel(level), message, exception, isToDatabase, memberName, filePath, lineNumber);
            logEvent.Properties["LogToSpecialFile"] = true;
            Logger.Log(typeof(NLogLogger<T>), logEvent);
        }

        #endregion

        #region 接口日志方法
        /// <summary>
        /// 记录MES接口日志
        /// </summary>
        public void LogMesInterface(string interfaceName, string requestData, string responseData,
            bool isSuccess, int executionTime = 0, string errorMessage = null!)
        {
            var logger = NLog.LogManager.GetLogger("MesInterface." + interfaceName);
            var logEvent = NLog.LogEventInfo.Create(
                isSuccess ? NLog.LogLevel.Info : NLog.LogLevel.Error,
                logger.Name,
                $"MES接口调用: {interfaceName}");

            logEvent.Properties["InterfaceName"] = interfaceName;
            logEvent.Properties["RequestData"] = requestData;
            logEvent.Properties["ResponseData"] = responseData;
            logEvent.Properties["IsSuccess"] = isSuccess;
            logEvent.Properties["ExecutionTime"] = executionTime;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                logEvent.Exception = new Exception(errorMessage);
            }

            logger.Log(logEvent);
        }

        /// <summary>
        /// 记录Web接口日志
        /// </summary>
        public void LogWebInterface(string apiPath, string httpMethod, string requestBody,
            string responseBody, string clientIP, int statusCode, long executionTime)
        {
            var logger = NLog.LogManager.GetLogger("WebInterface." + apiPath.Replace("/", "."));
            var logEvent = NLog.LogEventInfo.Create(
                statusCode >= 200 && statusCode < 300 ? NLog.LogLevel.Info : NLog.LogLevel.Error,
                logger.Name,
                $"Web API调用: {httpMethod} {apiPath}");

            logEvent.Properties["ApiPath"] = apiPath;
            logEvent.Properties["HttpMethod"] = httpMethod;
            logEvent.Properties["RequestBody"] = requestBody;
            logEvent.Properties["ResponseBody"] = responseBody;
            logEvent.Properties["ClientIP"] = clientIP;
            logEvent.Properties["StatusCode"] = statusCode;
            logEvent.Properties["ExecutionTime"] = executionTime;

            logger.Log(logEvent);
        }
        #endregion



        #region 私有辅助方法

        /// <summary>
        /// 核心日志记录方法，包含调用位置信息
        /// </summary>
        private void LogWithCallSite(NLog.LogLevel level, string message, Exception? exception, bool isToDatabase,
            string memberName, string filePath, int lineNumber)
        {
            var logEvent = CreateLogEventWithCallSite(level, message, exception, isToDatabase, memberName, filePath, lineNumber);
            Logger.Log(typeof(NLogLogger<T>), logEvent);
        }

        /// <summary>
        /// 创建包含调用位置信息的 LogEventInfo
        /// </summary>
        private NLog.LogEventInfo CreateLogEventWithCallSite(NLog.LogLevel level, string message, Exception? exception, bool isToDatabase,
            string memberName, string filePath, int lineNumber)
        {
            var logEvent = new NLog.LogEventInfo(level, Logger.Name, message);
            logEvent.Exception = exception;

            // 设置是否记录到数据库
            logEvent.Properties["IsToDatabase"] = isToDatabase;

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
        private static NLog.LogLevel ConvertToNLogLevel(CoreLogLevel level)
        {
            return level switch
            {
                CoreLogLevel.Trace => NLog.LogLevel.Trace,
                CoreLogLevel.Debug => NLog.LogLevel.Debug,
                CoreLogLevel.Info => NLog.LogLevel.Info,
                CoreLogLevel.Warn => NLog.LogLevel.Warn,
                CoreLogLevel.Error => NLog.LogLevel.Error,
                CoreLogLevel.Fatal => NLog.LogLevel.Fatal,
                _ => NLog.LogLevel.Debug, // 默认级别
            };
        }

        #endregion
    }
}
