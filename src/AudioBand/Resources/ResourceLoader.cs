using System;
using System.Drawing;
using System.IO;
using AudioBand.Extensions;
using AudioBand.Logging;
using NLog;
using Svg;

namespace AudioBand.Resources
{
    /// <summary>
    /// Default resource loader
    /// </summary>
    public class ResourceLoader : IResourceLoader
    {
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<ResourceLoader>();

        /// <inheritdoc/>
        public SvgDocument LoadSVGFromResource(byte[] resource)
        {
            return SvgDocument.Open<SvgDocument>(new MemoryStream(resource));
        }

        /// <inheritdoc/>
        public Image TryLoadImageFromPath(string path, Image fallbackImage)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return fallbackImage;
                }

                if (path.EndsWith(".svg"))
                {
                    return SvgDocument.Open(path).ToBitmap();
                }

                return Image.FromFile(path);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error loading image from {path}", path);
                return fallbackImage;
            }
        }
    }
}
