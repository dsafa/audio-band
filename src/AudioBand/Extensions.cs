using Svg;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AudioBand
{
    internal static class Extensions
    {
        public static Bitmap ToBitmap(this SvgDocument svg)
        {
            var bmp = new Bitmap((int)svg.Width.Value, (int)svg.Height.Value);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.High;
                svg.Draw(graphics);
                return bmp;
            }
        }
    }
}
