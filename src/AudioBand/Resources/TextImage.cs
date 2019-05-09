using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AudioBand.Extensions;

namespace AudioBand.Resources
{
    /// <summary>
    /// An image that contains text.
    /// </summary>
    public class TextImage : IImage
    {
        private const float FontSize = 72;
        private readonly string _text;
        private readonly string _fontFamily;
        private readonly Color _color;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextImage"/> class.
        /// </summary>
        /// <param name="text">The text to use.</param>
        /// <param name="fontFamily">The font family to use.</param>
        /// <param name="color">The color of the text.</param>
        public TextImage(string text, string fontFamily, Color color)
        {
            _text = text;
            _fontFamily = fontFamily;
            _color = color;

            DesiredSize = TextRenderer.MeasureText(text, new Font(fontFamily, FontSize)); // Get an initial measurement
        }

        /// <inheritdoc />
        public Size DesiredSize { get; }

        /// <inheritdoc />
        public Image Draw(int width, int height)
        {
            if (string.IsNullOrEmpty(_text) || width < 1 || height < 1)
            {
                return new Bitmap(1, 1);
            }

            using (var bitmap = new Bitmap(DesiredSize.Width, DesiredSize.Height, PixelFormat.Format32bppArgb))
            using (var g = Graphics.FromImage(bitmap))
            using (var font = new Font(_fontFamily, FontSize))
            {
                // Draw large font and scale it down
                var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                TextRenderer.DrawText(g, _text, font, new Rectangle(0, 0, bitmap.Width, bitmap.Height), _color, flags);
                return bitmap.Scale(width, height);
            }
        }
    }
}
