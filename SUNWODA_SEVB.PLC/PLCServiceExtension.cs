using Microsoft.Extensions.DependencyInjection;
using SUNWODA_SEVB.Core.Interfaces;

namespace SUNWODA_SEVB.PLC
{
    public static class PLCServiceExtension
    {
        // <summary>
        /// 添加PLC服务
        /// </summary>
        public static IServiceCollection AddPLCService(
            this IServiceCollection services)
        {
            // 注册服务
            services.AddSingleton<PLCService>();
            services.AddSingleton<IPLCService>(provider => provider.GetRequiredService<PLCService>());

            // 注册后台服务
            services.AddHostedService<PLCHostedService>();

            return services;
        }
    }
}
