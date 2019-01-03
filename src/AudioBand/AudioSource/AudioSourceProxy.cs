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
    internal class AudioSourceProxy : IAudioSource, IAudioSourceListener
    {
        private IAudioSourceHost _host;
        private ServiceHost _serviceHost;

        public AudioSourceProxy(Uri listenerUri, Uri hostUri)
        {
            _serviceHost = new ServiceHost(this);
            _serviceHost.AddServiceEndpoint(typeof(IAudioSourceListener), new NetNamedPipeBinding(), listenerUri);
            _serviceHost.Open();

            _host = new ChannelFactory<IAudioSourceHost>(new NetNamedPipeBinding(), new EndpointAddress(hostUri)).CreateChannel();
        }

        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        public event EventHandler TrackPlaying;

        public event EventHandler TrackPaused;

        public event EventHandler<TimeSpan> TrackProgressChanged;

        public string Name => _host.GetName();

        public IAudioSourceLogger Logger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

        public void BeginSession()
        {
            throw new NotImplementedException();
        }
    }
}
