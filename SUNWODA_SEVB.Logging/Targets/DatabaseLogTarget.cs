using NLog;
using NLog.Config;
using NLog.Targets;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using NLog.Common;
using NLog.Targets.Wrappers;
using SUNWODA_SEVB.Core.Models.Data;
using SUNWODA_SEVB.Core.Interfaces.Data;

namespace SUNWODA_SEVB.Logging.Targets
{
    /// <summary>
    /// 数据库日志目标类型
    /// </summary>
    public enum DatabaseLogType
    {
        AppLog,
        MesInterfaceLog,
        WebInterfaceLog
    }

    /// <summary>
    /// NLog数据库日志目标
    /// </summary>
    [Target("DatabaseLog")]
    public sealed class DatabaseLogTarget : TargetWithLayout
    {
        private static IServiceProvider? _serviceProvider;
        private static readonly object _initLock = new object();
        private readonly ConcurrentQueue<object> _logQueue = new();
        private Timer? _flushTimer;
        private readonly object _flushLock = new();
        private const int BatchSize = 10;
        private const int FlushIntervalSeconds = 2;
        private bool _isInitialized = false;

        /// <summary>
        /// 连接字符串属性 - 修复NLog配置错误
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// 日志类型
        /// </summary>
        [DefaultValue("AppLog")]
        public string LogType { get; set; } = "AppLog";

        public DatabaseLogTarget()
        {
            // 设置默认布局
            Layout = "${longdate} | ${level:uppercase=true} | ${logger} | ${message} ${exception:format=tostring}";
        }

        /// <summary>
        /// 初始化服务提供者（静态方法，应用启动时调用一次）
        /// </summary>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            lock (_initLock)
            {
                _serviceProvider = serviceProvider;
                InternalLogger.Info($"DatabaseLogTarget: ServiceProvider initialized at {DateTime.Now:HH:mm:ss.fff}");

                // 找到所有已注册的 DatabaseLogTarget 实例并重新初始化
                var config = LogManager.Configuration;
                if (config != null)
                {
                    foreach (var target in config.AllTargets)
                    {
                        if (target is DatabaseLogTarget dbTarget)
                        {
                            dbTarget.ReinitializeIfNeeded();
                        }
                        else if (target is WrapperTargetBase wrapper)
                        {
                            // 处理 AsyncWrapper 等包装器
                            var innerTarget = GetInnermostTarget(wrapper);
                            if (innerTarget is DatabaseLogTarget dbInnerTarget)
                            {
                                dbInnerTarget.ReinitializeIfNeeded();
                            }
                        }
                    }
                }
            }
        }

        private static Target? GetInnermostTarget(WrapperTargetBase wrapper)
        {
            var current = wrapper.WrappedTarget;
            while (current is WrapperTargetBase innerWrapper)
            {
                current = innerWrapper.WrappedTarget;
            }
            return current;
        }

        private void ReinitializeIfNeeded()
        {
            if (_serviceProvider != null && !_isInitialized)
            {
                InitializeInstance();
            }
        }

        private void InitializeInstance()
        {
            lock (_flushLock)
            {
                if (_isInitialized) return;

                _isInitialized = true;

                // 确保名称正确
                if (string.IsNullOrEmpty(Name))
                {
                    Name = $"DatabaseLog_{LogType}";
                }

                // 停止旧的定时器（如果存在）
                _flushTimer?.Dispose();

                // 创建新的定时器
                _flushTimer = new Timer(
                    callback: _ =>
                    {
                        try
                        {
                            Task.Run(async () => await FlushLogsAsync().ConfigureAwait(false));
                        }
                        catch (Exception ex)
                        {
                            InternalLogger.Error(ex, "Timer flush error");
                        }
                    },
                    state: null,
                    dueTime: TimeSpan.FromSeconds(1),
                    period: TimeSpan.FromSeconds(FlushIntervalSeconds));

                InternalLogger.Info($"DatabaseLogTarget '{Name}' instance initialized for LogType: {LogType}, ConnectionString: {(!string.IsNullOrEmpty(ConnectionString) ? "Configured" : "Not Set")}");
            }
        }

