using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using AudioBand.AudioSource;
using NLog;
using ServiceContracts;

namespace AudioSourceHost
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant, IncludeExceptionDetailInFaults = true)]
    public class AudioSourceHostService : IAudioSourceHost
    {
        private readonly IAudioSource _audioSource;
        private readonly Logger _logger;
        private readonly Stopwatch _audioBandCheckStopwatch = new Stopwatch();
        private readonly Timer _checkAudioBandTimer = new Timer(1000) { AutoReset = false };
        private readonly TimeSpan PingKeepAliveTime = TimeSpan.FromSeconds(10);
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

            _checkAudioBandTimer.Elapsed += TimerOnElapsed;
            _checkAudioBandTimer.Start();
            _audioBandCheckStopwatch.Start();
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

            _logger.ConditionalDebug("Activated");
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

        public void IsAlive()
        {
            _audioBandCheckStopwatch.Restart();
        }

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
                _callback.TrackProgressChanged(e);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void AudioSourceOnTrackPlaying(object sender, System.EventArgs e)
        {
            try
            {
                _callback.TrackPlaying();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void AudioSourceOnTrackPaused(object sender, System.EventArgs e)
        {
            try
            {
                _callback.TrackPaused();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void AudioSourceOnTrackInfoChanged(object sender, TrackInfoChangedEventArgs e)
        {
            try
            {
                _callback.TrackInfoChanged((TrackInfo)e);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void AudioSourceOnSettingChanged(object sender, SettingChangedEventArgs e)
        {
            try
            {
                _callback.SettingChanged((SettingChangedInfo)e);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void HandleError(Exception e)
        {
            _logger.Error(e, "Error during call to callback");
            Program.Exit();
        }

        private async void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (_audioBandCheckStopwatch.Elapsed > PingKeepAliveTime)
            {
                _logger.Error($"Audioband has not pinged in the last {PingKeepAliveTime}. Closing host.");
                await Close();
                Program.Exit();
            }
        }
    }
}
