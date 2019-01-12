using System;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using ServiceContracts;

namespace AudioBand.AudioSource
{
    internal class AudioSourceProxy : IAudioSource
    {
        private readonly ILogger _logger;
        private readonly System.Timers.Timer _pingTimer = new System.Timers.Timer(1000) { AutoReset = false };
        private IAudioSourceHost _host;
        private IAudioSourceHost _pingChannel;
        private DuplexChannelFactory<IAudioSourceHost> _channelFactory;
        private bool _isClosed;

        public AudioSourceProxy(Uri hostUri)
        {
            Uri = hostUri;
            _logger = LogManager.GetLogger($"AudioSourceProxy@{hostUri}");

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
            };
            _channelFactory = new DuplexChannelFactory<IAudioSourceHost>(callbackInstance, channelBinding, new EndpointAddress(hostUri));
            _host = _channelFactory.CreateChannel();

            _pingChannel = _channelFactory.CreateChannel();
            ((IClientChannel)_pingChannel).OperationTimeout = TimeSpan.FromSeconds(10);
            _pingTimer.Elapsed += PingTimerOnElapsed;
            _pingTimer.Start();
        }

        public event EventHandler Errored;

        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        public event EventHandler TrackPlaying;

        public event EventHandler TrackPaused;

        public event EventHandler<TimeSpan> TrackProgressChanged;

        public Uri Uri { get; set; }

        public string Name
        {
            get
            {
                if (InvalidState)
                {
                    return null;
                }

                try
                {
                    return _host.GetName();
                }
                catch (Exception e)
                {
                    HandleError(e);
                    return null;
                }
            }
        }

        public IAudioSourceLogger Logger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private bool InvalidState => _isClosed || (_channelFactory.State != CommunicationState.Opened);

        public void Close()
        {
            if (_isClosed)
            {
                return;
            }

            _isClosed = true;

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

        public async Task ActivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (InvalidState)
            {
                return;
            }

            try
            {
                await _host.ActivateAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        public async Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (InvalidState)
            {
                return;
            }

            try
            {
                await _host.DeactivateAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        public async Task NextTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (InvalidState)
            {
                return;
            }

            try
            {
                await _host.NextTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        public async Task PauseTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (InvalidState)
            {
                return;
            }

            try
            {
                await _host.PauseTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        public async Task PlayTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (InvalidState)
            {
                return;
            }

            try
            {
                await _host.PlayTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }

        public async Task PreviousTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (InvalidState)
            {
                return;
            }

            try
            {
                await _host.PreviousTrackAsync().ConfigureAwait(false);
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
                _logger.Error(e, $"Error during call to host");
            }

            Errored?.Invoke(this, EventArgs.Empty);
        }

        private void PingTimerOnElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (InvalidState)
                {
                    return;
                }

                _pingChannel.IsAlive();
                _pingTimer.Start();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }
}
