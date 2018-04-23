using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioBand
{
    // So we can draw with our own color
    public class EnhancedProgressBar : ProgressBar
    {
        private double _progress;

        public double Progress
        {
            get => _progress;
            set
            {
                try
                {
                    var progress = Convert.ToInt32(value);
                    if (progress <= Minimum)
                    {
                        _progress = Minimum;
                    }
                    else if (progress >= Maximum)
                    {
                        _progress = Maximum;
                    }
                    else
                    {
                        _progress = value;
                    }
                }
                catch (OverflowException)
                {
                    _progress = Minimum;
                }
                
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
            rect.Width = (int)(rect.Width * Progress / Maximum);
            e.Graphics.FillRectangle(new SolidBrush(ForeColor), 0, 0, rect.Width, rect.Height);
        }
    }
}
