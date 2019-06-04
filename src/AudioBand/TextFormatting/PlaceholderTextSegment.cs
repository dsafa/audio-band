using System;
using System.Windows.Media;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A <see cref="TextSegment"/> that supports values provided by a <see cref="TextPlaceholder"/>.
    /// </summary>
    public class PlaceholderTextSegment : TextSegment
    {
        private readonly TextPlaceholder _textPlaceholder;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceholderTextSegment"/> class.
        /// </summary>
        /// <param name="textPlaceholder">The text placeholder to use.</param>
        /// <param name="flags">The text flags.</param>
        /// <param name="color">The text color.</param>
        public PlaceholderTextSegment(TextPlaceholder textPlaceholder, FormattedTextFlags flags, Color color)
        {
            Flags = flags;
            Color = color;
            Text = textPlaceholder.GetText();

            textPlaceholder.TextChanged += TextPlaceholderTextChanged;
            _textPlaceholder = textPlaceholder;
        }

        private void TextPlaceholderTextChanged(object sender, EventArgs e)
        {
            Text = _textPlaceholder.GetText();
        }
    }
}
