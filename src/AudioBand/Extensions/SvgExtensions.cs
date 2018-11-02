using Svg;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AudioBand.Extensions
{
    internal static class SvgExtensions
    {
        /// <summary>
        /// Convert to a bitmap.
        /// </summary>
        /// <param name="svg">Svg to convert.</param>
        /// <returns>A bitmap representation of the svg</returns>
        public static Bitmap ToBitmap(this SvgDocument svg)
        {
            return ToBitmap(svg, (int)svg.Width.Value, (int)svg.Height.Value);
        }

        /// <summary>
        /// Convert to a bitmap.
        /// </summary>
        /// <param name="svg">Svg to convert.</param>
        /// <param name="width">Width of the converted image.</param>
        /// <param name="height">Height of the converted image.</param>
        /// <returns>A bitmap representation of the svg</returns>
        public static Bitmap ToBitmap(this SvgDocument svg, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                svg.Draw(graphics);
                return bmp;
            }
        }
    }
}
