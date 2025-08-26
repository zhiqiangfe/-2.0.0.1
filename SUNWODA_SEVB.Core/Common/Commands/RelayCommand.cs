using System.Windows.Input;

namespace SUNWODA_SEVB.Core.Common.Commands
{
    /// <summary>
    /// 基础命令实现
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;
        private readonly HashSet<WeakReference> _canExecuteChangedHandlers = new();

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                if (value != null)
                {
                    _canExecuteChangedHandlers.Add(new WeakReference(value));
                }
            }
            remove
            {
                if (value != null)
                {
                    _canExecuteChangedHandlers.RemoveWhere(wr => !wr.IsAlive || wr.Target?.Equals(value) == true);
                }
            }
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();

        public void NotifyCanExecuteChanged()
        {
            _canExecuteChangedHandlers.RemoveWhere(wr => !wr.IsAlive);
            foreach (var weakRef in _canExecuteChangedHandlers)
            {
                if (weakRef.Target is EventHandler handler)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
    }

    /// <summary>
    /// 带参数的命令实现
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Predicate<T?>? _canExecute;
        private readonly HashSet<WeakReference> _canExecuteChangedHandlers = new();

        public RelayCommand(Action<T?> execute, Predicate<T?>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                if (value != null)
                {
                    _canExecuteChangedHandlers.Add(new WeakReference(value));
                }
            }
            remove
            {
                if (value != null)
                {
                    _canExecuteChangedHandlers.RemoveWhere(wr => !wr.IsAlive || wr.Target?.Equals(value) == true);
                }
            }
        }

        public bool CanExecute(object? parameter)
        {
            if (parameter is null)
            {
                return typeof(T).IsClass || Nullable.GetUnderlyingType(typeof(T)) != null;
            }
            return parameter is T t && (_canExecute?.Invoke(t) ?? true);
        }

        public void Execute(object? parameter)
        {
            if (parameter is T t || (parameter is null && (typeof(T).IsClass || Nullable.GetUnderlyingType(typeof(T)) != null)))
            {
                _execute((T?)parameter);
            }
        }

        public void NotifyCanExecuteChanged()
        {
            _canExecuteChangedHandlers.RemoveWhere(wr => !wr.IsAlive);
            foreach (var weakRef in _canExecuteChangedHandlers)
            {
                if (weakRef.Target is EventHandler handler)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
    }
}