        /// <summary>
        /// 初始化目标（NLog调用）
        /// </summary>
        protected override void InitializeTarget()
        {
            base.InitializeTarget();

            // 验证参数
            if (string.IsNullOrEmpty(LogType))
            {
                LogType = "AppLog";
            }

            if (!Enum.TryParse<DatabaseLogType>(LogType, out _))
            {
                InternalLogger.Warn($"Invalid LogType '{LogType}', defaulting to 'AppLog'");
                LogType = "AppLog";
            }

            // 记录连接字符串状态（不记录实际值以保护安全）
            InternalLogger.Info($"DatabaseLogTarget InitializeTarget - ConnectionString: {(!string.IsNullOrEmpty(ConnectionString) ? "Set" : "Empty")}");

            // 如果服务提供者已经可用，立即初始化
            if (_serviceProvider != null && !_isInitialized)
            {
                InitializeInstance();
            }

            InternalLogger.Info($"DatabaseLogTarget InitializeTarget completed for '{Name}' with LogType: {LogType}");
        }

        protected override void Write(LogEventInfo logEvent)
        {
            try
            {
                // 检查服务提供者
                if (_serviceProvider == null)
                {
                    // 不要频繁记录这个警告
                    if (DateTime.Now.Second % 10 == 0)
                    {
                        InternalLogger.Warn($"DatabaseLogTarget '{Name}': ServiceProvider is null");
                    }
                    return;
                }

                // 确保已初始化
                if (!_isInitialized)
                {
                    InitializeInstance();
                }
                //// 是否写入数据库
                bool writeToDatabase = false;
                if (logEvent.Properties.ContainsKey("IsToDatabase"))
                {
                    var value = logEvent.Properties["IsToDatabase"];
                    if (value is bool boolValue)
                    {
                        writeToDatabase = boolValue;
                    }
                    else if (value != null)
                    {
                        writeToDatabase = Convert.ToBoolean(value);
                    }
                }

                // 如果不写入数据库，则直接返回
                if (!writeToDatabase)
                {
                    return;
                }

                var logType = Enum.TryParse<DatabaseLogType>(LogType, out var type) ? type : DatabaseLogType.AppLog;

                switch (logType)
                {
                    case DatabaseLogType.AppLog:
                        
                        var appLog = new AppLogModel
                        {
                            LogTime = logEvent.TimeStamp,
                            LogLevel = logEvent.Level.Name,
                            Logger = string.IsNullOrEmpty(logEvent.CallerMemberName)? (logEvent.LoggerName ?? "Unknown"): $"{logEvent.LoggerName ?? "Unknown"}.{logEvent.CallerMemberName}",
                            Message = logEvent.FormattedMessage ?? string.Empty, // 只保存纯消息内容
                            Exception = logEvent.Exception?.ToString(),
                           
                        };
                        _logQueue.Enqueue(appLog);
                        break;

                    case DatabaseLogType.MesInterfaceLog:
                        if (logEvent.Properties.Count > 0)
                        {
                            var mesLog = new MesInterfaceLogModel
                            {
                                Method = logEvent.Properties["Method"]?.ToString() ?? "",
                                InputJson = logEvent.Properties["InputJson"]?.ToString() ?? "",
                                OutputJson = logEvent.Properties["OutputJson"]?.ToString() ?? "",
                                SuccessFlag = Convert.ToBoolean(logEvent.Properties["SuccessFlag"] ?? false),
                                StartTime = logEvent.TimeStamp
                            };
                            _logQueue.Enqueue(mesLog);
                        }
                        break;

                    case DatabaseLogType.WebInterfaceLog:
                        if (logEvent.Properties.Count > 0)
                        {
                            var webLog = new WebInterfaceLogModel
                            {
                                Method = logEvent.Properties["Method"]?.ToString() ?? "",
                                InputJson = logEvent.Properties["InputJson"]?.ToString() ?? "",
                                OutputJson = logEvent.Properties["OutputJson"]?.ToString() ?? "",
                                SuccessFlag = Convert.ToBoolean(logEvent.Properties["SuccessFlag"] ?? false),
                                StartTime = logEvent.TimeStamp
                            };
                            _logQueue.Enqueue(webLog);
                        }
                        break;
                }

                // 如果队列过大，立即刷新
                if (_logQueue.Count >= BatchSize)
                {
                    Task.Run(async () => await FlushLogsAsync().ConfigureAwait(false));
                }
            }
            catch (Exception ex)
            {
                InternalLogger.Error(ex, $"DatabaseLogTarget Write error");
            }
        }

