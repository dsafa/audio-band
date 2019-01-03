using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using ServiceContracts;

namespace AudioBand.AudioSource
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    internal class AudioSourceProxy : IAudioSource, IAudioSourceListener
    {
        private ServiceHost _serviceHost;

        public AudioSourceProxy(Uri listenerUri)
        {
            _serviceHost = new ServiceHost(this);
            _serviceHost.AddServiceEndpoint(typeof(IAudioSourceListener), new NetNamedPipeBinding(), listenerUri);
            _serviceHost.Open();

            _serviceHost.Opened += (o_, e) => OnReady();
            _serviceHost.Closed += (o, e) => OnFaulted();
            _serviceHost.Faulted += (o, e) => OnFaulted();
        }

        public event EventHandler Faulted;

        public event EventHandler Ready;

        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        public event EventHandler TrackPlaying;

        public event EventHandler TrackPaused;

        public event EventHandler<TimeSpan> TrackProgressChanged;

        public string Name => Host.GetName();

        public IAudioSourceLogger Logger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private IAudioSourceHost Host => OperationContext.Current.GetCallbackChannel<IAudioSourceHost>();

        public void Close()
        {
            _serviceHost.Close();
        }

        public async Task ActivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await Host.ActivateAsync();
        }

        public async Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await Host.DeactivateAsync();
        }

        public async Task NextTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await Host.NextTrackAsync();
        }

        public async Task PauseTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await Host.PauseTrackAsync();
        }

        public async Task PlayTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await Host.PlayTrackAsync();
        }

        public async Task PreviousTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await Host.PreviousTrackAsync();
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

        private void OnReady()
        {
            Ready?.Invoke(this, EventArgs.Empty);
        }

        private void OnFaulted()
        {
            Faulted?.Invoke(this, EventArgs.Empty);
        }
    }
}
