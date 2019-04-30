using System.Drawing;
using AudioBand.Extensions;

namespace AudioBand.Resources
{
    /// <summary>
    /// Image class for system.drawing images.
    /// </summary>
    public class DrawingImage : IImage
    {
        private readonly Image _internalImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingImage"/> class.
        /// </summary>
        /// <param name="image">The image to use.</param>
        public DrawingImage(Image image)
        {
            _internalImage = image;
        }

        /// <inheritdoc />
        public Size DesiredSize => _internalImage.Size;

        /// <inheritdoc />
        public Image Draw(int width, int height)
        {
            return _internalImage.Scale(width, height);
        }
    }
}
