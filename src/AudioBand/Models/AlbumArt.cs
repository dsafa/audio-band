namespace AudioBand.Models
{
    internal class AlbumArt : ModelBase
    {
        private bool _isVisible = true;
        private int _width = 30;
        private int _height = 30;
        private int _xPosition = 0;
        private int _yPosition = 0;
        private string _placeholderPath = "";

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

        public string PlaceholderPath
        {
            get => _placeholderPath;
            set => SetProperty(ref _placeholderPath, value);
        }
    }
}
