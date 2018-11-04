using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using AudioBand.Extensions;
using CSDeskBand;
using Size = System.Drawing.Size;

namespace AudioBand.Views.Winforms
{
    public class AlbumArtTooltip : ToolTip
    {
        private readonly MethodInfo _setToolMethod = typeof(ToolTip).GetMethod("SetTool", BindingFlags.Instance | BindingFlags.NonPublic);

        private Image _albumArt;
        private Size _size;

        public AlbumArtTooltip()
        {
            OwnerDraw = true;
            IsBalloon = false;
            ShowAlways = true;
            AutomaticDelay = 200;

            Popup += OnPopup;
            Draw += OnDraw;
        }

        public void ShowWithoutRequireFocus(string name, MainControl control, TaskbarInfo taskbarInfo)
        {
            if (!control.DataBindings[nameof(MainControl.AlbumArtPopupIsVisible)].As<bool>())
            {
                return;
            }

            // since tooltips dont support binding, we will bind to the main control and just read the properties here
            _albumArt = control.DataBindings[nameof(MainControl.AlbumArtPopupImage)].As<Image>();
            _size = new Size(control.DataBindings[nameof(MainControl.AlbumArtPopupWidth)].As<int>(), control.DataBindings[nameof(MainControl.AlbumArtPopupHeight)].As<int>());

            var margin = control.DataBindings[nameof(MainControl.AlbumArtPopupY)].As<int>();
            var xOffSet = control.DataBindings[nameof(MainControl.AlbumArtPopupX)].As<int>();
            int yOffset = 0;

            if (taskbarInfo.Edge == Edge.Top)
            {
                yOffset = control.Height + margin;
            }
            else
            {
                yOffset = -_size.Height - margin;
            }

            const int AbsolutePos = 2;
            var controlScreenLocation = control.PointToScreen(new Point(0, 0));
            _setToolMethod.Invoke(this, new object[] { control, name, AbsolutePos, new Point(controlScreenLocation.X + xOffSet, controlScreenLocation.Y + yOffset) });
        }

        private void OnPopup(object sender, PopupEventArgs popupEventArgs)
        {
            popupEventArgs.ToolTipSize = _size;
        }

        private void OnDraw(object sender, DrawToolTipEventArgs drawToolTipEventArgs)
        {
            if (_albumArt == null)
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
            graphics.DrawImage(_albumArt, bounds);
        }
    }
}
