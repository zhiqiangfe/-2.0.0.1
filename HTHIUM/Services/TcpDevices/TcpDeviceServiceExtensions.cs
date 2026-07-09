using Microsoft.Extensions.DependencyInjection;
using HTHIUM.Core.Interfaces.TcpDevices;

namespace HTHIUM.Services.TcpDevices
{
    /// <summary>
    /// TCP 设备通讯模块的依赖注入注册入口。
    /// App.xaml.cs 调用 AddTcpDeviceServices 后，业务类即可注入 ITcpDeviceService 使用。
    /// </summary>
    public static class TcpDeviceServiceExtensions
    {
        /// <summary>
        /// 注册 TCP 设备服务、接口映射和后台托管服务。
        /// </summary>
        public static IServiceCollection AddTcpDeviceServices(this IServiceCollection services)
        {
            // TcpDeviceService 是整个 TCP 模块的总管理器，保存设备缓存和客户端实例。
            services.AddSingleton<TcpDeviceService>();

            // 对外暴露接口，业务层只依赖 ITcpDeviceService，降低耦合。
            services.AddSingleton<ITcpDeviceService>(provider => provider.GetRequiredService<TcpDeviceService>());

            // 注册 HostedService，使程序启动后自动加载数据库配置并执行自动连接逻辑。
            services.AddHostedService<TcpDeviceHostedService>();

            return services;
        }
    }
}