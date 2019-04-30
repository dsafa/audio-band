using System.Collections.Generic;
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

        /// <summary>
        /// Show the rename dialog.
        /// </summary>
        /// <param name="currentName">The current name.</param>
        /// <param name="profiles">The list of profiles.</param>
        /// <returns>Null if canceled, otherwise the new name.</returns>
        string ShowRenameDialog(string currentName, IEnumerable<string> profiles);

        /// <summary>
        /// Show the image picker dialog.
        /// </summary>
        /// <returns>The path of the image.</returns>
        string ShowImagePickerDialog();
    }
}
