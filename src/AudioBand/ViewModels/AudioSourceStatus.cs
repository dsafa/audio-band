using AudioBand.Annotations;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace AudioBand.ViewModels
{
    internal class AudioSourceStatus : INotifyPropertyChanged
    {
        private bool _isPlaying;
        private double _audioProgress;
        private int _audioLength;
        private string _songName = "";
        private string _artist = "";
        private Image _albumArt = new Bitmap(1, 1);

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

        public int AudioLength
        {
            get => _audioLength;
            set
            {
                if (value == _audioLength) return;
                _audioLength = value;
                OnPropertyChanged();
            }
        }

        public string SongName
        {
            get => _songName;
            set
            {
                if (value == _songName) return;
                _songName = value;
                OnPropertyChanged();
            }
        }

        public string Artist
        {
            get => _artist;
            set
            {
                if (value == _artist) return;
                _artist = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
