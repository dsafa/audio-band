using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioBand.Views.Winforms.TextFormatting
{
    /// <summary>
    /// A chunk of text that has its own custom rendering.
    /// </summary>
    public class TextChunk
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextChunk"/> class
        /// with text, color and type.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="type">The text type.</param>
        /// <param name="color">The text color.</param>
        public TextChunk(string text, FormattedTextFlags type, Color color)
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
        public void Draw(Graphics g, Font font, int x, int y)
        {
            TextRenderer.DrawText(g, Text, font, new Point(x, y), Color, TextFormatFlags.NoPrefix);
        }
    }
}
