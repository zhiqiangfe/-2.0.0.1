using Microsoft.Extensions.DependencyInjection;
using System;
using NLog;

namespace SUNWODA_SEVB.Logging
{
    /// <summary>
    /// 日志服务的依赖注入配置扩展
    /// </summary>
    public static class LoggingServiceExtensions
    {
        /// <summary>
        /// 添加NLog日志服务到依赖注入容器
        /// </summary>
        public static IServiceCollection AddNLogServices(this IServiceCollection services)
        {
            // 注册日志管理服务
            services.AddSingleton<ILogManagementService, NLogManagementService>();

            // 注册泛型日志服务
            services.AddTransient(typeof(ILoggerService<>), typeof(NLogLogger<>));

            return services;
        }
    }
}
