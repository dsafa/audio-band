using System.Drawing;

namespace AudioBand.Models
{
    public class CustomLabel
    {
        public int Tag { get; set; }
        public bool IsVisible { get; set; } = true;
        public int Width { get; set; } = 220;
        public int Height { get; set; } = 15;
        public int XPosition { get; set; } = 30;
        public int YPosition { get; set; } = 0;
        public string FontFamily { get; set; } = "Segoe UI";
        public float FontSize { get; set; } = 8.5f;
        public Color Color { get; set; } = Color.White;
        public string FormatString { get; set; } = "{artist} - {song}";
        public TextAlignment Alignment { get; set; } = TextAlignment.Center;
        public string Name { get; set; } = "Now Playing";
        public int ScrollSpeed { get; set; } = 50;

        public enum TextAlignment
        {
            Left,
            Right,
            Center
        }
    }
}
