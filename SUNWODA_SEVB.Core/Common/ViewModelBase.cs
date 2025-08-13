using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace SUNWODA_SEVB.Core.Common
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly Dispatcher _dispatcher;

        public ViewModelBase()
        {
            // 保存UI的Dispatcher，用于后续的UI更新
            _dispatcher = Application.Current.Dispatcher;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetProperty<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return;
            field = value;
            OnPropertyChanged(propName);
        }

        // 生命周期方法
        public virtual void OnNavigatedTo(object? parameter)
        {
            // 默认调用异步版本
            Task.Run(async () => await OnNavigatedToAsync(parameter));
        }

        public virtual void OnNavigatedFrom()
        {
            // 默认调用异步版本
            Task.Run(async () => await OnNavigatedFromAsync());
        }

        public virtual bool CanNavigateFrom() => true;

        public virtual void OnInitialize()
        {
            // 默认调用异步版本
            Task.Run(async () => await OnInitializeAsync());
        }

        public virtual void OnCleanup()
        {
            // 默认调用异步版本
            Task.Run(async () => await OnCleanupAsync());
        }

        // 新增异步生命周期方法
        protected virtual async Task OnNavigatedToAsync(object? parameter)
        {
            await Task.CompletedTask;
        }

        protected virtual async Task OnNavigatedFromAsync()
        {
            await Task.CompletedTask;
        }

        protected virtual async Task OnInitializeAsync()
        {
            await Task.CompletedTask;
        }

        protected virtual async Task OnCleanupAsync()
        {
            await Task.CompletedTask;
        }

        // UI线程辅助方法
        protected async Task RunOnUIThreadAsync(Action action)
        {
            if (_dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                await _dispatcher.InvokeAsync(action);
            }
        }

        protected void RunOnUIThread(Action action)
        {
            if (_dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                _dispatcher.Invoke(action);
            }
        }

        // 后台任务辅助方法
        protected async Task<T> RunInBackgroundAsync<T>(Func<Task<T>> func)
        {
            return await Task.Run(func);
        }

        protected async Task RunInBackgroundAsync(Func<Task> func)
        {
            await Task.Run(func);
        }

        // 延迟执行UI线程辅助方法
        protected async Task DelayedExecuteAsync(Action action, int delayMilliseconds)
        {
            await Task.Delay(delayMilliseconds);
            await RunOnUIThreadAsync(action);
        }
    }
}
