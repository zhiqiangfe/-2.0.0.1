// DatabaseLogTarget.cs
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Common;
using NLog.Targets;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Logging
{
    /// <summary>
    /// NLog数据库日志目标
    /// </summary>
  /*  [Target("DatabaseLog")]
    public class DatabaseLogTarget : AsyncTaskTarget
    {
        private static IServiceProvider _serviceProvider;
        private static readonly ConcurrentQueue<LogEventInfo> _logQueue = new ConcurrentQueue<LogEventInfo>();
        private static Timer _flushTimer;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// 日志类型
        /// </summary>
        public string LogType { get; set; } = "AppLog";

        /// <summary>
        /// 批量写入的最大条数
        /// </summary>
        public int BatchSize { get; set; } = 100;

        /// <summary>
        /// 刷新间隔（毫秒）
        /// </summary>
        public int FlushInterval { get; set; } = 5000;

        /// <summary>
        /// 设置服务提供者
        /// </summary>
        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            // 初始化定时器
            if (_flushTimer == null)
            {
                _flushTimer = new Timer(async _ => await FlushLogsAsync(), null, 5000, 5000);
            }
        }

        protected override async Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken cancellationToken)
        {
            if (_serviceProvider == null)
            {
                // 如果服务提供者还未设置，先缓存日志
                _logQueue.Enqueue(logEvent);
                return;
            }

            try
            {
                await WriteLogToDatabase(logEvent);
            }
            catch (Exception ex)
            {
                // 写入失败时缓存日志
                _logQueue.Enqueue(logEvent);
                InternalLogger.Error(ex, "Failed to write log to database");
            }
        }

        private async Task WriteLogToDatabase(LogEventInfo logEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetService<IDataRepository>();
                if (repository == null)
                {
                    _logQueue.Enqueue(logEvent);
                    return;
                }

                switch (LogType)
                {
                    case "AppLog":
                        await WriteAppLog(repository, logEvent);
                        break;
                    case "MesInterfaceLog":
                        await WriteMesInterfaceLog(repository, logEvent);
                        break;
                    case "WebInterfaceLog":
                        await WriteWebInterfaceLog(repository, logEvent);
                        break;
                }
            }
        }

        private async Task WriteAppLog(IDataRepository repository, LogEventInfo logEvent)
        {
            var appLog = new AppLogs
            {
                LogTime = logEvent.TimeStamp,
                LogLevel = logEvent.Level.ToString(),
                Logger = logEvent.LoggerName ?? "Unknown",
                Message = logEvent.FormattedMessage ?? string.Empty,
                Exception = logEvent.Exception?.ToString()
            };

            await repository.InsertAsync(appLog);
        }

        private async Task WriteMesInterfaceLog(IDataRepository repository, LogEventInfo logEvent)
        {
            // 从日志事件属性中获取接口信息
            var interfaceName = logEvent.Properties.ContainsKey("InterfaceName")
                ? logEvent.Properties["InterfaceName"].ToString()
                : "Unknown";

            var requestData = logEvent.Properties.ContainsKey("RequestData")
                ? logEvent.Properties["RequestData"].ToString()
                : null;

            var responseData = logEvent.Properties.ContainsKey("ResponseData")
                ? logEvent.Properties["ResponseData"].ToString()
                : null;

            var isSuccess = logEvent.Properties.ContainsKey("IsSuccess")
                && bool.Parse(logEvent.Properties["IsSuccess"].ToString());

            var mesLog = new MesInterfaceLog
            {
                InterfaceName = interfaceName,
                RequestData = requestData,
                ResponseData = responseData,
                RequestTime = logEvent.TimeStamp,
                ResponseTime = DateTime.Now,
                IsSuccess = isSuccess,
                ErrorMessage = logEvent.Exception?.Message,
                ExecutionTime = logEvent.Properties.ContainsKey("ExecutionTime")
                    ? Convert.ToInt32(logEvent.Properties["ExecutionTime"])
                    : 0
            };

            await repository.InsertAsync(mesLog);
        }

        private async Task WriteWebInterfaceLog(IDataRepository repository, LogEventInfo logEvent)
        {
            // 从日志事件属性中获取API信息
            var apiPath = logEvent.Properties.ContainsKey("ApiPath")
                ? logEvent.Properties["ApiPath"].ToString()
                : "Unknown";

            var httpMethod = logEvent.Properties.ContainsKey("HttpMethod")
                ? logEvent.Properties["HttpMethod"].ToString()
                : "Unknown";

            var statusCode = logEvent.Properties.ContainsKey("StatusCode")
                ? Convert.ToInt32(logEvent.Properties["StatusCode"])
                : 0;

            var webLog = new WebInterfaceLog
            {
                ApiPath = apiPath,
                HttpMethod = httpMethod,
                RequestBody = logEvent.Properties.ContainsKey("RequestBody")
                    ? logEvent.Properties["RequestBody"].ToString()
                    : null,
                ResponseBody = logEvent.Properties.ContainsKey("ResponseBody")
                    ? logEvent.Properties["ResponseBody"].ToString()
                    : null,
                ClientIP = logEvent.Properties.ContainsKey("ClientIP")
                    ? logEvent.Properties["ClientIP"].ToString()
                    : null,
                StatusCode = statusCode,
                RequestTime = logEvent.TimeStamp,
                ExecutionTime = logEvent.Properties.ContainsKey("ExecutionTime")
                    ? Convert.ToInt64(logEvent.Properties["ExecutionTime"])
                    : 0
            };

            await repository.InsertAsync(webLog);
        }

        /// <summary>
        /// 定时刷新缓存的日志
        /// </summary>
        private static async Task FlushLogsAsync()
        {
            if (_serviceProvider == null || _logQueue.IsEmpty)
                return;

            var logs = new System.Collections.Generic.List<LogEventInfo>();
            while (_logQueue.TryDequeue(out var log) && logs.Count < 100)
            {
                logs.Add(log);
            }

            if (logs.Count > 0)
            {
                foreach (var log in logs)
                {
                    try
                    {
                        // 重新尝试写入数据库
                        var target = LogManager.Configuration.FindTargetByName<DatabaseLogTarget>("database_app");
                        if (target != null)
                        {
                            await target.WriteLogToDatabase(log);
                        }
                    }
                    catch
                    {
                        // 如果还是失败，放回队列
                        _logQueue.Enqueue(log);
                    }
                }
            }
        }

        protected override void CloseTarget()
        {
            _flushTimer?.Dispose();
            base.CloseTarget();
        }
    }*/
}