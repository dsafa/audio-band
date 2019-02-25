namespace AudioBand.Models
{
    /// <summary>
    /// Model for the play/pause button.
    /// </summary>
    public class PlayPauseButton : ModelBase
    {
        private string _playButtonImagePath = "";
        private string _pauseButtonImagePath = "";
        private int _xPosition = 103;
        private int _yPosition = 15;
        private int _width = 73;
        private int _height = 12;
        private bool _isVisible = true;

        /// <summary>
        /// Gets or sets the path for the play image.
        /// </summary>
        public string PlayButtonImagePath
        {
            get => _playButtonImagePath;
            set => SetProperty(ref _playButtonImagePath, value);
        }

        /// <summary>
        /// Gets or sets the path for the pause image.
        /// </summary>
        public string PauseButtonImagePath
        {
            get => _pauseButtonImagePath;
            set => SetProperty(ref _pauseButtonImagePath, value);
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
    }
}
