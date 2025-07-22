using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Logging
{
    /// <summary>
    /// 日志级别枚举
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 跟踪级别（最详细）
        /// </summary>
        Trace = 0,

        /// <summary>
        /// 调试级别
        /// </summary>
        Debug = 1,

        /// <summary>
        /// 信息级别
        /// </summary>
        Info = 2,

        /// <summary>
        /// 警告级别
        /// </summary>
        Warn = 3,

        /// <summary>
        /// 错误级别
        /// </summary>
        Error = 4,

        /// <summary>
        /// 致命错误级别
        /// </summary>
        Fatal = 5
    }
}
