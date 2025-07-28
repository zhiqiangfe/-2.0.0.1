using Microsoft.Extensions.DependencyInjection;
using SUNWODA_SEVB.Data.DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Data
{
    /// <summary>
    /// 把 Data 层需要的服务一次性注册
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            // MySQL 助手
            services.AddSingleton<IDbHelperMySQL, DbHelperMySQL>();

            // 注册 DAL 和 BLL 层服务         
            services.AddScoped<DAL.plc_rw_config>();
            services.AddScoped<BLL.plc_rw_config>(); 
            
            services.AddScoped<DAL.plc_config>();
            services.AddScoped<BLL.plc_config>();

            services.AddScoped<DAL.log_http_interface>();
            services.AddScoped<BLL.log_http_interface>();

            services.AddScoped<DAL.log_mes_interface>();
            services.AddScoped<BLL.log_mes_interface>();

            services.AddScoped<DAL.log_web_interface>();
            services.AddScoped<BLL.log_web_interface>();

            services.AddScoped<DAL.plc_address_config>();
            services.AddScoped<BLL.plc_address_config>();



            return services;
        }
    }
}
