using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioBand.Views.Winforms
{
    /// <summary>
    /// Control for the album art
    /// </summary>
    public class AlbumArtDisplay : AudioBandControl
    {
        private Image _albumArt;

        /// <summary>
        /// Gets or sets the album art
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public Image AlbumArt
        {
            get => _albumArt;
            set
            {
                _albumArt = value;
                InvokeRefresh();
            }
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (AlbumArt == null)
            {
                return;
            }

            Graphics graphics = e.Graphics;
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            graphics.DrawImage(AlbumArt, e.ClipRectangle);
        }
    }
}
