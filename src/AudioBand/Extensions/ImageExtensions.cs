using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AudioBand.Extensions
{
    /// <summary>
    /// Extension methods for images.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Converts a system drawing image to an image source.
        /// </summary>
        /// <param name="image">The image to convert.</param>
        /// <returns>An image source.</returns>
        public static ImageSource ToImageSource(this System.Drawing.Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}
