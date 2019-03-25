using System.Windows;
using System.Windows.Media;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Provides functionality to show dialogs.
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Show a confirmation dialog.
        /// </summary>
        /// <param name="title">Title of the dialog.</param>
        /// <param name="message">Message of the dialog.</param>
        /// <returns>True if accepted; false otherwise.</returns>
        bool ShowConfirmationDialog(string title, string message);

        /// <summary>
        /// Show the color picker dialog.
        /// </summary>
        /// <param name="initialColor">The initial color.</param>
        /// <returns>The new color; otherwise the action was cancelled.</returns>
        Color? ShowColorPickerDialog(Color initialColor);
    }
}
