using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace AudioBand
{
    public class AlbumArtTooltip : ToolTip
    {
        private Image _albumArt;
        private readonly MethodInfo _setToolMethod = typeof(ToolTip).GetMethod("SetTool", BindingFlags.Instance | BindingFlags.NonPublic);

        public Image AlbumArt
        {
            get => _albumArt;
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

        public void ShowWithoutRequireFocus(string name, Control control, Point relativePosition)
        {
            if (AlbumArt == null)
            {
                return;
            }

            const int AbsolutePos = 2;
            var point = control.PointToScreen(new Point(0, 0));
            _setToolMethod.Invoke(this, new object[] { control, name, AbsolutePos, new Point(point.X + relativePosition.X, point.Y + relativePosition.Y) });
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
