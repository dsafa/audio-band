using System;
using System.Drawing;
using System.IO;
using AudioBand.Logging;
using NLog;
using Svg;

namespace AudioBand.Resources
{
    /// <summary>
    /// Default resource loader.
    /// </summary>
    public class ResourceLoader : IResourceLoader
    {
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<ResourceLoader>();
        private static readonly IImage DefaultPlay = LoadSvgFromResource(Properties.Resources.play);
        private static readonly IImage DefaultPause = LoadSvgFromResource(Properties.Resources.pause);
        private static readonly IImage DefaultPlaceholder = LoadSvgFromResource(Properties.Resources.placeholder_album);
        private static readonly IImage DefaultNext = LoadSvgFromResource(Properties.Resources.next);
        private static readonly IImage DefaultPrevious = LoadSvgFromResource(Properties.Resources.previous);

        /// <inheritdoc />
        public IImage DefaultPlayImage => DefaultPlay;

        /// <inheritdoc />
        public IImage DefaultPauseImage => DefaultPause;

        /// <inheritdoc />
        public IImage DefaultPlaceholderAlbumImage => DefaultPlaceholder;

        /// <inheritdoc />
        public IImage DefaultNextImage => DefaultNext;

        /// <inheritdoc />
        public IImage DefaultPreviousImage => DefaultPrevious;

        /// <inheritdoc/>
        public IImage TryLoadImageFromPath(string path, IImage fallbackImage)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return fallbackImage;
                }

                if (path.EndsWith(".svg"))
                {
                    return new SvgImage(SvgDocument.Open(path));
                }

                return new DrawingImage(Image.FromFile(path));
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error loading image from {path}", path);
                return fallbackImage;
            }
        }

        private static IImage LoadSvgFromResource(byte[] resource)
        {
            using (var ms = new MemoryStream(resource))
            {
                return new SvgImage(SvgDocument.Open<SvgDocument>(ms));
            }
        }
    }
}
