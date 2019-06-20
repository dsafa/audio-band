using System;
using System.Threading.Tasks;

namespace AudioBand.Commands
{
    /// <summary>
    /// An async relay command that does not have a parameter.
    /// </summary>
    public class AsyncRelayCommand : AsyncRelayCommand<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The function to execute.</param>
        public AsyncRelayCommand(Func<Task> execute)
            : base(o => execute())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The function to execute.</param>
        /// <param name="canExecute">The predicate to determine if the command can execute.</param>
        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute)
            : base(o => execute(), o => canExecute())
        {
        }
    }
}
