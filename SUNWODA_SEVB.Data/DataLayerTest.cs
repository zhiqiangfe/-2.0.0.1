using Microsoft.Extensions.DependencyInjection;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Interfaces.Data;
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

        private void LogError(string message)
        {
            Console.WriteLine($"ERROR: {message}");
            _logger?.Error(message);
        }

        /// <summary>
        /// 测试设备表的CRUD操作
        /// </summary>
        public async Task TestDeviceCRUD()
        {
            var deviceRepo = _serviceProvider.GetRequiredService<IDeviceRepository>();
            const string testDeviceNumber = "TEST001";

            try
            {
                // 1. 创建前检查是否已存在
                LogInfo("1. 检查设备是否已存在...");
                var existingDevices = await deviceRepo.GetByNumberAsync(testDeviceNumber);
                if (existingDevices.Any())
                {
                    LogInfo($"   设备 {testDeviceNumber} 已存在，先删除旧数据");
                    foreach (var dev in existingDevices)
                    {
                        await deviceRepo.DeleteByIdAsync(dev.ID);
                    }
                }

                // 2. 创建测试数据
                var newDevice = new DeviceModel
                {
                    Number = testDeviceNumber,
                    Name = "测试设备1",
                    BaseName = "测试基地",
                    LineName = "测试产线",
                    Remark = "测试备注"
                };

                LogInfo("\n2. 添加设备...");
                var addResult = await deviceRepo.AddAsync(newDevice);
                if (!addResult)
                {
                    LogError("   添加设备失败");
                    return;
                }
                LogInfo($"   添加结果: 成功");

                // 3. 查询刚添加的设备
                LogInfo("\n3. 查询设备...");
                var devices = await deviceRepo.GetByNumberAsync(testDeviceNumber);
                if (!devices.Any())
                {
                    LogError("   未找到刚添加的设备");
                    return;
                }

                var device = devices.First();
                LogInfo($"   找到设备: ID={device.ID}, Name={device.Name}");

                // 4. 更新前验证设备存在
                LogInfo("\n4. 更新设备...");
                var deviceToUpdate = await deviceRepo.GetByIdAsync(device.ID);
                if (deviceToUpdate == null)
                {
                    LogError("   要更新的设备不存在");
                    return;
                }

                deviceToUpdate.Remark = "更新后的备注";
                var updateResult = await deviceRepo.UpdateAsync(deviceToUpdate);
                if (!updateResult)
                {
                    LogError("   更新设备失败");
                    return;
                }
                LogInfo($"   更新结果: 成功");

                // 5. 验证更新
                LogInfo("\n5. 验证更新...");
                var updatedDevice = await deviceRepo.GetByIdAsync(device.ID);
                if (updatedDevice == null)
                {
                    LogError("   未找到更新后的设备");
                    return;
                }
                LogInfo($"   更新后备注: {updatedDevice.Remark}");

                // 验证更新是否成功
                if (updatedDevice.Remark != "更新后的备注")
                {
                    LogError("   更新验证失败：备注未正确更新");
                    return;
                }

                // 6. 删除前验证设备存在
                LogInfo("\n6. 删除设备...");
                var deviceToDelete = await deviceRepo.GetByIdAsync(device.ID);
                if (deviceToDelete == null)
                {
                    LogError("   要删除的设备不存在");
                    return;
                }

                var deleteResult = await deviceRepo.DeleteByIdAsync(device.ID);
                if (!deleteResult)
                {
                    LogError("   删除设备失败");
                    return;
                }
                LogInfo($"   删除结果: 成功");

                // 7. 验证删除
                LogInfo("\n7. 验证删除...");
                var deletedDevice = await deviceRepo.GetByIdAsync(device.ID);
                LogInfo($"   设备是否还存在: {deletedDevice != null}");

                if (deletedDevice != null)
                {
                    LogError("   删除验证失败：设备仍然存在");
                }
                else
                {
                    LogInfo("   删除验证成功：设备已被删除");
                }
            }
            catch (Exception ex)
            {
                LogError($"设备CRUD测试失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 测试全局设置的CRUD操作
        /// </summary>
        public async Task TestGlobalSettingCRUD()
        {
            var settingRepo = _serviceProvider.GetRequiredService<IGlobalSettingRepository>();
            const string testSettingName = "Test.Setting";

            try
            {
                // 1. 添加前检查是否已存在
                LogInfo("1. 检查设置是否已存在...");
                var existingSetting = await settingRepo.GetByNameAsync(testSettingName);
                if (existingSetting != null)
                {
                    LogInfo($"   设置 {testSettingName} 已存在，先删除旧数据");
                    await settingRepo.DeleteAsync(existingSetting);
                }

                // 2. 添加设置
                var newSetting = new GlobalSettingModel
                {
                    Name = testSettingName,
                    Value = "100",
                    Type = "INT",
                    Unit = "ms",
                    Remark = "测试设置"
                };

                LogInfo("\n2. 添加设置...");
                var addResult = await settingRepo.AddAsync(newSetting);
                if (!addResult)
                {
                    LogError("   添加设置失败");
                    return;
                }
                LogInfo($"   添加结果: 成功");

                // 3. 获取设置值（自动转换类型）
                LogInfo("\n3. 获取设置值...");
                var value = await settingRepo.GetSettingValueAsync(testSettingName);
                if (value == null)
                {
                    LogError("   获取设置值失败");
                    return;
                }
                LogInfo($"   值: {value}, 类型: {value.GetType().Name}");

                // 4. 更新前验证设置存在
                LogInfo("\n4. 更新设置值...");
                var settingToUpdate = await settingRepo.GetByNameAsync(testSettingName);
                if (settingToUpdate == null)
                {
                    LogError("   要更新的设置不存在");
                    return;
                }

                var updateResult = await settingRepo.UpdateSettingValueAsync(testSettingName, 200);
                if (!updateResult)
                {
                    LogError("   更新设置失败");
                    return;
                }
                LogInfo($"   更新结果: 成功");

                // 5. 验证更新
                LogInfo("\n5. 验证更新...");
                var newValue = await settingRepo.GetSettingValueAsync(testSettingName);
                if (newValue == null)
                {
                    LogError("   获取更新后的值失败");
                    return;
                }
                LogInfo($"   新值: {newValue}");

                // 验证值是否正确更新
                if (!newValue.Equals(200))
                {
                    LogError($"   更新验证失败：期望值200，实际值{newValue}");
                    return;
                }

                // 6. 删除设置
                LogInfo("\n6. 删除设置...");
                var settingToDelete = await settingRepo.GetByNameAsync(testSettingName);
                if (settingToDelete == null)
                {
                    LogError("   要删除的设置不存在");
                    return;
                }

                var deleteResult = await settingRepo.DeleteAsync(settingToDelete);
                if (!deleteResult)
                {
                    LogError("   删除设置失败");
                    return;
                }
                LogInfo($"   删除结果: 成功");

                // 7. 验证删除
                LogInfo("\n7. 验证删除...");
                var deletedSetting = await settingRepo.GetByNameAsync(testSettingName);
                if (deletedSetting != null)
                {
                    LogError("   删除验证失败：设置仍然存在");
                }
                else
                {
                    LogInfo("   删除验证成功：设置已被删除");
                }
            }
            catch (Exception ex)
            {
                LogError($"全局设置CRUD测试失败: {ex.Message}");
                throw;
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

                // 测试场景1：成功的事务
                LogInfo("\n场景1：测试成功的事务...");
                await TestSuccessfulTransaction(unitOfWork, plcConfigRepo, plcRWConfigRepo);

                // 测试场景2：失败的事务（应该回滚）
                LogInfo("\n场景2：测试失败的事务（应该回滚）...");
                await TestFailedTransaction(unitOfWork, plcConfigRepo, plcRWConfigRepo);
            }
            catch (Exception ex)
            {
                LogError($"事务测试失败: {ex.Message}");
                throw;
            }
        }

        private async Task TestSuccessfulTransaction(
            IUnitOfWork unitOfWork,
            IPLCConfigRepository plcConfigRepo,
            IPLCRWConfigRepository plcRWConfigRepo)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync();

                // 先清理可能存在的测试数据
                var existingPlc = (await plcConfigRepo.GetAllAsync())
                    .FirstOrDefault(p => p.Name == "测试PLC_成功");
                if (existingPlc != null)
                {
                    await plcConfigRepo.DeleteByIdAsync(existingPlc.ID);
                }

                // 添加PLC配置
                var plcConfig = new PLCConfigModel
                {
                    Name = "测试PLC_成功",
                    IP = "192.168.1.200",
                    Port = 502,
                    BrandSpecificationProtocal = "MODBUS_TCP",
                    CycleReadTime = 1000,
                    CycleWriteTime = 1000,
                    IsEnable = true,
                    Remark = "事务成功测试"
                };

                var addPlcResult = await plcConfigRepo.AddAsync(plcConfig);
                if (!addPlcResult)
                {
                    throw new Exception("添加PLC配置失败");
                }
                LogInfo("   PLC配置已添加");

                // 获取刚添加的PLC的ID
                var addedPlc = (await plcConfigRepo.GetAllAsync())
                    .FirstOrDefault(p => p.Name == "测试PLC_成功");
                if (addedPlc == null)
                {
                    throw new Exception("未找到刚添加的PLC配置");
                }

                // 添加PLC读写配置
                var rwConfig = new PLCRWConfigModel
                {
                    Name = "测试读写区_成功",
                    PLCID = addedPlc.ID, // 使用正确的ID
                    AreaName = "DB1",
                    StartAddress = "0",
                    Length = 100,
                    RWMode = "Read",
                    Cycle = 1000,
                    AddressType = 1,
                    IsEnable = true
                };

                var addRwResult = await plcRWConfigRepo.AddAsync(rwConfig);
                if (!addRwResult)
                {
                    throw new Exception("添加读写配置失败");
                }
                LogInfo("   读写配置已添加");

                await unitOfWork.CommitAsync();
                LogInfo("   事务提交成功");

                // 验证数据是否真的被保存
                var savedPlc = (await plcConfigRepo.GetAllAsync())
                    .FirstOrDefault(p => p.Name == "测试PLC_成功");
                var savedRw = (await plcRWConfigRepo.GetAllAsync())
                    .FirstOrDefault(r => r.Name == "测试读写区_成功");

                if (savedPlc != null && savedRw != null)
                {
                    LogInfo("   验证成功：数据已保存到数据库");

                    // 清理测试数据
                    await plcRWConfigRepo.DeleteByIdAsync(savedRw.ID);
                    await plcConfigRepo.DeleteByIdAsync(savedPlc.ID);
                }
                else
                {
                    LogError("   验证失败：数据未保存到数据库");
                }
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                LogError($"   成功事务测试失败并回滚: {ex.Message}");
                throw;
            }
        }

        private async Task TestFailedTransaction(
            IUnitOfWork unitOfWork,
            IPLCConfigRepository plcConfigRepo,
            IPLCRWConfigRepository plcRWConfigRepo)
        {
            // 将变量声明移到try-catch外面
            PLCConfigModel? plcConfig = null;
            string testPlcName = "测试PLC_失败_" + DateTime.Now.Ticks;
            string testRwName = "测试读写区_失败_" + DateTime.Now.Ticks;

            try
            {
                LogInfo("开始测试事务回滚功能...");

                // 记录测试前的数据数量
                var beforePlcCount = (await plcConfigRepo.GetAllAsync()).Count();
                var beforeRwCount = (await plcRWConfigRepo.GetAllAsync()).Count();
                LogInfo($"测试前数据量 - PLC配置: {beforePlcCount}, 读写配置: {beforeRwCount}");

                await unitOfWork.BeginTransactionAsync();
                LogInfo("事务已开始");

                // 1. 添加PLC配置
                plcConfig = new PLCConfigModel
                {
                    Name = testPlcName,
                    IP = "192.168.1.201",
                    Port = 502,
                    BrandSpecificationProtocal = "MODBUS_TCP",
                    CycleReadTime = 1000,
                    CycleWriteTime = 1000,
                    IsEnable = true,
                    Remark = "事务失败测试"
                };

                await plcConfigRepo.AddAsync(plcConfig);
                LogInfo($"PLC配置已添加，ID: {plcConfig.ID}");

                // 2. 故意触发异常的几种方式，选择一种：

                // 方式1：使用不存在的外键（如果数据库有外键约束）
                var rwConfig = new PLCRWConfigModel
                {
                    Name = testRwName,
                    PLCID = 99999, // 不存在的ID
                    AreaName = "DB1",
                    StartAddress = "0",
                    Length = 100,
                    RWMode = "Read",
                    Cycle = 1000,
                    AddressType = 1,
                    IsEnable = true
                };
                await plcRWConfigRepo.AddAsync(rwConfig);

                // 方式2：如果方式1不行，直接抛出异常来模拟业务错误
                // throw new InvalidOperationException("模拟业务异常");

                // 方式3：强制触发异常以测试回滚（如果上面的方式都不触发异常）
                // throw new Exception("强制触发异常以测试回滚");

                await unitOfWork.CommitAsync();
                LogError("错误：事务不应该提交成功！");

                // 如果到了这里，说明没有触发异常，我们手动触发一个
                throw new Exception("强制触发异常以测试回滚");
            }
            catch (Exception ex)
            {
                LogInfo($"捕获到异常（预期行为）: {ex.Message}");

                try
                {
                    await unitOfWork.RollbackAsync();
                    LogInfo("事务已回滚");
                }
                catch (Exception rollbackEx)
                {
                    LogError($"回滚失败: {rollbackEx.Message}");
                }

                // 等待一下，确保回滚完成
                await Task.Delay(100);

                // 验证回滚效果 - 现在可以访问这些变量了
                await VerifyRollback(plcConfigRepo, plcRWConfigRepo, testPlcName, testRwName, plcConfig?.ID);
            }
        }

        /// <summary>
        /// 运行所有测试
        /// </summary>
        public async Task RunAllTests()
        {
            try
            {
                LogInfo("=== 开始数据层测试 ===\n");

                LogInfo("--- 测试设备CRUD ---");
                await TestDeviceCRUD();

                LogInfo("\n--- 测试全局设置CRUD ---");
                await TestGlobalSettingCRUD();

                LogInfo("\n--- 测试事务 ---");
                await TestTransaction();

                LogInfo("\n=== 所有测试完成 ===");
            }
            catch (Exception ex)
            {
                LogError($"\n测试过程中发生错误: {ex.Message}");
                LogError($"堆栈跟踪: {ex.StackTrace}");
            }
        }
        // 测试验证
        private async Task VerifyRollback(IPLCConfigRepository plcConfigRepo,IPLCRWConfigRepository plcRWConfigRepo,string testPlcName,string testRwName,long? plcId)
        {
            try
            {
                LogInfo("开始验证回滚效果...");

                // 检查PLC配置是否被回滚
                var savedPlc = (await plcConfigRepo.GetAllAsync())
                    .FirstOrDefault(p => p.Name == testPlcName);

                if (savedPlc == null)
                {
                    LogInfo("✓ 验证成功：PLC配置已正确回滚");
                }
                else
                {
                    LogError("✗ 验证失败：PLC配置未正确回滚");
                    LogError($"  发现未回滚的PLC配置 - ID: {savedPlc.ID}, Name: {savedPlc.Name}");
                    // 清理数据
                    try
                    {
                        await plcConfigRepo.DeleteByIdAsync(savedPlc.ID);
                        LogInfo("已清理未回滚的PLC测试数据");
                    }
                    catch (Exception cleanEx)
                    {
                        LogError($"清理PLC数据失败: {cleanEx.Message}");
                    }
                }

                // 检查读写配置是否被回滚
                var savedRw = (await plcRWConfigRepo.GetAllAsync())
                    .FirstOrDefault(r => r.Name == testRwName);

                if (savedRw == null)
                {
                    LogInfo("✓ 验证成功：读写配置已正确回滚");
                }
                else
                {
                    LogError("✗ 验证失败：读写配置未正确回滚");
                    LogError($"  发现未回滚的读写配置 - ID: {savedRw.ID}, Name: {savedRw.Name}");
                    try
                    {
                        await plcRWConfigRepo.DeleteByIdAsync(savedRw.ID);
                        LogInfo("已清理未回滚的读写测试数据");
                    }
                    catch (Exception cleanEx)
                    {
                        LogError($"清理读写数据失败: {cleanEx.Message}");
                    }
                }

                // 如果有plcId，也可以通过ID来验证
                if (plcId.HasValue)
                {
                    try
                    {
                        var plcById = await plcConfigRepo.GetByIdAsync(plcId.Value);
                        if (plcById == null)
                        {
                            LogInfo($"✓ 通过ID验证成功：ID为{plcId}的PLC配置已正确回滚");
                        }
                        else
                        {
                            LogError($"✗ 通过ID验证失败：ID为{plcId}的PLC配置未正确回滚");
                        }
                    }
                    catch (Exception ex)
                    {
                        // 如果通过ID查找抛出异常，可能说明数据确实被删除了
                        LogInfo($"通过ID查找时出现异常（可能表示数据已回滚）: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"验证回滚时出错: {ex.Message}");
            }
        }
    }
}