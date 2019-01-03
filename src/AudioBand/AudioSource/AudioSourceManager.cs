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
        private List<IAudioSource> _waitingAudioSource = new List<IAudioSource>();
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
            var proxy = new AudioSourceProxy(listenerEndpoint);
            proxy.Ready += OnAudioSourceReady;
            proxy.Faulted += OnAudioSourceFaulted;
            _waitingAudioSource.Add(proxy);

            Process.Start(new ProcessStartInfo()
            {
                FileName = HostExePath,
                Arguments = $"{directory} {listenerEndpoint}"
            });
        }

        private void OnAudioSourceFaulted(object sender, EventArgs e)
        {
            var proxy = _audioSources.Find(s => s == sender);
            if (proxy == null)
            {
                Logger.Warn("Faulted audio source not found");
                return;
            }

            _audioSources.Remove(proxy);
            AudioSourcesChanged?.Invoke(this, EventArgs.Empty);
            // TODO recreate
        }

        private void OnAudioSourceReady(object sender, EventArgs e)
        {
            var proxy = _waitingAudioSource.Find(audioSource => audioSource == sender);
            if (proxy == null)
            {
                Logger.Warn("Audio source ready but not found in waiting list");
                return;
            }

            _waitingAudioSource.Remove(proxy);
            _audioSources.Add(proxy);
            AudioSourcesChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
