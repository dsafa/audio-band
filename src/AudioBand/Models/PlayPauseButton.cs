namespace AudioBand.Models
{
    internal class PlayPauseButton
    {
        public string PlayButtonImagePath { get; set; } = "";
        public string PauseButtonImagePath { get; set; } = "";
        public int XPosition { get; set; } = 103;
        public int YPosition { get; set; } = 15;
        public int Width { get; set; } = 73;
        public int Height { get; set; } = 12;
        public bool IsVisible { get; set; } = true;
    }
}
