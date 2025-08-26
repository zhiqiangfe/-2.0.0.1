

namespace SUNWODA_SEVB.WEB.Services
{
    /// <summary>
    /// WEB服务状态管理
    /// </summary>
    public class WebStateService
    {
        private readonly object _lockObject = new object();
        private bool _isConnected;
        private bool _isDeviceBound;
        private bool _isRunning;

        public bool IsConnected
        {
            get
            {
                lock (_lockObject)
                {
                    return _isConnected;
                }
            }
        }

        public bool IsDeviceBound
        {
            get
            {
                lock (_lockObject)
                {
                    return _isDeviceBound;
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                lock (_lockObject)
                {
                    return _isRunning;
                }
            }
        }

        public void UpdateConnectionStatus(bool isConnected)
        {
            lock (_lockObject)
            {
                _isConnected = isConnected;
            }
        }

        public void UpdateDeviceBindingStatus(bool isBound)
        {
            lock (_lockObject)
            {
                _isDeviceBound = isBound;
            }
        }

        public void UpdateRunningStatus(bool isRunning)
        {
            lock (_lockObject)
            {
                _isRunning = isRunning;
            }
        }
    }
}
