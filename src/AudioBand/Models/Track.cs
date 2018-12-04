using System;
using System.Drawing;

namespace AudioBand.Models
{
    /// <summary>
    /// Model for the current track.
    /// </summary>
    internal class Track : ModelBase
    {
        private bool _isPlaying;
        private TimeSpan _trackProgress;
        private string _trackName;
        private TimeSpan _trackLength;
        private string _artist;
        private string _albumName;
        private Image _albumArt;
        private Image _placeholderImage;

        /// <summary>
        /// Gets or sets a value indicating whether the track is playing.
        /// </summary>
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                SetProperty(ref _isPlaying, value);
                RaisePropertyChanged(nameof(AlbumArt));
            }
        }

        /// <summary>
        /// Gets or sets the playback progress of the track.
        /// </summary>
        public TimeSpan TrackProgress
        {
            get => _trackProgress;
            set => SetProperty(ref _trackProgress, value);
        }

        /// <summary>
        /// Gets or sets the length of the track.
        /// </summary>
        public TimeSpan TrackLength
        {
            get => _trackLength;
            set => SetProperty(ref _trackLength, value);
        }

        /// <summary>
        /// Gets or sets the name of the track.
        /// </summary>
        public string TrackName
        {
            get => _trackName;
            set => SetProperty(ref _trackName, value);
        }

        /// <summary>
        /// Gets or sets the artist of the track.
        /// </summary>
        public string Artist
        {
            get => _artist;
            set => SetProperty(ref _artist, value);
        }

        /// <summary>
        /// Gets or sets the album name.
        /// </summary>
        public string AlbumName
        {
            get => _albumName;
            set => SetProperty(ref _albumName, value);
        }

        /// <summary>
        /// Gets or sets the album art.
        /// </summary>
        public Image AlbumArt
        {
            get => _albumArt ?? _placeholderImage;
            set => SetProperty(ref _albumArt, value);
        }

        /// <summary>
        /// Change the placeholder image.
        /// </summary>
        /// <param name="placeholder">The new placeholder image.</param>
        public void UpdatePlaceholder(Image placeholder)
        {
            _placeholderImage = placeholder;
            RaisePropertyChanged(nameof(AlbumArt));
        }
    }
}
