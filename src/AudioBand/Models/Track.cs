using System;
using System.Drawing;

namespace AudioBand.Models
{
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

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                SetProperty(ref _isPlaying, value);
                RaisePropertyChanged(nameof(AlbumArt));
            }
        }

        public TimeSpan TrackProgress
        {
            get => _trackProgress;
            set => SetProperty(ref _trackProgress, value);
        }

        public TimeSpan TrackLength
        {
            get => _trackLength;
            set => SetProperty(ref _trackLength, value);
        }

        public string TrackName
        {
            get => _trackName;
            set => SetProperty(ref _trackName, value);
        }

        public string Artist
        {
            get => _artist;
            set => SetProperty(ref _artist, value);
        }

        public string AlbumName
        {
            get => _albumName;
            set => SetProperty(ref _albumName, value);
        }

        public Image AlbumArt
        {
            get => IsPlaying ? _albumArt : _placeholderImage;
            set => SetProperty(ref _albumArt, value);
        }

        public void UpdatePlaceholder(Image placeholder)
        {
            _placeholderImage = placeholder;
            RaisePropertyChanged(nameof(AlbumArt));
        }
    }
}
