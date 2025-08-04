using System;
using SUNWODA_SEVB.Core.Services;
using System.Runtime.CompilerServices;
using SUNWODA_SEVB.Core.Interfaces;

namespace SUNWODA_SEVB.Logging
{
    /// <summary>
    ///  日志服务基类，提供通用实现
    /// </summary>
    public abstract class ILoggerServiceBase : ILoggerService
    {
        // 基础日志方法（仅消息）
        public void Trace(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Log(CoreLogLevel.Trace, message, memberName, filePath, lineNumber);
        }

        public void Debug(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Log(CoreLogLevel.Debug, message, memberName, filePath, lineNumber);
        }

        public void Info(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Log(CoreLogLevel.Info, message, memberName, filePath, lineNumber);
        }

        public void Warn(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Log(CoreLogLevel.Warn, message, memberName, filePath, lineNumber);
        }

        public void Error(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Log(CoreLogLevel.Error, message, memberName, filePath, lineNumber);
        }

        public void Fatal(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Log(CoreLogLevel.Fatal, message, memberName, filePath, lineNumber);
        }

        // 带异常的日志方法
        public void Trace(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Log(CoreLogLevel.Trace, message, exception, memberName, filePath, lineNumber);
        }

        public void Debug(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Log(CoreLogLevel.Debug, message, exception, memberName, filePath, lineNumber);
        }

        public void Info(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Log(CoreLogLevel.Info, message, exception, memberName, filePath, lineNumber);
        }

        public void Warn(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Log(CoreLogLevel.Warn, message, exception, memberName, filePath, lineNumber);
        }

        public void Error(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Log(CoreLogLevel.Error, message, exception, memberName, filePath, lineNumber);
        }

        public void Fatal(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Log(CoreLogLevel.Fatal, message, exception, memberName, filePath, lineNumber);
        }

        // 通用日志方法
        public abstract void Log(CoreLogLevel level, string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        public abstract void Log(CoreLogLevel level, string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        // 特殊日志方法
        public abstract void LogSpecial(CoreLogLevel level, string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        public abstract void LogSpecialException(CoreLogLevel level, string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        // 接口日志
        public abstract void LogToSpecialFile(string fileName, string message, CoreLogLevel level = CoreLogLevel.Info,
           [CallerMemberName] string memberName = "",
           [CallerFilePath] string filePath = "",
           [CallerLineNumber] int lineNumber = 0);

        public abstract void LogMesInterface(string interfaceName, string requestData, string responseData,
            bool isSuccess, int executionTime = 0, string errorMessage = null!);

        public abstract void LogWebInterface(string apiPath, string httpMethod, string requestBody,
            string responseBody, string clientIP, int statusCode, long executionTime);

        #region 辅助方法

        /// <summary>
        /// 格式化日志消息
        /// </summary>
        protected virtual string FormatMessage(string message, string memberName, string filePath, int lineNumber)
        {
            var fileName = System.IO.Path.GetFileName(filePath);
            return $"[{fileName}:{lineNumber}] {memberName} - {message}";
        }

        /// <summary>
        /// 格式化异常信息
        /// </summary>
        protected virtual string FormatException(Exception exception)
        {
            return exception?.ToString() ?? string.Empty;
        }

        #endregion
    }

    /// <summary>
    /// 泛型日志服务基类
    /// </summary>
    public abstract class LoggerServiceBase<T> : ILoggerServiceBase, ILoggerService<T>
    {
        public Type ContextType => typeof(T);

        protected string ContextTypeName => typeof(T).Name;

        protected override string FormatMessage(string message, string memberName, string filePath, int lineNumber)
        {
            var fileName = System.IO.Path.GetFileName(filePath);
            return $"[{ContextTypeName}] [{fileName}:{lineNumber}] {memberName} - {message}";
        }
    }
}
