using System;
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
        private double _scaling = 1;

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
        [Bindable(BindableSupport.Yes)]
        public Image AlbumArt { get; set; }

        /// <summary>
        /// Gets or sets the x position of the tooltip.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public int XPosition { get; set; }

        /// <summary>
        /// Gets or sets the margin of the tooltip.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public int Margin { get; set; }

        /// <summary>
        /// Gets or sets the size of the tooltip.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
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
        /// <param name="scaling">The scaling factor.</param>
        public void ShowWithoutRequireFocus(string name, MainControl control, TaskbarInfo taskbarInfo, double scaling)
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

            _scaling = scaling;
            const int TooltipTypeAbsolutePos = 2;
            var controlScreenLocation = control.PointToScreen(new Point(0, 0));
            var popupLocation = new Point(controlScreenLocation.X + (int)Math.Round(XPosition * scaling), controlScreenLocation.Y + (int)Math.Round(yOffset * scaling));
            _setToolMethod.Invoke(this, new object[] { control, name, TooltipTypeAbsolutePos, popupLocation });
        }

        private void OnPopup(object sender, PopupEventArgs popupEventArgs)
        {
            popupEventArgs.ToolTipSize = new Size((int)Math.Round(Size.Width * _scaling), (int)Math.Round(Size.Height * _scaling));
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
