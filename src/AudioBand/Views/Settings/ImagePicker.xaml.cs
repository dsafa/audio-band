using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using AudioBand.Commands;
using AudioBand.ViewModels;

namespace AudioBand.Views.Settings
{
    /// <summary>
    /// Image picker control.
    /// </summary>
    public partial class ImagePicker : UserControl
    {
        /// <summary>
        /// The dependency property for <see cref="ImagePath"/>.
        /// </summary>
        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register(nameof(ImagePath), typeof(string), typeof(ImagePicker), new PropertyMetadata(null));

        /// <summary>
        /// Dependency property for <see cref="DialogService"/>.
        /// </summary>
        public static readonly DependencyProperty DialogServiceProperty =
            DependencyProperty.Register(nameof(DialogService), typeof(IDialogService), typeof(ImagePicker));

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagePicker"/> class.
        /// </summary>
        public ImagePicker()
        {
            ResetImageCommand = new RelayCommand(ResetImagePathOnExecuted, ResetImagePathCanExecute);
            BrowseForImageCommand = new RelayCommand(BrowseForImageOnExecuted);

            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the curren image path.
        /// </summary>
        public string ImagePath
        {
            get => (string)GetValue(ImagePathProperty);
            set => SetValue(ImagePathProperty, value);
        }

        /// <summary>
        /// Gets or sets the dialog service.
        /// </summary>
        public IDialogService DialogService
        {
            get => (IDialogService)GetValue(DialogServiceProperty);
            set => SetValue(DialogServiceProperty, value);
        }

        /// <summary>
        /// Gets the command to reset the image path.
        /// </summary>
        public RelayCommand ResetImageCommand { get; }

        /// <summary>
        /// Gets the command to open a browser for the image.
        /// </summary>
        public RelayCommand BrowseForImageCommand { get; }

        private void ResetImagePathOnExecuted(object parameter)
        {
            ImagePath = "";
        }

        private bool ResetImagePathCanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(ImagePath);
        }

        private void BrowseForImageOnExecuted(object parameter)
        {
            Debug.Assert(DialogService != null, "Dialog service not assigned");

            var path = DialogService.ShowImagePickerDialog();
            if (path == null)
            {
                return;
            }

            ImagePath = path;
        }
    }
}
