using Mapster;
using SUNWODA_SEVB.Core.Entities;
using SUNWODA_SEVB.Core.Models;
using SUNWODA_SEVB.Data.Models;

namespace SUNWODA_SEVB.Data.Mappings
{
    /// <summary>
    /// 统一的Mapster映射配置
    /// </summary>
    public static class MapsterConfig
    {
        private static bool _isConfigured = false;

        public static void Configure()
        {
            if (_isConfigured) return;

            // AppLog映射配置
            TypeAdapterConfig<AppLogModel, AppLog>
                .NewConfig()
                .Map(dest => dest.ID, src => src.ID)
                .Map(dest => dest.LogTime, src => src.LogTime)
                .Map(dest => dest.LogLevel, src => src.LogLevel)
                .Map(dest => dest.Logger, src => src.Logger)
                .Map(dest => dest.Message, src => src.Message)
                .Map(dest => dest.Exception, src => src.Exception);

            TypeAdapterConfig<AppLog, AppLogModel>
                .NewConfig()
                .Map(dest => dest.ID, src => src.ID)
                .Map(dest => dest.LogTime, src => src.LogTime)
                .Map(dest => dest.LogLevel, src => src.LogLevel)
                .Map(dest => dest.Logger, src => src.Logger)
                .Map(dest => dest.Message, src => src.Message)
                .Map(dest => dest.Exception, src => src.Exception);

            // Device映射配置
            TypeAdapterConfig<DeviceModel, DeviceModel>
                .NewConfig()
                .PreserveReference(true);

            TypeAdapterConfig<DeviceModel, DeviceModel>
                .NewConfig()
                .PreserveReference(true);

            // GlobalSetting映射配置
            TypeAdapterConfig<GlobalSettingModel, GlobalSettingModel>
                .NewConfig()
                .PreserveReference(true);

            TypeAdapterConfig<GlobalSettingModel, GlobalSettingModel>
                .NewConfig()
                .PreserveReference(true);

            // MesInterfaceLog映射配置
            TypeAdapterConfig<MesInterfaceLogModel, MesInterfaceLogModel>
                .NewConfig()
                .PreserveReference(true);

            TypeAdapterConfig<MesInterfaceLogModel, MesInterfaceLogModel>
                .NewConfig()
                .PreserveReference(true);

            // PLCAddressConfig映射配置
            TypeAdapterConfig<PLCAddressConfigModel, PLCAddressConfigModel>
                .NewConfig()
                .PreserveReference(true);

            TypeAdapterConfig<PLCAddressConfigModel, PLCAddressConfigModel>
                .NewConfig()
                .PreserveReference(true);

            // PLCConfig映射配置
            TypeAdapterConfig<PLCConfigModel, PLCConfigModel>
                .NewConfig()
                .PreserveReference(true);

            TypeAdapterConfig<PLCConfigModel, PLCConfigModel>
                .NewConfig()
                .PreserveReference(true);

            // PLCRWConfig映射配置
            TypeAdapterConfig<PLCRWConfigModel, PLCRWConfigModel>
                .NewConfig()
                .PreserveReference(true);

            TypeAdapterConfig<PLCRWConfigModel, PLCRWConfigModel>
                .NewConfig()
                .PreserveReference(true);

            // ProjectSetting映射配置
            TypeAdapterConfig<ProjectSettingModel, ProjectSettingModel>
                .NewConfig()
                .PreserveReference(true);

            TypeAdapterConfig<ProjectSettingModel, ProjectSettingModel>
                .NewConfig()
                .PreserveReference(true);

            // User映射配置
            TypeAdapterConfig<UsersModel, UsersModel>
                .NewConfig()
                .PreserveReference(true);

            TypeAdapterConfig<UsersModel, UsersModel>
                .NewConfig()
                .PreserveReference(true);

            // WebInterfaceLog映射配置
            TypeAdapterConfig<WebInterfaceLogModel, WebInterfaceLogModel>
                .NewConfig()
                .PreserveReference(true);

            TypeAdapterConfig<WebInterfaceLogModel, WebInterfaceLogModel>
                .NewConfig()
                .PreserveReference(true);

            // WorkSpaceProject映射配置
            TypeAdapterConfig<WorkSpaceProjectModel, WorkSpaceProjectModel>
                .NewConfig()
                .PreserveReference(true);

            TypeAdapterConfig<WorkSpaceProjectModel, WorkSpaceProjectModel>
                .NewConfig()
                .PreserveReference(true);

            _isConfigured = true;
        }
    }
}
