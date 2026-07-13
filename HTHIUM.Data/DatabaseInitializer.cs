using Microsoft.Extensions.Configuration;
using SqlSugar;
using HTHIUM.Core.Attributes;
using HTHIUM.Core.Common;
using HTHIUM.Core.Enumerations;
using HTHIUM.Core.Interfaces;
using HTHIUM.Core.Interfaces.Data;
using HTHIUM.Data.Models;
using System.Reflection;

namespace HTHIUM.Data
{
    /// <summary>
    /// 数据库初始化器实现
    /// </summary>
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ISqlSugarClient _db;
        private readonly ILoggerService<DatabaseInitializer> _logger;
        private readonly IConfiguration _configuration;
        private static readonly object _lock = new object();
        private static bool _initialized = false;

        public DatabaseInitializer(
            ISqlSugarClient db,
            ILoggerService<DatabaseInitializer> logger,
            IConfiguration configuration)
        {
            _db = db;
            _logger = logger;
            _configuration = configuration;
        }

        public bool Initialize()
        {
            // 双重检查锁定模式
            if (_initialized)
            {
                _logger.Info("数据库已经初始化，跳过重复初始化");
                return true;
            }

            lock (_lock)
            {
                if (_initialized)
                {
                    return true;
                }

                try
                {
                    _logger.Info("========== 开始数据库初始化 ==========");

                    // 步骤1：创建数据库
                    CreateDatabase();

                    // 步骤2：创建表结构
                    CreateTables();

                    // 步骤3：初始化默认数据
                    InitializeDefaultData();

                    _initialized = true;
                    _logger.Info("========== 数据库初始化成功 ==========");
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.Error($"数据库初始化失败: {ex.Message}", ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        private void CreateDatabase()
        {
            try
            {
                _logger.Info("检查/创建数据库...");
                _db.DbMaintenance.CreateDatabase();
                _logger.Info("数据库检查/创建完成");
            }
            catch (Exception ex)
            {
                _logger.Error($"创建数据库失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 创建表结构
        /// </summary>
        private void CreateTables()
        {
            _logger.Info("开始创建/更新表结构...");

            var modelTypes = GetAllModelTypes();
            if (!modelTypes.Any())
            {
                _logger.Warn("未找到任何数据模型类型");
                return;
            }

            // 分批创建表，避免一次性创建太多表导致超时
            const int batchSize = 5;
            for (int i = 0; i < modelTypes.Count; i += batchSize)
            {
                var batch = modelTypes.Skip(i).Take(batchSize).ToArray();
                try
                {
                    _db.CodeFirst.InitTables(batch);
                    _logger.Info($"已创建/检查 {batch.Length} 个表 (第 {i / batchSize + 1} 批)");
                }
                catch (Exception ex)
                {
                    _logger.Error($"创建表失败 (批次 {i / batchSize + 1}): {ex.Message}", ex);
                    throw;
                }
            }

            _logger.Info($"表结构创建/更新完成，共处理 {modelTypes.Count} 个表");
        }

        /// <summary>
        /// 获取所有模型类型
        /// </summary>
        private List<Type> GetAllModelTypes()
        {
            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => a.FullName?.Contains("HTHIUM") ?? false);

                var modelTypes = new List<Type>();

                foreach (var assembly in assemblies)
                {
                    var types = assembly.GetTypes()
                        .Where(t => t.Namespace == "HTHIUM.Data.Models"
                            && t.IsClass
                            && !t.IsAbstract
                            && !t.IsInterface
                            && !t.IsGenericType);

                    modelTypes.AddRange(types);
                }

                _logger.Info($"找到 {modelTypes.Count} 个数据模型类型");
                return modelTypes;
            }
            catch (Exception ex)
            {
                _logger.Error($"获取模型类型失败: {ex.Message}", ex);
                return new List<Type>();
            }
        }

        /// <summary>
        /// 初始化默认数据
        /// </summary>
        private void InitializeDefaultData()
        {
            _logger.Info("开始初始化默认数据...");

            // 使用事务确保数据一致性
            _db.Ado.BeginTran();
            try
            {
                // 按依赖顺序初始化数据
                InitializeGlobalSettings();
                InitializeDevices();
                InitializePLCConfigs();
                InitializePLCRWConfigs();
                InitializePLCAddressConfigs();
                InitializeUsers();
                InitializeHmiAlarmData();
                InitializeWorkSpaceProjects();
                //InitializeMESSettings();// 后续启用

                _db.Ado.CommitTran();
                _logger.Info("默认数据初始化完成");
            }
            catch (Exception ex)
            {
                _db.Ado.RollbackTran();
                _logger.Error($"初始化默认数据失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 初始化全局设置
        /// </summary>
        private void InitializeGlobalSettings()
        {
            var settings = new[]
            {
                new GlobalSetting { Name = "IsCycleReadPLC", Value = "true", Type = "bool", Remark = "是否开启循环读取PLC", RoleRank = 2 },
                new GlobalSetting { Name = "IsCycleWritePLC", Value = "true", Type = "bool", Remark = "是否开启循环写入PLC", RoleRank = 2 },
                new GlobalSetting { Name = "DefaultProject", Value = "", Type = "string", Remark = "启动默认显示项目(VM)", RoleRank = 2 },
                new GlobalSetting { Name = "CurrentUserAccount", Value = "guest", Type = "string", Remark = "当前用户", RoleRank = 0b1000 },//000访客 001工程师  010管理员 100超级管理员 
                new GlobalSetting { Name = "IsMESEnabled", Value = "false", Type = "bool", Remark = "是否启用MES功能", RoleRank = 2 },
                new GlobalSetting { Name = "PLCConnectTime", Value = "5000", Type = "int", Unit = "ms", Remark = "PLC重连时间", RoleRank = 2 },
                new GlobalSetting { Name = "IsHmiAlarmMonitorEnabled", Value = "true", Type = "bool", Remark = "是否启用HMI报警后台采集", RoleRank = 2 },
                new GlobalSetting { Name = "HmiAlarmMonitorCycleMs", Value = "1000", Type = "int", Unit = "ms", Remark = "HMI报警后台采集周期", RoleRank = 2 },
                new GlobalSetting { Name = "HmiAlarmActiveParameterName", Value = "报警触发地址", Type = "string", Remark = "PLC报警触发信号对应的parameter_name", RoleRank = 2 },
                new GlobalSetting { Name = "HmiAlarmCodeParameterName", Value = "报警代码", Type = "string", Remark = "PLC报警代码对应的parameter_name", RoleRank = 2 },
                new GlobalSetting { Name = "LineName", Value = "L1 密封钉线", Type = "string", Remark = "当前产线名称", RoleRank = 2 },
            };

            foreach (var setting in settings)
            {
                if (!_db.Queryable<GlobalSetting>().Any(it => it.Name == setting.Name))
                {
                    _db.Insertable(setting).ExecuteCommand();
                }
            }
            _logger.Info("全局设置初始化完成");
        }

        /// <summary>
        /// 初始化设备
        /// </summary>
        private void InitializeDevices()
        {
            if (!_db.Queryable<Device>().Any(it => it.Name == "测试设备1"))
            {
                _db.Insertable(new Device
                {
                    Name = "测试设备1",
                    Number = "test-01",
                    BaseName = "惠州",
                    LineName = "测试线"
                }).ExecuteCommand();
            }
            _logger.Info("设备数据初始化完成");
        }

        /// <summary>
        /// 初始化PLC配置
        /// </summary>
        private void InitializePLCConfigs()
        {
            if (!_db.Queryable<PLCConfig>().Any(it => it.Name == "欧姆龙测试PLC"))
            {
                _db.Insertable(new PLCConfig
                {
                    Name = "欧姆龙测试PLC",
                    DeviceID = 1,
                    IP = "127.0.0.1",
                    Port = 9600,
                    BrandSpecificationProtocal = "Omron_Fins_TCP",
                    CycleReadTime = 500,
                    CycleWriteTime = 500,
                    IsEnable = true
                }).ExecuteCommand();
            }
            _logger.Info("PLC配置初始化完成");
        }

        /// <summary>
        /// 初始化PLC读写配置
        /// </summary>
        private void InitializePLCRWConfigs()
        {
            if (!_db.Queryable<PLCRWConfig>().Any(it => it.Name == "测试地址段1"))
            {
                _db.Insertable(new PLCRWConfig
                {
                    Name = "测试地址段1",
                    PLCID = 1,
                    AreaName = "D",
                    StartAddress = "100",
                    Length = 6,
                    RWMode = "R",
                    Cycle = 100,
                    AddressType = 1,
                    IsEnable = true
                }).ExecuteCommand();
            }
            _logger.Info("PLC读写配置初始化完成");
        }

        /// <summary>
        /// 初始化PLC地址配置
        /// </summary>
        private void InitializePLCAddressConfigs()
        {
            var configs = new[]
            {
                new PLCAddressConfig { PLCID = 1, PLCRWID = 1, CategoryID = 1, ParameterName = "测试地址1", Type = "float", Length = 1, Address = "D100", IsMonitor = true },
                new PLCAddressConfig { PLCID = 1, PLCRWID = 1, CategoryID = 1, ParameterName = "测试地址2", Type = "int", Length = 1, Address = "D102", IsMonitor = true },
                new PLCAddressConfig { PLCID = 1, PLCRWID = 1, CategoryID = 1, ParameterName = "测试地址3", Type = "float", Length = 1, Address = "D104", IsMonitor = true }
            };

            foreach (var config in configs)
            {
                if (!_db.Queryable<PLCAddressConfig>().Any(it => it.ParameterName == config.ParameterName))
                {
                    _db.Insertable(config).ExecuteCommand();
                }
            }
            _logger.Info("PLC地址配置初始化完成");
        }

        /// <summary>
        /// 初始化用户
        /// </summary>
        private void InitializeUsers()
        {
            var users = new[]
            {
                new Users { UserAccount = "guest", UserName = "访客(默认)", Password = BCrypt.Net.BCrypt.HashPassword(""), RoleId = (int)UserRole.Guest, CreatedTime = DateTime.Now },
                new Users { UserAccount = "engineer", UserName = "工程师(默认)", Password = BCrypt.Net.BCrypt.HashPassword("swd123456"), RoleId = (int)UserRole.Engineer, CreatedTime = DateTime.Now },
                new Users { UserAccount = "admin", UserName = "管理员(默认)", Password = BCrypt.Net.BCrypt.HashPassword("ime123456"), RoleId = (int)UserRole.Admin, CreatedTime = DateTime.Now },
                new Users { UserAccount = "sadmin", UserName = "超级管理员(默认)", Password = BCrypt.Net.BCrypt.HashPassword("sime123456"), RoleId = (int)UserRole.SuperAdmin, CreatedTime = DateTime.Now }
            };

            foreach (var user in users)
            {
                if (!_db.Queryable<Users>().Any(it => it.UserAccount == user.UserAccount))
                {
                    _db.Insertable(user).ExecuteCommand();
                }
            }
            _logger.Info("用户数据初始化完成");
        }

        /// <summary>
        /// 初始化HMI报警分析页面演示数据
        /// </summary>
        private void InitializeHmiAlarmData()
        {
            var alarmMaps = new[]
            {
                new HmiAlarmCodeMap
                {
                    AlarmCode = "HMI-LASER-203",
                    AlarmName = "激光器通信异常",
                    AlarmLevel = "高",
                    DeviceName = "密封钉设备 01",
                    StationName = "激光焊接",
                    ProcessName = "密封钉焊接",
                    PossibleReason = "1. 激光器网口通信中断\r\n2. 交换机端口异常\r\n3. PLC通信点位未刷新\r\n4. 激光器控制器掉电或重启",
                    HandleSuggestion = "优先检查激光器网线、交换机端口和控制器电源；通信丢失超过3秒时同步抓取PLC快照和HMI操作记录。"
                },
                new HmiAlarmCodeMap
                {
                    AlarmCode = "HMI-SCAN-104",
                    AlarmName = "条码枪读取超时",
                    AlarmLevel = "中",
                    DeviceName = "密封钉设备 01",
                    StationName = "扫码工位",
                    ProcessName = "扫码上料",
                    PossibleReason = "1. 条码污损或反光\r\n2. 扫码枪焦距偏移\r\n3. 光源亮度不足\r\n4. 产品到位信号延迟",
                    HandleSuggestion = "检查扫码枪焦距、光源亮度和条码表面；连续超时超过3次时推送班组复扫提醒。"
                },
                new HmiAlarmCodeMap
                {
                    AlarmCode = "HMI-GLUE-087",
                    AlarmName = "胶压低于下限",
                    AlarmLevel = "中",
                    DeviceName = "密封钉设备 01",
                    StationName = "点胶工位",
                    ProcessName = "点胶",
                    PossibleReason = "1. 胶桶余量不足\r\n2. 供胶泵压力波动\r\n3. 点胶阀堵塞\r\n4. 压力传感器漂移",
                    HandleSuggestion = "联动胶压曲线和供胶泵状态，低压持续超过10秒自动暂停点胶并提示检查胶桶余量。"
                },
                new HmiAlarmCodeMap
                {
                    AlarmCode = "HMI-TRAY-311",
                    AlarmName = "料盘满料未取走",
                    AlarmLevel = "高",
                    DeviceName = "密封钉设备 01",
                    StationName = "下料工位",
                    ProcessName = "下料",
                    PossibleReason = "1. AGV取料任务未响应\r\n2. 下料缓存区已满\r\n3. 满料传感器误触发\r\n4. 人工取料未确认",
                    HandleSuggestion = "将满料信号与AGV取料任务绑定，超过60秒未取走时升级通知并打开缓存预警。"
                },
                new HmiAlarmCodeMap
                {
                    AlarmCode = "HMI-SENSOR-066",
                    AlarmName = "传感器波动",
                    AlarmLevel = "低",
                    DeviceName = "密封钉设备 01",
                    StationName = "检测工位",
                    ProcessName = "视觉检测",
                    PossibleReason = "1. 传感器安装松动\r\n2. 环境温度或振动影响\r\n3. 信号线屏蔽不良\r\n4. 检测位置存在偏移",
                    HandleSuggestion = "记录传感器波动频次与温度、振动数据，超过阈值时安排点检并更换易漂移传感器。"
                },
                new HmiAlarmCodeMap
                {
                    AlarmCode = "HMI-MES-502",
                    AlarmName = "MES 上传重试",
                    AlarmLevel = "中",
                    DeviceName = "密封钉设备 01",
                    StationName = "MES 通讯",
                    ProcessName = "过站上传",
                    PossibleReason = "1. MES接口响应超时\r\n2. 网络延迟过高\r\n3. 批次校验失败\r\n4. 本地缓存补传队列阻塞",
                    HandleSuggestion = "统计接口响应时间和重试次数，连续重试时先缓存过站数据，恢复后自动补传。"
                }
            };

            foreach (var map in alarmMaps)
            {
                if (!_db.Queryable<HmiAlarmCodeMap>().Any(it => it.AlarmCode == map.AlarmCode && it.DeviceName == map.DeviceName))
                {
                    _db.Insertable(map).ExecuteCommand();
                }
            }

            var alarmDate = new DateTime(2026, 7, 2);
            if (!_db.Queryable<HmiAlarmRecord>().Any(it => it.TriggerTime >= alarmDate && it.TriggerTime < alarmDate.AddDays(1)))
            {
                var records = new[]
                {
                    CreateHmiAlarmRecord(alarmDate, "10:42:11", "11:00:23", "HMI-LASER-203", "激光器通信异常", "高", "激光焊接", "密封钉焊接", -320, 128, "LaserCommLost"),
                    CreateHmiAlarmRecord(alarmDate, "11:18:06", "11:22:41", "HMI-SCAN-104", "条码枪读取超时", "中", "扫码工位", "扫码上料", -54, 76, "ScannerReadDone"),
                    CreateHmiAlarmRecord(alarmDate, "13:52:48", "13:57:09", "HMI-GLUE-087", "胶压低于下限", "中", "点胶工位", "点胶", -62, 94, "GluePressureLow"),
                    CreateHmiAlarmRecord(alarmDate, "16:31:12", "16:43:18", "HMI-TRAY-311", "料盘满料未取走", "高", "下料工位", "下料", -145, 168, "TrayFull"),
                    CreateHmiAlarmRecord(alarmDate, "17:08:19", "17:19:30", "HMI-SENSOR-066", "传感器波动", "低", "检测工位", "视觉检测", -96, 132, "SensorFluctuation"),
                    CreateHmiAlarmRecord(alarmDate, "19:04:10", "19:08:36", "HMI-MES-502", "MES 上传重试", "中", "MES 通讯", "过站上传", -38, 85, "MesUploadRetry"),
                    CreateHmiAlarmRecord(alarmDate, "09:16:32", "09:18:46", "HMI-LASER-203", "激光器通信异常", "高", "激光焊接", "密封钉焊接", -45, 92, "LaserCommLost"),
                    CreateHmiAlarmRecord(alarmDate, "14:27:08", "14:30:54", "HMI-SCAN-104", "条码枪读取超时", "中", "扫码工位", "扫码上料", -41, 118, "ScannerReadDone"),
                    CreateHmiAlarmRecord(alarmDate, "15:41:36", "15:47:20", "HMI-GLUE-087", "胶压低于下限", "中", "点胶工位", "点胶", -58, 147, "GluePressureLow"),
                    CreateHmiAlarmRecord(alarmDate, "18:22:15", "18:30:05", "HMI-TRAY-311", "料盘满料未取走", "高", "下料工位", "下料", -82, 175, "TrayFull")
                };

                _db.Insertable(records).ExecuteCommand();
            }

            _logger.Info("HMI报警数据初始化完成");
        }

        private static HmiAlarmRecord CreateHmiAlarmRecord(
            DateTime alarmDate,
            string triggerTime,
            string recoverTime,
            string alarmCode,
            string alarmName,
            string alarmLevel,
            string stationName,
            string processName,
            int impactQty,
            int responseSeconds,
            string rawValue)
        {
            var trigger = alarmDate.Add(TimeSpan.Parse(triggerTime));
            var recover = alarmDate.Add(TimeSpan.Parse(recoverTime));

            return new HmiAlarmRecord
            {
                LineName = "L1 密封钉线",
                DeviceName = "密封钉设备 01",
                StationName = stationName,
                ProcessName = processName,
                AlarmCode = alarmCode,
                AlarmName = alarmName,
                AlarmLevel = alarmLevel,
                TriggerTime = trigger,
                RecoverTime = recover,
                DurationSeconds = (int)(recover - trigger).TotalSeconds,
                AlarmStatus = "已恢复",
                Source = "HMI",
                RawValue = rawValue,
                ImpactQty = impactQty,
                ResponseSeconds = responseSeconds,
                CreatedTime = trigger
            };
        }

        /// <summary>
        /// 初始化工作区项目
        /// </summary>
        private void InitializeWorkSpaceProjects()
        {
            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => a.FullName?.StartsWith("HTHIUM") ?? false);

                int addedCount = 0;
                foreach (var assembly in assemblies)
                {
                    var viewModelTypes = assembly.GetTypes()
                        .Where(t => t.IsClass && !t.IsAbstract
                            && t.IsSubclassOf(typeof(ViewModelBase))
                            && t.GetCustomAttribute<ModuleAttribute>() != null);

                    foreach (var vmType in viewModelTypes)
                    {
                        var moduleAttr = vmType.GetCustomAttribute<ModuleAttribute>();
                        if (moduleAttr != null)
                        {
                            if (!_db.Queryable<WorkSpaceProject>().Any(it => it.VMClassName == vmType.Name))
                            {
                                var isEnabled = moduleAttr.Type == ModuleType.Settings
                                    || moduleAttr.Type == ModuleType.UserCenter
                                    || vmType.Name == "VM_DeviceOeeAnalysisPage"
                                    || vmType.Name == "VM_StationCtBottleneckAnalysisPage"
                                    || vmType.Name == "VM_HmiAlarmTopAnalysisPage"
                                    || vmType.Name == "VM_PlcSnapshotTracePage"
                                    || vmType.Name == "VM_ProductQualityTracePage"
                                    || vmType.Name == "VM_CpkProcessCapabilityPage"
                                    || vmType.Name == "VM_ServoFrequentAlarmMonitorPage"
                                    || vmType.Name == "VM_CylinderMonitorPage"
                                    || vmType.Name == "VM_ProcessParameterTracePage";

                                _db.Insertable(new WorkSpaceProject
                                {
                                    VMClassName = vmType.Name,
                                    IsEnabled = isEnabled
                                }).ExecuteCommand();

                                addedCount++;
                            }
                        }
                    }
                }

                _logger.Info($"工作区项目初始化完成，新增 {addedCount} 个项目");
            }
            catch (Exception ex)
            {
                _logger.Error($"初始化工作区项目失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 初始化MES设置
        /// </summary>
        private void InitializeMESSettings()
        {
            // 如果有MES相关的默认设置，在这里初始化
            _logger.Info("MES设置初始化完成");
        }
    }
}
