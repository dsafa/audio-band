using System.Windows.Media;

namespace AudioBand.Models
{
    /// <summary>
    /// Model for the progress bar.
    /// </summary>
    public class ProgressBar : ModelBase
    {
        private Color _foregroundColor = Colors.DodgerBlue;
        private Color _backgroundColor = Colors.DimGray;
        private Color _hoverColor = Colors.DeepSkyBlue;
        private bool _isVisible = true;
        private double _xPosition = 325;
        private double _yPosition = 22;
        private double _width = 130;
        private double _height = 4;

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        public Color ForegroundColor
        {
            get => _foregroundColor;
            set => SetProperty(ref _foregroundColor, value);
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }

        /// <summary>
        /// Gets or sets the hover color.
        /// </summary>
        public Color HoverColor
        {
            get => _hoverColor;
            set => SetProperty(ref _hoverColor, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the progress bar is visible.
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        /// <summary>
        /// Gets or sets the width of the progress bar.
        /// </summary>
        public double Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        /// <summary>
        /// Gets or sets the height of the progress bar.
        /// </summary>
        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        /// <summary>
        /// Gets or sets the x position of the progress bar.
        /// </summary>
        public double XPosition
        {
            get => _xPosition;
            set => SetProperty(ref _xPosition, value);
        }

        /// <summary>
        /// Gets or sets the y position of the progress bar.
        /// </summary>
        public double YPosition
        {
            get => _yPosition;
            set => SetProperty(ref _yPosition, value);
        }
    }
}
