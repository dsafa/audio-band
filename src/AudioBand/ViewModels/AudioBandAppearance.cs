using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace AudioBand.ViewModels
{
    internal class AudioBandAppearance : INotifyPropertyChanged
    {
        private Color _trackProgressColor = Color.DodgerBlue;
        private Font _nowPlayingArtistFont = new Font(new FontFamily("Segoe UI"), 8.5f, FontStyle.Bold, GraphicsUnit.Point);
        private Color _nowPlayingArtistColor = Color.LightSlateGray;
        private Font _nowPlayingTrackNameFont = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
        private Color _nowPlayingTrackNameColor = Color.White;
        private Color _trackProgressBackColor = Color.Black;

        public Color TrackProgressColor
        {
            get => _trackProgressColor;
            set
            {
                if (value.Equals(_trackProgressColor)) return;
                _trackProgressColor = value;
                OnPropertyChanged();
            }
        }

        public Color TrackProgressBackColor
        {
            get => _trackProgressBackColor;
            set
            {
                if (value.Equals(_trackProgressBackColor)) return;
                _trackProgressBackColor = value;
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
