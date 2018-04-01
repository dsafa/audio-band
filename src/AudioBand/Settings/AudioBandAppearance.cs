using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using CSDeskBand.Annotations;

namespace AudioBand.Settings
{
    internal class AudioBandAppearance : INotifyPropertyChanged
    {
        private Color _trackProgessColor = Color.DodgerBlue;
        private Font _nowPlayingArtistFont = new Font(new FontFamily("Segoe UI"), 8.5f, FontStyle.Bold, GraphicsUnit.Point);
        private Color _nowPlayingArtistColor = Color.LightSlateGray;
        private Font _nowPlayingTrackNameFont = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
        private Color _nowPlayingTrackNameColor = Color.White;

        public Color TrackProgessColor
        {
            get => _trackProgessColor;
            set
            {
                if (value.Equals(_trackProgessColor)) return;
                _trackProgessColor = value;
                OnPropertyChanged();
            }
        }

        public Font NowPlayingArtistFont
        {
            get => _nowPlayingArtistFont;
            set
            {
                if (Equals(value, _nowPlayingArtistFont)) return;
                _nowPlayingArtistFont = value;
                OnPropertyChanged();
            }
        }

        public Color NowPlayingArtistColor
        {
            get => _nowPlayingArtistColor;
            set
            {
                if (value.Equals(_nowPlayingArtistColor)) return;
                _nowPlayingArtistColor = value;
                OnPropertyChanged();
            }
        }

        public Font NowPlayingTrackNameFont
        {
            get => _nowPlayingTrackNameFont;
            set
            {
                if (Equals(value, _nowPlayingTrackNameFont)) return;
                _nowPlayingTrackNameFont = value;
                OnPropertyChanged();
            }
        }

        public Color NowPlayingTrackNameColor
        {
            get => _nowPlayingTrackNameColor;
            set
            {
                if (value.Equals(_nowPlayingTrackNameColor)) return;
                _nowPlayingTrackNameColor = value;
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
