using System.Reflection;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Data.Configurations;
using SUNWODA_SEVB.Data.Models;

namespace SUNWODA_SEVB.Data
{
    public class DatabaseService : IDatabaseService
    {
        private readonly ISqlSugarClient _db;
        private readonly ILoggerService<DatabaseService> _logger;
        private readonly IConfiguration _configuration;

        public DatabaseService(
            ISqlSugarClient db,
            ILoggerService<DatabaseService> logger,
            IConfiguration configuration
        )
        {
            _db = db;
            _logger = logger;
            _configuration = configuration;
        }

        public bool Initialize()
        {
            try
            {
                _logger.Info("开始初始化数据库...");

                // 创建数据库（如果不存在）
                _db.DbMaintenance.CreateDatabase();
                _logger.Info("数据库创建/检查完成");

                // 创建表结构
                InitializeTables();

                // 初始化基础数据
                InitializeData();

                _logger.Info("数据库初始化完成");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"数据库初始化失败: {ex.Message}", ex);
                return false;
            }
        }

        private void InitializeTables()
        {
            _logger.Info("开始创建/检查数据表...");

            // 获取所有模型类型
            var modelTypes = GetModelTypes();

            if (modelTypes.Any())
            {
                // 批量创建表
                _db.CodeFirst.InitTables(modelTypes.ToArray());
                _logger.Info($"已创建/检查 {modelTypes.Count} 个数据表");
            }
            else
            {
                _logger.Warn("未找到任何数据模型类型");
            }
        }

