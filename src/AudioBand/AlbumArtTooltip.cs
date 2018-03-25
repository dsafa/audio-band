using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioBand
{
    public class AlbumArtTooltip : ToolTip
    {
        private Image _albumArt;

        public Image AlbumArt
        {
            get { return _albumArt; }
            set
            {
                _albumArt = value;
                Active = value != null;
            }
        }

        public Size Size { get; set; } = new Size(200, 200);

        public AlbumArtTooltip()
        {
            OwnerDraw = true;
            IsBalloon = false;
            ShowAlways = true;
            AutomaticDelay = 200;

            Popup += OnPopup;
            Draw += OnDraw;
        }

        private void OnPopup(object sender, PopupEventArgs popupEventArgs)
        {
            popupEventArgs.ToolTipSize = Size;
        }

        private void OnDraw(object sender, DrawToolTipEventArgs drawToolTipEventArgs)
        {
            if (AlbumArt == null)
            {
                return;
            }

            var graphics = drawToolTipEventArgs.Graphics;
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var bounds = drawToolTipEventArgs.Bounds;
            graphics.DrawImage(AlbumArt, bounds);
        }
    }
}
