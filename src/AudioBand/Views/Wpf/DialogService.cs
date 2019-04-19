using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using AudioBand.ViewModels;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Implementation of <see cref="IDialogService"/>.
    /// </summary>
    public class DialogService : IDialogService
    {
        private AboutDialog _instance;

        /// <inheritdoc/>
        public Color? ShowColorPickerDialog(Color initialColor)
        {
            var colorPickerDialog = new Dsafa.WpfColorPicker.ColorPickerDialog(initialColor)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            var res = colorPickerDialog.ShowDialog();
            if (res.HasValue && res.Value)
            {
                return colorPickerDialog.Color;
            }

            return null;
        }

        /// <inheritdoc />
        public void ShowAboutDialog()
        {
            if (_instance != null)
            {
                _instance.Activate();
            }
            else
            {
                _instance = new AboutDialog();
                _instance.Closed += (o, e) => _instance = null;
                _instance.Show();
            }
        }

        /// <inheritdoc />
        public string ShowRenameDialog(string currentName, IEnumerable<string> profiles)
        {
            var vm = new RenameProfileDialogVM(currentName, profiles);
            var dialog = new RenameProfileDialog(vm);
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                return vm.NewProfileName;
            }

            return null;
        }

        /// <inheritdoc/>
        public bool ShowConfirmationDialog(ConfirmationDialogType confirmType, params object[] data)
        {
            var dialog = new ConfirmationDialog(confirmType, data);
            var result = dialog.ShowDialog();
            return result.GetValueOrDefault(false);
        }
    }
}
