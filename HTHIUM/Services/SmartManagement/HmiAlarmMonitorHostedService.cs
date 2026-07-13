using HTHIUM.Core.Interfaces;
using HTHIUM.Core.Interfaces.Data;
using HTHIUM.Core.Models.PLC;
using HTHIUM.Data.Models;
using HTHIUM.PLC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlSugar;

namespace HTHIUM.Services.SmartManagement
{
    /// <summary>
    /// HMI报警后台采集服务。
    /// </summary>
    public class HmiAlarmMonitorHostedService : BackgroundService
    {
        private const string EnabledSettingName = "IsHmiAlarmMonitorEnabled";
        private const string CycleSettingName = "HmiAlarmMonitorCycleMs";
        private const string ActiveParameterSettingName = "HmiAlarmActiveParameterName";
        private const string CodeParameterSettingName = "HmiAlarmCodeParameterName";
        private const string LineNameSettingName = "LineName";

        private readonly PLCService _plcService;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILoggerService<HmiAlarmMonitorHostedService> _logger;

        private string? _lastActiveCode;
        private long? _activeRecordId;
        private bool _lastActiveState;

        public HmiAlarmMonitorHostedService(
            PLCService plcService,
            IServiceScopeFactory scopeFactory,
            ILoggerService<HmiAlarmMonitorHostedService> logger)
        {
            _plcService = plcService;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Info("HMI报警后台采集服务启动");

            while (!stoppingToken.IsCancellationRequested)
            {
                var cycleMs = 1000;
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var globalSettingRepo = scope.ServiceProvider.GetRequiredService<IGlobalSettingRepository>();

                    var enabled = await globalSettingRepo.GetSettingValueAsync<bool>(EnabledSettingName, false);
                    cycleMs = Math.Max(200, await globalSettingRepo.GetSettingValueAsync<int>(CycleSettingName, 1000));

                    if (!enabled)
                    {
                        await Task.Delay(cycleMs, stoppingToken);
                        continue;
                    }

                    if (!_plcService.IsInitialized)
                    {
                        await Task.Delay(cycleMs, stoppingToken);
                        continue;
                    }

                    var activeParameterName = await globalSettingRepo.GetSettingValueAsync<string>(
                        ActiveParameterSettingName,
                        "报警触发地址");
                    var codeParameterName = await globalSettingRepo.GetSettingValueAsync<string>(
                        CodeParameterSettingName,
                        "报警代码");

                    var lineName = await globalSettingRepo.GetSettingValueAsync<string>(
                        LineNameSettingName,
                        "-");

                    var activeAddress = FindAddress(activeParameterName);
                    var codeAddress = FindAddress(codeParameterName);

                    if (activeAddress == null || codeAddress == null)
                    {
                        _logger.Debug($"未找到报警采集点位: {activeParameterName}/{codeParameterName}");
                        await Task.Delay(cycleMs, stoppingToken);
                        continue;
                    }

                    var isActive = ToBool(activeAddress.MonitorValue);
                    var alarmCode = NormalizeAlarmCode(codeAddress.MonitorValue);

                    await HandleAlarmStateAsync(scope.ServiceProvider, isActive, alarmCode, codeAddress, lineName, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.Error("HMI报警后台采集异常", ex, true);
                }

                try
                {
                    await Task.Delay(cycleMs, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            _logger.Info("HMI报警后台采集服务停止");
        }

        private PLCRWAddress? FindAddress(string parameterName)
        {
            return _plcService.RWAddresses.Values.FirstOrDefault(it =>
                string.Equals(it.ParameterName, parameterName, StringComparison.OrdinalIgnoreCase));
        }

        private async Task HandleAlarmStateAsync(
            IServiceProvider serviceProvider,
            bool isActive,
            string alarmCode,
            PLCRWAddress codeAddress,
            string lineName,
            CancellationToken cancellationToken)
        {
            var db = serviceProvider.GetRequiredService<ISqlSugarClient>();

            if (isActive && !string.IsNullOrWhiteSpace(alarmCode) && alarmCode != "0")
            {
                if (_lastActiveState && !string.Equals(_lastActiveCode, alarmCode, StringComparison.OrdinalIgnoreCase))
                {
                    await RecoverActiveAlarmAsync(db, GetDeviceName(codeAddress, null), cancellationToken);
                    _activeRecordId = null;
                }

                if (_lastActiveState && string.Equals(_lastActiveCode, alarmCode, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                var alarmMap = await db.Queryable<HmiAlarmCodeMap>()
                    .Where(it => it.IsEnable && it.AlarmCode == alarmCode)
                    .FirstAsync(cancellationToken);
                var deviceName = GetDeviceName(codeAddress, alarmMap);

                var existing = await db.Queryable<HmiAlarmRecord>()
                    .Where(it =>
                        it.AlarmCode == alarmCode &&
                        it.AlarmStatus == "触发中" &&
                        it.DeviceName == deviceName)
                    .OrderBy(it => it.TriggerTime, OrderByType.Desc)
                    .FirstAsync(cancellationToken);

                if (existing != null)
                {
                    _activeRecordId = existing.ID;
                    _lastActiveCode = alarmCode;
                    _lastActiveState = true;
                    return;
                }

                var now = DateTime.Now;
                var record = new HmiAlarmRecord
                {
                    LineName = string.IsNullOrWhiteSpace(lineName) ? "-" : lineName,
                    DeviceName = deviceName,
                    StationName = alarmMap?.StationName,
                    ProcessName = alarmMap?.ProcessName,
                    AlarmCode = alarmCode,
                    AlarmName = alarmMap?.AlarmName ?? alarmCode,
                    AlarmLevel = alarmMap?.AlarmLevel,
                    TriggerTime = now,
                    AlarmStatus = "触发中",
                    Source = "PLC",
                    RawValue = codeAddress.ParameterName,
                    CreatedTime = now
                };

                _activeRecordId = await db.Insertable(record).ExecuteReturnBigIdentityAsync();
                _lastActiveCode = alarmCode;
                _lastActiveState = true;

                _logger.Info($"采集到PLC报警: Code={alarmCode}, Name={record.AlarmName}");
                return;
            }

            if (_lastActiveState || !isActive)
            {
                await RecoverActiveAlarmAsync(db, GetDeviceName(codeAddress, null), cancellationToken);
            }

            _lastActiveState = false;
            _lastActiveCode = null;
            _activeRecordId = null;
        }

        private async Task RecoverActiveAlarmAsync(ISqlSugarClient db, string deviceName, CancellationToken cancellationToken)
        {
            HmiAlarmRecord? record = null;
            if (_activeRecordId.HasValue)
            {
                record = await db.Queryable<HmiAlarmRecord>()
                    .Where(it => it.ID == _activeRecordId.Value)
                    .FirstAsync(cancellationToken);
            }

            if (record == null && !string.IsNullOrWhiteSpace(_lastActiveCode))
            {
                record = await db.Queryable<HmiAlarmRecord>()
                    .Where(it =>
                        it.AlarmCode == _lastActiveCode &&
                        it.AlarmStatus == "触发中" &&
                        it.DeviceName == deviceName)
                    .OrderBy(it => it.TriggerTime, OrderByType.Desc)
                    .FirstAsync(cancellationToken);
            }

            if (record == null)
            {
                var activeRecords = await db.Queryable<HmiAlarmRecord>()
                    .Where(it => it.AlarmStatus == "触发中" && it.DeviceName == deviceName)
                    .ToListAsync(cancellationToken);

                foreach (var activeRecord in activeRecords)
                {
                    await RecoverRecordAsync(db, activeRecord, cancellationToken);
                }

                return;
            }

            await RecoverRecordAsync(db, record, cancellationToken);
        }

        private async Task RecoverRecordAsync(ISqlSugarClient db, HmiAlarmRecord record, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            record.RecoverTime = now;
            record.DurationSeconds = Math.Max(0, (int)(now - record.TriggerTime).TotalSeconds);
            record.AlarmStatus = "已恢复";
            record.UpdatedTime = now;
            await db.Updateable(record).ExecuteCommandAsync(cancellationToken);
            _logger.Info($"PLC报警恢复: Code={record.AlarmCode}, Duration={record.DurationSeconds}s");
        }

        private static string GetDeviceName(PLCRWAddress address, HmiAlarmCodeMap? alarmMap)
        {
            return string.IsNullOrWhiteSpace(alarmMap?.DeviceName) ? $"PLC-{address.PlcId}" : alarmMap.DeviceName;
        }

        private static string NormalizeAlarmCode(object? value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (value is string text)
            {
                return text.Trim();
            }

            try
            {
                return Convert.ToInt64(value).ToString();
            }
            catch
            {
                return value.ToString()?.Trim() ?? string.Empty;
            }
        }

        private static bool ToBool(object? value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is bool boolValue)
            {
                return boolValue;
            }

            if (value is string text)
            {
                if (bool.TryParse(text, out var parsedBool))
                {
                    return parsedBool;
                }

                return decimal.TryParse(text, out var parsedNumber) && parsedNumber != 0;
            }

            try
            {
                return Convert.ToDecimal(value) != 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
