using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using AudioBand.Annotations;

namespace AudioBand.ViewModels
{
    internal class AlbumArtDisplay : INotifyPropertyChanged
    {
        private bool _isVisible = true;
        private int _width = 30;
        private int _height = 30;
        private int _xPosition = 0;
        private int _yPosition = 0;
        private Image _placeholder;
        private string _placeholderPath;

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

        public Image Placeholder
        {
            get => _placeholder;
            set
            {
                if (Equals(value, _placeholder)) return;
                _placeholder = value;
                OnPropertyChanged();
            }
        }

        public string PlaceholderPath
        {
            get => _placeholderPath;
            set
            {
                if (value == _placeholderPath) return;
                _placeholderPath = value;
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

    internal class AlbumArtPopup : INotifyPropertyChanged
    {
        private bool _isVisible = true;
        private int _width = 250;
        private int _height = 250;
        private int _xOffset;
        private int _margin;

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

        public int XOffset
        {
            get => _xOffset;
            set
            {
                if (value == _xOffset) return;
                _xOffset = value;
                OnPropertyChanged();
            }
        }

        public int Margin
        {
            get => _margin;
            set
            {
                if (value == _margin) return;
                _margin = value;
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
