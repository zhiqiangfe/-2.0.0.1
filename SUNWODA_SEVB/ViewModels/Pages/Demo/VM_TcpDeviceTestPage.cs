using SUNWODA_SEVB.Core.Attributes;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Core.Common.Commands;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Interfaces.TcpDevices;
using SUNWODA_SEVB.Core.Models.TcpDevices;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SUNWODA_SEVB.ViewModels.Pages.Demo
{
    [Module("TcpDeviceTestPage", "TCP 设备测试")]
    public class VM_TcpDeviceTestPage : ViewModelBase
    {
        private readonly ITcpDeviceService _deviceService;
        private readonly ILoggerService<VM_TcpDeviceTestPage> _logger;
        private TcpDeviceRuntimeModel? _selectedDevice;
        private string _sendText = "PING";
        private string _statusText = "就绪";

        public ObservableCollection<TcpDeviceRuntimeModel> Devices { get; } = new();
        public ObservableCollection<string> Messages { get; } = new();

        public TcpDeviceRuntimeModel? SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                if (SetProperty(ref _selectedDevice, value))
                {
                    OnSelectedDeviceChanged();
                }
            }
        }

        /// <summary>是否已选择设备。</summary>
        public bool IsDeviceSelected => SelectedDevice != null;

        /// <summary>是否未选择设备。</summary>
        public bool IsNoDeviceSelected => SelectedDevice == null;

        /// <summary>选中设备的 IP:端口 显示文本。</summary>
        public string SelectedDeviceEndpoint => SelectedDevice == null
            ? string.Empty
            : $"{SelectedDevice.Config.IP}:{SelectedDevice.Config.Port}";

        /// <summary>选中设备的编码/换行符显示文本。</summary>
        public string SelectedDeviceEncodingInfo => SelectedDevice == null
            ? string.Empty
            : $"{SelectedDevice.Config.EncodingName} / {SelectedDevice.Config.NewLine}";

        /// <summary>选中设备的心跳间隔显示文本。</summary>
        public string SelectedDeviceHeartbeatText => SelectedDevice == null
            ? string.Empty
            : $"{SelectedDevice.Config.HeartbeatIntervalMs} ms";

        /// <summary>选中设备是否自动连接的中文显示。</summary>
        public string SelectedDeviceAutoConnectText => SelectedDevice?.Config.IsAutoConnect == true ? "是" : "否";

        /// <summary>选中设备连接状态的中文显示。</summary>
        public string SelectedDeviceConnectedText => SelectedDevice?.IsConnected == true ? "已连接" : "未连接";

        public string SendText
        {
            get => _sendText;
            set => SetProperty(ref _sendText, value);
        }

        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        public ICommand ReloadCommand { get; }
        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }
        public ICommand SendCommand { get; }
        public ICommand ClearMessagesCommand { get; }

        public VM_TcpDeviceTestPage(ITcpDeviceService deviceService, ILoggerService<VM_TcpDeviceTestPage> logger)
        {
            _deviceService = deviceService;
            _logger = logger;

            ReloadCommand = new RelayCommand(async () => await ReloadAsync());
            ConnectCommand = new RelayCommand(async () => await ConnectAsync());
            DisconnectCommand = new RelayCommand(async () => await DisconnectAsync());
            SendCommand = new RelayCommand(async () => await SendAsync());
            ClearMessagesCommand = new RelayCommand(() => Messages.Clear());

            _deviceService.MessageReceived += DeviceService_MessageReceived;
            _deviceService.StatusChanged += DeviceService_StatusChanged;
        }

        protected override async Task OnNavigatedToAsync(object? parameter)
        {
            await base.OnNavigatedToAsync(parameter);
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            try
            {
                await _deviceService.ReloadAsync();
                await RunOnUIThreadAsync(() =>
                {
                    var selectedId = SelectedDevice?.ID;
                    Devices.Clear();
                    foreach (var device in _deviceService.Devices.Values.OrderBy(x => x.ID))
                    {
                        Devices.Add(device);
                    }

                    SelectedDevice = Devices.FirstOrDefault(x => x.ID == selectedId) ?? Devices.FirstOrDefault();
                    StatusText = $"已加载 {Devices.Count} 个 TCP 设备配置";
                });
            }
            catch (Exception ex)
            {
                _logger.Error("重新加载 TCP 设备配置失败", ex);
                StatusText = $"加载失败: {ex.Message}";
            }
        }

        private async Task ConnectAsync()
        {
            if (SelectedDevice == null)
            {
                StatusText = "请先选择要连接的设备";
                return;
            }

            StatusText = $"正在连接 {SelectedDevice.Name}...";
            var success = await _deviceService.ConnectAsync(SelectedDevice.ID);
            StatusText = success
                ? $"{SelectedDevice.Name} 连接成功"
                : $"{SelectedDevice.Name} 连接失败";
        }

        private async Task DisconnectAsync()
        {
            if (SelectedDevice == null)
            {
                StatusText = "请先选择要断开的设备";
                return;
            }

            await _deviceService.DisconnectAsync(SelectedDevice.ID);
            StatusText = $"{SelectedDevice.Name} 已断开";
        }

        private async Task SendAsync()
        {
            if (SelectedDevice == null)
            {
                StatusText = "请先选择要发送消息的设备";
                return;
            }

            if (string.IsNullOrWhiteSpace(SendText))
            {
                StatusText = "发送内容不能为空";
                return;
            }

            var success = await _deviceService.SendAsync(SelectedDevice.ID, SendText);
            StatusText = success
                ? $"向 {SelectedDevice.Name} 发送成功"
                : $"向 {SelectedDevice.Name} 发送失败";
        }

        private async void DeviceService_MessageReceived(object? sender, TcpDeviceMessageEventArgs e)
        {
            await RunOnUIThreadAsync(() =>
            {
                var directionText = e.Direction == "TX" ? "发送" : "接收";
                Messages.Insert(0, $"{DateTime.Now:HH:mm:ss.fff} [{e.DeviceName}] {directionText}: {e.Message}");
                while (Messages.Count > 300)
                {
                    Messages.RemoveAt(Messages.Count - 1);
                }
            });
        }

        private async void DeviceService_StatusChanged(object? sender, TcpDeviceRuntimeModel e)
        {
            await RunOnUIThreadAsync(() =>
            {
                var statusText = MapStatusToChinese(e.StatusText);
                StatusText = $"{e.Name}: {statusText}";

                if (SelectedDevice?.ID == e.ID)
                {
                    OnPropertyChanged(nameof(SelectedDeviceConnectedText));
                }
            });
        }

        /// <summary>
        /// 选中设备变更时，触发相关计算属性更新。
        /// </summary>
        private void OnSelectedDeviceChanged()
        {
            OnPropertyChanged(nameof(IsDeviceSelected));
            OnPropertyChanged(nameof(IsNoDeviceSelected));
            OnPropertyChanged(nameof(SelectedDeviceEndpoint));
            OnPropertyChanged(nameof(SelectedDeviceEncodingInfo));
            OnPropertyChanged(nameof(SelectedDeviceHeartbeatText));
            OnPropertyChanged(nameof(SelectedDeviceAutoConnectText));
            OnPropertyChanged(nameof(SelectedDeviceConnectedText));
        }

        /// <summary>
        /// 将运行时状态文本转换为中文显示。
        /// </summary>
        private static string MapStatusToChinese(string? status)
        {
            return status?.ToLowerInvariant() switch
            {
                "connected" => "已连接",
                "disconnected" => "未连接",
                "connecting" => "连接中",
                "error" => "错误",
                _ => status ?? "未知"
            };
        }
    }
}
