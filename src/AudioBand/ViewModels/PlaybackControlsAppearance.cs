using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using AudioBand.Annotations;

namespace AudioBand.ViewModels
{
    internal class PlayPauseButtonAppearance : INotifyPropertyChanged
    {
        private Image _playImage = new Bitmap(1, 1);
        private string _playImagePath;
        private Image _pauseImage = new Bitmap(1, 1);
        private string _pauseImagePath;
        private int _yPosition = 14;
        private int _xPosition = 103;
        private int _height = 12;
        private int _width = 73;
        private bool _isVisible = true;
        private Image _currentImage = new Bitmap(1, 1);

        public Image PlayImage
        {
            get => _playImage;
            set
            {
                if (Equals(value, _playImage)) return;
                _playImage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        public string PlayImagePath
        {
            get => _playImagePath;
            set
            {
                if (value == _playImagePath) return;
                _playImagePath = value;
                OnPropertyChanged();
            }
        }

        public Image PauseImage
        {
            get => _pauseImage;
            set
            {
                if (Equals(value, _pauseImage)) return;
                _pauseImage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentImage));
            }
        }

        public string PauseImagePath
        {
            get => _pauseImagePath;
            set
            {
                if (value == _pauseImagePath) return;
                _pauseImagePath = value;
                OnPropertyChanged();
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public int XPosition
        {
            get => _xPosition;
            set
            {
                if (value == _xPosition) return;
                _xPosition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public int YPosition
        {
            get => _yPosition;
            set
            {
                if (value == _yPosition) return;
                _yPosition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public Image CurrentImage
        {
            get => _currentImage;
            set
            {
                if (Equals(value, _currentImage)) return;
                _currentImage = value;
                OnPropertyChanged();
            }
        }

        public Point Location => new Point(_xPosition, _yPosition);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class NextSongButtonAppearance : INotifyPropertyChanged
    {
        private Image _image = new Bitmap(1, 1);
        private string _imagePath;
        private int _yPosition = 14;
        private int _xPosition = 176;
        private int _height = 12;
        private int _width = 73;
        private bool _isVisible = true;

        public Image Image
        {
            get => _image;
            set
            {
                if (Equals(value, _image)) return;
                _image = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (value == _imagePath) return;
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public int XPosition
        {
            get => _xPosition;
            set
            {
                if (value == _xPosition) return;
                _xPosition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public int YPosition
        {
            get => _yPosition;
            set
            {
                if (value == _yPosition) return;
                _yPosition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public Point Location => new Point(_xPosition, _yPosition);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class PreviousSongButtonAppearance : INotifyPropertyChanged
    {
        private Image _image = new Bitmap(1, 1);
        private string _imagePath;
        private bool _isVisible = true;
        private int _width = 73;
        private int _height = 12;
        private int _xPosition = 30;
        private int _yPosition = 14;

        public Image Image
        {
            get => _image;
            set
            {
                if (Equals(value, _image)) return;
                _image = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (value == _imagePath) return;
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public int XPosition
        {
            get => _xPosition;
            set
            {
                if (value == _xPosition) return;
                _xPosition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public int YPosition
        {
            get => _yPosition;
            set
            {
                if (value == _yPosition) return;
                _yPosition = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public Point Location => new Point(_xPosition, _yPosition);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
