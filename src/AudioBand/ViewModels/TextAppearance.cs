using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using AudioBand.Annotations;

namespace AudioBand.ViewModels
{
    internal class TextAppearance : INotifyPropertyChanged, IEditableObject
    {
        private bool _isVisible = true;
        private string _fontFamily = "Segoe UI";
        private float _fontSize = 8.5f;
        private Color _color = Color.White;
        private string _formatString = "{artist} - {song}";
        private TextAlignment _textAlignment = TextAlignment.Center;
        private int _yPosition = 0;
        private int _xPosition = 30;
        private int _height = 14;
        private int _width = 220;
        private string _name = "Now Playing";
        private int _scrollSpeed = 50;

        private TextAppearance _backup;

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

        public Color Color
        {
            get => _color;
            set
            {
                if (value.Equals(_color)) return;
                _color = value;
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

        public int ScrollSpeed
        {
            get => _scrollSpeed;
            set
            {
                if (value == _scrollSpeed) return;
                _scrollSpeed = value;
                OnPropertyChanged();
            }
        }

        public Point Location => new Point(_xPosition, _yPosition);

        public int Tag { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void BeginEdit()
        {
            _backup = new TextAppearance
            {
                IsVisible = IsVisible,
                Width = Width,
                Height = Height,
                XPosition = XPosition,
                YPosition = YPosition,
                Color = Color,
                TextAlignment = TextAlignment,
                FontFamily = FontFamily,
                FontSize = FontSize,
                FormatString = FormatString,
                Name = Name,
                ScrollSpeed = ScrollSpeed
            };
        }

        public void EndEdit()
        {

        }

        public void CancelEdit()
        {
            IsVisible = _backup.IsVisible;
            Width = _backup.Width;
            Height = _backup.Height;
            XPosition = _backup.XPosition;
            YPosition = _backup.YPosition;
            Color = _backup.Color;
            TextAlignment = _backup.TextAlignment;
            FontFamily = _backup.FontFamily;
            FontSize = _backup.FontSize;
            FormatString = _backup.FormatString;
            Name = _backup.Name;
            ScrollSpeed = _backup.ScrollSpeed;
        }
    }

    public enum TextAlignment
    {
        Left,
        Right,
        Center
    }
}