using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using AudioBand.ServiceContracts;
using NLog;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Implements <see cref="IAudioSourceHost"/>, handling restarts if the host closes.
    /// </summary>
    internal class AudioSourceHostService : IAudioSourceHostService
    {
        private static readonly string HostExePath = Path.Combine(DirectoryHelper.BaseDirectory, "AudioSourceHost.exe");
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly object _channelsLock = new object();
        private readonly object _startingLock = new object();
        private readonly IAudioSourceServer _audioSourceServer;
        private readonly string _directory;
        private Dictionary<string, IAudioSourceHost> _channels = new Dictionary<string, IAudioSourceHost>();
        private DuplexChannelFactory<IAudioSourceHost> _channelFactory;
        private Uri _hostUri;
        private Process _hostProcess;
        private bool _isStarting;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceHostService"/> class
        /// with the audio source directory and the audio source server used for the host.
        /// </summary>
        /// <param name="audioSourceDirectory">The directory of the audio source.</param>
        /// <param name="audioSourceServer">The server used by the host.</param>
        public AudioSourceHostService(string audioSourceDirectory, IAudioSourceServer audioSourceServer)
        {
            _directory = audioSourceDirectory;
            _audioSourceServer = audioSourceServer;
            _audioSourceServer.HostRegistered += AudioSourceServerOnHostRegistered;

            StartHost();
        }

        /// <inheritdoc/>
        public event EventHandler Restarted;

        /// <inheritdoc/>
        public AudioSourceHostCallback HostCallback { get; } = new AudioSourceHostCallback();

        private bool HostIsStarting
        {
            get
            {
                lock (_startingLock)
                {
                    return _isStarting;
                }
            }

            set
            {
                lock (_startingLock)
                {
                    _isStarting = value;
                }
            }
        }

        /// <inheritdoc/>
        public IAudioSourceHost GetHost([CallerMemberName] string caller = "")
        {
            lock (_channelsLock)
            {
                if (!_channels.TryGetValue(caller, out IAudioSourceHost host))
                {
                    host = _channelFactory.CreateChannel();
                    ((ICommunicationObject)host).Open();

                    _channels[caller] = host;
                }

                return host;
            }
        }

        /// <inheritdoc/>
        public void Restart()
        {
            StartHost();
        }

        private void CreateChannelFactory()
        {
            var callbackInstance = new InstanceContext(HostCallback);
            var channelBinding = new NetNamedPipeBinding()
            {
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                ReceiveTimeout = TimeSpan.FromSeconds(10),
                SendTimeout = TimeSpan.FromSeconds(10),
            };

            lock (_channelsLock)
            {
                _channelFactory = new DuplexChannelFactory<IAudioSourceHost>(callbackInstance, channelBinding, new EndpointAddress(_hostUri));
                _channels.Clear();
            }

            GetHost().OpenCallbackChannel();
        }

        private void AudioSourceServerOnHostRegistered(object sender, Uri e)
        {
            _hostUri = e;
            CreateChannelFactory();

            Debug.Assert(HostIsStarting, $"{nameof(HostIsStarting)} should be true");
            HostIsStarting = false;

            Restarted?.Invoke(this, EventArgs.Empty);
        }

        private void StartHost()
        {
            if (HostIsStarting)
            {
                Logger.Debug("Host is already restarting");
                return;
            }

            if (_hostProcess != null && !_hostProcess.HasExited)
            {
                Logger.Debug("Cannot start host, previous process is still alive");
                return;
            }

            Logger.Debug($"Starting host at {_directory}");

            _hostProcess = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = HostExePath,
                    Arguments = $"{_directory} {_audioSourceServer.Endpoint}",
                },
                EnableRaisingEvents = true
            };

            _hostProcess.Exited += ProcessOnExited;
            _hostProcess.Start();

            HostIsStarting = true;
        }

        private void ProcessOnExited(object sender, EventArgs e)
        {
            Logger.Debug($"Host process at {_hostUri} exited.");

            StartHost();
        }
    }
}
