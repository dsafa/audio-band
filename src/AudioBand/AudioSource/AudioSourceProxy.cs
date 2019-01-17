using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using ServiceContracts;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Proxy class for an IAudioSource in another process.
    /// </summary>
    internal class AudioSourceProxy : IAudioSource
    {
        private static readonly string HostExePath = Path.Combine(DirectoryHelper.BaseDirectory, "AudioSourceHost.exe");
        private readonly ILogger _logger;
        private readonly object _errorLock = new object();
        private readonly object _isClosingLock = new object();
        private readonly TaskCompletionSource<bool> _initializationCompletionSource;
        private readonly Uri _audioSourceServerEndpoint;
        private Dictionary<string, IAudioSourceHost> _channels = new Dictionary<string, IAudioSourceHost>();
        private DuplexChannelFactory<IAudioSourceHost> _channelFactory;
        private ServiceHost _audioSourceServerHost;
        private string _directory;
        private Uri _hostUri;
        private bool _firstTimeInitialized;
        private bool _isErrored;
        private bool _isClosing;
        private Process _hostProcess;

        private AudioSourceProxy(string directory, TaskCompletionSource<bool> completionSource)
        {
            _initializationCompletionSource = completionSource;
            _directory = directory;
            _logger = LogManager.GetLogger($"AudioSourceProxy({new DirectoryInfo(directory).Name})");

            var audioSourceServer = new AudioSourceServer();
            audioSourceServer.HostRegistered += AudioSourceServerOnHostRegistered;
            _audioSourceServerHost = new ServiceHost(audioSourceServer);
            _audioSourceServerEndpoint = ServiceHelper.GetAudioSourceServerEndpoint(directory);
            _audioSourceServerHost.AddServiceEndpoint(typeof(IAudioSourceServer), new NetNamedPipeBinding(), _audioSourceServerEndpoint);
            _audioSourceServerHost.Open();

            StartHost(directory);
        }

        /// <inheritdoc/>
        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        /// <inheritdoc/>
        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        /// <inheritdoc/>
        public event EventHandler TrackPlaying;

        /// <inheritdoc/>
        public event EventHandler TrackPaused;

        /// <inheritdoc/>
        public event EventHandler<TimeSpan> TrackProgressChanged;

        /// <inheritdoc/>
        public string Name
        {
            get
            {
                try
                {
                    return GetHost().GetName();
                }
                catch (Exception e)
                {
                    HandleError(e);
                    return null;
                }
            }
        }

        /// <inheritdoc/>
        public IAudioSourceLogger Logger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private bool HostIsRestarting
        {
            get
            {
                lock (_errorLock)
                {
                    return _isErrored;
                }
            }

            set
            {
                lock (_errorLock)
                {
                    _isErrored = value;
                }
            }
        }

        private bool IsClosing
        {
            get
            {
                lock (_isClosingLock)
                {
                    return _isClosing;
                }
            }

            set
            {
                lock (_isClosingLock)
                {
                    _isClosing = value;
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="AudioSourceProxy"/> for the given directory.
        /// </summary>
        /// <param name="directory">Directory containing the audio source.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation that creates an <see cref="AudioSourceProxy"/>.</returns>
        public static async Task<AudioSourceProxy> CreateProxy(string directory)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            var proxy = new AudioSourceProxy(directory, taskCompletionSource);

            if (await taskCompletionSource.Task)
            {
                return proxy;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Try to shut down gracefully.
        /// </summary>
        public void Close()
        {
            IsClosing = true;

            try
            {
                _logger.Debug("Closing channel");
                _channelFactory.Close();
            }
            catch (Exception)
            {
                _channelFactory.Abort();
            }
        }

        /// <inheritdoc/>
        public async Task ActivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsClosing || HostIsRestarting)
            {
                return;
            }

            try
            {
                await GetHost().ActivateAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        /// <inheritdoc/>
        public async Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsClosing || HostIsRestarting)
            {
                return;
            }

            try
            {
                await GetHost().DeactivateAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        /// <inheritdoc/>
        public async Task NextTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsClosing || HostIsRestarting)
            {
                return;
            }

            try
            {
                await GetHost().NextTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        /// <inheritdoc/>
        public async Task PauseTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsClosing || HostIsRestarting)
            {
                return;
            }

            try
            {
                await GetHost().PauseTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        /// <inheritdoc/>
        public async Task PlayTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsClosing || HostIsRestarting)
            {
                return;
            }

            try
            {
                await GetHost().PlayTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        /// <inheritdoc/>
        public async Task PreviousTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsClosing || HostIsRestarting)
            {
                return;
            }

            try
            {
                await GetHost().PreviousTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        private void HandleError(Exception e)
        {
            if (e is CommunicationObjectFaultedException || e is CommunicationObjectAbortedException)
            {
                _logger.Debug("Communication already closed");
            }
            else
            {
                _logger.Error(e);
            }

            // if this is the first time the host is registered, exit.
            if (!_firstTimeInitialized)
            {
                _initializationCompletionSource.SetResult(false);
                return;
            }

            if (HostIsRestarting)
            {
                return;
            }

            HostIsRestarting = true;
            StartHost(_directory);
        }

        private void StartHost(string directory)
        {
            _hostProcess = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = HostExePath,
                    Arguments = $"{directory} {_audioSourceServerEndpoint}",
                },
                EnableRaisingEvents = true
            };

            _hostProcess.Exited += ProcessOnExited;
            _hostProcess.Start();
        }

        private void ProcessOnExited(object sender, EventArgs e)
        {
            if (IsClosing)
            {
                return;
            }

            HandleError(new Exception("Host process exited"));
        }

        private void AudioSourceServerOnHostRegistered(object sender, AudioSourceRegisteredEventArgs e)
        {
            CreateChannelFactory(e.HostServiceUri);

            // if this is the first time that the host was registered, signal ready.
            if (!_firstTimeInitialized)
            {
                _firstTimeInitialized = true;
                _initializationCompletionSource.SetResult(true);
            }

            HostIsRestarting = false;
        }

        private void CreateChannelFactory(Uri hostUri)
        {
            _hostUri = hostUri;

            var callback = new AudioSourceHostCallback();
            callback.SettingChanged += (o, e) => SettingChanged?.Invoke(this, e);
            callback.TrackInfoChanged += (o, e) => TrackInfoChanged?.Invoke(this, e);
            callback.TrackPlaying += (o, e) => TrackPlaying?.Invoke(this, e);
            callback.TrackPaused += (o, e) => TrackPaused?.Invoke(this, e);
            callback.TrackProgressChanged += (o, e) => TrackProgressChanged?.Invoke(this, e);

            var callbackInstance = new InstanceContext(callback);
            var channelBinding = new NetNamedPipeBinding()
            {
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                ReceiveTimeout = TimeSpan.FromSeconds(10),
                SendTimeout = TimeSpan.FromSeconds(10),
            };

            _channelFactory = new DuplexChannelFactory<IAudioSourceHost>(callbackInstance, channelBinding, new EndpointAddress(hostUri));
            _channels.Clear();
            GetHost().OpenCallbackChannel();
        }

        // Use this because can't use caller member name inside property
        private IAudioSourceHost GetHost([CallerMemberName] string caller = "")
        {
            if (!_channels.TryGetValue(caller, out IAudioSourceHost host))
            {
                host = _channelFactory.CreateChannel();
                _channels[caller] = host;
            }

            return host;
        }
    }
}
