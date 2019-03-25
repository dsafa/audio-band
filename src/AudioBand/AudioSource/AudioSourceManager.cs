using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using AudioBand.Logging;
using NLog;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Detects and loads audio sources.
    /// </summary>
    public class AudioSourceManager : IAudioSourceManager
    {
        private const string PluginFolderName = "AudioSources";
        private static readonly string PluginFolderPath = Path.Combine(DirectoryHelper.BaseDirectory, PluginFolderName);
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<AudioSourceManager>();
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
            Logger.Debug("Loading audio sources as {path}", PluginFolderPath);
            foreach (var dir in Directory.EnumerateDirectories(PluginFolderPath))
            {
                try
                {
                    var proxy = new AudioSourceProxy(dir);
                    AudioSources.Add(proxy);
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Error creating proxy for audiosource in {path}", dir);
                }
            }
        }
    }
}
