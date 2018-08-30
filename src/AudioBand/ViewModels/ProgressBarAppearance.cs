using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using AudioBand.Annotations;

namespace AudioBand.ViewModels
{
    internal class ProgressBarAppearance : INotifyPropertyChanged
    {
        private Color _foregroundColor;
        private Color _backgroundColor;
        private bool _isVisible;
        private int _yPosition;
        private int _xPosition;
        private int _height;
        private int _width;

        public Color ForegroundColor
        {
            get => _foregroundColor;
            set
            {
                if (value.Equals(_foregroundColor)) return;
                _foregroundColor = value;
                OnPropertyChanged();
            }
        }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                if (value.Equals(_backgroundColor)) return;
                _backgroundColor = value;
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
