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
    internal class AudioSourceManager : IAudioSourceServer
    {
        private const string PluginFolderName = "AudioSources";
        private const string ManifestFileName = "AudioSource.manifest";
        private static readonly string HostExePath = Path.Combine(DirectoryHelper.BaseDirectory, "AudioSourceHost.exe");
        private static readonly string PluginFolderPath = Path.Combine(DirectoryHelper.BaseDirectory, PluginFolderName);
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private Dictionary<string, AudioSourceProxy> _audioSources = new Dictionary<string, AudioSourceProxy>();
        private ServiceHost _serviceHost;

        public AudioSourceManager()
        {
            _serviceHost = new ServiceHost(this);
            _serviceHost.AddServiceEndpoint(typeof(IAudioSourceServer), new NetNamedPipeBinding(), ServiceHelper.AudioSourceServerEndpoint);
            _serviceHost.Open();
        }

        /// <summary>
        /// Occurs when the detected audio sources have changed.
        /// </summary>
        public event EventHandler AudioSourcesChanged;

        /// <summary>
        /// Gets the list of audio sources available.
        /// </summary>
        public IEnumerable<IAudioSource> AudioSources => _audioSources.Values;

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
            Process.Start(new ProcessStartInfo()
            {
                FileName = HostExePath,
                Arguments = directory
            });
        }

        public Uri RegisterAudioSource(string name, Uri hostUri)
        {
            var listenerUri = ServiceHelper.GetAudioSourceListenerEndpoint(name);
            if (_audioSources.ContainsKey(name))
            {
                return listenerUri;
            }

            _audioSources[name] = new AudioSourceProxy(listenerUri, hostUri);
            AudioSourcesChanged?.Invoke(this, EventArgs.Empty);
            return listenerUri;
        }
    }
}
