using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AudioBand.Extensions;
using AudioBand.Resources;

namespace AudioBand.Views.Winforms
{
    /// <summary>
    /// Custom button
    /// </summary>
    public class CustomButton : AudioBandControl
    {
        private IImage _image;
        private Color _defaultBackColor = Color.Transparent;

        /// <summary>
        /// Gets or sets the button's image.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public IImage Image
        {
            get => _image;
            set
            {
                _image = value;
                InvokeRefresh();
            }
        }

        /// <summary>
        /// Gets or sets the default back color
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public Color DefaultBackgroundColor
        {
            get => _defaultBackColor;
            set
            {
                _defaultBackColor = value;
                BackColor = _defaultBackColor;
                InvokeRefresh();
            }
        }

        /// <summary>
        /// Gets or sets the hover back color
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public Color HoveredBackgroundColor { get; set; } = Color.FromArgb(25, 255, 255, 255);

        /// <summary>
        /// Gets or sets the clicked back color
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public Color ClickedBackgroundColor { get; set; } = Color.FromArgb(15, 255, 255, 255);

        /// <inheritdoc/>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            BackColor = HoveredBackgroundColor;
        }

        /// <inheritdoc/>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            BackColor = DefaultBackgroundColor;
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                BackColor = ClickedBackgroundColor;
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                BackColor = HoveredBackgroundColor;
            }
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Image == null)
            {
                return;
            }

            Graphics graphics = e.Graphics;
            graphics.CompositingMode = CompositingMode.SourceOver;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var image = Image.GetScaledSize(Width, Height))
            {
                var centerX = (e.ClipRectangle.Width - image.Width) / 2;
                var centerY = (e.ClipRectangle.Height - image.Height) / 2;
                graphics.DrawImageUnscaled(image, new Point(centerX, centerY));
            }
        }
    }
}
