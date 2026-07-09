using Microsoft.Extensions.Hosting;
using HTHIUM.Core.Interfaces;

namespace HTHIUM.Services.TcpDevices
{
    /// <summary>
    /// TCP 设备后台托管服务。
    /// 该类由 Host 自动启动，职责是把 TcpDeviceService 初始化起来。
    /// </summary>
    public class TcpDeviceHostedService : BackgroundService
    {
        private readonly TcpDeviceService _deviceService;
        private readonly ILoggerService<TcpDeviceHostedService> _logger;

        /// <summary>
        /// 注入 TCP 设备管理服务和日志服务。
        /// </summary>
        public TcpDeviceHostedService(TcpDeviceService deviceService, ILoggerService<TcpDeviceHostedService> logger)
        {
            _deviceService = deviceService;
            _logger = logger;
        }

        /// <summary>
        /// Host 启动后自动执行。
        /// 数据库初始化已经在 App.xaml.cs 中提前完成，所以这里可以直接加载 TCP 配置。
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.Info("Start TCP device hosted service");

                // 初始化时会加载 tcp_device_config，并启动自动连接/心跳维护循环。
                await _deviceService.InitializeAsync(stoppingToken);

                // 保持后台服务存活，直到程序退出或 Host 停止。
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.Info("TCP device hosted service stopped");
            }
            catch (Exception ex)
            {
                _logger.Error("TCP device hosted service error", ex, true);
                throw;
            }
        }
    }
}