using System.Drawing;
using Svg;

namespace AudioBand.Resources
{
    /// <summary>
    /// Loads audioband resources
    /// </summary>
    public interface IResourceLoader
    {
        /// <summary>
        /// Load svg from a resource.
        /// </summary>
        /// <param name="resource">The svg resource.</param>
        /// <returns>The svg document representing the resource.</returns>
        SvgDocument LoadSVGFromResource(byte[] resource);

        /// <summary>
        /// Tries to load an image from a path or returns the fallback image if it fails.
        /// </summary>
        /// <param name="path">The file path of the image.</param>
        /// <param name="fallbackImage">The fallback image if unable to load.s</param>
        /// <returns>The image from the path or the fallback image.</returns>
        Image TryLoadImageFromPath(string path, Image fallbackImage);
    }
}
