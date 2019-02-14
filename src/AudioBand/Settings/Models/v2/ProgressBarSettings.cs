using System.Drawing;

namespace AudioBand.Settings.Models.V2
{
    internal class ProgressBarSettings
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
