using System.Drawing;

namespace AudioBand.Resources
{
    /// <summary>
    /// Interface to wraps images.
    /// </summary>
    public interface IImage
    {
        /// <summary>
        /// Gets the desired size.
        /// </summary>
        Size DesiredSize { get; }

        /// <summary>
        /// Gets an image instance.
        /// </summary>
        /// <param name="width">The target width.</param>
        /// <param name="height">The target height.</param>
        /// <returns>An image.</returns>
        Image Draw(int width, int height);
    }
}
