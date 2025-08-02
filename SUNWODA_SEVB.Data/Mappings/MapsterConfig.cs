using Mapster;
using SUNWODA_SEVB.Core.Models;
using SUNWODA_SEVB.Data.Models;

namespace SUNWODA_SEVB.Data.Mappings
{
    /// <summary>
    /// Mapster映射配置
    /// </summary>
    public static class MapsterConfig
    {
        public static void Configure()
        {
            // AppLogs 映射配置
            TypeAdapterConfig<AppLogParameters, AppLogModel>
                .NewConfig()
                .Map(dest => dest.ID, src => src.ID);

            TypeAdapterConfig<AppLogModel, AppLogParameters>
                .NewConfig()
                .Map(dest => dest.ID, src => src.ID)
                .Ignore(dest => dest.LogTime); // 创建时自动设置

         /*   // VariableParameters 映射配置
            TypeAdapterConfig<VariableParameters, VariableParameterModel>
                .NewConfig();

            TypeAdapterConfig<VariableParameterModel, VariableParameters>
                .NewConfig()
                .Ignore(dest => dest.CreateTime)
                .Ignore(dest => dest.UpdateTime);

            // PlcParameters 映射配置
            TypeAdapterConfig<PlcParameters, PlcParameterModel>
                .NewConfig();

            TypeAdapterConfig<PlcParameterModel, PlcParameters>
                .NewConfig()
                .Ignore(dest => dest.CreateTime)
                .Ignore(dest => dest.UpdateTime);*/
        }
    }
}
