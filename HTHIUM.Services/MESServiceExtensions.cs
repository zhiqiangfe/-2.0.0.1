using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using HTHIUM.Core.Interfaces;
using HTHIUM.Core.Interfaces.MES;
using HTHIUM.MES.Client;
using HTHIUM.MES.Services;
using HTHIUM.Core.Models.MES;
using HTHIUM.MES.Providers;
using System.Net.Http;



namespace HTHIUM.MES.Extensions
{
    /// <summary>
    /// MES服务依赖注入扩展
    /// </summary>
    public static class MESServiceExtensions
    {
        /// <summary>
        /// 添加MES服务
        /// </summary>
        public static IServiceCollection AddMesServices(this IServiceCollection services)
        {
            // 1. 注册MES配置提供者（单例，负责从数据库加载配置）
            services.AddSingleton<IMesConfigurationProvider, MesConfigurationProvider>();

            // 2. 注册MES配置（作用域，每次请求时从提供者获取）
            services.AddScoped(provider =>
            {
                var configProvider = provider.GetRequiredService<IMesConfigurationProvider>();
                var configTask = configProvider.GetConfigurationAsync();
                return configTask.ConfigureAwait(false).GetAwaiter().GetResult() ?? new MesApiConfiguration();
            });

            // 3. 注册HTTP客户端管理器（单例）
            services.AddSingleton<IMesHttpClientManager, MesHttpClientManager>();

            // 4. 注册HttpClient和MES API客户端
            services.AddHttpClient<IMesApiClient, MesApiClient>((provider, client) =>
            {
                var config = provider.GetRequiredService<MesApiConfiguration>();

                client.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);

                // 设置headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.ParseAdd("*/*");
                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Apifox/1.0.0 (https://apifox.com)");

                // 这个很重要，保持与RestSharp一致
                client.DefaultRequestHeaders.ExpectContinue = false;
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    UseProxy = false,
                    AllowAutoRedirect = false,
                    // 添加这个设置，确保使用HTTP/1.1
                    MaxConnectionsPerServer = 10
                };
            });

            // 5. 注册MES主服务（单例）
            services.AddSingleton<IMesService, MesService>();

            // 6. 注册具体的业务服务（作用域）
            services.AddScoped<IOfflineDataUploadService, OfflineDataUploadService>();
            services.AddScoped<IMarkingDataUploadService, MarkingDataUploadService>();

            // 7. 注册MES服务工厂
            services.AddScoped<Func<string, IMesService>>(provider =>
            {
                return serviceName =>
                {
                    return serviceName.ToLower() switch
                    {
                        "offlinedataupload" => provider.GetRequiredService<IOfflineDataUploadService>(),
                        "increasemarking" => provider.GetRequiredService<IMarkingDataUploadService>(),
                        _ => provider.GetRequiredService<IMesService>()
                    };
                };
            });

            return services;
        }
    }


}
