namespace AudioBand.Models
{
    internal class AlbumArtPopup : ModelBase
    {
        private bool _isVisible = true;
        private int _width = 250;
        private int _height = 250;
        private int _xPosition = 0;
        private int _margin = 4;

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

        public int Margin
        {
            get => _margin;
            set => SetProperty(ref _margin, value);
        }
    }
}
