using AudioBand.Commands;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Controls;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for ImagePicker.xaml
    /// </summary>
    public partial class ImagePicker : UserControl
    {
        public string ImagePath
        {
            get => (string) GetValue(ImagePathProperty);
            set => SetValue(ImagePathProperty, value);
        }

        public static readonly DependencyProperty ImagePathProperty = 
            DependencyProperty.Register(nameof(ImagePath), typeof(string), typeof(ImagePicker), new PropertyMetadata());

        public RelayCommand ResetImageCommand { get; }
        public RelayCommand BrowseForImageCommand { get; }

        public ImagePicker()
        {
            InitializeComponent();

            ResetImageCommand = new RelayCommand(ResetImagePathOnExecuted, ResetImagePathCanExecute);
            BrowseForImageCommand = new RelayCommand(BrowseForImageOnExecuted);

            DataContext = this;
        }

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
            return !String.IsNullOrEmpty(ImagePath);
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