        /// <summary>
        /// 异步刷新日志到数据库
        /// </summary>
        private async Task FlushLogsAsync()
        {
            if (_serviceProvider == null) return;

            try
            {
                List<AppLogModel> appLogs;
                List<MesInterfaceLogModel> mesLogs;
                List<WebInterfaceLogModel> webLogs;

                lock (_flushLock)
                {
                    if (_logQueue.IsEmpty) return;

                    appLogs = new List<AppLogModel>();
                    mesLogs = new List<MesInterfaceLogModel>();
                    webLogs = new List<WebInterfaceLogModel>();

                    int count = 0;
                    while (_logQueue.TryDequeue(out var log) && count < BatchSize * 2)
                    {
                        switch (log)
                        {
                            case AppLogModel appLog:
                                appLogs.Add(appLog);
                                break;
                            case MesInterfaceLogModel mesLog:
                                mesLogs.Add(mesLog);
                                break;
                            case WebInterfaceLogModel webLog:
                                webLogs.Add(webLog);
                                break;
                        }
                        count++;
                    }
                }

                if (appLogs.Count > 0 || mesLogs.Count > 0 || webLogs.Count > 0)
                {
                    await SaveLogsToDatabase(appLogs, mesLogs, webLogs).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                InternalLogger.Error(ex, "FlushLogsAsync error");
            }
        }

      

        /// <summary>
        /// 保存日志到数据库
        /// </summary>
        private async Task SaveLogsToDatabase(
            List<AppLogModel> appLogs,
            List<MesInterfaceLogModel> mesLogs,
            List<WebInterfaceLogModel> webLogs)
        {
            try
            {
                using var scope = _serviceProvider!.CreateScope();

                if (appLogs.Count > 0)
                {
                    var appLogRepo = scope.ServiceProvider.GetRequiredService<IAppLogRepository>();
                    var success = await appLogRepo.BulkInsertAsync(appLogs).ConfigureAwait(false);
                    if (success)
                    {
                        InternalLogger.Info($"Successfully saved {appLogs.Count} app logs to database");
                    }
                }

                if (mesLogs.Count > 0)
                {
                    var mesLogRepo = scope.ServiceProvider.GetRequiredService<IMesInterfaceLogRepository>();
                    await mesLogRepo.BulkInsertAsync(mesLogs).ConfigureAwait(false);
                    InternalLogger.Info($"Saved {mesLogs.Count} MES logs to database");
                }

                if (webLogs.Count > 0)
                {
                    var webLogRepo = scope.ServiceProvider.GetRequiredService<IWebInterfaceLogRepository>();
                    await webLogRepo.BulkInsertAsync(webLogs).ConfigureAwait(false);
                    InternalLogger.Info($"Saved {webLogs.Count} Web logs to database");
                }
            }
            catch (Exception ex)
            {
                InternalLogger.Error(ex, $"Failed to save logs to database: {ex.Message}");

                // 将失败的日志重新加入队列（限制重试次数）
                if (_logQueue.Count < BatchSize * 3)
                {
                    foreach (var log in appLogs) _logQueue.Enqueue(log);
                    foreach (var log in mesLogs) _logQueue.Enqueue(log);
                    foreach (var log in webLogs) _logQueue.Enqueue(log);
                }
            }
        }

        /// <summary>
        /// 异步刷新（NLog调用）
        /// </summary>
        protected override void FlushAsync(AsyncContinuation asyncContinuation)
        {
            Task.Run(async () =>
            {
                try
                {
                    await FlushLogsAsync().ConfigureAwait(false);
                    asyncContinuation(null);
                }
                catch (Exception ex)
                {
                    asyncContinuation(ex);
                }
            });
        }

        /// <summary>
        /// 关闭目标
        /// </summary>
        protected override void CloseTarget()
        {
            try
            {
                InternalLogger.Info($"Closing DatabaseLogTarget '{Name}'");

                // 停止定时器
                _flushTimer?.Change(Timeout.Infinite, Timeout.Infinite);
                _flushTimer?.Dispose();
                _flushTimer = null;

                // 最后一次刷新
                var flushTask = FlushLogsAsync();
                if (!flushTask.Wait(TimeSpan.FromSeconds(10)))
                {
                    InternalLogger.Warn("Final flush timeout during close");
                }

                _isInitialized = false;
            }
            catch (Exception ex)
            {
                InternalLogger.Error(ex, "Error closing DatabaseLogTarget");
            }
            finally
            {
                base.CloseTarget();
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _flushTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}