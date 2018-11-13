using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AudioBand.Commands
{
    /// <summary>
    /// Basic async command.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class AsyncRelayCommand<T> : ICommand
    {
        private readonly Func<T, Task> _execute;
        private readonly Predicate<T> _canExecute;

        /// <summary>
        /// Create an instance of <see cref="AsyncRelayCommand{T}"/> with a delegate to execute.
        /// </summary>
        /// <param name="execute">Action to execute.</param>
        public AsyncRelayCommand(Func<T, Task> execute) : this(execute, null) { }

        /// <summary>
        /// Create an instace of <see cref="AsyncRelayCommand{T}"/> with a delegate to execute and a predicate to determine if it can be executed.
        /// </summary>
        /// <param name="execute">Async delegate to execute.</param>
        /// <param name="canExecute">Predicate to check if the commmand can execute.</param>
        public AsyncRelayCommand(Func<T, Task> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <inheritdoc cref="ICommand.CanExecute"/>
        public bool CanExecute(object parameter)
        {
            return  _canExecute?.Invoke((T)parameter) ?? true;
        }

        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <inheritdoc cref="ICommand.Execute"/>
        public async void Execute(object parameter)
        {
            await _execute((T)parameter);
        }

        /// <summary>
        /// Execute the command asynchronously.
        /// </summary>
        /// <param name="parameter">Command paramerter.</param>
        /// <returns>Task</returns>
        public async Task ExecuteAsync(object parameter)
        {
            await _execute((T)parameter);
        }
    }
}
