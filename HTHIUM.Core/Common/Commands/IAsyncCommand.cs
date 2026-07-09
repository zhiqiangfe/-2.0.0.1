using System.Windows.Input;

namespace HTHIUM.Core.Common.Commands
{
    /// <summary>
    /// 异步命令接口
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object? parameter);
        bool IsExecuting { get; }
        bool CanBeCanceled { get; }
        void Cancel();
    }

    /// <summary>
    /// 异步命令接口（泛型）
    /// </summary>
    public interface IAsyncCommand<T> : IAsyncCommand
    {
        Task ExecuteAsync(T? parameter);
    }
}
