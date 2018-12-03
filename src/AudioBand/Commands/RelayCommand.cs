using System;

namespace AudioBand.Commands
{
    /// <inheritdoc cref="RelayCommand{T}"/>
    public class RelayCommand : RelayCommand<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// with an action to be executed.
        /// </summary>
        /// <param name="execute">Action to execute.</param>
        public RelayCommand(Action<object> execute)
            : base(execute) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// with an action to be executed and a predicate to determine if it can be executed.
        /// </summary>
        /// <param name="execute">Action to execute</param>
        /// <param name="canExecute">Predicate to check if the command can be executed.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            : base(execute, canExecute) { }
    }
}
