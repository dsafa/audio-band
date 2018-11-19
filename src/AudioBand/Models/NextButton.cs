namespace AudioBand.Models
{
    internal class NextButton : ModelBase
    {
        private string _imagePath = "";
        private bool _isVisible = true;
        private int _width = 73;
        private int _height = 12;
        private int _xPosition = 176;
        private int _yPosition = 15;

        public string ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
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
