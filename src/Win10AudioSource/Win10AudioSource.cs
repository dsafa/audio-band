using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using AudioBand.AudioSource;
using Windows.Foundation.Metadata;
using Windows.Media;
using Windows.Media.Control;
using Windows.Storage.Streams;

namespace Win10AudioSource
{
    public class Win10AudioSource : IAudioSource
    {
        private GlobalSystemMediaTransportControlsSessionManager _mtcManager;
        private GlobalSystemMediaTransportControlsSession _currentSession;

        public event EventHandler<SettingChangedEventArgs> SettingChanged
        {
            add { }
            remove { }
        }

        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        public event EventHandler<bool> IsPlayingChanged;

        public event EventHandler<TimeSpan> TrackProgressChanged;

        public event EventHandler<float> VolumeChanged
        {
            add { }
            remove { }
        }

        public event EventHandler<bool> ShuffleChanged;

        public event EventHandler<RepeatMode> RepeatModeChanged;

        public string Name { get; } = "Windows 10";

        public IAudioSourceLogger Logger { get; set; }

        public async Task ActivateAsync()
        {
            if (!ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
            {
                Logger.Info("Audio source only available on windows 10 1809 and later");
                return;
            }

            _mtcManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            await UpdateSession(_mtcManager.GetCurrentSession());
            _mtcManager.CurrentSessionChanged += MtcManagerOnCurrentSessionChanged;
        }

        public Task DeactivateAsync()
        {
            UnsubscribeFromSession();
            _mtcManager.CurrentSessionChanged -= MtcManagerOnCurrentSessionChanged;
            _mtcManager = null;

            return Task.CompletedTask;
        }

        public async Task PlayTrackAsync()
        {
            if (_currentSession == null)
            {
                return;
            }

            await _currentSession.TryPlayAsync();
        }

        public async Task PauseTrackAsync()
        {
            if (_currentSession == null)
            {
                return;
            }

            await _currentSession.TryPauseAsync();
        }

        public async Task PreviousTrackAsync()
        {
            if (_currentSession == null)
            {
                return;
            }

            await _currentSession.TrySkipPreviousAsync();
        }

        public async Task NextTrackAsync()
        {
            if (_currentSession == null)
            {
                return;
            }

            await _currentSession.TrySkipNextAsync();
        }

        public Task SetVolumeAsync(float newVolume)
        {
            return Task.CompletedTask;
        }

        public async Task SetPlaybackProgressAsync(TimeSpan newProgress)
        {
            if (_currentSession == null)
            {
                return;
            }

            await _currentSession.TryChangePlaybackPositionAsync((long)newProgress.TotalSeconds);
        }

        public async Task SetShuffleAsync(bool shuffleOn)
        {
            if (_currentSession == null)
            {
                return;
            }

            await _currentSession.TryChangeShuffleActiveAsync(shuffleOn);
        }

        public async Task SetRepeatModeAsync(RepeatMode newRepeatMode)
        {
            if (_currentSession == null)
            {
                return;
            }

            await _currentSession.TryChangeAutoRepeatModeAsync(ToWindowsRepeatMode(newRepeatMode));
        }

        private async void MtcManagerOnCurrentSessionChanged(GlobalSystemMediaTransportControlsSessionManager sender, CurrentSessionChangedEventArgs args)
        {
            await UpdateSession(sender.GetCurrentSession());
        }

        private void CurrentSessionOnTimelinePropertiesChanged(GlobalSystemMediaTransportControlsSession sender, TimelinePropertiesChangedEventArgs args)
        {
            UpdateTimelineProperties();
        }

        private void CurrentSessionOnPlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs args)
        {
            UpdatePlaybackProperties();
        }

        private async void CurrentSessionOnMediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
        {
            await UpdateMediaProperties();
        }

        private async Task UpdateSession(GlobalSystemMediaTransportControlsSession newSession)
        {
            UnsubscribeFromSession();
            _currentSession = newSession;

            await UpdateMediaProperties();
            UpdateTimelineProperties();
            UpdatePlaybackProperties();
            SubscribeToSession();
        }

