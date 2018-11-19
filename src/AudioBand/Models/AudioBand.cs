namespace AudioBand.Models
{
    internal class AudioBand : ModelBase
    {
        private int _width = 250;
        private int _height = 30;

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
    }
}
