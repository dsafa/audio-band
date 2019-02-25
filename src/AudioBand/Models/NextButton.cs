namespace AudioBand.Models
{
    /// <summary>
    /// Model for the next button.
    /// </summary>
    public class NextButton : ModelBase
    {
        private string _imagePath = "";
        private bool _isVisible = true;
        private int _width = 73;
        private int _height = 12;
        private int _xPosition = 176;
        private int _yPosition = 15;

        /// <summary>
        /// Gets or sets the path of the button image.
        /// </summary>
        public string ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
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
