using System.Windows.Media;

namespace AudioBand.Models
{
    /// <summary>
    /// Base class for next and previous button.
    /// </summary>
    public class PlaybackButtonBase : ModelBase
    {
        private string _imagePath = "";
        private string _hoveredImagePath = "";
        private string _clickedImagePath = "";
        private bool _isVisible = true;
        private int _width;
        private int _height;
        private int _xPosition;
        private int _yPosition;
        private Color _backgroundColor = Colors.Transparent;
        private Color _hoveredBackgroundColor = Color.FromArgb(25, 255, 255, 255);
        private Color _clickedBackgroundColor = Color.FromArgb(15, 255, 255, 255);
        private ButtonContentType _contentType = ButtonContentType.Image;
        private string _textFontFamily = "Segoe MDL2 Assets";
        private string _text;
        private Color _textColor = Colors.White;
        private Color _textHoveredColor = Colors.White;
        private Color _textClickedColor = Colors.White;

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

        /// <summary>
        /// Gets or sets the content type for the button.
        /// </summary>
        public ButtonContentType ContentType
        {
            get => _contentType;
            set => SetProperty(ref _contentType, value);
        }

        /// <summary>
        /// Gets or sets the font family for the button text.
        /// </summary>
        public string TextFontFamily
        {
            get => _textFontFamily;
            set => SetProperty(ref _textFontFamily, value);
        }

        /// <summary>
        /// Gets or sets the text for button.
        /// </summary>
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        public Color TextColor
        {
            get => _textColor;
            set => SetProperty(ref _textColor, value);
        }

        /// <summary>
        /// Gets or sets the text color when hovered.
        /// </summary>
        public Color TextHoveredColor
        {
            get => _textHoveredColor;
            set => SetProperty(ref _textHoveredColor, value);
        }

        /// <summary>
        /// Gets or sets the text color when clicked.
        /// </summary>
        public Color TextClickedColor
        {
            get => _textClickedColor;
            set => SetProperty(ref _textClickedColor, value);
        }
    }
}
