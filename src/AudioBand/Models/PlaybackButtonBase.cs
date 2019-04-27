using System.Drawing;

namespace AudioBand.Models
{
    /// <summary>
    /// Base class for next and previous button
    /// </summary>
    public class PlaybackButtonBase : ModelBase
    {
        private string _imagePath;
        private string _hoveredImagePath;
        private string _clickedImagePath;
        private bool _isVisible;
        private int _width;
        private int _height;
        private int _xPosition;
        private int _yPosition;
        private Color _backgroundColor;
        private Color _hoveredBackgroundColor;
        private Color _clickedBackgroundColor;

        /// <summary>
        /// Gets or sets the path of the button image.
        /// </summary>
        public string ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
        }

        /// <summary>
        /// Gets or sets the path of the button image when hovered.
        /// </summary>
        public string HoveredImagePath
        {
            get => _hoveredImagePath;
            set => SetProperty(ref _hoveredImagePath, value);
        }

        /// <summary>
        /// Gets or sets the path of the button image when clicked.
        /// </summary>
        public string ClickedImagePath
        {
            get => _clickedImagePath;
            set => SetProperty(ref _clickedImagePath, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the button is visible.
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        /// <summary>
        /// Gets or sets the width of the button.
        /// </summary>
        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        /// <summary>
        /// Gets or sets the height of the button.
        /// </summary>
        public int Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        /// <summary>
        /// Gets or sets the x position of the button.
        /// </summary>
        public int XPosition
        {
            get => _xPosition;
            set => SetProperty(ref _xPosition, value);
        }

        /// <summary>
        /// Gets or sets the y position of the button.
        /// </summary>
        public int YPosition
        {
            get => _yPosition;
            set => SetProperty(ref _yPosition, value);
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
        /// Gets or sets the background color when hovered.
        /// </summary>
        public Color HoveredBackgroundColor
        {
            get => _hoveredBackgroundColor;
            set => SetProperty(ref _hoveredBackgroundColor, value);
        }

        /// <summary>
        /// Gets or sets the background color when clicked.
        /// </summary>
        public Color ClickedBackgroundColor
        {
            get => _clickedBackgroundColor;
            set => SetProperty(ref _clickedBackgroundColor, value);
        }
    }
}
