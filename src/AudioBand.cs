using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSDeskBand;
using CSDeskBand.Win;
using Svg;
using Size = System.Drawing.Size;

namespace AudioBand
{
    [Guid("957D8782-5B07-4126-9B24-1E917BAAAD64")]
    [ComVisible(true)]
    [CSDeskBandRegistration(Name = "Audio Band")]
    public partial class AudioBand : CSDeskBandWin
    {
        private const int FixedWidth = 250;
        private readonly int MaxHeight = CSDeskBandOptions.TaskbarHorizontalHeightLarge;
        private readonly int MinHeight = CSDeskBandOptions.TaskbarHorizontalHeightSmall;
        private readonly EnhancedProgressBar audioProgress = new EnhancedProgressBar();
        private readonly SvgDocument _playButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.play));
        private readonly SvgDocument _nextButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.next));
        private readonly SvgDocument _previousButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.previous));

        public AudioBand()
        {
            InitializeComponent();

            mainTable.Controls.Add(audioProgress, 0, 2);
            mainTable.SetColumnSpan(audioProgress, 2);
            audioProgress.Dock = DockStyle.Fill;
            audioProgress.ForeColor = Color.DodgerBlue;
            audioProgress.Location = new Point(0, 28);
            audioProgress.Margin = new Padding(0);
            audioProgress.Name = "audioProgress";
            audioProgress.Size = new Size(300, 2);
            audioProgress.TabIndex = 3;
            audioProgress.Value = 100;

            Options.Fixed = true;
            Options.Increment = 0;
            Options.Horizontal = Size = new Size(FixedWidth, MaxHeight);
            Options.MinHorizontal = MinimumSize = new Size(FixedWidth, MinHeight);
            Options.MaxHorizontal = MaximumSize = Size;

            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            UpdateAlbumArt();
            DrawControlSvgs();
        }

        private void UpdateAlbumArt()
        {
            var length = mainTable.GetRowHeights().Take(2).Sum();
            nowPlayingText.Text = length + "";
            mainTable.ColumnStyles[0].SizeType = SizeType.Absolute;
            mainTable.ColumnStyles[0].Width = length;
            nowPlayingText.Text += " " + mainTable.ColumnStyles[1].Width;

            albumArt.Image = new Bitmap(length, length);
            using (var g = Graphics.FromImage(albumArt.Image))
            {
                g.Clear(Color.Red);
            }
        }

        private void DrawControlSvgs()
        {
            // Issues with svg
            const int padding = 3;
            var height = buttonsTable.GetRowHeights()[0] - padding;

            _playButtonSvg.Width = height;
            _playButtonSvg.Height = height;
            playPauseButton.Image = DrawSvg(_playButtonSvg);

            _nextButtonSvg.Width = nextButton.Width;
            _nextButtonSvg.Height = height;
            nextButton.Image = DrawSvg(_nextButtonSvg);

            _previousButtonSvg.Width = previousButton.Width;
            _previousButtonSvg.Height = height;
            previousButton.Image = DrawSvg(_previousButtonSvg);
        }

        private Bitmap DrawSvg(SvgDocument svg)
        {
            var bmp = new Bitmap((int)svg.Width.Value, (int)svg.Height.Value);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.High;
                svg.Draw(g);
                return bmp;
            }
        }
    }
}
