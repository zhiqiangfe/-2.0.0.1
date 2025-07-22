using NLog.Config;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Logging
{
    /// <summary>
    /// NLog 日志帮助类
    /// </summary>
    public class LoggerHelper : ILoggerService
    {
        private static readonly Lazy<LoggerHelper> _instance = new Lazy<LoggerHelper>(() => new LoggerHelper());
        private readonly Logger _logger;
        private static readonly object _lock = new object();

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static LoggerHelper Instance => _instance.Value;

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private LoggerHelper()
        {
            InitializeNLog();
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// 初始化 NLog 配置
        /// </summary>
        private void InitializeNLog()
        {
            try
            {
                // 获取配置文件路径
                var configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NLog.config");

                // 如果配置文件不存在，从嵌入资源中创建
                if (!File.Exists(configFile))
                {
                    CreateDefaultConfig(configFile);
                }

                // 加载配置
                LogManager.Configuration = new XmlLoggingConfiguration(configFile);
            }
            catch (Exception ex)
            {
                // 如果配置加载失败，使用默认配置
                CreateProgrammaticConfig();
                _logger?.Error(ex, "加载 NLog 配置文件失败，使用默认配置");
            }
        }

        /// <summary>
        /// 创建默认配置文件
        /// </summary>
        private void CreateDefaultConfig(string configFile)
        {
            var defaultConfig = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                <nlog xmlns=""http://www.nlog-project.org/schemas/NLog.xsd""
                      xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                      autoReload=""true""
                      throwExceptions=""false"">

                  <variable name=""logDirectory"" value=""${basedir}/logs""/>
  
                  <targets>
                    <target xsi:type=""File"" 
                            name=""logfile"" 
                            fileName=""${logDirectory}/${shortdate}/${date:format=yyyy-MM-dd-HH}.txt""
                            layout=""${longdate} | ${level:uppercase=true:padding=5} | ${logger} | ${message} ${exception:format=tostring}""
                            encoding=""utf-8""
                            createDirs=""true""/>

                    <target xsi:type=""File"" 
                            name=""errorfile"" 
                            fileName=""${logDirectory}/${shortdate}/error-${date:format=yyyy-MM-dd-HH}.txt""
                            layout=""${longdate} | ${level:uppercase=true:padding=5} | ${logger} | ${message} | ${exception:format=tostring} | ${stacktrace}""
                            encoding=""utf-8""
                            createDirs=""true""/>
                  </targets>

                  <rules>
                    <logger name=""*"" minlevel=""Trace"" writeTo=""logfile"" />
                    <logger name=""*"" minlevel=""Error"" writeTo=""errorfile"" />
                  </rules>
                </nlog>";

            File.WriteAllText(configFile, defaultConfig);
        }

        /// <summary>
        /// 通过代码创建配置（备用方案）
        /// </summary>
        private void CreateProgrammaticConfig()
        {
            var config = new LoggingConfiguration();

            // 创建文件目标
            var fileTarget = new NLog.Targets.FileTarget("logfile")
            {
                FileName = "${basedir}/logs/${shortdate}/${date:format=yyyy-MM-dd-HH}.txt",
                Layout = "${longdate} | ${level:uppercase=true:padding=5} | ${logger} | ${message} ${exception:format=tostring}",
                Encoding = System.Text.Encoding.UTF8,
                CreateDirs = true,
                KeepFileOpen = true,
                AutoFlush = true,
                OpenFileCacheTimeout = 30
            };

            var specialFileTarget = new NLog.Targets.FileTarget("specialfile")
            {
                FileName = "${basedir}/logs/${shortdate}/error-${date:format=yyyy-MM-dd-HH}.txt",
                Layout = "${longdate} | ${level:uppercase=true:padding=5} | ${logger} | ${message} | ${exception:format=tostring} | ${stacktrace}",
                Encoding = System.Text.Encoding.UTF8,
                CreateDirs = true,
                KeepFileOpen = true,
                AutoFlush = true,
                OpenFileCacheTimeout = 30
            };

            // 添加规则
            config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, fileTarget);
            config.AddRule(NLog.LogLevel.Error, NLog.LogLevel.Fatal, specialFileTarget);

            // 应用配置
            LogManager.Configuration = config;
        }

        #region ILoggerService 实现

        public void Trace(string message)
        {
            lock (_lock)
            {
                _logger.Trace(message);
            }
        }

        public void Debug(string message)
        {
            lock (_lock)
            {
                _logger.Debug(message);
            }
        }

        public void Info(string message)
        {
            lock (_lock)
            {
                _logger.Info(message);
            }
        }

        public void Warn(string message)
        {
            lock (_lock)
            {
                _logger.Warn(message);
            }
        }

        public void Error(string message)
        {
            lock (_lock)
            {
                _logger.Error(message);
            }
        }

        public void Error(string message, Exception exception)
        {
            lock (_lock)
            {
                _logger.Error(exception, message);
            }
        }

        public void Fatal(string message)
        {
            lock (_lock)
            {
                _logger.Fatal(message);
            }
        }

        public void Fatal(string message, Exception exception)
        {
            lock (_lock)
            {
                _logger.Fatal(exception, message);
            }
        }

        public void Log(LogLevel level, string message)
        {
            lock (_lock)
            {
                var nlogLevel = ConvertToNLogLevel(level);
                _logger.Log(nlogLevel, message);
            }
        }

        public void Log(LogLevel level, string message, Exception exception)
        {
            lock (_lock)
            {
                var nlogLevel = ConvertToNLogLevel(level);
                _logger.Log(nlogLevel, exception, message);
            }
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

        /// <summary>
        /// 获取指定日期的日志文件路径
        /// </summary>
        public string GetLogFilePath(DateTime date)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var dateStr = date.ToString("yyyy-MM-dd");
            var hourStr = date.ToString("yyyy-MM-dd-HH");
            return Path.Combine(baseDir, "logs", dateStr, $"{hourStr}.txt");
        }

        /// <summary>
        /// 清理过期日志（保留指定天数）
        /// </summary>
        public void CleanupOldLogs(int daysToKeep = 30)
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
                    if (DateTime.TryParse(dirName, out var dirDate))
                    {
                        if (dirDate < cutoffDate)
                        {
                            Directory.Delete(dir, true);
                            Info($"已删除过期日志目录: {dir}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error("清理过期日志时发生错误", ex);
            }
        }

        /// <summary>
        /// 刷新日志缓冲区
        /// </summary>
        public void Flush()
        {
            LogManager.Flush();
        }

        /// <summary>
        /// 关闭日志系统
        /// </summary>
        public void Shutdown()
        {
            LogManager.Shutdown();
        }

        #endregion
    }
}
