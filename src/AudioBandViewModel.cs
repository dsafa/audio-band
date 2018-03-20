using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CSDeskBand.Annotations;

namespace AudioBand
{
    public class AudioBandViewModel : INotifyPropertyChanged
    {
        private bool _isPlaying;
        private string _nowPlayingText;
        private Bitmap _albumArt = new Bitmap(10, 10);
        private int _audioProgress;

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

        public Size AlbumArtSize { get; set; } = new Size(10, 10);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
