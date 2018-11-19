namespace AudioBand.Models
{
    internal class PlayPauseButton : ModelBase
    {
        private string _playButtonImagePath = "";
        private string _pauseButtonImagePath = "";
        private int _xPosition = 103;
        private int _yPosition = 15;
        private int _width = 73;
        private int _height = 12;
        private bool _isVisible = true;

        public string PlayButtonImagePath
        {
            get => _playButtonImagePath;
            set => SetProperty(ref _playButtonImagePath, value);
        }

        public string PauseButtonImagePath
        {
            get => _pauseButtonImagePath;
            set => SetProperty(ref _pauseButtonImagePath, value);
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
