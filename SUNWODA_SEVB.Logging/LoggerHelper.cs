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

namespace SUNWODA_SEVB.Logging
{
    /// <summary>
    /// NLog 日志帮助类
    /// </summary>
    public class LoggerHelper : ILogger
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
        #region ILogger 接口属性实现 (代理到内部 _logger 实例)

        public event EventHandler<EventArgs> LoggerReconfigured
        {
            add => _logger.LoggerReconfigured += value;
            remove => _logger.LoggerReconfigured -= value;
        }

        public bool IsTraceEnabled => _logger.IsTraceEnabled;
        public bool IsDebugEnabled => _logger.IsDebugEnabled;
        public bool IsInfoEnabled => _logger.IsInfoEnabled;
        public bool IsWarnEnabled => _logger.IsWarnEnabled;
        public bool IsErrorEnabled => _logger.IsErrorEnabled;
        public bool IsFatalEnabled => _logger.IsFatalEnabled;
        public string Name => _logger.Name;
        public LogFactory Factory => _logger.Factory;

        #endregion
        #region 初始化配置
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
                FileName = "${basedir}/logs/${shortdate}/error-${date:format=yyyy-MM-dd}.txt",
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
        public void CleanupOldLogs(int daysToKeep = 90)
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

        #region 接口      

        public void Trace(object? value)
        {
            lock (_lock)
            {
                _logger.Trace(value);
            }
        }

