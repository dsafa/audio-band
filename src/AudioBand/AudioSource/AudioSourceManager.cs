using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using Nett;
using NLog;
using ServiceContracts;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Detects and loads audio sources.
    /// </summary>
    internal class AudioSourceManager
    {
        private const string PluginFolderName = "AudioSources";
        private const string ManifestFileName = "AudioSource.manifest";
        private static readonly string HostExePath = Path.Combine(DirectoryHelper.BaseDirectory, "AudioSourceHost.exe");
        private static readonly string PluginFolderPath = Path.Combine(DirectoryHelper.BaseDirectory, PluginFolderName);
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private List<IAudioSource> _audioSources = new List<IAudioSource>();
        private ServiceHost _loggerServiceHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceManager"/> class.
        /// </summary>
        public AudioSourceManager()
        {
            _loggerServiceHost = new ServiceHost(typeof(AudioSourceLoggerService));
            _loggerServiceHost.AddServiceEndpoint(typeof(ILoggerService), new NetNamedPipeBinding(), ServiceHelper.LoggerEndpoint);
            _loggerServiceHost.Open();
        }

        /// <summary>
        /// Occurs when the detected audio sources have changed.
        /// </summary>
        public event EventHandler AudioSourcesChanged;

        /// <summary>
        /// Gets the list of audio sources available.
        /// </summary>
        public IEnumerable<IAudioSource> AudioSources => _audioSources;

        /// <summary>
        /// Load all audio sources.
        /// </summary>
        public void LoadAudioSources()
        {
            Logger.Debug("Loading audio sources");
            foreach (var directory in Directory.EnumerateDirectories(PluginFolderPath))
            {
                Logger.Debug($"Starting host at {directory}");
                StartHost(directory);
            }
        }

        private void StartHost(string directory)
        {
            var listenerEndpoint = ServiceHelper.GetAudioSourceListenerEndpoint(directory);
            _audioSources.Add(new AudioSourceProxy(listenerEndpoint));

            Process.Start(new ProcessStartInfo()
            {
                FileName = HostExePath,
                Arguments = $"{directory} {listenerEndpoint}"
            });
        }
    }
}
