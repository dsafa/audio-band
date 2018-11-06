using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioBand.Settings.Models.v1
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
