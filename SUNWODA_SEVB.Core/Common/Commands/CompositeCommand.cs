

using System.Windows.Input;

namespace SUNWODA_SEVB.Core.Common.Commands
{
    /// <summary>
    /// 命令组合器，支持多个命令的组合执行
    /// </summary>
    public class CompositeCommand : ICommand
    {
        private readonly List<ICommand> _commands = new();

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                foreach (var command in _commands)
                {
                    command.CanExecuteChanged += value;
                }
            }
            remove
            {
                foreach (var command in _commands)
                {
                    command.CanExecuteChanged -= value;
                }
            }
        }

        public void RegisterCommand(ICommand command)
        {
            ArgumentNullException.ThrowIfNull(command);
            _commands.Add(command);
        }

        public void UnregisterCommand(ICommand command)
        {
            _commands.Remove(command);
        }

        public bool CanExecute(object? parameter)
        {
            return _commands.All(cmd => cmd.CanExecute(parameter));
        }

        public void Execute(object? parameter)
        {
            foreach (var command in _commands.Where(cmd => cmd.CanExecute(parameter)))
            {
                command.Execute(parameter);
            }
        }
    }
}
