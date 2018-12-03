using System;
using System.Windows.Input;

namespace AudioBand.Commands
{
    /// <inheritdoc cref="ICommand"/>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// with an action to be executed.
        /// </summary>
        /// <param name="execute">Action to execute.</param>
        public RelayCommand(Action<T> execute)
            : this(execute, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// with an action to be executed and a predicate to determine if it can be executed.
        /// </summary>
        /// <param name="execute">Action to execute.</param>
        /// <param name="canExecute">Predicate to check if the commmand can be executed.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <inheritdoc cref="ICommand.CanExecute"/>
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        /// <inheritdoc cref="ICommand.Execute"/>
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}
