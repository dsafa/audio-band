using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using AudioBand.ServiceContracts;
using NLog;

namespace AudioSourceHost
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class AudioSourceHostService : IAudioSourceHost
    {
        private readonly IAudioSource _audioSource;
        private readonly Logger _logger;
        private readonly Dictionary<string, AudioSourceSetting> _audioSourceSettings = new Dictionary<string, AudioSourceSetting>();
        private List<AudioSourceSetting> _audioSourceSettingsList; // so we can keep the order of the settings.
        private bool _isActive;

        public AudioSourceHostService(IAudioSource audioSource)
        {
            _logger = LogManager.GetLogger($"AudioSourceHostService({audioSource.Name})");

            _audioSource = audioSource;
            _audioSource.SettingChanged += AudioSourceOnSettingChanged;
            _audioSource.TrackInfoChanged += AudioSourceOnTrackInfoChanged;
            _audioSource.TrackPaused += AudioSourceOnTrackPaused;
            _audioSource.TrackPlaying += AudioSourceOnTrackPlaying;
            _audioSource.TrackProgressChanged += AudioSourceOnTrackProgressChanged;
            _audioSource.VolumeChanged += AudioSourceOnVolumeChanged;

            if (_audioSource is ISupportsRatings supportRatings)
            {
                supportRatings.TrackRatingChanged += AudioSourceOnRatingChanged;
            }

            _audioSourceSettingsList = _audioSource.GetSettings();
            foreach (AudioSourceSetting setting in _audioSourceSettingsList)
            {
                _audioSourceSettings.Add(setting.Attribute.Name, setting);
            }
        }

        private IAudioSourceHostCallback Callback { get; set; }

        public async Task ActivateAsync()
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;

            try
            {
                await _audioSource.ActivateAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public async Task DeactivateAsync()
        {
            if (!_isActive)
            {
                return;
            }

            _isActive = false;

            try
            {
                await _audioSource.DeactivateAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public async Task NextTrackAsync()
        {
            if (!_isActive)
            {
                return;
            }

            try
            {
                await _audioSource.NextTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public async Task PauseTrackAsync()
        {
            if (!_isActive)
            {
                return;
            }

            try
            {
                await _audioSource.PauseTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public async Task PlayTrackAsync()
        {
            if (!_isActive)
            {
                return;
            }

            try
            {
                await _audioSource.PlayTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public async Task PreviousTrackAsync()
        {
            if (!_isActive)
            {
                return;
            }

            try
            {
                await _audioSource.PreviousTrackAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        public string GetName()
        {
            return _audioSource.Name;
        }

        public async Task SetVolume(float volume)
        {
            await _audioSource.SetVolumeAsync(volume);
        }

        public async Task SetPlaybackProgress(TimeSpan progress)
        {
            await _audioSource.SetPlaybackProgress(progress);
        }

        public async Task SetRatingAsync(TrackRating rating)
        {
            var supportsRatings = _audioSource as ISupportsRatings;
            await supportsRatings?.SetTrackRatingAsync(rating);
        }

        public void OpenCallbackChannel()
        {
            Callback = OperationContext.Current.GetCallbackChannel<IAudioSourceHostCallback>();
        }

        public List<AudioSourceSettingInfo> GetAudioSourceSettings()
        {
            return _audioSourceSettingsList.Select(s => (AudioSourceSettingInfo)s.Attribute).ToList();
        }

        public void UpdateSetting(string settingName, object value)
        {
            _audioSourceSettings[settingName].SettingValue = value;
        }

        public object GetSettingValue(string settingName)
        {
            return _audioSourceSettings[settingName].SettingValue;
        }

        /// <summary>
        /// Try to gracefully close.
        /// </summary>
        /// <returns>Task</returns>
        public async Task Close()
        {
            try
            {
                await DeactivateAsync();
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }

        private void AudioSourceOnTrackProgressChanged(object sender, TimeSpan e)
        {
            try
            {
                Callback.TrackProgressChanged(e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void AudioSourceOnTrackPlaying(object sender, EventArgs e)
        {
            try
            {
                Callback.TrackPlaying();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void AudioSourceOnTrackPaused(object sender, EventArgs e)
        {
            try
            {
                Callback.TrackPaused();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void AudioSourceOnTrackInfoChanged(object sender, TrackInfoChangedEventArgs e)
        {
            try
            {
                Callback.TrackInfoChanged((TrackInfo)e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void AudioSourceOnSettingChanged(object sender, SettingChangedEventArgs e)
        {
            try
            {
                Callback.SettingChanged((SettingChangedInfo)e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void AudioSourceOnVolumeChanged(object sender, float e)
        {
            try
            {
                Callback.VolumeChanged(e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void AudioSourceOnRatingChanged(object sender, TrackRating e)
        {
            try
            {
                Callback.TrackRatingChanged(e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HandleException(Exception e)
        {
            // Handle exceptions within wcf context by closing quickly so it can restart.
            _logger.Error(e);
            Program.Exit();
        }
    }
}
