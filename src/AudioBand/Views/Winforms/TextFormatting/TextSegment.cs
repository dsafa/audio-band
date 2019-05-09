using System.Drawing;

namespace AudioBand.Views.Winforms.TextFormatting
{
    /// <summary>
    /// A chunk of text that has its own custom rendering.
    /// </summary>
    public class TextSegment
    {
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
        public string Text { get; set; }

        /// <summary>
        /// Gets the text type.
        /// </summary>
        public FormattedTextFlags Type { get; }

        /// <summary>
        /// Gets or sets the chunk color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Draws the chunk onto the graphics.
        /// </summary>
        /// <param name="g">The graphics to draw on.</param>
        /// <param name="font">The font.</param>
        /// <param name="x">The x position of the text.</param>
        /// <param name="y">The y position of the text.</param>
        public void Draw(Graphics g, Font font, float x, float y)
        {
            using (var brush = new SolidBrush(Color))
            {
                g.DrawString(Text, font, brush, x, y, StringFormat.GenericTypographic);
            }
        }
    }
}
