namespace AudioBand.Models
{
    internal class AlbumArt
    {
        public bool IsVisible { get; set; } = true;
        public int Width { get; set; } = 30;
        public int Height { get; set; } = 30;
        public int XPosition { get; set; } = 0;
        public int YPosition { get; set; } = 0;
        public string PlaceholderPath { get; set; } = "";
    }
}
