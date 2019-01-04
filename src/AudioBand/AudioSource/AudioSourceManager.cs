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
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    internal class AudioSourceManager : IAudioSourceServer
    {
        private const string PluginFolderName = "AudioSources";
        private const string ManifestFileName = "AudioSource.manifest";
        private static readonly string HostExePath = Path.Combine(DirectoryHelper.BaseDirectory, "AudioSourceHost.exe");
        private static readonly string PluginFolderPath = Path.Combine(DirectoryHelper.BaseDirectory, PluginFolderName);
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static readonly object _audioSourcesLock = new object();
        private Dictionary<Uri, AudioSourceProxy> _audioSources = new Dictionary<Uri, AudioSourceProxy>();
        private List<Process> _hostProcesses = new List<Process>();
        private ServiceHost _audioSourceServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceManager"/> class.
        /// </summary>
        public AudioSourceManager()
        {
            _audioSourceServer = new ServiceHost(this);
            _audioSourceServer.AddServiceEndpoint(typeof(IAudioSourceServer), new NetNamedPipeBinding(), ServiceHelper.AudioSourceServerEndpoint);
            _audioSourceServer.Open();
        }

        /// <summary>
        /// Occurs when the detected audio sources have changed.
        /// </summary>
        public event EventHandler AudioSourcesChanged;

        /// <summary>
        /// Gets the list of audio sources available.
        /// </summary>
        public IEnumerable<IAudioSource> AudioSources
        {
            get
            {
                lock (_audioSourcesLock)
                {
                    return new List<IAudioSource>(_audioSources.Values);
                }
            }
        }

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

        /// <summary>
        /// Close all services.
        /// </summary>
        public void Close()
        {
            foreach (var process in _hostProcesses)
            {
                process.Close();
            }

            foreach (var audioSourceProxy in _audioSources.Values)
            {
                audioSourceProxy.Close();
            }
        }

        private void StartHost(string directory)
        {
            Logger.Debug($"Starting audiosource host at {directory}");

            var process = Process.Start(new ProcessStartInfo()
            {
                FileName = HostExePath,
                Arguments = directory
            });
        }

        public bool RegisterHost(Uri hostServiceUri)
        {
            lock (_audioSourcesLock)
            {
                if (_audioSources.ContainsKey(hostServiceUri))
                {
                    Logger.Warn($"Trying to register host at {hostServiceUri} but already exists");
                    return false;
                }

                var proxy = new AudioSourceProxy(hostServiceUri);
                proxy.Errored += ProxyOnErrored;
                _audioSources[hostServiceUri] = proxy;

                Logger.Debug($"Created new audio source proxy for host at {hostServiceUri}");
            }

            AudioSourcesChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }

        private void ProxyOnErrored(object sender, EventArgs e)
        {
            var proxy = sender as AudioSourceProxy;
            _audioSources.Remove(proxy.Uri);
        }
    }
}
