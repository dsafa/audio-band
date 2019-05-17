using System.Windows.Media;
using AudioBand.Models;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A chunk of text that has its own custom rendering.
    /// </summary>
    public class TextSegment : ModelBase
    {
        private string _text;
        private Color _color;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextSegment"/> class
        /// with text, color and type.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="type">The text type.</param>
        /// <param name="color">The text color.</param>
        public TextSegment(string text, FormattedTextFlags type, Color color)
        {
            Text = text;
            Type = type;
            Color = color;
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
        /// Gets the text type.
        /// </summary>
        public FormattedTextFlags Type { get; }

        /// <summary>
        /// Gets or sets the chunk color.
        /// </summary>
        public Color Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }
    }
}
