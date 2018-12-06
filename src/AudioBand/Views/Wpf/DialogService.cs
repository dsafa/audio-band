using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using AudioBand.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Implementation of <see cref="IDialogService"/>.
    /// </summary>
    internal class DialogService : IDialogService
    {
        private readonly MetroWindow _window;

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogService"/> class
        /// with the window.
        /// </summary>
        /// <param name="window">The parent window.</param>
        public DialogService(MetroWindow window)
        {
            _window = window;
        }

        /// <summary>
        /// Show the color picker dialog.
        /// </summary>
        /// <param name="window">Parent window</param>
        /// <param name="initialColor">The initial color.</param>
        /// <returns>The new color; otherwise the action was cancelled.</returns>
        public static Color? ShowColorPickerDialog(Window window, Color initialColor)
        {
            var colorPickerDialog = new ColorPickerDialog
            {
                Owner = window,
                Color = initialColor
            };

            var res = colorPickerDialog.ShowDialog();
            if (res.HasValue && res.Value)
            {
                return colorPickerDialog.Color;
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<bool> ShowConfirmationDialogAsync(string title, string message)
        {
            var dialogSettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
                AnimateHide = false,
                AnimateShow = false,
            };

            var res = await _window.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative, dialogSettings);
            return res == MessageDialogResult.Affirmative;
        }
    }
}
