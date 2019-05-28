using System.Windows.Media;

namespace AudioBand.Models
{
    /// <summary>
    /// The model for the toolbar.
    /// </summary>
    public class AudioBand : ModelBase
    {
        private double _width = 500;
        private double _height = 30;
        private Color _backgroundColor = Colors.Transparent;

        /// <summary>
        /// Gets or sets the width of the toolbar.
        /// </summary>
        public double Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        /// <summary>
        /// Gets or sets the height of the toolbar.
        /// </summary>
        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        /// <summary>
        /// Gets or sets the background color of the toolbar.
        /// </summary>
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }
    }
}
