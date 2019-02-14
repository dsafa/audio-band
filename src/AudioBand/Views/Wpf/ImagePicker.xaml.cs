using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Controls;
using AudioBand.Commands;
using Microsoft.Win32;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for ImagePicker.xaml
    /// </summary>
    public partial class ImagePicker : UserControl
    {
        /// <summary>
        /// The dependency property for <see cref="ImagePath"/>.
        /// </summary>
        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register(nameof(ImagePath), typeof(string), typeof(ImagePicker), new PropertyMetadata(null));

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
        /// Gets the command to reset the image path.
        /// </summary>
        public RelayCommand ResetImageCommand { get; }

        /// <summary>
        /// Gets the command to open a browser for the image.
        /// </summary>
        public RelayCommand BrowseForImageCommand { get; }

        private static string SelectImage()
        {
            var dlg = new OpenFileDialog();

            var codecs = ImageCodecInfo.GetImageEncoders();
            var filters = new List<string>();
            var fileExtensions = new List<string>();

            foreach (var codec in codecs)
            {
                var codecName = codec.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                filters.Add($"{codecName} ({codec.FilenameExtension})|{codec.FilenameExtension}");
                fileExtensions.Add(codec.FilenameExtension);
            }

            var allFilter = "All |" + string.Join(";", fileExtensions);
            dlg.Filter = allFilter + "|" + string.Join("|", filters);

            var res = dlg.ShowDialog();
            if (res.HasValue && res.Value)
            {
                return dlg.FileName;
            }

            return null;
        }

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
            var path = SelectImage();
            if (path == null)
            {
                return;
            }

            ImagePath = path;
        }
    }
}
