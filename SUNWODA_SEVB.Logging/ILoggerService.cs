using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Logging
{
    /// <summary>
    /// 日志服务接口
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// 记录跟踪日志
        /// </summary>
        void Trace(string message);

        /// <summary>
        /// 记录调试日志
        /// </summary>
        void Debug(string message);

        /// <summary>
        /// 记录信息日志
        /// </summary>
        void Info(string message);

        /// <summary>
        /// 记录警告日志
        /// </summary>
        void Warn(string message);

        /// <summary>
        /// 记录错误日志
        /// </summary>
        void Error(string message);

        /// <summary>
        /// 记录错误日志（带异常）
        /// </summary>
        void Error(string message, Exception exception);

        /// <summary>
        /// 记录致命错误日志
        /// </summary>
        void Fatal(string message);

        /// <summary>
        /// 记录致命错误日志（带异常）
        /// </summary>
        void Fatal(string message, Exception exception);

        /// <summary>
        /// 记录日志（指定级别）
        /// </summary>
        void Log(LogLevel level, string message);

        /// <summary>
        /// 记录日志（指定级别，带异常）
        /// </summary>
        void Log(LogLevel level, string message, Exception exception);
    }
}
