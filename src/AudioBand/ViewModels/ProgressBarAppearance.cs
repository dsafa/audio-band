using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using AudioBand.Annotations;

namespace AudioBand.ViewModels
{
    internal class ProgressBarAppearance : INotifyPropertyChanged, IEditableObject, IResettableObject
    {
        private Color _foregroundColor;
        private Color _backgroundColor;
        private bool _isVisible;
        private int _yPosition;
        private int _xPosition;
        private int _height;
        private int _width;

        private ProgressBarAppearance _backup;

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

        public ProgressBarAppearance()
        {
            Reset();
        }

        public void BeginEdit()
        {
            _backup = new ProgressBarAppearance
            {
                IsVisible = IsVisible,
                Width = Width,
                Height = Height,
                XPosition = XPosition,
                YPosition = YPosition,
                BackgroundColor = BackgroundColor,
                ForegroundColor = ForegroundColor
            };
        }

        public void EndEdit()
        {
            // No op
        }

        public void CancelEdit()
        {
            IsVisible = _backup.IsVisible;
            Width = _backup.Width;
            Height = _backup.Height;
            XPosition = _backup.XPosition;
            YPosition = _backup.YPosition;
            BackgroundColor = _backup.BackgroundColor;
            ForegroundColor = _backup.ForegroundColor;
        }

        public void Reset()
        {
            ForegroundColor = Color.DodgerBlue;
            BackgroundColor = Color.Black;
            IsVisible = true;
            YPosition = 28;
            XPosition = 30;
            Height = 2;
            Width = 220;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
