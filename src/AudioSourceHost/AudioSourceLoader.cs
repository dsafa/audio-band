using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using AudioBand.AudioSource;
using Nett;

namespace AudioSourceHost
{
    /// <summary>
    /// Detects and loads audio sources.
    /// </summary>
    internal class AudioSourceLoader
    {
        private const string ManifestFileName = "AudioSource.manifest";

        /// <summary>
        /// Load audio source from a directory.
        /// </summary>
        /// <param name="directory">The directory that contains the audio source.</param>
        /// <returns>A list of audiosources found in the directory.</returns>
        public static IAudioSource LoadFromDirectory(string directory)
        {
            var manifestPath = Directory.GetFiles(directory, ManifestFileName, SearchOption.TopDirectoryOnly).FirstOrDefault();
            if (manifestPath == null)
            {
                throw new ApplicationException($"No manifest found in {directory}");
            }

            var manifestData = Toml.ReadFile<Manifest>(manifestPath);
            if (manifestData?.AudioSource == null)
            {
                throw new ApplicationException("Invalid manifest format");
            }

            var audioSourcePath = Path.Combine(directory, manifestData.AudioSource);

            var container = new CompositionContainer(new AssemblyCatalog(audioSourcePath));
            return container.GetExportedValue<IAudioSource>();
        }

        private class Manifest
        {
            public string AudioSource { get; set; }
        }
    }
}