        public void Trace(IFormatProvider? formatProvider, object? value)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, value);
            }
        }

        public void Trace([Localizable(false)] string message, object? arg1, object? arg2)
        {
            lock (_lock)
            {
                _logger.Trace(message, arg1, arg2);
            }
        }

        public void Trace([Localizable(false)] string message, object? arg1, object? arg2, object? arg3)
        {
            lock (_lock)
            {
                _logger.Trace(message, arg1, arg2, arg3);
            }
        }

        public void Trace(IFormatProvider? formatProvider, [Localizable(false)] string message, bool argument)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument);
            }
        }

        public void Trace([Localizable(false)] string message, bool argument)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument);
            }
        }

        public void Trace(IFormatProvider? formatProvider, [Localizable(false)] string message, char argument)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument);
            }
        }

        public void Trace([Localizable(false)] string message, char argument)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument);
            }
        }

        public void Trace(IFormatProvider? formatProvider, [Localizable(false)] string message, byte argument)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument);
            }
        }

        public void Trace([Localizable(false)] string message, byte argument)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument);
            }
        }

        public void Trace(IFormatProvider? formatProvider, [Localizable(false)] string message, string? argument)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument);
            }
        }

        public void Trace([Localizable(false)] string message, string? argument)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument);
            }
        }

        public void Trace(IFormatProvider? formatProvider, [Localizable(false)] string message, int argument)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument);
            }
        }

        public void Trace([Localizable(false)] string message, int argument)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument);
            }
        }

        public void Trace(IFormatProvider? formatProvider, [Localizable(false)] string message, long argument)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument);
            }
        }

        public void Trace([Localizable(false)] string message, long argument)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument);
            }
        }

        public void Trace(IFormatProvider? formatProvider, [Localizable(false)] string message, float argument)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument);
            }
        }

        public void Trace([Localizable(false)] string message, float argument)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument);
            }
        }

        public void Trace(IFormatProvider? formatProvider, [Localizable(false)] string message, double argument)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument);
            }
        }

        public void Trace([Localizable(false)] string message, double argument)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument);
            }
        }

        public void Trace(IFormatProvider? formatProvider, [Localizable(false)] string message, decimal argument)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument);
            }
        }

        public void Trace([Localizable(false)] string message, decimal argument)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument);
            }
        }

        public void Trace(IFormatProvider? formatProvider, [Localizable(false)] string message, object? argument)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument);
            }
        }

        public void Trace([Localizable(false)] string message, object? argument)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument);
            }
        }

        public void Trace(IFormatProvider? formatProvider, [Localizable(false)] string message, sbyte argument)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument);
            }
        }

        public void Trace([Localizable(false)] string message, sbyte argument)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument);
            }
        }

        public void Trace(IFormatProvider? formatProvider, [Localizable(false)] string message, uint argument)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument);
            }
        }

        public void Trace([Localizable(false)] string message, uint argument)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument);
            }
        }

        public void Trace(IFormatProvider? formatProvider, [Localizable(false)] string message, ulong argument)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument);
            }
        }

        public void Trace([Localizable(false)] string message, ulong argument)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument);
            }
        }

        public void Debug(object? value)
        {
            lock (_lock)
            {
                _logger.Debug(value);
            }
        }

        public void Debug(IFormatProvider? formatProvider, object? value)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, value);
            }
        }

        public void Debug([Localizable(false)] string message, object? arg1, object? arg2)
        {
            lock (_lock)
            {
                _logger.Debug(message, arg1, arg2);
            }
        }

        public void Debug([Localizable(false)] string message, object? arg1, object? arg2, object? arg3)
        {
            lock (_lock)
            {
                _logger.Debug(message, arg1, arg2, arg3);
            }
        }

        public void Debug(IFormatProvider? formatProvider, [Localizable(false)] string message, bool argument)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument);
            }
        }

        public void Debug([Localizable(false)] string message, bool argument)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument);
            }
        }

        public void Debug(IFormatProvider? formatProvider, [Localizable(false)] string message, char argument)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument);
            }
        }

        public void Debug([Localizable(false)] string message, char argument)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument);
            }
        }

        public void Debug(IFormatProvider? formatProvider, [Localizable(false)] string message, byte argument)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument);
            }
        }

        public void Debug([Localizable(false)] string message, byte argument)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument);
            }
        }

        public void Debug(IFormatProvider? formatProvider, [Localizable(false)] string message, string? argument)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument);
            }
        }

        public void Debug([Localizable(false)] string message, string? argument)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument);
            }
        }

        public void Debug(IFormatProvider? formatProvider, [Localizable(false)] string message, int argument)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument);
            }
        }

        public void Debug([Localizable(false)] string message, int argument)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument);
            }
        }

        public void Debug(IFormatProvider? formatProvider, [Localizable(false)] string message, long argument)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument);
            }
        }

        public void Debug([Localizable(false)] string message, long argument)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument);
            }
        }

        public void Debug(IFormatProvider? formatProvider, [Localizable(false)] string message, float argument)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument);
            }
        }

        public void Debug([Localizable(false)] string message, float argument)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument);
            }
        }

        public void Debug(IFormatProvider? formatProvider, [Localizable(false)] string message, double argument)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument);
            }
        }

        public void Debug([Localizable(false)] string message, double argument)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument);
            }
        }

        public void Debug(IFormatProvider? formatProvider, [Localizable(false)] string message, decimal argument)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument);
            }
        }

        public void Debug([Localizable(false)] string message, decimal argument)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument);
            }
        }

        public void Debug(IFormatProvider? formatProvider, [Localizable(false)] string message, object? argument)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument);
            }
        }

        public void Debug([Localizable(false)] string message, object? argument)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument);
            }
        }

        public void Debug(IFormatProvider? formatProvider, [Localizable(false)] string message, sbyte argument)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument);
            }
        }

        public void Debug([Localizable(false)] string message, sbyte argument)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument);
            }
        }

        public void Debug(IFormatProvider? formatProvider, [Localizable(false)] string message, uint argument)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument);
            }
        }

        public void Debug([Localizable(false)] string message, uint argument)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument);
            }
        }

        public void Debug(IFormatProvider? formatProvider, [Localizable(false)] string message, ulong argument)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument);
            }
        }

        public void Debug([Localizable(false)] string message, ulong argument)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument);
            }
        }

        public void Info(object? value)
        {
            lock (_lock)
            {
                _logger.Info(value);
            }
        }

        public void Info(IFormatProvider? formatProvider, object? value)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, value);
            }
        }

        public void Info([Localizable(false)] string message, object? arg1, object? arg2)
        {
            lock (_lock)
            {
                _logger.Info(message, arg1, arg2);
            }
        }

        public void Info([Localizable(false)] string message, object? arg1, object? arg2, object? arg3)
        {
            lock (_lock)
            {
                _logger.Info(message, arg1, arg2, arg3);
            }
        }

        public void Info(IFormatProvider? formatProvider, [Localizable(false)] string message, bool argument)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument);
            }
        }

        public void Info([Localizable(false)] string message, bool argument)
        {
            lock (_lock)
            {
                _logger.Info(message, argument);
            }
        }

        public void Info(IFormatProvider? formatProvider, [Localizable(false)] string message, char argument)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument);
            }
        }

        public void Info([Localizable(false)] string message, char argument)
        {
            lock (_lock)
            {
                _logger.Info(message, argument);
            }
        }

        public void Info(IFormatProvider? formatProvider, [Localizable(false)] string message, byte argument)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument);
            }
        }

        public void Info([Localizable(false)] string message, byte argument)
        {
            lock (_lock)
            {
                _logger.Info(message, argument);
            }
        }

        public void Info(IFormatProvider? formatProvider, [Localizable(false)] string message, string? argument)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument);
            }
        }

        public void Info([Localizable(false)] string message, string? argument)
        {
            lock (_lock)
            {
                _logger.Info(message, argument);
            }
        }

        public void Info(IFormatProvider? formatProvider, [Localizable(false)] string message, int argument)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument);
            }
        }

        public void Info([Localizable(false)] string message, int argument)
        {
            lock (_lock)
            {
                _logger.Info(message, argument);
            }
        }

        public void Info(IFormatProvider? formatProvider, [Localizable(false)] string message, long argument)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument);
            }
        }

        public void Info([Localizable(false)] string message, long argument)
        {
            lock (_lock)
            {
                _logger.Info(message, argument);
            }
        }

        public void Info(IFormatProvider? formatProvider, [Localizable(false)] string message, float argument)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument);
            }
        }

        public void Info([Localizable(false)] string message, float argument)
        {
            lock (_lock)
            {
                _logger.Info(message, argument);
            }
        }

        public void Info(IFormatProvider? formatProvider, [Localizable(false)] string message, double argument)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument);
            }
        }

        public void Info([Localizable(false)] string message, double argument)
        {
            lock (_lock)
            {
                _logger.Info(message, argument);
            }
        }

        public void Info(IFormatProvider? formatProvider, [Localizable(false)] string message, decimal argument)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument);
            }
        }

        public void Info([Localizable(false)] string message, decimal argument)
        {
            lock (_lock)
            {
                _logger.Info(message, argument);
            }
        }

        public void Info(IFormatProvider? formatProvider, [Localizable(false)] string message, object? argument)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument);
            }
        }

        public void Info([Localizable(false)] string message, object? argument)
        {
            lock (_lock)
            {
                _logger.Info(message, argument);
            }
        }

        public void Info(IFormatProvider? formatProvider, [Localizable(false)] string message, sbyte argument)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument);
            }
        }

        public void Info([Localizable(false)] string message, sbyte argument)
        {
            lock (_lock)
            {
                _logger.Info(message, argument);
            }
        }

        public void Info(IFormatProvider? formatProvider, [Localizable(false)] string message, uint argument)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument);
            }
        }

        public void Info([Localizable(false)] string message, uint argument)
        {
            lock (_lock)
            {
                _logger.Info(message, argument);
            }
        }

        public void Info(IFormatProvider? formatProvider, [Localizable(false)] string message, ulong argument)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument);
            }
        }

        public void Info([Localizable(false)] string message, ulong argument)
        {
            lock (_lock)
            {
                _logger.Info(message, argument);
            }
        }

        public void Warn(object? value)
        {
            lock (_lock)
            {
                _logger.Warn(value);
            }
        }

        public void Warn(IFormatProvider? formatProvider, object? value)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, value);
            }
        }

        public void Warn([Localizable(false)] string message, object? arg1, object? arg2)
        {
            lock (_lock)
            {
                _logger.Warn(message, arg1, arg2);
            }
        }

        public void Warn([Localizable(false)] string message, object? arg1, object? arg2, object? arg3)
        {
            lock (_lock)
            {
                _logger.Warn(message, arg1, arg2, arg3);
            }
        }

        public void Warn(IFormatProvider? formatProvider, [Localizable(false)] string message, bool argument)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument);
            }
        }

        public void Warn([Localizable(false)] string message, bool argument)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument);
            }
        }

        public void Warn(IFormatProvider? formatProvider, [Localizable(false)] string message, char argument)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument);
            }
        }

        public void Warn([Localizable(false)] string message, char argument)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument);
            }
        }

        public void Warn(IFormatProvider? formatProvider, [Localizable(false)] string message, byte argument)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument);
            }
        }

        public void Warn([Localizable(false)] string message, byte argument)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument);
            }
        }

        public void Warn(IFormatProvider? formatProvider, [Localizable(false)] string message, string? argument)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument);
            }
        }

        public void Warn([Localizable(false)] string message, string? argument)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument);
            }
        }

        public void Warn(IFormatProvider? formatProvider, [Localizable(false)] string message, int argument)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument);
            }
        }

        public void Warn([Localizable(false)] string message, int argument)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument);
            }
        }

        public void Warn(IFormatProvider? formatProvider, [Localizable(false)] string message, long argument)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument);
            }
        }

        public void Warn([Localizable(false)] string message, long argument)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument);
            }
        }

        public void Warn(IFormatProvider? formatProvider, [Localizable(false)] string message, float argument)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument);
            }
        }

        public void Warn([Localizable(false)] string message, float argument)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument);
            }
        }

        public void Warn(IFormatProvider? formatProvider, [Localizable(false)] string message, double argument)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument);
            }
        }

        public void Warn([Localizable(false)] string message, double argument)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument);
            }
        }

        public void Warn(IFormatProvider? formatProvider, [Localizable(false)] string message, decimal argument)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument);
            }
        }

        public void Warn([Localizable(false)] string message, decimal argument)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument);
            }
        }

        public void Warn(IFormatProvider? formatProvider, [Localizable(false)] string message, object? argument)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument);
            }
        }

        public void Warn([Localizable(false)] string message, object? argument)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument);
            }
        }

        public void Warn(IFormatProvider? formatProvider, [Localizable(false)] string message, sbyte argument)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument);
            }
        }

        public void Warn([Localizable(false)] string message, sbyte argument)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument);
            }
        }

        public void Warn(IFormatProvider? formatProvider, [Localizable(false)] string message, uint argument)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument);
            }
        }

        public void Warn([Localizable(false)] string message, uint argument)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument);
            }
        }

        public void Warn(IFormatProvider? formatProvider, [Localizable(false)] string message, ulong argument)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument);
            }
        }

        public void Warn([Localizable(false)] string message, ulong argument)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument);
            }
        }

        public void Error(object? value)
        {
            lock (_lock)
            {
                _logger.Error(value);
            }
        }

        public void Error(IFormatProvider? formatProvider, object? value)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, value);
            }
        }

        public void Error([Localizable(false)] string message, object? arg1, object? arg2)
        {
            lock (_lock)
            {
                _logger.Error(message, arg1, arg2);
            }
        }

        public void Error([Localizable(false)] string message, object? arg1, object? arg2, object? arg3)
        {
            lock (_lock)
            {
                _logger.Error(message, arg1, arg2, arg3);
            }
        }

        public void Error(IFormatProvider? formatProvider, [Localizable(false)] string message, bool argument)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument);
            }
        }

        public void Error([Localizable(false)] string message, bool argument)
        {
            lock (_lock)
            {
                _logger.Error(message, argument);
            }
        }

        public void Error(IFormatProvider? formatProvider, [Localizable(false)] string message, char argument)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument);
            }
        }

        public void Error([Localizable(false)] string message, char argument)
        {
            lock (_lock)
            {
                _logger.Error(message, argument);
            }
        }

        public void Error(IFormatProvider? formatProvider, [Localizable(false)] string message, byte argument)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument);
            }
        }

        public void Error([Localizable(false)] string message, byte argument)
        {
            lock (_lock)
            {
                _logger.Error(message, argument);
            }
        }

        public void Error(IFormatProvider? formatProvider, [Localizable(false)] string message, string? argument)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument);
            }
        }

        public void Error([Localizable(false)] string message, string? argument)
        {
            lock (_lock)
            {
                _logger.Error(message, argument);
            }
        }

        public void Error(IFormatProvider? formatProvider, [Localizable(false)] string message, int argument)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument);
            }
        }

        public void Error([Localizable(false)] string message, int argument)
        {
            lock (_lock)
            {
                _logger.Error(message, argument);
            }
        }

        public void Error(IFormatProvider? formatProvider, [Localizable(false)] string message, long argument)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument);
            }
        }

        public void Error([Localizable(false)] string message, long argument)
        {
            lock (_lock)
            {
                _logger.Error(message, argument);
            }
        }

        public void Error(IFormatProvider? formatProvider, [Localizable(false)] string message, float argument)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument);
            }
        }

        public void Error([Localizable(false)] string message, float argument)
        {
            lock (_lock)
            {
                _logger.Error(message, argument);
            }
        }

        public void Error(IFormatProvider? formatProvider, [Localizable(false)] string message, double argument)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument);
            }
        }

        public void Error([Localizable(false)] string message, double argument)
        {
            lock (_lock)
            {
                _logger.Error(message, argument);
            }
        }

        public void Error(IFormatProvider? formatProvider, [Localizable(false)] string message, decimal argument)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument);
            }
        }

        public void Error([Localizable(false)] string message, decimal argument)
        {
            lock (_lock)
            {
                _logger.Error(message, argument);
            }
        }

        public void Error(IFormatProvider? formatProvider, [Localizable(false)] string message, object? argument)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument);
            }
        }

        public void Error([Localizable(false)] string message, object argument)
        {
            lock (_lock)
            {
                _logger.Error(message, argument);
            }
        }

        public void Error(IFormatProvider? formatProvider, [Localizable(false)] string message, sbyte argument)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument);
            }
        }

        public void Error([Localizable(false)] string message, sbyte argument)
        {
            lock (_lock)
            {
                _logger.Error(message, argument);
            }
        }

        public void Error(IFormatProvider? formatProvider, [Localizable(false)] string message, uint argument)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument);
            }
        }

        public void Error([Localizable(false)] string message, uint argument)
        {
            lock (_lock)
            {
                _logger.Error(message, argument);
            }
        }

        public void Error(IFormatProvider? formatProvider, [Localizable(false)] string message, ulong argument)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument);
            }
        }

        public void Error([Localizable(false)] string message, ulong argument)
        {
            lock (_lock)
            {
                _logger.Error(message, argument);
            }
        }

        public void Fatal(object? value)
        {
            lock (_lock)
            {
                _logger.Fatal(value);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, object? value)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, value);
            }
        }

        public void Fatal([Localizable(false)] string message, object? arg1, object? arg2)
        {
            lock (_lock)
            {
                _logger.Fatal(message, arg1, arg2);
            }
        }

        public void Fatal([Localizable(false)] string message, object? arg1, object? arg2, object? arg3)
        {
            lock (_lock)
            {
                _logger.Fatal(message, arg1, arg2, arg3);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, [Localizable(false)] string message, bool argument)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument);
            }
        }

        public void Fatal([Localizable(false)] string message, bool argument)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, [Localizable(false)] string message, char argument)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument);
            }
        }

        public void Fatal([Localizable(false)] string message, char argument)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, [Localizable(false)] string message, byte argument)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument);
            }
        }

        public void Fatal([Localizable(false)] string message, byte argument)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, [Localizable(false)] string message, string? argument)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument);
            }
        }

        public void Fatal([Localizable(false)] string message, string? argument)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, [Localizable(false)] string message, int argument)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument);
            }
        }

        public void Fatal([Localizable(false)] string message, int argument)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, [Localizable(false)] string message, long argument)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument);
            }
        }

        public void Fatal([Localizable(false)] string message, long argument)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, [Localizable(false)] string message, float argument)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument);
            }
        }

        public void Fatal([Localizable(false)] string message, float argument)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, [Localizable(false)] string message, double argument)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument);
            }
        }

        public void Fatal([Localizable(false)] string message, double argument)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, [Localizable(false)] string message, decimal argument)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument);
            }
        }

        public void Fatal([Localizable(false)] string message, decimal argument)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, [Localizable(false)] string message, object? argument)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument);
            }
        }

        public void Fatal([Localizable(false)] string message, object? argument)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, [Localizable(false)] string message, sbyte argument)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument);
            }
        }

        public void Fatal([Localizable(false)] string message, sbyte argument)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, [Localizable(false)] string message, uint argument)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument);
            }
        }

        public void Fatal([Localizable(false)] string message, uint argument)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, [Localizable(false)] string message, ulong argument)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument);
            }
        }

        public void Fatal([Localizable(false)] string message, ulong argument)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument);
            }
        }

        public void Trace<T>(T? value)
        {
            lock (_lock)
            {
                _logger.Trace(value);
            }
        }

        public void Trace<T>(IFormatProvider? formatProvider, T? value)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, value);
            }
        }

        public void Trace(LogMessageGenerator messageFunc)
        {
            lock (_lock)
            {
                _logger.Trace(messageFunc);
            }
        }

        public void Trace(Exception? exception, [Localizable(false)] string message)
        {
            lock (_lock)
            {
                _logger.Trace(exception, message);
            }
        }

        public void Trace(Exception? exception, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Trace(exception, message, args);
            }
        }

        public void Trace(Exception? exception, IFormatProvider? formatProvider, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Trace(exception, formatProvider, message, args);
            }
        }

        public void Trace(IFormatProvider? formatProvider, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, args);
            }
        }

        public void Trace([Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Trace(message, args);
            }
        }

        public void Trace([Localizable(false)] string message, Exception? exception)
        {
            lock (_lock)
            {
                _logger.Trace(message, exception);
            }
        }

        public void Trace<TArgument>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument? argument)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument);
            }
        }

        public void Trace<TArgument>([Localizable(false)] string message, TArgument? argument)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument);
            }
        }

        public void Trace<TArgument1, TArgument2>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument1, argument2);
            }
        }

        public void Trace<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument1, argument2);
            }
        }

        public void Trace<TArgument1, TArgument2, TArgument3>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2, TArgument3? argument3)
        {
            lock (_lock)
            {
                _logger.Trace(formatProvider, message, argument1, argument2, argument3);
            }
        }

        public void Trace<TArgument1, TArgument2, TArgument3>([Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2, TArgument3? argument3)
        {
            lock (_lock)
            {
                _logger.Trace(message, argument1, argument2, argument3);
            }
        }

        public void Debug<T>(T? value)
        {
            lock (_lock)
            {
                _logger.Debug(value);
            }
        }

        public void Debug<T>(IFormatProvider? formatProvider, T? value)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, value);
            }
        }

        public void Debug(LogMessageGenerator messageFunc)
        {
            lock (_lock)
            {
                _logger.Debug(messageFunc);
            }
        }

        public void Debug(Exception? exception, [Localizable(false)] string message)
        {
            lock (_lock)
            {
                _logger.Debug(exception, message);
            }
        }

        public void Debug(Exception? exception, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Debug(exception, message, args);
            }
        }

        public void Debug(Exception? exception, IFormatProvider? formatProvider, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Debug(exception, formatProvider, message, args);
            }
        }

        public void Debug(IFormatProvider? formatProvider, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, args);
            }
        }

        public void Debug([Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Debug(message, args);
            }
        }

        public void Debug<TArgument>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument? argument)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument);
            }
        }

        public void Debug<TArgument>([Localizable(false)] string message, TArgument? argument)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument);
            }
        }

        public void Debug<TArgument1, TArgument2>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument1, argument2);
            }
        }

        public void Debug<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument1, argument2);
            }
        }

        public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2, TArgument3? argument3)
        {
            lock (_lock)
            {
                _logger.Debug(formatProvider, message, argument1, argument2, argument3);
            }
        }

        public void Debug<TArgument1, TArgument2, TArgument3>([Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2, TArgument3? argument3)
        {
            lock (_lock)
            {
                _logger.Debug(message, argument1, argument2, argument3);
            }
        }

        public void Debug([Localizable(false)] string message, Exception exception)
        {
            lock (_lock)
            {
                _logger.Debug(message, exception);
            }
        }

        public void Info<T>(T? value)
        {
            lock (_lock)
            {
                _logger.Info(value);
            }
        }

        public void Info<T>(IFormatProvider? formatProvider, T? value)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, value);
            }
        }

        public void Info(LogMessageGenerator messageFunc)
        {
            lock (_lock)
            {
                _logger.Info(messageFunc);
            }
        }

        public void Info(Exception? exception, [Localizable(false)] string message)
        {
            lock (_lock)
            {
                _logger.Info(exception, message);
            }
        }

        public void Info(Exception? exception, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Info(exception, message, args);
            }
        }

        public void Info(Exception? exception, IFormatProvider? formatProvider, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Info(exception, formatProvider, message, args);
            }
        }

        public void Info(IFormatProvider? formatProvider, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, args);
            }
        }

        public void Info([Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Info(message, args);
            }
        }

        public void Info<TArgument>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument? argument)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument);
            }
        }

        public void Info<TArgument>([Localizable(false)] string message, TArgument argument)
        {
            lock (_lock)
            {
                _logger.Info(message, argument);
            }
        }

        public void Info<TArgument1, TArgument2>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument1, argument2);
            }
        }

        public void Info<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2)
        {
            lock (_lock)
            {
                _logger.Info(message, argument1, argument2);
            }
        }

        public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2, TArgument3? argument3)
        {
            lock (_lock)
            {
                _logger.Info(formatProvider, message, argument1, argument2, argument3);
            }
        }

        public void Info<TArgument1, TArgument2, TArgument3>([Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2, TArgument3? argument3)
        {
            lock (_lock)
            {
                _logger.Info(message, argument1, argument2, argument3);
            }
        }

        public void Info([Localizable(false)] string message, Exception? exception)
        {
            lock (_lock)
            {
                _logger.Info(message, exception);
            }
        }

        public void Warn<T>(T? value)
        {
            lock (_lock)
            {
                _logger.Warn(value);
            }
        }

        public void Warn<T>(IFormatProvider? formatProvider, T? value)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, value);
            }
        }

        public void Warn(LogMessageGenerator messageFunc)
        {
            lock (_lock)
            {
                _logger.Warn(messageFunc);
            }
        }

        public void Warn(Exception? exception, [Localizable(false)] string message)
        {
            lock (_lock)
            {
                _logger.Warn(exception, message);
            }
        }

        public void Warn(Exception? exception, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Warn(exception, message, args);
            }
        }

        public void Warn(Exception? exception, IFormatProvider? formatProvider, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Warn(exception, formatProvider, message, args);
            }
        }

        public void Warn(IFormatProvider? formatProvider, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, args);
            }
        }

        public void Warn([Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Warn(message, args);
            }
        }

        public void Warn<TArgument>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument? argument)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument);
            }
        }

        public void Warn<TArgument>([Localizable(false)] string message, TArgument? argument)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument);
            }
        }

        public void Warn<TArgument1, TArgument2>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument1, argument2);
            }
        }

        public void Warn<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument1, argument2);
            }
        }

        public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2, TArgument3? argument3)
        {
            lock (_lock)
            {
                _logger.Warn(formatProvider, message, argument1, argument2, argument3);
            }
        }

        public void Warn<TArgument1, TArgument2, TArgument3>([Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2, TArgument3? argument3)
        {
            lock (_lock)
            {
                _logger.Warn(message, argument1, argument2, argument3);
            }
        }

        public void Warn([Localizable(false)] string message, Exception? exception)
        {
            lock (_lock)
            {
                _logger.Warn(message, exception);
            }
        }

        public void Error<T>(T? value)
        {
            lock (_lock)
            {
                _logger.Error(value);
            }
        }

        public void Error<T>(IFormatProvider? formatProvider, T? value)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, value);
            }
        }

        public void Error(LogMessageGenerator messageFunc)
        {
            lock (_lock)
            {
                _logger.Error(messageFunc);
            }
        }

        public void Error(Exception? exception, [Localizable(false)] string message)
        {
            lock (_lock)
            {
                _logger.Error(exception, message);
            }
        }

        public void Error(Exception? exception, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Error(exception, message, args);
            }
        }

        public void Error(Exception? exception, IFormatProvider? formatProvider, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Error(exception, formatProvider, message, args);
            }
        }

        public void Error(IFormatProvider? formatProvider, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, args);
            }
        }

        public void Error([Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Error(message, args);
            }
        }

        public void Error<TArgument>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument? argument)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument);
            }
        }

        public void Error<TArgument>([Localizable(false)] string message, TArgument? argument)
        {
            lock (_lock)
            {
                _logger.Error(message, argument);
            }
        }

        public void Error<TArgument1, TArgument2>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument1, argument2);
            }
        }

        public void Error<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2)
        {
            lock (_lock)
            {
                _logger.Error(message, argument1, argument2);
            }
        }

        public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2, TArgument3? argument3)
        {
            lock (_lock)
            {
                _logger.Error(formatProvider, message, argument1, argument2, argument3);
            }
        }

        public void Error<TArgument1, TArgument2, TArgument3>([Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2, TArgument3? argument3)
        {
            lock (_lock)
            {
                _logger.Error(message, argument1, argument2, argument3);
            }
        }


        public void Fatal<T>(T? value)
        {
            lock (_lock)
            {
                _logger.Fatal(value);
            }
        }

        public void Fatal<T>(IFormatProvider? formatProvider, T? value)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, value);
            }
        }

        public void Fatal(LogMessageGenerator messageFunc)
        {
            lock (_lock)
            {
                _logger.Fatal(messageFunc);
            }
        }

        public void Fatal(Exception? exception, [Localizable(false)] string message)
        {
            lock (_lock)
            {
                _logger.Fatal(exception, message);
            }
        }

        public void Fatal(Exception? exception, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Fatal(exception, message, args);
            }
        }

        public void Fatal(Exception? exception, IFormatProvider? formatProvider, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Fatal(exception, formatProvider, message, args);
            }
        }

        public void Fatal(IFormatProvider? formatProvider, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, args);
            }
        }

        public void Fatal([Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Fatal(message, args);
            }
        }

        public void Fatal<TArgument>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument? argument)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument);
            }
        }

        public void Fatal<TArgument>([Localizable(false)] string message, TArgument? argument)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument);
            }
        }

        public void Fatal<TArgument1, TArgument2>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument1, argument2);
            }
        }

        public void Fatal<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument1, argument2);
            }
        }

        public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2, TArgument3? argument3)
        {
            lock (_lock)
            {
                _logger.Fatal(formatProvider, message, argument1, argument2, argument3);
            }
        }

        public void Fatal<TArgument1, TArgument2, TArgument3>([Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2, TArgument3? argument3)
        {
            lock (_lock)
            {
                _logger.Fatal(message, argument1, argument2, argument3);
            }
        }

        public void Log(NLog.LogLevel level, object? value)
        {
            lock (_lock)
            {
                _logger.Log(level, value);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, object? value)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, value);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, object? arg1, object? arg2)
        {
            lock (_lock)
            {
                _logger.Log(level, message, arg1, arg2);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, object? arg1, object? arg2, object? arg3)
        {
            lock (_lock)
            {
                _logger.Log(level, message, arg1, arg2, arg3);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, bool argument)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, bool argument)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, char argument)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, char argument)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, byte argument)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, byte argument)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, string? argument)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, string? argument)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, int argument)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, int argument)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, long argument)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, long argument)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, float argument)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, float argument)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, double argument)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, double argument)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, decimal argument)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, decimal argument)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, object? argument)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, object? argument)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, sbyte argument)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, sbyte argument)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, uint argument)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, uint argument)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, ulong argument)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, ulong argument)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument);
            }
        }

        public bool IsEnabled(NLog.LogLevel level)
        {
            lock (_lock)
            {
                return _logger.IsEnabled(level);
            }
        }

        public void Log(LogEventInfo logEvent)
        {
            lock (_lock)
            {
                _logger.Log(logEvent);
            }
        }

        public void Log(System.Type wrapperType, LogEventInfo logEvent)
        {
            lock(_lock)
            {
                _logger.Log(wrapperType, logEvent);
            }
        }

        public void Log<T>(NLog.LogLevel level, T? value)
        {
            lock (_lock)
            {
                _logger.Log(level, value);
            }
        }

        public void Log<T>(NLog.LogLevel level, IFormatProvider? formatProvider, T? value)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, value);
            }
        }

        public void Log(NLog.LogLevel level, LogMessageGenerator messageFunc)
        {
            lock (_lock)
            {
                _logger.Log(level, messageFunc);
            }
        }

        public void Log(NLog.LogLevel level, Exception? exception, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Log(level, exception, message, args);
            }
        }

        public void Log(NLog.LogLevel level, Exception? exception, IFormatProvider? formatProvider, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Log(level, exception, formatProvider, message, args);
            }
        }

        public void Log(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, args);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message)
        {
            lock (_lock)
            {
                _logger.Log(level, message);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, params object?[] args)
        {
            lock (_lock)
            {
                _logger.Log(level, message, args);
            }
        }

        public void Log<TArgument>(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument? argument)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument);
            }
        }

        public void Log<TArgument>(NLog.LogLevel level, [Localizable(false)] string message, TArgument? argument)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument);
            }
        }

        public void Log<TArgument1, TArgument2>(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument1, argument2);
            }
        }

        public void Log<TArgument1, TArgument2>(NLog.LogLevel level, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument1, argument2);
            }
        }

        public void Log<TArgument1, TArgument2, TArgument3>(NLog.LogLevel level, IFormatProvider? formatProvider, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2, TArgument3? argument3)
        {
            lock (_lock)
            {
                _logger.Log(level, formatProvider, message, argument1, argument2, argument3);
            }
        }

        public void Log<TArgument1, TArgument2, TArgument3>(NLog.LogLevel level, [Localizable(false)] string message, TArgument1? argument1, TArgument2? argument2, TArgument3? argument3)
        {
            lock (_lock)
            {
                _logger.Log(level, message, argument1, argument2, argument3);
            }
        }

        public void Log(NLog.LogLevel level, [Localizable(false)] string message, Exception? exception)
        {
            lock (_lock)
            {
                _logger.Log(level, message, exception);
            }
        }

        public void Trace([Localizable(false)] string message)
        {
            lock (_lock)
            {
                _logger.Trace(message);
            }
        }

        public void TraceException([Localizable(false)] string message, Exception? exception)
        {
            lock (_lock)
            {
                _logger.Trace(message, exception);
            }
        }

        public void Debug([Localizable(false)] string message)
        {
            lock (_lock)
            {
                _logger.Debug(message);
            }
        }

        public void DebugException([Localizable(false)] string message, Exception? exception)
        {
            lock (_lock)
            {
                _logger.Debug(message, exception);
            }
        }

        public void Info([Localizable(false)] string message)
        {
            lock (_lock)
            {
                _logger.Info(message);
            }
        }

        public void InfoException([Localizable(false)] string message, Exception? exception)
        {
            lock (_lock)
            {
                _logger.Info(message, exception);
            }
        }

        public void Warn([Localizable(false)] string message)
        {
            lock (_lock)
            {
                _logger.Warn(message);
            }
        }

        public void WarnException([Localizable(false)] string message, Exception? exception)
        {
            lock (_lock)
            {
                _logger.Warn(message, exception);
            }
        }

        public void Error([Localizable(false)] string message)
        {
            lock (_lock)
            {
                _logger.Error(message);
            }
        }

        public void Error([Localizable(false)] string message, Exception? exception)
        {
            lock (_lock)
            {
                _logger.Error(message, exception);
            }
        }

        public void ErrorException([Localizable(false)] string message, Exception? exception)
        {
            lock (_lock)
            {
                _logger.Error(message, exception);
            }
        }

        public void Fatal([Localizable(false)] string message)
        {
            lock (_lock)
            {
                _logger.Fatal(message);
            }
        }

        public void Fatal([Localizable(false)] string message, Exception? exception)
        {
            lock (_lock)
            {
                _logger.Fatal(message, exception);
            }
        }

        public void FatalException([Localizable(false)] string message, Exception? exception)
        {
            lock (_lock)
            {
                _logger.Fatal(message, exception);
            }
        }

        public void Swallow(Action action)
        {
            lock(_lock)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "An error occurred while executing the action.");
                }
            }
        }

        public T? Swallow<T>(Func<T?> func)
        {
            throw new NotImplementedException();
        }

        public T? Swallow<T>(Func<T?> func, T? fallback)
        {
            throw new NotImplementedException();
        }

        public void Swallow(Task task)
        {
            throw new NotImplementedException();
        }

        public Task SwallowAsync(Task task)
        {
            throw new NotImplementedException();
        }

        public Task SwallowAsync(Func<Task> asyncAction)
        {
            throw new NotImplementedException();
        }

        public Task<TResult?> SwallowAsync<TResult>(Func<Task<TResult?>> asyncFunc)
        {
            throw new NotImplementedException();
        }

        public Task<TResult?> SwallowAsync<TResult>(Func<Task<TResult?>> asyncFunc, TResult? fallback)
        {
            throw new NotImplementedException();
        }

        public void LogException(NLog.LogLevel level, [Localizable(false)] string message, Exception? exception)
        {
            throw new NotImplementedException();
        }

        #endregion





        #endregion
    }
}
