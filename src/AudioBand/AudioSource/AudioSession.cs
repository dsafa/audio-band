using System;
using System.Drawing;
using AudioBand.Models;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Persistent audio source session that updates when the audio source changes.
    /// </summary>
    public class AudioSession : ModelBase, IAudioSession
    {
        private IInternalAudioSource _currentAudioSource;
        private bool _isPlaying;
        private string _songArtist;
        private string _songName;
        private string _albumName;
        private TimeSpan _songProgress;
        private TimeSpan _songLength;
        private bool _isShuffleOn;
        private RepeatMode _repeatMode;
        private Image _album;

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

        private void AudioSourceOnIsPlayingChanged(object sender, bool e)
        {
            IsPlaying = e;
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
    }
}
