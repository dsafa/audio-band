using System.Windows.Media;

namespace AudioBand.Models
{
    /// <summary>
    /// Model for a custom label.
    /// </summary>
    public class CustomLabel : LayoutModelBase
    {
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
        public string FontFamily { get; set; } = "Segoe UI";

        /// <summary>
        /// Gets or sets the font size of the label.
        /// </summary>
        public float FontSize { get; set; } = 12;

        /// <summary>
        /// Gets or sets the color of the label.
        /// </summary>
        public Color Color { get; set; } = Colors.White;

        /// <summary>
        /// Gets or sets the format for the label.
        /// </summary>
        public string FormatString { get; set; } = "{artist} - {song}";

        /// <summary>
        /// Gets or sets the alignment of the text.
        /// </summary>
        public TextAlignment Alignment { get; set; } = TextAlignment.Center;

        /// <summary>
        /// Gets or sets the name of the label.
        /// </summary>
        public string Name { get; set; } = "Now Playing";

        /// <summary>
        /// Gets or sets the number to milliseconds to scroll across.
        /// </summary>
        public int ScrollSpeed { get; set; } = 5000;

        /// <summary>
        /// Gets or sets the text overflow behavior.
        /// </summary>
        public TextOverflow TextOverflow { get; set; } = TextOverflow.Scroll;

        /// <summary>
        /// Gets or sets the scroll behavior.
        /// </summary>
        public ScrollBehavior ScrollBehavior { get; set; } = ScrollBehavior.Always;

        /// <summary>
        /// Gets or sets the fade effect.
        /// </summary>
        public TextFadeEffect FadeEffect { get; set; } = TextFadeEffect.OnlyWhenScrolling;

        /// <summary>
        /// Gets or sets the fade offset for the left side.
        /// </summary>
        public double LeftFadeOffset { get; set; } = 0.1;

        /// <summary>
        /// Gets or sets the fade offset for the right side.
        /// </summary>
        public double RightFadeOffset { get; set; } = 0.9;
    }
}
