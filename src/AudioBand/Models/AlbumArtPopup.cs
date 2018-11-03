namespace AudioBand.Models
{
    internal class AlbumArtPopup
    {
        public bool IsVisible { get; set; } = true;
        public int Width { get; set; } = 250;
        public int Height { get; set; } = 250;
        public int XPosition { get; set; } = 0;
        public int Margin { get; set; } = 4;
    }
}
