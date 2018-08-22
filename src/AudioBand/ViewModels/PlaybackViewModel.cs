using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace AudioBand.ViewModels
{
    internal class PlaybackViewModel : INotifyPropertyChanged
    {
        private bool _isPlaying;
        private double _audioProgress;
        private Image _albumArt = new Bitmap(1, 1);
        private Image _previousButtonBitmap = new Bitmap(1, 1);
        private Image _nextButtonBitmap = new Bitmap(1, 1);
        private Image _playPauseButtonBitmap = new Bitmap(1, 1);
        private NowPlayingText _nowPlayingText;

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                if (value == _isPlaying) return;
                _isPlaying = value;
                OnPropertyChanged();
            }
        }

        public NowPlayingText NowPlayingText
        {
            get => _nowPlayingText;
            set
            {
                if (value.Equals(_nowPlayingText)) return;
                _nowPlayingText = value;
                OnPropertyChanged();
            }
        }

        public Image AlbumArt
        {
            get => _albumArt;
            set
            {
                if (Equals(value, _albumArt)) return;
                _albumArt = value;
                OnPropertyChanged();
            }
        }

        public double AudioProgress
        {
            get => _audioProgress;
            set
            {
                if (Math.Abs(value - _audioProgress) < 0.001) return;
                _audioProgress = value;
                OnPropertyChanged();
            }
        }

        public Image PreviousButtonBitmap
        {
            get => _previousButtonBitmap;
            set
            {
                if (Equals(value, _previousButtonBitmap)) return;
                _previousButtonBitmap = value;
                OnPropertyChanged();
            }
        }

        public Image NextButtonBitmap
        {
            get => _nextButtonBitmap;
            set
            {
                if (Equals(value, _nextButtonBitmap)) return;
                _nextButtonBitmap = value;
                OnPropertyChanged();
            }
        }

        public Image PlayPauseButtonBitmap
        {
            get => _playPauseButtonBitmap;
            set
            {
                if (Equals(value, _playPauseButtonBitmap)) return;
                _playPauseButtonBitmap = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
