

using NLog.Config;
using NLog.Targets;
using NLog;
using HTHIUM.Logging.Targets;

namespace HTHIUM.Logging
{
    public static class NLogSetup
    {
        public static void ConfigureNLog(IServiceProvider serviceProvider)
        {
            var config = new LoggingConfiguration();

            // 文件目标
            var fileTarget = new FileTarget("file")
            {
                FileName = "${basedir}/logs/${date:format=yyyy-MM-dd}/${processname}.log",
                Layout = "${longdate} ${level:uppercase=true} ${logger} ${message} ${exception:format=ToString}",
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveAboveSize = 10485760, // 10MB
                MaxArchiveFiles = 30,
                KeepFileOpen = false,
                Encoding = System.Text.Encoding.UTF8
            };

            // 控制台目标
            var consoleTarget = new ColoredConsoleTarget("console")
            {
                Layout = "${longdate} ${level:uppercase=true} ${logger:shortName=true} ${message} ${exception:format=ToString}"
            };

            // 数据库目标
            //var databaseTarget = new DatabaseTarget { Name = "database" };
            //databaseTarget.Initialize(serviceProvider);

            // 特殊文件目标（用于接口日志等）
            var specialFileTarget = new FileTarget("specialfile")
            {
                FileName = "${basedir}/logs/${date:format=yyyy-MM-dd}/${event-properties:SpecialFileName:whenEmpty=special}.log",
                Layout = "${longdate} ${level:uppercase=true} ${message} ${exception:format=ToString}",
                ArchiveEvery = FileArchivePeriod.Day,
                MaxArchiveFiles = 30
            };

            // 添加目标
            config.AddTarget(fileTarget);
            config.AddTarget(consoleTarget);
            //config.AddTarget(databaseTarget);
            config.AddTarget(specialFileTarget);

            // 规则配置
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, consoleTarget);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, fileTarget);
            //config.AddRule(LogLevel.Info, LogLevel.Fatal, databaseTarget);

            // 特殊文件规则
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, specialFileTarget, "SpecialFile.*");

            // 应用配置
            LogManager.Configuration = config;
        }
    }
}
