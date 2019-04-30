using System.Drawing;
using System.Drawing.Drawing2D;
using AudioBand.Resources;

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
            var newImage = new Bitmap(width, height, image.PixelFormat);
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
            var size = Scale(image.Size, new Size(targetWidth, targetHeight));
            return Resize(image, size.Width, size.Height);
        }

        /// <summary>
        /// Scale an image to fill as much space as possible while maintaining aspect ratio.
        /// </summary>
        /// <param name="image">The image to scale.</param>
        /// <param name="targetWidth">The target width.</param>
        /// <param name="targetHeight">The target height.</param>
        /// <returns>The scaled image.</returns>
        public static Image GetScaledSize(this IImage image, int targetWidth, int targetHeight)
        {
            var size = Scale(image.DesiredSize, new Size(targetWidth, targetHeight));
            return image.Draw(size.Width, size.Height);
        }

        private static Size Scale(Size sourceSize, Size targetSize)
        {
            var ratioW = (float)sourceSize.Width / targetSize.Width;
            var ratioH = (float)sourceSize.Height / targetSize.Height;

            if (ratioW > ratioH)
            {
                // The width is bigger so the new width will become the target's width and the height will be scaled with the ratio between the original width and the target width
                return new Size(targetSize.Width, (int)(sourceSize.Height / ratioW));
            }

            return new Size((int)(sourceSize.Width / ratioH), targetSize.Height);
        }
    }
}
