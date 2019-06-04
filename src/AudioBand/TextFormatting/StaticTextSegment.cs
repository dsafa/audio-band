using System.Windows.Media;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A <see cref="TextSegment"/> that has a static value.
    /// </summary>
    public class StaticTextSegment : TextSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticTextSegment"/> class
        /// with text, color and type.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="flags">The text type.</param>
        /// <param name="color">The text color.</param>
        public StaticTextSegment(string text, FormattedTextFlags flags, Color color)
        {
            Text = text;
            Flags = flags;
            Color = color;
        }
    }
}
