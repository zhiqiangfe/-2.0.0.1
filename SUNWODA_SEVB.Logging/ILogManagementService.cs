using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Logging
{
    /// <summary>
    /// 定义日志管理相关的功能接口
    /// </summary>
    public interface ILogManagementService
    {
        /// <summary>
        /// 获取指定日期的日志文件路径
        /// </summary>
        string GetLogFilePath(DateTime date);

        /// <summary>
        /// 清理过期的日志文件
        /// </summary>
        void CleanupOldLogs(int daysToKeep = 90);

        /// <summary>
        /// 将日志缓冲区的内容立即写入目标
        /// </summary>
        void Flush();

        /// <summary>
        /// 关闭日志系统，释放资源
        /// </summary>
        void Shutdown();
    }
}
