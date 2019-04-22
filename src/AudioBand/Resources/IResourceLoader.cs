namespace AudioBand.Resources
{
    /// <summary>
    /// Loads audioband resources
    /// </summary>
    public interface IResourceLoader
    {
        /// <summary>
        /// Gets the default play image.
        /// </summary>
        IImage DefaultPlayImage { get; }

        /// <summary>
        /// Gets the default pause image.
        /// </summary>
        IImage DefaultPauseImage { get; }

        /// <summary>
        /// Gets the default placeholder album image.
        /// </summary>
        IImage DefaultPlaceholderAlbumImage { get; }

        /// <summary>
        /// Gets the default next image.
        /// </summary>
        IImage DefaultNextImage { get; }

        /// <summary>
        /// Gets the default previous image.
        /// </summary>
        IImage DefaultPreviousImage { get; }

        /// <summary>
        /// Tries to load an image from a path or returns the fallback image if it fails.
        /// </summary>
        /// <param name="path">The file path of the image.</param>
        /// <param name="fallbackImage">The fallback image if unable to load.s</param>
        /// <returns>The image from the path or the fallback image.</returns>
        IImage TryLoadImageFromPath(string path, IImage fallbackImage);
    }
}
