namespace AudioBand.Models
{
    /// <summary>
    /// Model for the album art popup.
    /// </summary>
    internal class AlbumArtPopup : ModelBase
    {
        private bool _isVisible = true;
        private int _width = 250;
        private int _height = 250;
        private int _xPosition = 0;
        private int _margin = 4;

        /// <summary>
        /// Gets or sets a value indicating whether the album art popup is visible.
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        /// <summary>
        /// Gets or sets the width of the album art popup
        /// </summary>
        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        /// <summary>
        /// Gets or sets the height of the album art popup.
        /// </summary>
        public int Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        /// <summary>
        /// Gets or sets the x position of the album art popup.
        /// </summary>
        public int XPosition
        {
            get => _xPosition;
            set => SetProperty(ref _xPosition, value);
        }

        /// <summary>
        /// Gets or sets the margin between the album art popup and the taskbar.
        /// </summary>
        public int Margin
        {
            get => _margin;
            set => SetProperty(ref _margin, value);
        }
    }
}
