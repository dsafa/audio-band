using System.ServiceModel;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using NLog;
using ServiceContracts;

namespace AudioSourceHost
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class AudioSourceHostService : IAudioSourceHost
    {
        private readonly IAudioSource _audioSource;
        private readonly Logger _logger;
        private bool _isActive;
        private IAudioSourceHostCallback _callback;

        public AudioSourceHostService(IAudioSource audioSource)
        {
            _logger = LogManager.GetLogger($"AudioSourceHostService({audioSource.Name})");

            _audioSource = audioSource;
            _audioSource.SettingChanged += AudioSourceOnSettingChanged;
            _audioSource.TrackInfoChanged += AudioSourceOnTrackInfoChanged;
            _audioSource.TrackPaused += AudioSourceOnTrackPaused;
            _audioSource.TrackPlaying += AudioSourceOnTrackPlaying;
            _audioSource.TrackProgressChanged += AudioSourceOnTrackProgressChanged;
        }

        private void AudioSourceOnTrackProgressChanged(object sender, System.TimeSpan e)
        {
            _callback.TrackProgressChanged(e);
        }

        private void AudioSourceOnTrackPlaying(object sender, System.EventArgs e)
        {
            _callback.TrackPlaying();
        }

        private void AudioSourceOnTrackPaused(object sender, System.EventArgs e)
        {
            _callback.TrackPaused();
        }

        private void AudioSourceOnTrackInfoChanged(object sender, TrackInfoChangedEventArgs e)
        {
            _callback.TrackInfoChanged(e);
        }

        private void AudioSourceOnSettingChanged(object sender, SettingChangedEventArgs e)
        {
            _callback.SettingChanged(e);
        }

        public async Task ActivateAsync()
        {
            EnsureContext();
            _logger.ConditionalDebug("Activate called");

            if (_isActive)
            {
                return;
            }

            _isActive = true;
            await _audioSource.ActivateAsync().ConfigureAwait(false);
        }

        public async Task DeactivateAsync()
        {
            EnsureContext();
            _logger.ConditionalDebug("Deactivate called");

            if (!_isActive)
            {
                return;
            }

            _isActive = false;
            await _audioSource.DeactivateAsync().ConfigureAwait(false);
        }

        public async Task NextTrackAsync()
        {
            EnsureContext();
            _logger.ConditionalDebug("Next track called");

            if (!_isActive)
            {
                return;
            }

            await _audioSource.NextTrackAsync().ConfigureAwait(false);
        }

        public async Task PauseTrackAsync()
        {
            EnsureContext();
            _logger.ConditionalDebug("Pause Track called");

            if (!_isActive)
            {
                return;
            }

            await _audioSource.PauseTrackAsync().ConfigureAwait(false);
        }

        public async Task PlayTrackAsync()
        {
            EnsureContext();
            _logger.ConditionalDebug("Play Track called");

            if (!_isActive)
            {
                return;
            }

            await _audioSource.PlayTrackAsync().ConfigureAwait(false);
        }

        public async Task PreviousTrackAsync()
        {
            EnsureContext();
            _logger.ConditionalDebug("Previous Track called");

            if (!_isActive)
            {
                return;
            }

            await _audioSource.PreviousTrackAsync().ConfigureAwait(false);
        }

        public string GetName()
        {
            EnsureContext();
            _logger.ConditionalDebug("Get Name called");

            return _audioSource.Name;
        }

        private void EnsureContext()
        {
            if (_callback == null)
            {
                _callback = OperationContext.Current.GetCallbackChannel<IAudioSourceHostCallback>();
            }
        }
    }
}
