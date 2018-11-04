using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AudioBand.Views.Winforms
{
    // So we can draw with our own color
    public class EnhancedProgressBar : ProgressBar
    {
        private TimeSpan _progress;
        private TimeSpan _total;

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

        public EnhancedProgressBar()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

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
    }
}
