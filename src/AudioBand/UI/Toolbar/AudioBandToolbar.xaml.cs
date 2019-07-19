using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CSDeskBand;

namespace AudioBand.UI
{
    /// <summary>
    /// Interaction logic for AudioBandToolbar.xaml.
    /// </summary>
    public partial class AudioBandToolbar : UserControl
    {
        private readonly CSDeskBandOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioBandToolbar"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="options">The deskband options.</param>
        public AudioBandToolbar(AudioBandToolbarViewModel viewModel, CSDeskBandOptions options)
        {
            _options = options;
            InitializeComponent();
            DataContext = viewModel;
            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var scale = VisualTreeHelper.GetDpi(this).PixelsPerInchY / 96.0;
            var deskbandSize = new DeskBandSize((int)Math.Round(ActualWidth * scale), (int)Math.Round(ActualHeight * scale));
            _options.MinHorizontalSize = deskbandSize;
            _options.HorizontalSize = deskbandSize;
            _options.MaxHorizontalHeight = deskbandSize.Height;
        }
    }
}
