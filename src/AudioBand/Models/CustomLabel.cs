using System.Drawing;

namespace AudioBand.Models
{
    internal class CustomLabel : ModelBase
    {
        private int _tag;
        private bool _isVisible = true;
        private int _width = 220;
        private int _height = 15;
        private int _xPosition = 30;
        private int _yPosition = 0;
        private string _fontFamily = "Segoe UI";
        private float _fontSize = 8.5f;
        private Color _color = Color.White;
        private string _formatString = "{artist} - {song}";
        private TextAlignment _alignment = TextAlignment.Center;
        private string _name = "Now Playing";
        private int _scrollSpeed = 50;

        public int Tag
        {
            get => _tag;
            set => _tag = value;
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        public int Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        public int XPosition
        {
            get => _xPosition;
            set => SetProperty(ref _xPosition, value);
        }

        public int YPosition
        {
            get => _yPosition;
            set => SetProperty(ref _yPosition, value);
        }

        public string FontFamily
        {
            get => _fontFamily;
            set => SetProperty(ref _fontFamily, value);
        }

        public float FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }

        public Color Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        public string FormatString
        {
            get => _formatString;
            set => SetProperty(ref _formatString, value);
        }

        public TextAlignment Alignment
        {
            get => _alignment;
            set => SetProperty(ref _alignment, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int ScrollSpeed
        {
            get => _scrollSpeed;
            set => SetProperty(ref _scrollSpeed, value);
        }

        public enum TextAlignment
        {
            Left,
            Right,
            Center
        }
    }
}
