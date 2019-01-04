using System.ServiceModel;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using ServiceContracts;

namespace AudioSourceHost
{
    public class AudioSourceHostService
    {
        private readonly IAudioSource _audioSource;
        private readonly HostLogger _logger = LogManager.GetHostLogger();
        private IAudioSourceListener _listener;

        public AudioSourceHostService(IAudioSource audioSource, string hostEndpoint)
        {
            var instanceContext = new InstanceContext(new Host(audioSource));
            var channelFactory = new DuplexChannelFactory<IAudioSourceListener>(instanceContext, new NetNamedPipeBinding(), hostEndpoint);
            _listener = channelFactory.CreateChannel();

            _logger.Debug("Channel created");

            _audioSource = audioSource;
            _audioSource.SettingChanged += AudioSourceOnSettingChanged;
            _audioSource.TrackInfoChanged += AudioSourceOnTrackInfoChanged;
            _audioSource.TrackPaused += AudioSourceOnTrackPaused;
            _audioSource.TrackPlaying += AudioSourceOnTrackPlaying;
            _audioSource.TrackProgressChanged += AudioSourceOnTrackProgressChanged;

            _listener.OpenSession();
            _logger.Debug("Session opened");
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

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
        private class Host : IAudioSourceHost
        {
            private readonly IAudioSource _audioSource;
            private bool _isActive;

            public Host(IAudioSource audioSource)
            {
                _audioSource = audioSource;
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
        }
    }
}
