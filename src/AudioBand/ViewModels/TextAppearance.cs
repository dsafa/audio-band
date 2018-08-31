using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using AudioBand.Annotations;

namespace AudioBand.ViewModels
{
    internal class TextAppearance : INotifyPropertyChanged
    {
        private bool _isVisible = true;
        private string _fontFamily = "Segoe UI";
        private float _fontSize = 8.5f;
        private Color _backgroundColor = Color.Transparent;
        private string _formatString = "";
        private TextAlignment _textAlignment = TextAlignment.Left;
        private int _yPosition = 0;
        private int _xPosition = 0;
        private int _height;
        private int _width;
        private string _name;
        private bool _scrollingEnabled;
        private bool _scrollingFadeEnabled;
        private Color _scrollingFadeColor;

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
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

        public string FontFamily
        {
            get => _fontFamily;
            set
            {
                if (value == _fontFamily) return;
                _fontFamily = value;
                OnPropertyChanged();
            }
        }

        public float FontSize
        {
            get => _fontSize;
            set
            {
                if (value.Equals(_fontSize)) return;
                _fontSize = value;
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

        public string FormatString
        {
            get => _formatString;
            set
            {
                if (value == _formatString) return;
                _formatString = value;
                OnPropertyChanged();
            }
        }

        public TextAlignment TextAlignment
        {
            get => _textAlignment;
            set
            {
                if (value == _textAlignment) return;
                _textAlignment = value;
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

        public bool ScrollingEnabled
        {
            get => _scrollingEnabled;
            set
            {
                if (value == _scrollingEnabled) return;
                _scrollingEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool ScrollingFadeEnabled
        {
            get => _scrollingFadeEnabled;
            set
            {
                if (value == _scrollingFadeEnabled) return;
                _scrollingFadeEnabled = value;
                OnPropertyChanged();
            }
        }

        public Color ScrollingFadeColor
        {
            get => _scrollingFadeColor;
            set
            {
                if (value.Equals(_scrollingFadeColor)) return;
                _scrollingFadeColor = value;
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

    internal enum TextAlignment
    {
        Left,
        Right,
        Center
    }
}