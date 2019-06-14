using System.Windows.Media;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A segment of text to be rendered in a text label.
    /// </summary>
    public abstract class TextSegment : ObservableObject
    {
        private string _text;
        private Color _color;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get => _text;
            protected set => SetProperty(ref _text, value);
        }

        /// <summary>
        /// Gets or sets the text segment flags.
        /// </summary>
        public FormattedTextFlags Flags { get; protected set; }

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
