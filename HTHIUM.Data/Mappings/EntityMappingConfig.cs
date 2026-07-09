using Mapster;
using HTHIUM.Core.Entities;
using HTHIUM.Core.Models;
using HTHIUM.Data.Models;


namespace HTHIUM.Data.Mappings
{
    /// <summary>
    /// 实体与数据模型之间的映射配置
    /// </summary>
    public static class EntityMappingConfig
    {
        public static void Configure()
        {
            // AppLog <-> AppLogModel
            TypeAdapterConfig<AppLogModel, AppLogParameters>.NewConfig();
            TypeAdapterConfig<AppLogParameters, AppLogModel>.NewConfig();

            // Device <-> DeviceModel
            TypeAdapterConfig<Device, DeviceModel>.NewConfig();
            TypeAdapterConfig<DeviceModel, Device>.NewConfig();

            // GlobalSetting <-> GlobalSettingModel
            TypeAdapterConfig<GlobalSetting, GlobalSettingModel>.NewConfig();
            TypeAdapterConfig<GlobalSettingModel, GlobalSetting>.NewConfig();

            // MesInterfaceLog <-> MesInterfaceLogsModel
            TypeAdapterConfig<MesInterfaceLog, MesInterfaceLogModel>.NewConfig();
            TypeAdapterConfig<MesInterfaceLogModel, MesInterfaceLog>.NewConfig();

            //PLCAddressConfig <-> PLCAddressConfigModel
            TypeAdapterConfig<PLCAddressConfig,PLCAddressConfigModel>.NewConfig();
            TypeAdapterConfig<PLCAddressConfigModel, PLCAddressConfig>.NewConfig();

            // PLCConfig <-> PLCConfigModel
            TypeAdapterConfig<PLCConfig, PLCConfigModel>.NewConfig();
            TypeAdapterConfig<PLCConfigModel, PLCConfig>.NewConfig();

            // PLCRWConfig <-> PLCRWConfigModel
            TypeAdapterConfig<PLCRWConfig, PLCRWConfigModel>.NewConfig();
            TypeAdapterConfig<PLCRWConfigModel, PLCRWConfig>.NewConfig();

            // ProjectSetting <-> ProjectSettingModel
            TypeAdapterConfig<ProjectSetting, ProjectSettingModel>.NewConfig();
            TypeAdapterConfig<ProjectSettingModel, ProjectSetting>.NewConfig();

            // User <-> UserModel
            TypeAdapterConfig<Users, UsersModel>.NewConfig();
            TypeAdapterConfig<UsersModel, Users>.NewConfig();

            // WeblnterfaceLog <-> WeblnterfaceLogModel
            TypeAdapterConfig<WebInterfaceLog, WebInterfaceLogModel>.NewConfig();
            TypeAdapterConfig<WebInterfaceLogModel, WebInterfaceLog>.NewConfig();

            // WorkSpaceProject <-> WorkSpaceProjectModel
            TypeAdapterConfig<WorkSpaceProject, WorkSpaceProjectModel>.NewConfig();
            TypeAdapterConfig<WorkSpaceProjectModel, WorkSpaceProject>.NewConfig();


            // 其他实体映射...
        }
    }
}
