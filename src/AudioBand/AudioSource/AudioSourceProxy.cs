using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using ServiceContracts;

namespace AudioBand.AudioSource
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    internal class AudioSourceProxy : IAudioSource, IAudioSourceListener
    {
        private ILogger _logger;
        private IAudioSourceHost _host;

        public AudioSourceProxy(Uri listenerUri)
        {
            _logger = LogManager.GetLogger($"AudioSourceProxy@{listenerUri}");
            Uri = listenerUri;
        }

        public event EventHandler Ready;

        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        public event EventHandler TrackPlaying;

        public event EventHandler TrackPaused;

        public event EventHandler<TimeSpan> TrackProgressChanged;

        public Uri Uri { get; private set; }

        public string Name => _host.GetName();

        public IAudioSourceLogger Logger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void OpenSession()
        {
            _logger.Debug("Audiosource host connected");
            _host = OperationContext.Current.GetCallbackChannel<IAudioSourceHost>();
            Ready?.Invoke(this, EventArgs.Empty);
        }

        public async Task ActivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _host.ActivateAsync();
        }

        public async Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _host.DeactivateAsync();
        }

        public async Task NextTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _host.NextTrackAsync();
        }

        public async Task PauseTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _host.PauseTrackAsync();
        }

        public async Task PlayTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _host.PlayTrackAsync();
        }

        public async Task PreviousTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _host.PreviousTrackAsync();
        }

        void IAudioSourceListener.SettingChanged(SettingChangedEventArgs args)
        {
            SettingChanged?.Invoke(this, args);
        }

        void IAudioSourceListener.TrackInfoChanged(TrackInfoChangedEventArgs args)
        {
            TrackInfoChanged?.Invoke(this, args);
        }

        void IAudioSourceListener.TrackPaused()
        {
            TrackPaused?.Invoke(this, EventArgs.Empty);
        }

        void IAudioSourceListener.TrackPlaying()
        {
            TrackPlaying?.Invoke(this, EventArgs.Empty);
        }

        void IAudioSourceListener.TrackProgressChanged(TimeSpan progress)
        {
            TrackProgressChanged?.Invoke(this, progress);
        }
    }
}
