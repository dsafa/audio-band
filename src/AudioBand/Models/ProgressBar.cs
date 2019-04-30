using System.Drawing;

namespace AudioBand.Models
{
    /// <summary>
    /// Model for the progress bar.
    /// </summary>
    public class ProgressBar : ModelBase
    {
        private Color _foregroundColor = Color.DodgerBlue;
        private Color _backgroundColor = Color.Black;
        private Color _hoverColor = Color.DeepSkyBlue;
        private bool _isVisible = true;
        private int _xPosition = 30;
        private int _yPosition = 28;
        private int _width = 220;
        private int _height = 2;

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
        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        /// <summary>
        /// Gets or sets the height of the progress bar.
        /// </summary>
        public int Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        /// <summary>
        /// Gets or sets the x position of the progress bar.
        /// </summary>
        public int XPosition
        {
            get => _xPosition;
            set => SetProperty(ref _xPosition, value);
        }

        /// <summary>
        /// Gets or sets the y position of the progress bar.
        /// </summary>
        public int YPosition
        {
            get => _yPosition;
            set => SetProperty(ref _yPosition, value);
        }
    }
}
