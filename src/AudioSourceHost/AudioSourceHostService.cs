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
        private static readonly TimeSpan _pingKeepAliveTime = TimeSpan.FromSeconds(10);
        private readonly IAudioSource _audioSource;
        private readonly Logger _logger;
        private readonly Stopwatch _audioBandCheckStopwatch = new Stopwatch();
        private readonly Timer _checkAudioBandTimer = new Timer(1000) { AutoReset = false };
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
            CaptureContext();

            if (_isActive)
            {
                return;
            }

            _isActive = true;
            await _audioSource.ActivateAsync().ConfigureAwait(false);
        }

        public async Task DeactivateAsync()
        {
            CaptureContext();

            if (!_isActive)
            {
                return;
            }

            _isActive = false;
            await _audioSource.DeactivateAsync().ConfigureAwait(false);
        }

        public async Task NextTrackAsync()
        {
            CaptureContext();

            if (!_isActive)
            {
                return;
            }

            await _audioSource.NextTrackAsync().ConfigureAwait(false);
        }

        public async Task PauseTrackAsync()
        {
            CaptureContext();

            if (!_isActive)
            {
                return;
            }

            await _audioSource.PauseTrackAsync().ConfigureAwait(false);
        }

        public async Task PlayTrackAsync()
        {
            CaptureContext();

            if (!_isActive)
            {
                return;
            }

            await _audioSource.PlayTrackAsync().ConfigureAwait(false);
        }

        public async Task PreviousTrackAsync()
        {
            CaptureContext();

            if (!_isActive)
            {
                return;
            }

            await _audioSource.PreviousTrackAsync().ConfigureAwait(false);
        }

        public string GetName()
        {
            CaptureContext();

            return _audioSource.Name;
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

        private void CaptureContext()
        {
            if (_callback == null)
            {
                _callback = OperationContext.Current.GetCallbackChannel<IAudioSourceHostCallback>();
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
            _logger.Error(e, "Error with communication");
            Program.Exit();
        }

        private async void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (_audioBandCheckStopwatch.Elapsed > _pingKeepAliveTime)
            {
                _logger.Error($"Audioband has not pinged in the last {_pingKeepAliveTime}. Closing host.");
                await Close();
                Program.Exit();
            }

            _checkAudioBandTimer.Start();
        }
    }
}
