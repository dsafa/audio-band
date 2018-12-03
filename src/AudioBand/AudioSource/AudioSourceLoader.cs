using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using Nett;
using NLog;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Detects and loads audio sources.
    /// </summary>
    internal class AudioSourceLoader
    {
        private const string PluginFolderName = "AudioSources";
        private const string ManifestFileName = "AudioSource.manifest";
        private static readonly string PluginFolderPath = Path.Combine(DirectoryHelper.BaseDirectory, PluginFolderName);
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

#pragma warning disable CS0067 // The event 'AudioSourceLoader.AudioSourcesChanged' is never used
        /// <summary>
        /// Occurs when the detected audio sources have changed.
        /// </summary>
        public event EventHandler AudioSourcesChanged;
#pragma warning restore CS0067 // The event 'AudioSourceLoader.AudioSourcesChanged' is never used

        /// <summary>
        /// Gets the list of audio sources available.
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<IAudioSource> AudioSources { get; private set; } = new List<IAudioSource>();

        /// <summary>
        /// Load all audio sources.
        /// </summary>
        public void LoadAudioSources()
        {
            Logger.Debug($"Searching for audio sources in path `{PluginFolderPath}`");

            var catalog = new AggregateCatalog();
            var directories = Directory.EnumerateDirectories(PluginFolderPath, "*", SearchOption.TopDirectoryOnly);

            foreach (var directory in directories)
            {
                Logger.Debug($"Searching in {directory} for {ManifestFileName}");

                var manifest = Directory.GetFiles(directory, ManifestFileName, SearchOption.TopDirectoryOnly).FirstOrDefault();
                if (manifest == null)
                {
                    continue;
                }

                var manifestData = Toml.ReadFile<Manifest>(manifest);
                Logger.Debug($"Reading {manifest}. Audio sources: [{string.Join(",", manifestData.AudioSources)}]");

                foreach (var audioSourceFileName in manifestData.AudioSources)
                {
                    var audioSourcePath = Path.Combine(directory, audioSourceFileName);
                    catalog.Catalogs.Add(new AssemblyCatalog(audioSourcePath));
                }
            }

            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);

            foreach (var audioSource in AudioSources)
            {
                Logger.Debug($"Loaded audio source `{audioSource.Name}`");
                audioSource.Logger = new AudioSourceLogger(audioSource.Name);
            }
        }

        private class Manifest
        {
            public List<string> AudioSources { get; set; }
        }
    }
}
