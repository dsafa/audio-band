using System.ServiceModel;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using ServiceContracts;

namespace AudioSourceHost
{
    public class AudioSourceHostService : IAudioSourceHost
    {
        private readonly IAudioSource _audioSource;
        private readonly IAudioSourceServer _server;
        private bool _isActive;

        public AudioSourceHostService(IAudioSource audioSource)
        {
            _server = new ChannelFactory<IAudioSourceServer>(new NetNamedPipeBinding(), new EndpointAddress(ServiceHelper.ServerEndpoint)).CreateChannel();
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

        private void AudioSourceOnTrackProgressChanged(object sender, System.TimeSpan e)
        {
            _server.TrackProgressChanged(e);
        }

        private void AudioSourceOnTrackPlaying(object sender, System.EventArgs e)
        {
            _server.TrackPlaying();
        }

        private void AudioSourceOnTrackPaused(object sender, System.EventArgs e)
        {
            _server.TrackPaused();
        }

        private void AudioSourceOnTrackInfoChanged(object sender, TrackInfoChangedEventArgs e)
        {
            _server.TrackInfoChanged(e);
        }

        private void AudioSourceOnSettingChanged(object sender, SettingChangedEventArgs e)
        {
            _server.SettingChanged(e);
        }
    }
}
