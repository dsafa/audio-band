#pragma warning disable
using Color = System.Windows.Media.Color;

namespace AudioBand.Settings.Models.V1
{
    internal class ProgressBarAppearance
    {
        public Color ForegroundColor { get; set; }

        public Color BackgroundColor { get; set; }

        public bool IsVisible { get; set; }

        public int XPosition { get; set; }

        public int YPosition { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
