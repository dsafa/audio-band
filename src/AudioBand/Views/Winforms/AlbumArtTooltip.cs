using CSDeskBand;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using Size = System.Drawing.Size;

namespace AudioBand.Views.Winforms
{
    public class AlbumArtTooltip : ToolTip, IBindableComponent
    {
        private readonly MethodInfo _setToolMethod = typeof(ToolTip).GetMethod("SetTool", BindingFlags.Instance | BindingFlags.NonPublic);

        public Image AlbumArt { get; set; }
        public int XPosition { get; set; }
        public int Margin { get; set; }
        public Size Size { get; set; }

        [Browsable(false)]
        public BindingContext BindingContext { get; set; } = new BindingContext();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlBindingsCollection DataBindings { get; }

        public AlbumArtTooltip()
        {
            Popup += OnPopup;
            Draw += OnDraw;
            DataBindings = new ControlBindingsCollection(this);
        }

        public void ShowWithoutRequireFocus(string name, MainControl control, TaskbarInfo taskbarInfo)
        {
            if (!Active)
            {
                return;
            }

            int yOffset = 0;

            if (taskbarInfo.Edge == Edge.Top)
            {
                yOffset = control.Height + Margin;
            }
            else
            {
                yOffset = -Size.Height - Margin;
            }

            const int AbsolutePos = 2;
            var controlScreenLocation = control.PointToScreen(new Point(0, 0));
            _setToolMethod.Invoke(this, new object[] { control, name, AbsolutePos, new Point(controlScreenLocation.X + XPosition, controlScreenLocation.Y + yOffset) });
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