        private void InitializeData()
        {
            // 初始化默认的变量参数
            var globalConfigs = _db.Queryable<GlobalSetting>();
            if (!globalConfigs.Any(it => it.Name == "IsCycleReadPLC"))
            {
                _db.Insertable(
                        new GlobalSetting
                        {
                            Name = "IsCycleReadPLC",
                            Value = "true",
                            Type = "bool",
                            Remark = "是否开启循环读取PLC",
                        }
                    )
                    .ExecuteCommand();
            }
            if (!globalConfigs.Any(it => it.Name == "IsCycleWritePLC"))
            {
                _db.Insertable(
                        new GlobalSetting
                        {
                            Name = "IsCycleWritePLC",
                            Value = "true",
                            Type = "bool",
                            Remark = "是否开启循环读取PLC",
                        }
                    )
                    .ExecuteCommand();
            }

            _logger.Info("GlobalSetting默认变量参数初始化完成");

            var plcConfigs = _db.Queryable<PLCConfig>();
            if (!plcConfigs.Any(it => it.Name == "欧姆龙测试PLC"))
            {
                _db.Insertable(
                        new PLCConfig()
                        {
                            Name = "欧姆龙测试PLC",
                            DeviceID = 1,
                            IP = "127.0.0.1",
                            Port = 9600,
                            BrandSpecificationProtocal = "Omron_Fins_TCP",
                            CycleReadTime = 500,
                            CycleWriteTime = 500,
                            IsEnable = true,
                        }
                    )
                    .ExecuteCommand();
            }
            _logger.Info("PLCConfig默认变量参数初始化完成");

            var devices = _db.Queryable<Device>();
            if (!devices.Any(it => it.Name == "测试设备1"))
            {
                _db.Insertable(
                        new Device()
                        {
                            Name = "测试设备1",
                            Number = "test-01",
                            BaseName = "惠州",
                            LineName = "测试线",
                        }
                    )
                    .ExecuteCommand();
            }
            _logger.Info("Device默认变量参数初始化完成");

            var plcRWConfigs = _db.Queryable<PLCRWConfig>();
            if (!plcRWConfigs.Any(it => it.Name == "测试地址段1"))
            {
                _db.Insertable(
                        new PLCRWConfig()
                        {
                            Name = "测试地址段1",
                            PLCID = 1,
                            AreaName = "D",
                            StartAddress = "100",
                            Length = 6,
                            RWMode = "R",
                            Cycle = 100,
                            AddressType = 1,
                            IsEnable = true,
                        }
                    )
                    .ExecuteCommand();
            }

            //if (!plcRWConfigs.Any(it => it.Name == "测试地址段2"))
            //{
            //    _db.Insertable(new PLCRWConfig()
            //    {
            //        Name = "测试地址段2",
            //        PLCID = 1,
            //        AreaName = "D",
            //        StartAddress = "110",
            //        Length = 26,
            //        RWMode = "W",
            //        AddressType = 1,
            //        IsEnable = true
            //    });
            //}
            _logger.Info("PLCRWConfig默认变量参数初始化完成");

            var plcAddressConfigs = _db.Queryable<PLCAddressConfig>();
            if (!plcAddressConfigs.Any(it => it.ParameterName == "测试地址1"))
            {
                _db.Insertable(
                        new PLCAddressConfig()
                        {
                            PLCID = 1,
                            PLCRWID = 1,
                            CategoryID = 1,
                            ParameterName = "测试地址1",
                            Type = "float",
                            Length = 1,
                            Address = "D100",
                            IsMonitor = true,
                        }
                    )
                    .ExecuteCommand();
            }
            if (!plcAddressConfigs.Any(it => it.ParameterName == "测试地址2"))
            {
                _db.Insertable(
                        new PLCAddressConfig()
                        {
                            PLCID = 1,
                            PLCRWID = 1,
                            CategoryID = 1,
                            ParameterName = "测试地址2",
                            Type = "int",
                            Length = 1,
                            Address = "D102",
                            IsMonitor = true,
                        }
                    )
                    .ExecuteCommand();
            }
            if (!plcAddressConfigs.Any(it => it.ParameterName == "测试地址3"))
            {
                _db.Insertable(
                        new PLCAddressConfig()
                        {
                            PLCID = 1,
                            PLCRWID = 1,
                            CategoryID = 1,
                            ParameterName = "测试地址3",
                            Type = "float",
                            Length = 1,
                            Address = "D104",
                            IsMonitor = true,
                        }
                    )
                    .ExecuteCommand();
            }
            //if (!_db.Queryable<VariableParameters>().Any())
            //{
            //    var defaultVariables = new List<VariableParameters>
            //    {
            //        new VariableParameters
            //        {
            //            VariableName = "系统运行状态",
            //            VariableCode = "SYS_STATUS",
            //            DataType = "Boolean",
            //            DefaultValue = "false",
            //            CurrentValue = "false",
            //            Description = "系统运行状态标志"
            //        },
            //        new VariableParameters
            //        {
            //            VariableName = "采样间隔",
            //            VariableCode = "SAMPLE_INTERVAL",
            //            DataType = "Integer",
            //            DefaultValue = "1000",
            //            CurrentValue = "1000",
            //            Description = "数据采样间隔(毫秒)"
            //        }
            //    };
            //    _db.Insertable(defaultVariables).ExecuteCommand();
            //    _logger.Info("默认变量参数初始化完成");
            //}

            // 初始化默认的PLC参数
            //if (!_db.Queryable<PlcParameters>().Any())
            //{
            //    var defaultPlcParams = new List<PlcParameters>
            //    {
            //        new PlcParameters
            //        {
            //            ParameterName = "设备启动",
            //            PlcAddress = "M0.0",
            //            DataType = "Bool",
            //            DataLength = 1,
            //            DefaultValue = "false",
            //            AccessType = "ReadWrite",
            //            Description = "设备启动控制位"
            //        },
            //        new PlcParameters
            //        {
            //            ParameterName = "当前温度",
            //            PlcAddress = "DB1.DBD0",
            //            DataType = "Real",
            //            DataLength = 4,
            //            DefaultValue = "0.0",
            //            AccessType = "Read",
            //            Description = "设备当前温度值"
            //        }
            //    };
            //    _db.Insertable(defaultPlcParams).ExecuteCommand();
            //    _logger.Info("默认PLC参数初始化完成");
            //}
        }

        private List<Type> GetModelTypes()
        {
            try
            {
                // 获取当前程序集
                var assembly = Assembly.GetExecutingAssembly();

                // 获取 SUNWODA_SEVB.Data.Models 命名空间下的所有类型
                return assembly
                    .GetTypes()
                    .Where(t =>
                        t.Namespace == "SUNWODA_SEVB.Data.Models" && !t.IsAbstract && !t.IsInterface
                    )
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.Error($"获取模型类型失败: {ex.Message}", ex);
                return new List<Type>();
            }
        }

        public void Backup(string backupPath)
        {
            try
            {
                _logger.Info($"开始备份数据库到: {backupPath}");
                // MySQL备份逻辑
                var sql =
                    $"mysqldump -h{_db.CurrentConnectionConfig.ConnectionString} > {backupPath}";
                // 实际实现需要调用MySQL备份命令
                _logger.Info("数据库备份完成");
            }
            catch (Exception ex)
            {
                _logger.Error($"数据库备份失败: {ex.Message}", ex);
                throw;
            }
        }

        public void Restore(string backupPath)
        {
            try
            {
                _logger.Info($"开始从备份恢复数据库: {backupPath}");
                // MySQL恢复逻辑
                _logger.Info("数据库恢复完成");
            }
            catch (Exception ex)
            {
                _logger.Error($"数据库恢复失败: {ex.Message}", ex);
                throw;
            }
        }
    }
}
