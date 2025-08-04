using SqlSugar;
using SUNWODA_SEVB.Data.Models;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto.Generators;

namespace SUNWODA_SEVB.Data.Configurations
{
    /// <summary>
    /// 数据库初始化器
    /// </summary>
    public class DbInitializer
    {
        private readonly ISqlSugarClient _db;
        private readonly ILogger _logger;

        public DbInitializer(ISqlSugarClient db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                // 检查并初始化默认用户
                await SeedUsersAsync();

                // 检查并初始化全局设置
                await SeedGlobalSettingsAsync();

                // 检查并初始化PLC配置
                await SeedPLCConfigsAsync();

                _logger.LogInformation("数据库种子数据初始化完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化种子数据失败");
                throw;
            }
        }

        private async Task SeedUsersAsync()
        {
            var userCount = await _db.Queryable<Users>().CountAsync();
            if (userCount == 0)
            {
                var defaultUsers = new List<Users>
                {
                    new Users
                    {
                        UserName = "admin",
                        Password = "admin123",//BCrypt.Net.BCrypt.HashPassword("admin123")
                        RoleId = 1,
                        Remark = "系统管理员"
                    },
                    new Users
                    {
                        UserName = "operator",
                        Password = "operator123", //BCrypt.Net.BCrypt.HashPassword("operator123")
                        RoleId = 2,
                        Remark = "操作员"
                    }
                };

                await _db.Insertable(defaultUsers).ExecuteCommandAsync();
                _logger.LogInformation("创建默认用户成功");
            }
        }

        private async Task SeedGlobalSettingsAsync()
        {
            var settingCount = await _db.Queryable<GlobalSetting>().CountAsync();
            if (settingCount == 0)
            {
                var defaultSettings = new List<GlobalSetting>
                {
                    new GlobalSetting
                    {
                        Name = "System.Version",
                        Value = "1.0.0",
                        Type = "String",
                        Remark = "系统版本号"
                    },
                    new GlobalSetting
                    {
                        Name = "PLC.DefaultTimeout",
                        Value = "5000",
                        Type = "Integer",
                        Unit = "ms",
                        Remark = "PLC默认超时时间"
                    },
                    new GlobalSetting
                    {
                        Name = "MES.ApiUrl",
                        Value = "http://localhost:8080/api",
                        Type = "String",
                        Remark = "MES接口地址"
                    },
                    new GlobalSetting
                    {
                        Name = "Log.RetentionDays",
                        Value = "30",
                        Type = "Integer",
                        Unit = "天",
                        Remark = "日志保留天数"
                    }
                };

                await _db.Insertable(defaultSettings).ExecuteCommandAsync();
                _logger.LogInformation("创建默认全局设置成功");
            }
        }

        private async Task SeedPLCConfigsAsync()
        {
            var plcCount = await _db.Queryable<PLCConfig>().CountAsync();
            if (plcCount == 0)
            {
                var defaultPLC = new PLCConfig
                {
                    Name = "主PLC",
                    IP = "192.168.1.100",
                    Port = 502,
                    BrandSpecificationProtocal = "Modbus TCP",
                    IsEnable = true,
                    Remark = "生产线主控制器"
                };

                await _db.Insertable(defaultPLC).ExecuteCommandAsync();
                _logger.LogInformation("创建默认PLC配置成功");
            }
        }
    }
}
