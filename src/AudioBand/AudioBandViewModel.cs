using System;
using CSDeskBand.Annotations;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using Svg;

namespace AudioBand
{
    internal class AudioBandViewModel : INotifyPropertyChanged
    {
        private bool _isPlaying;
        private string _nowPlayingText;
        private int _audioProgress;
        private Image _albumArt = new Bitmap(1, 1);
        private Image _previousButtonBitmap = new Bitmap(1, 1);
        private Image _nextButtonBitmap = new Bitmap(1, 1);
        private Image _playPauseButtonBitmap = new Bitmap(1, 1);

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

        public string NowPlayingText
        {
            get => _nowPlayingText;
            set
            {
                if (value == _nowPlayingText) return;
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

        public int AudioProgress
        {
            get => _audioProgress;
            set
            {
                if (value == _audioProgress) return;
                _audioProgress = Math.Min(100, value);
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
