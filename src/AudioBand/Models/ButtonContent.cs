using System.Windows.Media;

namespace AudioBand.Models
{
    /// <summary>
    /// Model for button content, including image or text settings.
    /// </summary>
    public class ButtonContent : ModelBase
    {
        private ButtonContentType _contentType = ButtonContentType.Text;
        private string _imagePath;
        private string _hoveredImagePath;
        private string _clickedImagePath;
        private string _fontFamily = "Segoe MDL2 Assets";
        private string _text;
        private Color _textColor = Colors.White;
        private Color _hoveredTextColor = Colors.White;
        private Color _clickedTextColor = Colors.White;

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public ButtonContentType ContentType
        {
            get => _contentType;
            set => SetProperty(ref _contentType, value);
        }

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        public string ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
        }

        /// <summary>
        /// Gets or sets the hovered image path.
        /// </summary>
        public string HoveredImagePath
        {
            get => _hoveredImagePath;
            set => SetProperty(ref _hoveredImagePath, value);
        }

        /// <summary>
        /// Gets or sets the clicked image path.
        /// </summary>
        public string ClickedImagePath
        {
            get => _clickedImagePath;
            set => SetProperty(ref _clickedImagePath, value);
        }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        public string FontFamily
        {
            get => _fontFamily;
            set => SetProperty(ref _fontFamily, value);
        }

        /// <summary>
        /// Gets or sets the text.
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
        public Color HoveredTextColor
        {
            get => _hoveredTextColor;
            set => SetProperty(ref _hoveredTextColor, value);
        }

        /// <summary>
        /// Gets or sets the text color when clicked.
        /// </summary>
        public Color ClickedTextColor
        {
            get => _clickedTextColor;
            set => SetProperty(ref _clickedTextColor, value);
        }
    }
}
