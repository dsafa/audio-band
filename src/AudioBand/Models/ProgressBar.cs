using System.Drawing;

namespace AudioBand.Models
{
    internal class ProgressBar : ModelBase
    {
        private Color _foregroundColor = Color.DodgerBlue;
        private Color _backgroundColor = Color.Black;
        private bool _isVisible = true;
        private int _xPosition = 30;
        private int _yPosition = 28;
        private int _width = 220;
        private int _height = 2;

        public Color ForegroundColor
        {
            get => _foregroundColor;
            set => SetProperty(ref _foregroundColor, value);
        }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
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
    }
}
