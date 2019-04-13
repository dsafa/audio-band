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
        /// <param name="confirmType">Type of confirmation dialog.</param>
        /// <param name="data">Data for the confirmation dialog.</param>
        /// <returns>True if accepted; false otherwise.</returns>
        bool ShowConfirmationDialog(ConfirmationDialogType confirmType, params object[] data);

        /// <summary>
        /// Show the color picker dialog.
        /// </summary>
        /// <param name="initialColor">The initial color.</param>
        /// <returns>The new color; otherwise the action was cancelled.</returns>
        Color? ShowColorPickerDialog(Color initialColor);

        /// <summary>
        /// Show the about dialog.
        /// </summary>
        void ShowAboutDialog();
    }
}
