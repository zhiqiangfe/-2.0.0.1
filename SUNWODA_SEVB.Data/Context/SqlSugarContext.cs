using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlSugar;
using SUNWODA_SEVB.Data.Models;
using SUNWODA_SEVB.Data.Configurations;

namespace SUNWODA_SEVB.Data.Context
{
    /// <summary>
    /// SqlSugar上下文实现
    /// </summary>
    internal class SqlSugarContext
    {
        public ISqlSugarClient Db { get; private set; }
        private readonly ILogger<SqlSugarContext> _logger;

        public SqlSugarContext(IConfiguration configuration, ILogger<SqlSugarContext> logger)
        {
            _logger = logger;

            var config = new ConnectionConfig()
            {
                ConnectionString = configuration.GetConnectionString("DefaultConnection"),
                DbType = SqlSugar.DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                MoreSettings = new ConnMoreSettings()
                {
                    DisableNvarchar = true,
                    DefaultCacheDurationInSeconds = 60
                }
            };

            Db = new SqlSugarScope(config, db =>
            {
                // SQL 执行前事件
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    _logger.LogDebug("准备执行SQL: {Sql}", sql);
                };

                // SQL 执行后事件
                db.Aop.OnLogExecuted = (sql, pars) =>
                {
                    _logger.LogInformation("SQL执行成功，耗时: {Time}ms",
                        db.Ado.SqlExecutionTime.TotalMilliseconds);
                };

                // SQL 执行错误事件
                db.Aop.OnError = (ex) =>
                {
                    _logger.LogError(ex, "SQL执行失败: {Sql}", ex.Sql);
                };
            });
        }

        public async Task<bool> InitializeDatabaseAsync()
        {
            try
            {
                // 创建数据库
                Db.DbMaintenance.CreateDatabase();
                _logger.LogInformation("数据库创建/验证成功");

                // 获取所有数据模型类型
                var modelTypes = GetDataModelTypes();

                // 创建表
                foreach (var modelType in modelTypes)
                {
                    var tableName = GetTableName(modelType);
                    if (!Db.DbMaintenance.IsAnyTable(tableName))
                    {
                        Db.CodeFirst.InitTables(modelType);
                        _logger.LogInformation("创建表: {TableName}", tableName);
                    }
                    else
                    {
                        // 更新表结构
                        Db.CodeFirst.BackupTable().InitTables(modelType);
                        _logger.LogDebug("更新表结构: {TableName}", tableName);
                    }
                }

                // 创建索引
                await CreateIndexesAsync();

                _logger.LogInformation("数据库表结构初始化完成");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "数据库初始化失败");
                return false;
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                await Db.Queryable<dynamic>().Select("1").FirstAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task SeedDataAsync()
        {
            var seeder = new DbInitializer(Db, _logger);
            await seeder.SeedAsync();
        }

        private Type[] GetDataModelTypes()
        {
            return new[]
            {
                typeof(AppLogParameters),
                typeof(DeviceModel),
                typeof(GlobalSettingModel),
                typeof(MesInterfaceLogModel),
                typeof(PLCAddressConfigModel),
                typeof(PLCConfigModel),
                typeof(PLCRWConfigModel),
                typeof(ProjectSettingModel),
                typeof(UsersModel),
                typeof(WebInterfaceLogModel),
                typeof(WorkSpaceProjectModel)
            };
        }

        private string GetTableName(Type modelType)
        {
            var attribute = modelType.GetCustomAttributes(typeof(SugarTable), false)
                .FirstOrDefault() as SugarTable;
            return attribute?.TableName ?? modelType.Name;
        }

        private async Task CreateIndexesAsync()
        {
            try
            {
                // 创建设备表索引
                await CreateIndexAsync("devices", "idx_device_number", "Number");
                await CreateIndexAsync("devices", "idx_device_base", "BaseName");

                // 创建用户表索引
                await CreateIndexAsync("users", "idx_user_username", "UserName");

                // 创建全局设置表索引
                await CreateIndexAsync("global_setting", "idx_setting_name", "Name");

                // 创建日志表索引
                await CreateIndexAsync("app_logs", "idx_log_time", "logtime");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "创建索引时发生警告");
            }
        }

        private async Task CreateIndexAsync(string tableName, string indexName, string columnName)
        {
            var sql = $"CREATE INDEX IF NOT EXISTS {indexName} ON {tableName} ({columnName})";
            await Db.Ado.ExecuteCommandAsync(sql);
        }
    }
}
