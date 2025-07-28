using System;
using System.Runtime.CompilerServices;

namespace SUNWODA_SEVB.Logging
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILoggerService
    {
        // 基础日志方法（仅消息）
        void Trace(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        void Debug(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        void Info(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        void Warn(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        void Error(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        void Fatal(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        // 带异常的日志方法（使用不同的方法名避免重载冲突）
        void TraceException(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        void DebugException(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        void InfoException(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        void WarnException(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        void ErrorException(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        void FatalException(string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        // 通用日志方法
        void Log(LogLevel level, string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        void LogException(LogLevel level, string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        // 特殊日志方法
        void LogSpecial(LogLevel level, string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);

        void LogSpecialException(LogLevel level, string message, Exception exception,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0);
    }

    /// <summary>
    /// 供依赖注入使用的泛型日志接口，用于获取特定类型的日志记录器
    /// </summary>
    /// <typeparam name="T">需要记录日志的上下文类型（通常是类本身）</typeparam>
    public interface ILoggerService<T> : ILoggerService
    {
    }
}
