using System.Drawing;

namespace AudioBand.Models
{
    internal class ProgressBar
    {
        public Color ForegroundColor { get; set; } = Color.DodgerBlue;
        public Color BackgroundColor { get; set; } = Color.Black;
        public bool IsVisible { get; set; } = true;
        public int XPosition { get; set; } = 30;
        public int YPosition { get; set; } = 28;
        public int Width { get; set; } = 220;
        public int Height { get; set; } = 2;
    }
}
