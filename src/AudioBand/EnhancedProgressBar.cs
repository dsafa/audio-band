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
        public EnhancedProgressBar()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;
            rec.Width = (int)(rec.Width * Value / (double)Maximum);
            e.Graphics.FillRectangle(new SolidBrush(ForeColor), 0, 0, rec.Width, rec.Height);
        }
    }
}
