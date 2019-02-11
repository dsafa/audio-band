using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Detects and loads audio sources.
    /// </summary>
    internal class AudioSourceManager
    {
        private const string PluginFolderName = "AudioSources";
        private static readonly string PluginFolderPath = Path.Combine(DirectoryHelper.BaseDirectory, PluginFolderName);
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static readonly object _audioSourcesLock = new object();
        private readonly List<AudioSourceProxy> _uninitializedProxies = new List<AudioSourceProxy>();

        /// <summary>
        /// Gets the list of audio sources available.
        /// </summary>
        public ObservableCollection<IInternalAudioSource> AudioSources { get; private set; } = new ObservableCollection<IInternalAudioSource>();

        /// <summary>
        /// Load all audio sources.
        /// </summary>
        public void LoadAudioSources()
        {
            Logger.Debug("Loading audio sources");
            foreach (var dir in Directory.EnumerateDirectories(PluginFolderPath))
            {
                try
                {
                    var proxy = new AudioSourceProxy(dir);
                    AudioSources.Add(proxy);
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Error creating proxy for {dir}");
                }
            }
        }

        /// <summary>
        /// Close all services.
        /// </summary>
        public void Close()
        {
            foreach (var proxy in AudioSources.Cast<AudioSourceProxy>())
            {
                proxy.Close();
            }
        }
    }
}
