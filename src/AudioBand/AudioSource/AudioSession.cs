using AudioBand.Models;
using AudioBand.Settings;
using System;
using System.Drawing;
using System.Timers;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Persistent audio source session that updates when the audio source changes.
    /// </summary>
    public class AudioSession : ObservableObject, IAudioSession
    {
        private IAppSettings _appSettings;
        private IInternalAudioSource _currentAudioSource;
        private Timer _idleProfileTimer = new Timer();
        private bool _isIdle = true;
        private bool _isPlaying;
        private string _songArtist;
        private string _songName;
        private string _albumName;
        private TimeSpan _songProgress;
        private TimeSpan _songLength;
        private bool _isShuffleOn;
        private RepeatMode _repeatMode;
        private Image _album;
        private Image _lastRememberedAlbum;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSession"/> class.
        /// </summary>
        /// <param name="appSettings">The app settings.</param>
        public AudioSession(IAppSettings appSettings)
        {
            _appSettings = appSettings;

            _idleProfileTimer.AutoReset = false;
            _idleProfileTimer.Interval = GetInterval();
            _idleProfileTimer.Elapsed += OnIdleTimerElapsed;
        }

        /// <summary>
        /// Gets or sets the current audio source.
        /// </summary>
        public IInternalAudioSource CurrentAudioSource
        {
            get => _currentAudioSource;
            set
            {
                if (_currentAudioSource == value)
                {
                    return;
                }

                _currentAudioSource = value;
                AudioSourceChanged();
            }
        }

        /// <inheritdoc />
        public bool IsPlaying
        {
            get => _isPlaying;
            set => SetProperty(ref _isPlaying, value);
        }

        /// <inheritdoc />
        public string SongArtist
        {
            get => _songArtist;
            set => SetProperty(ref _songArtist, value);
        }

        /// <inheritdoc />
        public string SongName
        {
            get => _songName;
            set => SetProperty(ref _songName, value);
        }

        /// <inheritdoc />
        public string AlbumName
        {
            get => _albumName;
            set => SetProperty(ref _albumName, value);
        }

        /// <inheritdoc />
        public Image AlbumArt
        {
            get => _album;
            set => SetProperty(ref _album, value);
        }

        /// <inheritdoc />
        public TimeSpan SongProgress
        {
            get => _songProgress;
            set => SetProperty(ref _songProgress, value);
        }

        /// <inheritdoc />
        public TimeSpan SongLength
        {
            get => _songLength;
            set => SetProperty(ref _songLength, value);
        }

        /// <inheritdoc />
        public bool IsShuffleOn
        {
            get => _isShuffleOn;
            set => SetProperty(ref _isShuffleOn, value);
        }

        /// <inheritdoc />
        public RepeatMode RepeatMode
        {
            get => _repeatMode;
            set => SetProperty(ref _repeatMode, value);
        }

        private void AudioSourceChanged()
        {
            if (_currentAudioSource != null)
            {
                ClearSession();
                _currentAudioSource.TrackInfoChanged -= AudioSourceOnTrackInfoChanged;
                _currentAudioSource.IsPlayingChanged -= AudioSourceOnIsPlayingChanged;
                _currentAudioSource.TrackProgressChanged -= AudioSourceOnTrackProgressChanged;
                _currentAudioSource.RepeatModeChanged -= AudioSourceOnRepeatModeChanged;
                _currentAudioSource.ShuffleChanged -= AudioSourceOnShuffleChanged;
            }

            if (_currentAudioSource == null)
            {
                ClearSession();
                return;
            }

            _currentAudioSource.TrackInfoChanged += AudioSourceOnTrackInfoChanged;
            _currentAudioSource.IsPlayingChanged += AudioSourceOnIsPlayingChanged;
            _currentAudioSource.TrackProgressChanged += AudioSourceOnTrackProgressChanged;
            _currentAudioSource.RepeatModeChanged += AudioSourceOnRepeatModeChanged;
            _currentAudioSource.ShuffleChanged += AudioSourceOnShuffleChanged;
        }

        private void AudioSourceOnShuffleChanged(object sender, bool e)
        {
            IsShuffleOn = e;
        }

        private void AudioSourceOnRepeatModeChanged(object sender, RepeatMode e)
        {
            RepeatMode = e;
        }

        private void AudioSourceOnTrackProgressChanged(object sender, TimeSpan e)
        {
            SongProgress = e;
        }

        private void AudioSourceOnIsPlayingChanged(object sender, bool isPlaying)
        {
            if (_appSettings.AudioBandSettings.UseAutomaticIdleProfile)
            {
                HandleIdleProfile(isPlaying);
            }

            IsPlaying = isPlaying;
        }

        private void AudioSourceOnTrackInfoChanged(object sender, TrackInfoChangedEventArgs e)
        {
            SongArtist = e.Artist;
            SongLength = e.TrackLength;
            SongName = e.TrackName;
            AlbumName = e.Album;
            AlbumArt = e.AlbumArt;
        }

        private void ClearSession()
        {
            IsPlaying = false;
            SongArtist = null;
            SongName = null;
            AlbumName = null;
            SongProgress = TimeSpan.Zero;
            SongLength = TimeSpan.Zero;
        }

        private void HandleIdleProfile(bool isPlaying)
        {
            if (!isPlaying)
            {
                _idleProfileTimer.Interval = GetInterval();
                _idleProfileTimer.Start();
            }
            else if (_isIdle)
            {
                AlbumArt ??= _lastRememberedAlbum ?? AlbumArt;
                _appSettings.SelectProfile(_appSettings.AudioBandSettings.LastNonIdleProfileName);
                _isIdle = false;
            }
            else
            {
                _idleProfileTimer.Stop();
            }
        }

        private void OnIdleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _isIdle = true;
            _appSettings.SelectProfile(_appSettings.AudioBandSettings.IdleProfileName);
            _lastRememberedAlbum = AlbumArt;
            AlbumArt = null;
        }

        private int GetInterval()
        {
            return _appSettings.AudioBandSettings.ShouldGoIdleAfterInSeconds == 0
                ? 250
                : _appSettings.AudioBandSettings.ShouldGoIdleAfterInSeconds * 1000;
        }
    }
}
