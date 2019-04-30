using System.Drawing;
using AudioBand.Extensions;
using Svg;

namespace AudioBand.Resources
{
    /// <summary>
    /// Image class for svgs.
    /// </summary>
    public class SvgImage : IImage
    {
        private readonly SvgDocument _svgDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImage"/> class.
        /// </summary>
        /// <param name="svg">The svg document.</param>
        public SvgImage(SvgDocument svg)
        {
            _svgDocument = svg;
        }

        /// <inheritdoc />
        public Size DesiredSize => new Size((int)_svgDocument.Width.Value, (int)_svgDocument.Height.Value);

        /// <inheritdoc />
        public Image Draw(int width, int height)
        {
            return _svgDocument.ToBitmap(width, height);
        }
    }
}
