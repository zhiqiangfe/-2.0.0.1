using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Data.Mappings;
using SUNWODA_SEVB.Data.Repositories;
using System.Text.RegularExpressions;

namespace SUNWODA_SEVB.Data
{
    /// <summary>
    /// 数据层服务注册扩展
    /// </summary>
    public static class ServiceCollectionExtensions
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

                var db = new SqlSugarScope(new ConnectionConfig()
                {
                    ConnectionString = connectionString,
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsAutoRemoveDataCache = true,
                        IsWithNoLockQuery = false
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
                            if (p.PropertyInfo.PropertyType == typeof(DateTime) &&
                                (p.PropertyName.Equals("CreateTime", StringComparison.OrdinalIgnoreCase) ||
                                 p.PropertyName.Equals("UpdateTime", StringComparison.OrdinalIgnoreCase)))
                            {
                                p.DefaultValue = "CURRENT_TIMESTAMP";
                            }
                        }
                    }
                },
                db =>
                {
                    // SQL执行前事件 - 简化日志，只记录表名
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        var tableName = ExtractTableName(sql);
                        if (!string.IsNullOrEmpty(tableName))
                        {
                            logger?.Debug($"SQL执行: 表[{tableName}]");
                        }
                    };

                    // SQL执行后事件 - 只在慢查询时记录
                    db.Aop.OnLogExecuted = (sql, pars) =>
                    {
                        var executionTime = db.Ado.SqlExecutionTime.TotalMilliseconds;

                        if (executionTime > 1000) // 超过1秒的慢查询
                        {
                            var tableName = ExtractTableName(sql);
                            logger?.Warn($"慢查询警告: 表[{tableName}] - 执行时间: {executionTime:F2}ms");
                        }
                    };

                    // SQL出错事件
                    db.Aop.OnError = (exp) =>
                    {
                        logger?.Error($"SQL执行出错: {exp.Message}", exp);
                    };
                });

                return db;
            });

            // 注册工作单元
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 注册通用仓储
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

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

            // 匹配 FROM 子句中的表名
            var fromMatch = Regex.Match(sql, @"FROM\s+`?(\w+)`?", RegexOptions.IgnoreCase);
            if (fromMatch.Success)
                return fromMatch.Groups[1].Value;

            // 匹配 INSERT INTO 中的表名
            var insertMatch = Regex.Match(sql, @"INSERT\s+INTO\s+`?(\w+)`?", RegexOptions.IgnoreCase);
            if (insertMatch.Success)
                return insertMatch.Groups[1].Value;

            // 匹配 UPDATE 中的表名
            var updateMatch = Regex.Match(sql, @"UPDATE\s+`?(\w+)`?", RegexOptions.IgnoreCase);
            if (updateMatch.Success)
                return updateMatch.Groups[1].Value;

            // 匹配 DELETE FROM 中的表名
            var deleteMatch = Regex.Match(sql, @"DELETE\s+FROM\s+`?(\w+)`?", RegexOptions.IgnoreCase);
            if (deleteMatch.Success)
                return deleteMatch.Groups[1].Value;

            // 匹配 ALTER TABLE 中的表名
            var alterMatch = Regex.Match(sql, @"ALTER\s+TABLE\s+`?(\w+)`?", RegexOptions.IgnoreCase);
            if (alterMatch.Success)
                return alterMatch.Groups[1].Value;

            // 匹配 information_schema 查询
            if (sql.Contains("INFORMATION_SCHEMA"))
                return "INFORMATION_SCHEMA";

            return "未知表";
        }
    }
}

