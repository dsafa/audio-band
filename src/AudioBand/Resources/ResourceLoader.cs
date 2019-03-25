using System;
using System.Drawing;
using System.IO;
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
                return string.IsNullOrEmpty(path) ? fallbackImage : Image.FromFile(path);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error loading image from {path}", path);
                return fallbackImage;
            }
        }
    }
}
