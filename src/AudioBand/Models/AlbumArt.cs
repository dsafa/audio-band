namespace AudioBand.Models
{
    /// <summary>
    /// Model for the album art.
    /// </summary>
    internal class AlbumArt : ModelBase
    {
        private bool _isVisible = true;
        private int _width = 30;
        private int _height = 30;
        private int _xPosition = 0;
        private int _yPosition = 0;
        private string _placeholderPath = "";

        /// <summary>
        /// Gets or sets a value indicating whether the album art is visible.
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        /// <summary>
        /// Gets or sets the width of the album art.
        /// </summary>
        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        /// <summary>
        /// Gets or sets the height of the album art.
        /// </summary>
        public int Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        /// <summary>
        /// Gets or sets the x position of the album art.
        /// </summary>
        public int XPosition
        {
            get => _xPosition;
            set => SetProperty(ref _xPosition, value);
        }

        /// <summary>
        /// Gets or sets the y position of the album art.
        /// </summary>
        public int YPosition
        {
            get => _yPosition;
            set => SetProperty(ref _yPosition, value);
        }

        /// <summary>
        /// Gets or sets the path of the placeholder image.
        /// </summary>
        public string PlaceholderPath
        {
            get => _placeholderPath;
            set => SetProperty(ref _placeholderPath, value);
        }
    }
}
