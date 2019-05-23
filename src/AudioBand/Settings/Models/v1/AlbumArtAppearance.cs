#pragma warning disable
namespace AudioBand.Settings.Models.V1
{
    internal class AlbumArtAppearance
    {
        public bool IsVisible { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int XPosition { get; set; }

        public int YPosition { get; set; }

        public string PlaceholderPath { get; set; }
    }
}
