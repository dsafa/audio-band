using System.ServiceModel;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using ServiceContracts;

namespace AudioSourceHost
{
    public class AudioSourceHostService : IAudioSourceHost
    {
        private readonly IAudioSource _audioSource;
        private readonly IAudioSourceListener _listener;
        private readonly IAudioSourceServer _audioSourceServer;
        private bool _isActive;
        private ServiceHost _serviceHost;

        public AudioSourceHostService(IAudioSource audioSource)
        {
            _serviceHost = new ServiceHost(this);
            _serviceHost.AddServiceEndpoint(typeof(IAudioSourceHost), new NetNamedPipeBinding(), ServiceHelper.GetAudioSourceHostEndpoint(audioSource.Name));

            _audioSourceServer = new ChannelFactory<IAudioSourceServer>(new NetNamedPipeBinding(), new EndpointAddress(ServiceHelper.AudioSourceServerEndpoint)).CreateChannel();

            var listenerEndpoint = _audioSourceServer.RegisterAudioSource(audioSource.Name, ServiceHelper.GetAudioSourceHostEndpoint(audioSource.Name));
            _listener = new ChannelFactory<IAudioSourceListener>(new NetNamedPipeBinding(), new EndpointAddress(listenerEndpoint)).CreateChannel();

            _audioSource = audioSource;
            _audioSource.SettingChanged += AudioSourceOnSettingChanged;
            _audioSource.TrackInfoChanged += AudioSourceOnTrackInfoChanged;
            _audioSource.TrackPaused += AudioSourceOnTrackPaused;
            _audioSource.TrackPlaying += AudioSourceOnTrackPlaying;
            _audioSource.TrackProgressChanged += AudioSourceOnTrackProgressChanged;
        }

        public async Task ActivateAsync()
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;
            await _audioSource.ActivateAsync();
        }

        public async Task DeactivateAsync()
        {
            if (!_isActive)
            {
                return;
            }

            _isActive = false;
            await _audioSource.DeactivateAsync();
        }

        public async Task NextTrackAsync()
        {
            if (!_isActive)
            {
                return;
            }

            await _audioSource.NextTrackAsync();
        }

        public async Task PauseTrackAsync()
        {
            if (!_isActive)
            {
                return;
            }

            await _audioSource.PauseTrackAsync();
        }

        public async Task PlayTrackAsync()
        {
            if (!_isActive)
            {
                return;
            }

            await _audioSource.PlayTrackAsync();
        }

        public async Task PreviousTrackAsync()
        {
            if (!_isActive)
            {
                return;
            }

            await _audioSource.PreviousTrackAsync();
        }

        public string GetName()
        {
            return _audioSource.Name;
        }

        private void AudioSourceOnTrackProgressChanged(object sender, System.TimeSpan e)
        {
            _listener.TrackProgressChanged(e);
        }

        private void AudioSourceOnTrackPlaying(object sender, System.EventArgs e)
        {
            _listener.TrackPlaying();
        }

        private void AudioSourceOnTrackPaused(object sender, System.EventArgs e)
        {
            _listener.TrackPaused();
        }

        private void AudioSourceOnTrackInfoChanged(object sender, TrackInfoChangedEventArgs e)
        {
            _listener.TrackInfoChanged(e);
        }

        private void AudioSourceOnSettingChanged(object sender, SettingChangedEventArgs e)
        {
            _listener.SettingChanged(e);
        }
    }
}
