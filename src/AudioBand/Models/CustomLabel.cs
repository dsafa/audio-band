using System.Drawing;

namespace AudioBand.Models
{
    /// <summary>
    /// Model for a custom label.
    /// </summary>
    public class CustomLabel : ModelBase
    {
        private bool _isVisible = true;
        private int _width = 220;
        private int _height = 15;
        private int _xPosition = 30;
        private int _yPosition = 0;
        private string _fontFamily = "Segoe UI";
        private float _fontSize = 8.5f;
        private Color _color = Color.White;
        private string _formatString = "{artist} - {song}";
        private TextAlignment _alignment = TextAlignment.Center;
        private string _name = "Now Playing";
        private int _scrollSpeed = 50;

        /// <summary>
        /// Specifies the alignment of the text in the label.
        /// </summary>
        public enum TextAlignment
        {
            /// <summary>
            /// Align the text to the left.
            /// </summary>
            Left,

            /// <summary>
            /// Align the text to the right.
            /// </summary>
            Right,

            /// <summary>
            /// Align the text in the center.
            /// </summary>
            Center
        }

        /// <summary>
        /// Gets or sets a value indicating whether if the label is visible.
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        /// <summary>
        /// Gets or sets the width of the label.
        /// </summary>
        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        /// <summary>
        /// Gets or sets the height of the label.
        /// </summary>
        public int Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        /// <summary>
        /// Gets or sets the x position of the label.
        /// </summary>
        public int XPosition
        {
            get => _xPosition;
            set => SetProperty(ref _xPosition, value);
        }

        /// <summary>
        /// Gets or sets the y position of the label.
        /// </summary>
        public int YPosition
        {
            get => _yPosition;
            set => SetProperty(ref _yPosition, value);
        }

        /// <summary>
        /// Gets or sets the font family of the label.
        /// </summary>
        public string FontFamily
        {
            get => _fontFamily;
            set => SetProperty(ref _fontFamily, value);
        }

        /// <summary>
        /// Gets or sets the font size of the label.
        /// </summary>
        public float FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }

        /// <summary>
        /// Gets or sets the color of the label.
        /// </summary>
        public Color Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        /// <summary>
        /// Gets or sets the format for the label.
        /// </summary>
        public string FormatString
        {
            get => _formatString;
            set => SetProperty(ref _formatString, value);
        }

        /// <summary>
        /// Gets or sets the alignment of the text.
        /// </summary>
        public TextAlignment Alignment
        {
            get => _alignment;
            set => SetProperty(ref _alignment, value);
        }

        /// <summary>
        /// Gets or sets the name of the label.
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Gets or sets the scollspeed of the label.
        /// </summary>
        public int ScrollSpeed
        {
            get => _scrollSpeed;
            set => SetProperty(ref _scrollSpeed, value);
        }
    }
}
