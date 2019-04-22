using System.Drawing;
using System.Drawing.Drawing2D;

namespace AudioBand.Extensions
{
    /// <summary>
    /// Extension for <see cref="Image"/>.
    /// </summary>
    internal static class ImageExtensions
    {
        /// <summary>
        /// Resize an image.
        /// </summary>
        /// <param name="image">Image to resize.</param>
        /// <param name="width">Width of the new image.</param>
        /// <param name="height">Height of the new image.</param>
        /// <returns>A copy of the resized image.</returns>
        public static Image Resize(this Image image, int width, int height)
        {
            // Padding issues
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

        /// <summary>
        /// Scale an image to fill as much space as possible while maintaining aspect ratio.
        /// </summary>
        /// <param name="image">Image to scale.</param>
        /// <param name="targetWidth">Target width to scale to.</param>
        /// <param name="targetHeight">Target height to scale to.</param>
        /// <returns>A copy of the scaled image.</returns>
        public static Image Scale(this Image image, int targetWidth, int targetHeight)
        {
            var ratiow = (float)image.Width / targetWidth;
            var ratioh = (float)image.Height / targetHeight;

            if (ratiow > ratioh)
            {
                // The width is bigger so the new width will become the target's width and the height will be scaled with the ratio between the original width and the target width
                return Resize(image, targetWidth, (int)(image.Height / ratiow));
            }
            else
            {
                return Resize(image, (int)(image.Width / ratioh), targetHeight);
            }
        }
    }
}
