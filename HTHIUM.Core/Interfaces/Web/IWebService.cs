
namespace HTHIUM.Core.Interfaces.Web
{
    /// <summary>
    /// WEB服务主接口
    /// </summary>
    public interface IWebService
    {
        /// <summary>
        /// 启动WEB服务
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// 停止WEB服务
        /// </summary>
        Task StopAsync();

        /// <summary>
        /// 获取WEB连接状态
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 获取设备是否已绑定
        /// </summary>
        bool IsDeviceBound { get; }

        /// <summary>
        /// 获取服务是否正在运行
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 手动触发版本检查
        /// </summary>
        Task<bool> CheckVersionAsync();

        /// <summary>
        /// 手动触发PC信息上传
        /// </summary>
        Task<bool> UploadPcInfoAsync();

        /// <summary>
        /// 手动触发设备绑定
        /// </summary>
        Task<bool> BindDeviceAsync();
    }
}
