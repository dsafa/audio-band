using System.Drawing;

namespace AudioBand.Models
{
    /// <summary>
    /// Model for the play/pause button.
    /// </summary>
    public class PlayPauseButton : ModelBase
    {
        private string _playButtonImagePath = "";
        private string _playButtonHoveredImagePath = "";
        private string _playButtonClickedImagePath = "";
        private string _pauseButtonImagePath = "";
        private string _pauseButtonHoveredImagePath = "";
        private string _pauseButtonClickedImagePath = "";
        private int _xPosition = 103;
        private int _yPosition = 15;
        private int _width = 73;
        private int _height = 12;
        private bool _isVisible = true;
        private Color _defaultBackgroundColor = Color.Transparent;
        private Color _hoveredBackgroundColor = Color.FromArgb(25, 255, 255, 255);
        private Color _clickedBackgroundColor = Color.FromArgb(15, 255, 255, 255);
        private ButtonContentType _playButtonContentType = ButtonContentType.Image;
        private ButtonContentType _pauseButtonContentType = ButtonContentType.Image;
        private string _playButtonTextFontFamily = "Segoe MDL2 Assets";
        private string _pauseButtonTextFontFamily = "Segoe MDL2 Assets";
        private string _playButtonText = "";
        private string _pauseButtonText = "";

        /// <summary>
        /// Gets or sets the path for the play image.
        /// </summary>
        public string PlayButtonImagePath
        {
            get => _playButtonImagePath;
            set => SetProperty(ref _playButtonImagePath, value);
        }

        /// <summary>
        /// Gets or sets the path for the play image when hovered.
        /// </summary>
        public string PlayButtonHoveredImagePath
        {
            get => _playButtonHoveredImagePath;
            set => SetProperty(ref _playButtonHoveredImagePath, value);
        }

        /// <summary>
        /// Gets or sets the path for the play image when clicked.
        /// </summary>
        public string PlayButtonClickedImagePath
        {
            get => _playButtonClickedImagePath;
            set => SetProperty(ref _playButtonClickedImagePath, value);
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
        /// Gets or sets the path for the pause image when hovered.
        /// </summary>
        public string PauseButtonHoveredImagePath
        {
            get => _pauseButtonHoveredImagePath;
            set => SetProperty(ref _pauseButtonHoveredImagePath, value);
        }

        /// <summary>
        /// Gets or sets the path for the pause image when clicked.
        /// </summary>
        public string PauseButtonClickedImagePath
        {
            get => _pauseButtonClickedImagePath;
            set => SetProperty(ref _pauseButtonClickedImagePath, value);
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
        /// Gets or sets the background color in a normal state.
        /// </summary>
        public Color DefaultBackgroundColor
        {
            get => _defaultBackgroundColor;
            set => SetProperty(ref _defaultBackgroundColor, value);
        }

        /// <summary>
        /// Gets or sets the background color in a hovered state.
        /// </summary>
        public Color HoveredBackgroundColor
        {
            get => _hoveredBackgroundColor;
            set => SetProperty(ref _hoveredBackgroundColor, value);
        }

        /// <summary>
        /// Gets or sets the background color in a clicked state.
        /// </summary>
        public Color ClickedBackgroundColor
        {
            get => _clickedBackgroundColor;
            set => SetProperty(ref _clickedBackgroundColor, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="ButtonContentType"/> for the play button.
        /// </summary>
        public ButtonContentType PlayButtonContentType
        {
            get => _playButtonContentType;
            set => SetProperty(ref _playButtonContentType, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="ButtonContentType"/> for the pause button.
        /// </summary>
        public ButtonContentType PauseButtonContentType
        {
            get => _pauseButtonContentType;
            set => SetProperty(ref _pauseButtonContentType, value);
        }

        /// <summary>
        /// Gets or sets the font family for the play button.
        /// </summary>
        public string PlayButtonTextFontFamily
        {
            get => _playButtonTextFontFamily;
            set => SetProperty(ref _playButtonTextFontFamily, value);
        }

        /// <summary>
        /// Gets or sets the font family for the pause button.
        /// </summary>
        public string PauseButtonTextFontFamily
        {
            get => _pauseButtonTextFontFamily;
            set => SetProperty(ref _pauseButtonTextFontFamily, value);
        }

        /// <summary>
        /// Gets or sets the text for the play button.
        /// </summary>
        public string PlayButtonText
        {
            get => _playButtonText;
            set => SetProperty(ref _playButtonText, value);
        }

        /// <summary>
        /// Gets or sets the text for the pause button.
        /// </summary>
        public string PauseButtonText
        {
            get => _pauseButtonText;
            set => SetProperty(ref _pauseButtonText, value);
        }
    }
}
