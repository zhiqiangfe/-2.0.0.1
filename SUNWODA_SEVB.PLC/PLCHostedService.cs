using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SUNWODA_SEVB.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.PLC
{
    /// <summary>
    /// PLC后台服务
    /// </summary>
    public class PLCHostedService : BackgroundService
    {
        private readonly PLCService _plcService;
        private readonly ILoggerService<PLCHostedService> _logger;

        public PLCHostedService(
            PLCService plcService,
            ILoggerService<PLCHostedService> logger)
        {
            _plcService = plcService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.Info("启动PLC后台服务");
                await _plcService.InitializeAsync(stoppingToken);

                // 保持服务运行
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.Info("PLC后台服务停止");
            }
            catch (Exception ex)
            {
                _logger.Error("PLC后台服务异常", ex, true);
                throw;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Info("正在停止PLC后台服务");

            if (_plcService is IAsyncDisposable asyncDisposable)
            {
                _logger.Info("异步停止PLC后台服务");
                await asyncDisposable.DisposeAsync();
            }
            else if (_plcService is IDisposable disposable)
            {
                _logger.Info("同步停止PLC后台服务");
                disposable.Dispose();
            }

            await base.StopAsync(cancellationToken);
        }
    }
}
