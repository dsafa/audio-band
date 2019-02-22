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
        private int _maximum;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnhancedProgressBar"/> class.
        /// </summary>
        public EnhancedProgressBar()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

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
                Refresh();
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
                Refresh();
            }
        }

        [Browsable(true)]
        [Bindable(BindableSupport.Yes)]
        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                Refresh();
            }
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
            else if (progress >= Maximum)
            {
                progress = 1;
            }

            rect.Width = (int)(rect.Width * progress);
            e.Graphics.FillRectangle(new SolidBrush(ForeColor), 0, 0, rect.Width, rect.Height);
        }

        protected override void OnDpiChangedBeforeParent(EventArgs e)
        {
            base.OnDpiChangedBeforeParent(e);
            var a = DeviceDpi;
        }
    }
}
