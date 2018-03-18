using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSDeskBand;
using CSDeskBand.Win;
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
            audioProgress.Value = 50;

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
    }
}
