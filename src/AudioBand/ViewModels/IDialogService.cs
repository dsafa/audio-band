using System.Threading.Tasks;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Provides functionality to show dialogs.
    /// </summary>
    internal interface IDialogService
    {
        /// <summary>
        /// Show a confirmation dialog.
        /// </summary>
        /// <param name="title">Title of the dialog.</param>
        /// <param name="message">Message of the dialog.</param>
        /// <returns>True if accepted; false otherwise.</returns>
        Task<bool> ShowConfirmationDialogAsync(string title, string message);
    }
}