        private void UnsubscribeFromSession()
        {
            if (_currentSession == null)
            {
                return;
            }

            _currentSession.MediaPropertiesChanged -= CurrentSessionOnMediaPropertiesChanged;
            _currentSession.PlaybackInfoChanged -= CurrentSessionOnPlaybackInfoChanged;
            _currentSession.TimelinePropertiesChanged -= CurrentSessionOnTimelinePropertiesChanged;
        }

        private void SubscribeToSession()
        {
            if (_currentSession == null)
            {
                return;
            }

            _currentSession.MediaPropertiesChanged += CurrentSessionOnMediaPropertiesChanged;
            _currentSession.PlaybackInfoChanged += CurrentSessionOnPlaybackInfoChanged;
            _currentSession.TimelinePropertiesChanged += CurrentSessionOnTimelinePropertiesChanged;
        }

        private async Task UpdateMediaProperties()
        {
            if (_currentSession == null)
            {
                return;
            }

            var mediaProperties = await _currentSession.TryGetMediaPropertiesAsync();
            var albumArt = await GetAlbumArt(mediaProperties.Thumbnail);

            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs
            {
                Album = mediaProperties.AlbumTitle,
                AlbumArt = albumArt,
                Artist = mediaProperties.Artist,
                TrackName = mediaProperties.Title,
                TrackLength = _currentSession.GetTimelineProperties().EndTime,
            });
        }

        private void UpdateTimelineProperties()
        {
            if (_currentSession == null)
            {
                return;
            }

            var timelineProperties = _currentSession.GetTimelineProperties();
            TrackProgressChanged?.Invoke(this, timelineProperties.Position);
        }

        private void UpdatePlaybackProperties()
        {
            if (_currentSession == null)
            {
                ClearState();
                return;
            }

            var playbackInfo = _currentSession.GetPlaybackInfo();

            // We'll just make every other state count as paused.
            var isPlaying = playbackInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing;
            IsPlayingChanged?.Invoke(this, isPlaying);

            ShuffleChanged?.Invoke(this, playbackInfo.IsShuffleActive.GetValueOrDefault());
            RepeatModeChanged?.Invoke(this, ToAudioBandRepeatMode(playbackInfo.AutoRepeatMode.GetValueOrDefault()));
        }

        private RepeatMode ToAudioBandRepeatMode(MediaPlaybackAutoRepeatMode repeatMode)
        {
            switch (repeatMode)
            {
                case MediaPlaybackAutoRepeatMode.List:
                    return RepeatMode.RepeatContext;
                case MediaPlaybackAutoRepeatMode.None:
                    return RepeatMode.Off;
                case MediaPlaybackAutoRepeatMode.Track:
                    return RepeatMode.RepeatTrack;
            }

            return RepeatMode.Off;
        }

        private MediaPlaybackAutoRepeatMode ToWindowsRepeatMode(RepeatMode repeatMode)
        {
            switch (repeatMode)
            {
                case RepeatMode.Off:
                    return MediaPlaybackAutoRepeatMode.None;
                case RepeatMode.RepeatContext:
                    return MediaPlaybackAutoRepeatMode.List;
                case RepeatMode.RepeatTrack:
                    return MediaPlaybackAutoRepeatMode.Track;
            }

            return MediaPlaybackAutoRepeatMode.None;
        }

        private async Task<Image> GetAlbumArt(IRandomAccessStreamReference stream)
        {
            if (stream == null)
            {
                return null;
            }

            try
            {
                var read = await stream.OpenReadAsync();
                using (var netStream = read.AsStreamForRead())
                {
                    return Image.FromStream(netStream);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        private void ClearState()
        {
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs());
            IsPlayingChanged?.Invoke(this, false);
            TrackProgressChanged?.Invoke(this, TimeSpan.Zero);
            ShuffleChanged?.Invoke(this, false);
            RepeatModeChanged?.Invoke(this, RepeatMode.Off);
        }
    }
}
