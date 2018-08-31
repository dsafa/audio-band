using Svg;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AudioBand
{
    internal static class Extensions
    {
        public static Bitmap ToBitmap(this SvgDocument svg)
        {
            return ToBitmap(svg, (int)svg.Width.Value, (int)svg.Height.Value);
        }

        public static Bitmap ToBitmap(this SvgDocument svg, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.High;
                svg.Draw(graphics);
                return bmp;
            }
        }

        public static Image Resize(this Image image, int width, int height)
        {
            var rect = new Rectangle(0, 0, width, height);
            var newImage = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(image, rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            }

            return newImage;
        }
    }
}
