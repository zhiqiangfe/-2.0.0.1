
namespace HTHIUM.Core.Common.Commands
{
    /// <summary>
    /// 泛型异步命令实现
    /// </summary>
    public class AsyncRelayCommand<T> : ModelBase, IAsyncCommand<T>
    {
        private readonly Func<T?, CancellationToken, Task> _executeAsync;
        private readonly Predicate<T?>? _canExecute;
        private readonly bool _canBeCanceled;
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _isExecuting;

        public bool IsExecuting
        {
            get => _isExecuting;
            private set => SetProperty(ref _isExecuting, value);
        }

        public bool CanBeCanceled => _canBeCanceled;

        public event EventHandler? CanExecuteChanged;

        public AsyncRelayCommand(
            Func<T?, CancellationToken, Task> executeAsync,
            Predicate<T?>? canExecute = null,
            bool canBeCanceled = true)
        {
            ArgumentNullException.ThrowIfNull(executeAsync);
            _executeAsync = executeAsync;
            _canExecute = canExecute;
            _canBeCanceled = canBeCanceled;
        }

        public bool CanExecute(object? parameter)
        {
            if (IsExecuting)
                return false;

            if (parameter is null)
            {
                return typeof(T).IsClass || Nullable.GetUnderlyingType(typeof(T)) != null;
            }

            return parameter is T t && (_canExecute?.Invoke(t) ?? true);
        }

        public async void Execute(object? parameter) => await ExecuteAsync(parameter);

        public async Task ExecuteAsync(object? parameter)
        {
            if (parameter is T t || (parameter is null && (typeof(T).IsClass || Nullable.GetUnderlyingType(typeof(T)) != null)))
            {
                await ExecuteAsync((T?)parameter);
            }
        }

        public async Task ExecuteAsync(T? parameter)
        {
            if (!CanExecute(parameter))
                return;

            _cancellationTokenSource = new CancellationTokenSource();
            IsExecuting = true;
            NotifyCanExecuteChanged();

            try
            {
                await _executeAsync(parameter, _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                // 操作被取消，正常情况
            }
            finally
            {
                IsExecuting = false;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                NotifyCanExecuteChanged();
            }
        }

        public void Cancel()
        {
            if (_canBeCanceled && _cancellationTokenSource?.IsCancellationRequested == false)
            {
                _cancellationTokenSource.Cancel();
            }
        }

        public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
