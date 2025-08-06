using Microsoft.Extensions.DependencyInjection;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Models.Data;

namespace SUNWODA_SEVB.Data
{
    public class DataLayerTest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerService<DataLayerTest>? _logger;

        public DataLayerTest(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetService<ILoggerService<DataLayerTest>>();
        }

        private void LogInfo(string message)
        {
            Console.WriteLine(message);
            _logger?.Info(message);
        }

        /// <summary>
        /// 测试设备表的CRUD操作
        /// </summary>
        public async Task TestDeviceCRUD()
        {
            var deviceRepo = _serviceProvider.GetRequiredService<IDeviceRepository>();

            // 1. 创建测试数据
            var newDevice = new DeviceModel
            {
                Number = "TEST001",
                Name = "测试设备1",
                BaseName = "测试基地",
                LineName = "测试产线",
                Remark = "测试备注"
            };

            LogInfo("1. 添加设备...");
            var addResult = await deviceRepo.AddAsync(newDevice);
            LogInfo($"   添加结果: {addResult}");

            // 2. 查询刚添加的设备
            LogInfo("\n2. 查询设备...");
            var devices = await deviceRepo.GetByNumberAsync("TEST001");
            if (devices.Any())
            {
                var device = devices.First();
                LogInfo($"   找到设备: ID={device.ID}, Name={device.Name}");

                // 3. 更新设备
                LogInfo("\n3. 更新设备...");
                device.Remark = "更新后的备注";
                var updateResult = await deviceRepo.UpdateAsync(device);
                LogInfo($"   更新结果: {updateResult}");

                // 4. 验证更新
                LogInfo("\n4. 验证更新...");
                var updatedDevice = await deviceRepo.GetByIdAsync(device.ID);
                LogInfo($"   更新后备注: {updatedDevice?.Remark}");

                // 5. 删除设备
                LogInfo("\n5. 删除设备...");
                var deleteResult = await deviceRepo.DeleteByIdAsync(device.ID);
                LogInfo($"   删除结果: {deleteResult}");

                // 6. 验证删除
                LogInfo("\n6. 验证删除...");
                var deletedDevice = await deviceRepo.GetByIdAsync(device.ID);
                LogInfo($"   设备是否还存在: {deletedDevice != null}");
            }
        }

        /// <summary>
        /// 测试全局设置的CRUD操作
        /// </summary>
        public async Task TestGlobalSettingCRUD()
        {
            var settingRepo = _serviceProvider.GetRequiredService<IGlobalSettingRepository>();

            // 1. 添加设置
            var newSetting = new GlobalSettingModel
            {
                Name = "Test.Setting",
                Value = "100",
                Type = "INT",
                Unit = "ms",
                Remark = "测试设置"
            };

            LogInfo("1. 添加设置...");
            var addResult = await settingRepo.AddAsync(newSetting);
            LogInfo($"   添加结果: {addResult}");

            // 2. 获取设置值（自动转换类型）
            LogInfo("\n2. 获取设置值...");
            var value = await settingRepo.GetSettingValueAsync("Test.Setting");
            LogInfo($"   值: {value}, 类型: {value?.GetType().Name}");

            // 3. 更新设置值
            LogInfo("\n3. 更新设置值...");
            var updateResult = await settingRepo.UpdateSettingValueAsync("Test.Setting", 200);
            LogInfo($"   更新结果: {updateResult}");

            // 4. 验证更新
            LogInfo("\n4. 验证更新...");
            var newValue = await settingRepo.GetSettingValueAsync("Test.Setting");
            LogInfo($"   新值: {newValue}");

            // 5. 删除设置
            LogInfo("\n5. 删除设置...");
            var setting = await settingRepo.GetByNameAsync("Test.Setting");
            if (setting != null)
            {
                var deleteResult = await settingRepo.DeleteAsync(setting);
                LogInfo($"   删除结果: {deleteResult}");
            }
        }

        /// <summary>
        /// 测试事务操作
        /// </summary>
        public async Task TestTransaction()
        {
            var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
            var plcConfigRepo = _serviceProvider.GetRequiredService<IPLCConfigRepository>();
            var plcRWConfigRepo = _serviceProvider.GetRequiredService<IPLCRWConfigRepository>();

            try
            {
                LogInfo("开始事务测试...");
                await unitOfWork.BeginTransactionAsync();

                // 添加PLC配置
                var plcConfig = new PLCConfigModel
                {
                    Name = "测试PLC",
                    IP = "192.168.1.200",
                    Port = 502,
                    BrandSpecificationProtocal = "MODBUS_TCP",
                    CycleReadTime = 1000,
                    CycleWriteTime = 1000,
                    IsEnable = true,
                    Remark = "事务测试"
                };

                await plcConfigRepo.AddAsync(plcConfig);
                LogInfo("   PLC配置已添加");

                // 添加PLC读写配置
                var rwConfig = new PLCRWConfigModel
                {
                    Name = "测试读写区",
                    PLCID = 999, // 故意使用错误的ID来测试事务回滚
                    AreaName = "DB1",
                    StartAddress = "0",
                    Length = 100,
                    RWMode = "Read",
                    Cycle = 1000,
                    AddressType = 1,
                    IsEnable = true
                };

                await plcRWConfigRepo.AddAsync(rwConfig);
                LogInfo("   读写配置已添加");

                await unitOfWork.CommitAsync();
                LogInfo("事务提交成功");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                LogInfo($"事务回滚: {ex.Message}");
            }
        }
    }
}
