using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using CSDeskBand;
using Size = System.Drawing.Size;

namespace AudioBand.Views.Winforms
{
    /// <summary>
    /// The tooltip for the album art.
    /// </summary>
    public class AlbumArtTooltip : ToolTip, IBindableComponent
    {
        private readonly MethodInfo _setToolMethod = typeof(ToolTip).GetMethod("SetTool", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumArtTooltip"/> class.
        /// </summary>
        public AlbumArtTooltip()
        {
            Popup += OnPopup;
            Draw += OnDraw;
            DataBindings = new ControlBindingsCollection(this);
        }

        /// <summary>
        /// Gets or sets the album art of the tooltip.
        /// </summary>
        public Image AlbumArt { get; set; }

        /// <summary>
        /// Gets or sets the x position of the tooltip.
        /// </summary>
        public int XPosition { get; set; }

        /// <summary>
        /// Gets or sets the margin of the tooltip.
        /// </summary>
        public int Margin { get; set; }

        /// <summary>
        /// Gets or sets the size of the tooltip.
        /// </summary>
        public Size Size { get; set; }

        /// <inheritdoc/>
        [Browsable(false)]
        public BindingContext BindingContext { get; set; } = new BindingContext();

        /// <inheritdoc/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlBindingsCollection DataBindings { get; }

        /// <summary>
        /// Show the tooltip without needing focus.
        /// </summary>
        /// <param name="name">Name of the tooltip</param>
        /// <param name="control">The parent control.</param>
        /// <param name="taskbarInfo">The taskbar information.</param>
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
