using AudioBand.Annotations;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using Svg;

namespace AudioBand.ViewModels
{
    internal class AudioSourceStatus : INotifyPropertyChanged
    {
        private bool _isPlaying;
        private TimeSpan _songProgress;
        private TimeSpan _songLength;
        private string _songName = "";
        private string _artist = "";
        private string _albumName;

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

        public TimeSpan SongProgress
        {
            get => _songProgress;
            set
            {
                _songProgress = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan SongLength
        {
            get => _songLength;
            set
            {
                if (value == _songLength) return;
                _songLength = value;
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

        public string AlbumName
        {
            get => _albumName;
            set
            {
                if (value == _albumName) return;
                _albumName = value;
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
