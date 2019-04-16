using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AudioBand.Views.Winforms
{
    /// <summary>
    /// A enhanced progress bar supporting custom colors.
    /// </summary>
    public class EnhancedProgressBar : AudioBandControl
    {
        private TimeSpan _progress;
        private TimeSpan _total;
        private Color _hoverColor;
        private bool _isHovered;

        /// <summary>
        /// Gets or sets the current progress.
        /// </summary>
        [Browsable(true)]
        [Bindable(BindableSupport.Yes)]
        public TimeSpan Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                InvokeRefresh();
            }
        }

        /// <summary>
        /// Gets or sets the total time of the progress bar.
        /// </summary>
        [Browsable(true)]
        [Bindable(BindableSupport.Yes)]
        public TimeSpan Total
        {
            get => _total;
            set
            {
                _total = value;
                InvokeRefresh();
            }
        }

        /// <summary>
        /// Gets or sets the hover color
        /// </summary>
        [Browsable(true)]
        [Bindable(BindableSupport.Yes)]
        public Color HoverColor
        {
            get => _hoverColor;
            set
            {
                _hoverColor = value;
                InvokeRefresh();
            }
        }

        /// <inheritdoc />
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            InvokeRefresh();
        }

        /// <inheritdoc />
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            InvokeRefresh();
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = e.ClipRectangle;

            var progress = Progress.TotalSeconds / Total.TotalSeconds;
            if (progress <= 0)
            {
                progress = 0;
            }
            else if (progress >= 1)
            {
                progress = 1;
            }

            var foregroundBrush = _isHovered ? new SolidBrush(HoverColor) : new SolidBrush(ForeColor);

            rect.Width = (int)(rect.Width * progress);
            e.Graphics.FillRectangle(foregroundBrush, 0, 0, rect.Width, rect.Height);
        }
    }
}
