using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using CSDeskBand;
using Size = System.Drawing.Size;

namespace AudioBand.Views.Winforms
{
    /// <summary>
    /// The tooltip for the album art.
    /// </summary>
    public class AlbumArtTooltip : AudioBandTooltip
    {
        private double _scaling = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumArtTooltip"/> class.
        /// </summary>
        public AlbumArtTooltip()
            : base("AlbumArtTooltip")
        {
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

        /// <summary>
        /// Show the tooltip without needing focus.
        /// </summary>
        /// <param name="control">The parent control.</param>
        /// <param name="taskbarInfo">The taskbar information.</param>
        /// <param name="scaling">The scaling factor.</param>
        public void Show(MainControl control, TaskbarInfo taskbarInfo, double scaling)
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
            var controlScreenLocation = control.PointToScreen(new Point(0, 0));
            var popupLocation = new Point(controlScreenLocation.X + (int)Math.Round(XPosition * scaling), controlScreenLocation.Y + (int)Math.Round(yOffset * scaling));
            Show(control, PositionType.Absolute, popupLocation);
        }

        /// <inheritdoc />
        protected override void OnPopup(PopupEventArgs popupEventArgs)
        {
            popupEventArgs.ToolTipSize = new Size((int)Math.Round(Size.Width * _scaling), (int)Math.Round(Size.Height * _scaling));
        }

        /// <inheritdoc />
        protected override void OnDraw(DrawToolTipEventArgs drawToolTipEventArgs)
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
