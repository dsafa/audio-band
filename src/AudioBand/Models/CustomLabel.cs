using System.Windows.Media;

namespace AudioBand.Models
{
    /// <summary>
    /// Model for a custom label.
    /// </summary>
    public class CustomLabel : LayoutModelBase
    {
        private string _fontFamily = "Segoe UI";
        private float _fontSize = 8.5f;
        private Color _color = Colors.White;
        private string _formatString = "{artist} - {song}";
        private TextAlignment _alignment = TextAlignment.Center;
        private string _name = "Now Playing";
        private int _scrollSpeed = 5000;
        private TextOverflow _textOverflow = TextOverflow.Scroll;
        private ScrollBehavior _scrollBehavior = ScrollBehavior.Always;
        private TextFadeEffect _textFadeEffect = TextFadeEffect.OnlyWhenScrolling;
        private double _leftFadeOffset = 0.1;
        private double _rightFadeOffset = 0.9;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLabel"/> class.
        /// </summary>
        public CustomLabel()
        {
            Width = 220;
            Height = 15;
            YPosition = 0;
            XPosition = 0;
        }

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
            Center,
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
        /// Gets or sets the number to milliseconds to scroll across.
        /// </summary>
        public int ScrollSpeed
        {
            get => _scrollSpeed;
            set => SetProperty(ref _scrollSpeed, value);
        }

        /// <summary>
        /// Gets or sets the text overflow behavior.
        /// </summary>
        public TextOverflow TextOverflow
        {
            get => _textOverflow;
            set => SetProperty(ref _textOverflow, value);
        }

        /// <summary>
        /// Gets or sets the scroll behavior.
        /// </summary>
        public ScrollBehavior ScrollBehavior
        {
            get => _scrollBehavior;
            set => SetProperty(ref _scrollBehavior, value);
        }

        /// <summary>
        /// Gets or sets the fade effect.
        /// </summary>
        public TextFadeEffect FadeEffect
        {
            get => _textFadeEffect;
            set => SetProperty(ref _textFadeEffect, value);
        }

        /// <summary>
        /// Gets or sets the fade offset for the left side.
        /// </summary>
        public double LeftFadeOffset
        {
            get => _leftFadeOffset;
            set => SetProperty(ref _leftFadeOffset, value);
        }

        /// <summary>
        /// Gets or sets the fade offset for the right side.
        /// </summary>
        public double RightFadeOffset
        {
            get => _rightFadeOffset;
            set => SetProperty(ref _rightFadeOffset, value);
        }
    }
}
