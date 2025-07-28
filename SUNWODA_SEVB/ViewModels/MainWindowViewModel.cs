using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SUNWODA_SEVB.Logging;

namespace SUNWODA_SEVB.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ILoggerService<MainWindowViewModel> _logger;
        private string _statusMessage = "就绪";
        private string _logMessage = "";

        public MainWindowViewModel(ILoggerService<MainWindowViewModel> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.Info("MainWindowViewModel 已初始化");

            // 初始化命令
            TestLogCommand = new RelayCommand(ExecuteTestLog);
            ShowInfoCommand = new RelayCommand(ExecuteShowInfo);
            ShowWarningCommand = new RelayCommand(ExecuteShowWarning);
            ShowErrorCommand = new RelayCommand(ExecuteShowError);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                    _logger.Debug($"状态消息已更新: {value}");
                }
            }
        }

        public string LogMessage
        {
            get => _logMessage;
            set
            {
                if (_logMessage != value)
                {
                    _logMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand TestLogCommand { get; }
        public ICommand ShowInfoCommand { get; }
        public ICommand ShowWarningCommand { get; }
        public ICommand ShowErrorCommand { get; }

        private void ExecuteTestLog()
        {
            try
            {
                _logger.Info("用户点击了测试日志按钮");

                // 模拟一些业务操作
                StatusMessage = "正在执行测试操作...";

                // 记录不同级别的日志
                _logger.Trace("这是一条跟踪日志");
                _logger.Debug("这是一条调试日志");
                _logger.Info("这是一条信息日志");
                _logger.Warn("这是一条警告日志");

                StatusMessage = "测试操作完成";
                LogMessage = $"测试日志已写入 - {DateTime.Now:HH:mm:ss}";
            }
            catch (Exception ex)
            {
                _logger.ErrorException("执行测试日志时发生错误",ex);
                StatusMessage = "测试操作失败";
            }
        }

        private void ExecuteShowInfo()
        {
            _logger.Info($"用户查看信息 - {DateTime.Now}");
            StatusMessage = "显示信息完成";
        }

        private void ExecuteShowWarning()
        {
            _logger.Warn("用户触发了警告操作");
            StatusMessage = "警告操作已记录";
        }

        private void ExecuteShowError()
        {
            try
            {
                // 模拟一个错误
                throw new InvalidOperationException("这是一个示例错误");
            }
            catch (Exception ex)
            {
                _logger.ErrorException("模拟错误操作", ex);
                StatusMessage = "错误已记录到日志";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // 简单的命令实现
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();
    }
}