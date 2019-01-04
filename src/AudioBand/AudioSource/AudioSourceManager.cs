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
        private static readonly object _audioSourcesLock = new object();
        private List<AudioSourceProxy> _audioSources = new List<AudioSourceProxy>();
        private List<(AudioSourceProxy proxy, ServiceHost host, Process process)> _audioSourceInfos = new List<(AudioSourceProxy, ServiceHost, Process)>();
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
        public IEnumerable<IAudioSource> AudioSources
        {
            get
            {
                lock (_audioSourcesLock)
                {
                    return new List<IAudioSource>(_audioSources);
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
            _loggerServiceHost.Close();
            foreach (var info in _audioSourceInfos)
            {
                info.host.Abort();
                info.process.Close();
            }
        }

        private void StartHost(string directory)
        {
            var listenerEndpoint = ServiceHelper.GetAudioSourceListenerEndpoint(new DirectoryInfo(directory).Name);
            var proxy = new AudioSourceProxy(listenerEndpoint);
            proxy.Ready += OnAudioSourceReady;

            var serviceHost = new ServiceHost(proxy);
            var binding = new NetNamedPipeBinding
            {
                CloseTimeout = TimeSpan.FromSeconds(5),
                SendTimeout = TimeSpan.FromSeconds(10),
                ReceiveTimeout = TimeSpan.FromSeconds(10),
            };

            serviceHost.AddServiceEndpoint(typeof(IAudioSourceListener), binding, listenerEndpoint);
            serviceHost.Open();
            serviceHost.Faulted += ServiceHostOnFaulted;

            Logger.Debug($"Starting audiosource host at {directory}, listening on {listenerEndpoint}");

            var process = Process.Start(new ProcessStartInfo()
            {
                FileName = HostExePath,
                Arguments = $"{directory} {listenerEndpoint}"
            });

            _audioSourceInfos.Add((proxy, serviceHost, process));
        }

        private void ServiceHostOnFaulted(object sender, EventArgs e)
        {
            var serviceHost = sender as ServiceHost;
            var proxy = serviceHost.SingletonInstance as AudioSourceProxy;

            Logger.Error($"Audiosource host faulted. Host: {proxy.Uri}");

            var info = _audioSourceInfos.Find(x => x.host == serviceHost);
            _audioSourceInfos.Remove(info);

            info.process.Close();
            info.host.Abort();

            _audioSources.Remove(proxy);
            AudioSourcesChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnAudioSourceReady(object sender, EventArgs e)
        {
            lock (_audioSourcesLock)
            {
                _audioSources.Add((AudioSourceProxy)sender);
            }

            AudioSourcesChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
