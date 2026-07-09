using HTHIUM.Core.Enumerations.Logging;

namespace HTHIUM.Core.Interfaces
{
    /// <summary>
    /// 日志管理服务接口
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

        /// <summary>
        /// 重新配置日志系统
        /// </summary>
        void Reconfigure();

        /// <summary>
        /// 获取日志文件列表
        /// </summary>
        /// <param name="logDirectory">日志目录路径（可选）</param>
        /// <returns>日志文件路径集合</returns>
        IEnumerable<string> GetLogFiles(string logDirectory = null!);

        /// <summary>
        /// 获取当前日志配置信息
        /// </summary>
        LogConfiguration GetConfiguration();

        /// <summary>
        /// 更新日志配置
        /// </summary>
        void UpdateConfiguration(LogConfiguration configuration);
    }

    /// <summary>
    /// 日志配置类
    /// </summary>
    public class LogConfiguration
    {
        public CoreLogLevel MinimumLevel { get; set; } = CoreLogLevel.Info;
        public string LogDirectory { get; set; } = null!;
        public int MaxFileSizeInMB { get; set; } = 10;
        public int MaxBackupFiles { get; set; } = 10;
        public bool EnableConsoleOutput { get; set; } = true;
        public bool EnableFileOutput { get; set; } = true;
    }
}
