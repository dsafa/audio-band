using System.Threading.Tasks;
using System.Windows.Input;

namespace AudioBand.Commands
{
    /// <summary>
    /// An async <see cref="ICommand"/>.
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>A task representing the result of the command.</returns>
        Task ExecuteAsync(object parameter);
    }
}
