using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using AudioBand.ViewModels;
using Microsoft.Win32;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Implementation of <see cref="IDialogService"/>.
    /// </summary>
    public class DialogService : IDialogService
    {
        private static readonly string[] ImageFilters =
        {
            "Images (*.bmp;*.dib;*.rle;*.jpg;*.jpeg;*.jpe,*.jfif;*.tiff;*.tif;*.png;*.svg)|*.bmp;*.dib;*.rle;*.jpg;*.jpeg;*.jpe,*.jfif;*.tiff;*.tif;*.png;*.svg",
            "Bmp files (*.bmp;*.dib;*.rle)|*.bmp;*.dib;*.rle",
            "Jpeg Files (*.jpg;*.jpeg;*.jpe,*.jfif)|*.jpg;*.jpeg;*.jpe,*.jfif",
            "Tiff Files (*.tiff;*.tif)|*.tiff;*.tif",
            "Png Files (*.png)|*.png",
            "Svg Files (*.svg)|*.svg",
            "All Files (*.*)|*.*"
        };

        private static readonly string FileDialogImageFilter = string.Join("|", ImageFilters);

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
#if DEBUG
            switch (confirmType)
            {
                case ConfirmationDialogType.DeleteLabel:
                    Debug.Assert(data.Length == 1, "Expected Data[0] to contain the label name");
                    break;
                case ConfirmationDialogType.DeleteProfile:
                    Debug.Assert(data.Length == 1, "Expected data[0] to contain the profile name");
                    break;
            }
#endif

            var dialog = new ConfirmationDialog(confirmType, data);
            var result = dialog.ShowDialog();
            return result.GetValueOrDefault(false);
        }

        /// <inheritdoc />
        public string ShowImagePickerDialog()
        {
            var dialog = new OpenFileDialog { Filter = FileDialogImageFilter };

            var res = dialog.ShowDialog();
            if (res.HasValue && res.Value)
            {
                return dialog.FileName;
            }

            return null;
        }
    }
}
