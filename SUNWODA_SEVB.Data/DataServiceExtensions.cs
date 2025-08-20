using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Interfaces.Data;
using SUNWODA_SEVB.Data.Mappings;
using SUNWODA_SEVB.Data.Repositories;

namespace SUNWODA_SEVB.Data
{
    /// <summary>
    /// 数据层服务注册扩展
    /// </summary>

    public static class DataServiceExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            // 配置Mapster
            MapsterConfig.Configure();

            // 注册SqlSugar
            services.AddSingleton<ISqlSugarClient>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                var logger = provider.GetService<ILoggerService<ISqlSugarClient>>();

                var db = new SqlSugarScope(
                    new ConnectionConfig()
                    {
                        ConnectionString = connectionString,
                        DbType = DbType.MySql,
                        IsAutoCloseConnection = true,
                        InitKeyType = InitKeyType.Attribute,
                        MoreSettings = new ConnMoreSettings()
                        {
                            IsAutoRemoveDataCache = true,
                            IsWithNoLockQuery = false,
                        },
                        ConfigureExternalServices = new ConfigureExternalServices
                        {
                            // 配置实体服务
                            EntityService = (c, p) =>
                            {
                                // 统一设置主键为自增
                                if (p.IsPrimarykey && p.PropertyInfo.PropertyType == typeof(int))
                                {
                                    p.IsIdentity = true;
                                }

                                // 设置时间字段默认值
                                if (
                                    p.PropertyInfo.PropertyType == typeof(DateTime)
                                    && (
                                        p.PropertyName.Equals(
                                            "CreateTime",
                                            StringComparison.OrdinalIgnoreCase
                                        )
                                        || p.PropertyName.Equals(
                                            "UpdateTime",
                                            StringComparison.OrdinalIgnoreCase
                                        )
                                    )
                                )
                                {
                                    p.DefaultValue = "CURRENT_TIMESTAMP";
                                }
                            },
                        },
                    },
                    db =>
                    {
                        // SQL执行前事件 - 优化日志记录
                        db.Aop.OnLogExecuting = (sql, pars) =>
                        {
                            var tableName = ExtractTableName(sql);

                            // 过滤掉系统表查询的日志记录，减少噪音
                            if (IsSystemQuery(sql, tableName))
                            {
                                // 系统查询不记录，或者只在Debug级别记录
                                return;
                            }

                            // 只记录业务表操作
                            if (!string.IsNullOrEmpty(tableName) && IsBusinessTable(tableName))
                            {
                                var operation = GetSqlOperation(sql);
                                logger?.Debug($"SQL执行: {operation} 表[{tableName}]");
                            }
                        };

                        // SQL执行后事件 - 只在慢查询时记录
                        db.Aop.OnLogExecuted = (sql, pars) =>
                        {
                            var executionTime = db.Ado.SqlExecutionTime.TotalMilliseconds;

                            if (executionTime > 1000) // 超过1秒的慢查询
                            {
                                var tableName = ExtractTableName(sql);
                                var operation = GetSqlOperation(sql);
                                logger?.Warn(
                                    $"慢查询警告: {operation} 表[{tableName}] - 执行时间: {executionTime:F2}ms"
                                );
                            }
                        };

                        // SQL出错事件
                        db.Aop.OnError = (exp) =>
                        {
                            logger?.Error($"SQL执行出错: {exp.Message}", exp);
                        };
                    }
                );

                return db;
            });

            // 注册工作单元
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 注册通用仓储
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            // 注册特定仓储
            services.AddScoped<IAppLogRepository, AppLogRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IGlobalSettingRepository, GlobalSettingRepository>();
            services.AddScoped<IPLCAddressConfigRepository, PLCAddressConfigRepository>();
            services.AddScoped<IPLCConfigRepository, PLCConfigRepository>();
            services.AddScoped<IPLCRWConfigRepository, PLCRWConfigRepository>();
            services.AddScoped<IWorkSpaceProjectRepository, WorkSpaceProjectRepository>();
            services.AddScoped<IProjectSettingRepository, ProjectSettingRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();

            return services;
        }

        /// <summary>
        /// 从SQL语句中提取表名
        /// </summary>
        private static string ExtractTableName(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return string.Empty;

            sql = sql.ToUpper();

            var patterns = new[]
            {
                @"FROM\s+`?(\w+)`?",
                @"INSERT\s+INTO\s+`?(\w+)`?",
                @"UPDATE\s+`?(\w+)`?",
                @"DELETE\s+FROM\s+`?(\w+)`?",
                @"ALTER\s+TABLE\s+`?(\w+)`?",
                @"CREATE\s+TABLE\s+`?(\w+)`?",
                @"DROP\s+TABLE\s+`?(\w+)`?",
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(sql, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                    return match.Groups[1].Value;
            }

            if (sql.Contains("INFORMATION_SCHEMA"))
                return "INFORMATION_SCHEMA";

            return "未知表";
        }

        /// <summary>
        /// 判断是否为系统查询
        /// </summary>
        private static bool IsSystemQuery(string sql, string tableName)
        {
            if (string.IsNullOrEmpty(sql))
                return false;

            sql = sql.ToUpper();

            // 系统表查询
            if (
                tableName == "INFORMATION_SCHEMA"
                || sql.Contains("INFORMATION_SCHEMA")
                || sql.Contains("PERFORMANCE_SCHEMA")
                || sql.Contains("MYSQL.")
                || sql.Contains("SYS.")
            )
            {
                return true;
            }

            // 表结构检查相关的查询
            if (
                sql.Contains("SHOW TABLES")
                || sql.Contains("SHOW COLUMNS")
                || sql.Contains("DESCRIBE ")
                || sql.Contains("EXPLAIN ")
            )
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 判断是否为业务表
        /// </summary>
        private static bool IsBusinessTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return false;

            // 定义业务表列表
            var businessTables = new[]
            {
                "APP_LOGS",
                "MES_INTERFACE_LOGS",
                "WEB_INTERFACE_LOGS",
                "USERS",
                "DEVICE",
                "PLC_CONFIG",
                "PLC_ADDRESS_CONFIG",
                "PLC_RW_CONFIG",
                "GLOBAL_SETTING",
                "PROJECT_SETTING",
                "WORKSPACE_PROJECT",
            };

            return businessTables.Contains(tableName.ToUpper());
        }

        /// <summary>
        /// 获取SQL操作类型
        /// </summary>
        private static string GetSqlOperation(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return "UNKNOWN";

            sql = sql.ToUpper().Trim();

            if (sql.StartsWith("SELECT"))
                return "查询";
            if (sql.StartsWith("INSERT"))
                return "插入";
            if (sql.StartsWith("UPDATE"))
                return "更新";
            if (sql.StartsWith("DELETE"))
                return "删除";
            if (sql.StartsWith("CREATE"))
                return "创建";
            if (sql.StartsWith("ALTER"))
                return "修改";
            if (sql.StartsWith("DROP"))
                return "删除";

            return "操作";
        }
    }
}
