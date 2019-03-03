using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using AudioBand.AudioSource;
using AudioBand.Logging;
using Nett;
using NLog;

namespace AudioSourceHost
{
    /// <summary>
    /// Detects and loads audio sources.
    /// </summary>
    internal class AudioSourceLoader
    {
        private const string ManifestFileName = "AudioSource.manifest";
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<AudioSourceLoader>();

        /// <summary>
        /// Load audio source from a directory.
        /// </summary>
        /// <param name="directory">The directory that contains the audio source.</param>
        /// <returns>A list of audiosources found in the directory.</returns>
        public static IAudioSource LoadFromDirectory(string directory)
        {
            Logger.Debug($"Probing path `{directory}` for audio source");

            var manifestPath = Directory.GetFiles(directory, ManifestFileName, SearchOption.TopDirectoryOnly).FirstOrDefault();
            if (manifestPath == null)
            {
                throw new FileNotFoundException("No manifest found in {directory}");
            }

            var manifestData = Toml.ReadFile<Manifest>(manifestPath);
            if (manifestData?.AudioSource == null)
            {
                throw new InvalidOperationException("Invalid manifest format");
            }

            var audioSourcePath = Path.Combine(directory, manifestData.AudioSource);
            Logger.Debug($"Loading audio source from path `{audioSourcePath}`");

            var container = new CompositionContainer(new AssemblyCatalog(audioSourcePath));
            return container.GetExportedValue<IAudioSource>();
        }

        private class Manifest
        {
            public string AudioSource { get; set; }
        }
    }
}
