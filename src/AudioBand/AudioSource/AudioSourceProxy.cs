using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        }

        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        public event EventHandler TrackPlaying;

        public event EventHandler TrackPaused;

        public event EventHandler<TimeSpan> TrackProgressChanged;

        private IAudioSourceHost Host => OperationContext.Current.GetCallbackChannel<IAudioSourceHost>();

        public string Name => Host.GetName();

        public IAudioSourceLogger Logger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
    }
}
