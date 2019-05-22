#pragma warning disable
using AudioBand.Models;
using Color = System.Windows.Media.Color;

namespace AudioBand.Settings.Models.V1
{
    internal class TextAppearance
    {
        public bool IsVisible { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int XPosition { get; set; }

        public int YPosition { get; set; }

        public string FontFamily { get; set; }

        public float FontSize { get; set; }

        public Color Color { get; set; }

        public string FormatString { get; set; }

        public CustomLabel.TextAlignment Alignment { get; set; }

        public string Name { get; set; }

        public int ScrollSpeed { get; set; }
    }
}
