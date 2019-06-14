using System.Windows.Media;

namespace AudioBand.Models
{
    /// <summary>
    /// Model for button content, including image or text settings.
    /// </summary>
    public class ButtonContent
    {
        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public ButtonContentType ContentType { get; set; } = ButtonContentType.Text;

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets the hovered image path.
        /// </summary>
        public string HoveredImagePath { get; set; }

        /// <summary>
        /// Gets or sets the clicked image path.
        /// </summary>
        public string ClickedImagePath { get; set; }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        public string FontFamily { get; set; } = "Segoe MDL2 Assets";

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        public Color TextColor { get; set; } = Colors.White;

        /// <summary>
        /// Gets or sets the text color when hovered.
        /// </summary>
        public Color HoveredTextColor { get; set; } = Colors.White;

        /// <summary>
        /// Gets or sets the text color when clicked.
        /// </summary>
        public Color ClickedTextColor { get; set; } = Colors.White;
    }
}
