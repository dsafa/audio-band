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
        private Bitmap _albumArt = new Bitmap(1, 1);
        private Bitmap _previousButtonBitmap = new Bitmap(1, 1);
        private Bitmap _nextButtonBitmap = new Bitmap(1, 1);
        private Bitmap _playPauseButtonBitmap = new Bitmap(1, 1);

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

        public Bitmap AlbumArt
        {
            get => _albumArt;
            set
            {
                if (Equals(value, _albumArt)) return;
                _albumArt = new Bitmap(AlbumArtSize.Width, AlbumArtSize.Height);
                using (var graphics = Graphics.FromImage(_albumArt))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(value, 0, 0, AlbumArtSize.Width, AlbumArtSize.Height);
                }

                OnPropertyChanged();
            }
        }

        public int AudioProgress
        {
            get => _audioProgress;
            set
            {
                if (value == _audioProgress) return;
                _audioProgress = value;
                OnPropertyChanged();
            }
        }

        public Bitmap PreviousButtonBitmap
        {
            get => _previousButtonBitmap;
            set
            {
                if (Equals(value, _previousButtonBitmap)) return;
                _previousButtonBitmap = value;
                OnPropertyChanged();
            }
        }

        public Bitmap NextButtonBitmap
        {
            get => _nextButtonBitmap;
            set
            {
                if (Equals(value, _nextButtonBitmap)) return;
                _nextButtonBitmap = value;
                OnPropertyChanged();
            }
        }

        public Bitmap PlayPauseButtonBitmap
        {
            get => _playPauseButtonBitmap;
            set
            {
                if (Equals(value, _playPauseButtonBitmap)) return;
                _playPauseButtonBitmap = value;
                OnPropertyChanged();
            }
        }

        public Size AlbumArtSize { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
