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
    public partial class MainControl : CSDeskBandWin
    {
        private const int FixedWidth = 250;
        private readonly int _maxHeight = CSDeskBandOptions.TaskbarHorizontalHeightLarge;
        private readonly int _minHeight = CSDeskBandOptions.TaskbarHorizontalHeightSmall;
        private readonly SvgDocument _playButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.play));
        private readonly SvgDocument _pauseButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.pause));
        private readonly SvgDocument _nextButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.next));
        private readonly SvgDocument _previousButtonSvg = SvgDocument.Open<SvgDocument>(new MemoryStream(Properties.Resources.previous));
        private readonly AudioBandViewModel _audioBandViewModel = new AudioBandViewModel();
        private readonly Image _albumArt = new Bitmap(1, 1);

        public MainControl()
        {
            InitializeComponent();

            Options.Fixed = true;
            Options.Increment = 0;
            Options.Horizontal = Size = new Size(FixedWidth, _maxHeight);
            Options.MinHorizontal = MinimumSize = new Size(FixedWidth, _minHeight);
            Options.MaxHorizontal = MaximumSize = Size;

            SizeChanged += OnSizeChanged;
            playPauseButton.Click += PlayPauseButtonOnClick;
            previousButton.Click += PreviousButtonOnClick;
            nextButton.Click += NextButtonOnClick;
            _audioBandViewModel.PropertyChanged += AudioBandViewModelOnPropertyChanged;

            nowPlayingText.DataBindings.Add("Text", _audioBandViewModel, nameof(AudioBandViewModel.IsPlaying));
            albumArt.DataBindings.Add("Image", _audioBandViewModel, nameof(AudioBandViewModel.AlbumArt));
            audioProgress.DataBindings.Add("Value", _audioBandViewModel, nameof(AudioBandViewModel.AudioProgress));
            previousButton.DataBindings.Add("Image", _audioBandViewModel, nameof(AudioBandViewModel.PreviousButtonBitmap));
            playPauseButton.DataBindings.Add("Image", _audioBandViewModel, nameof(AudioBandViewModel.PlayPauseButtonBitmap));
            nextButton.DataBindings.Add("Image", _audioBandViewModel, nameof(AudioBandViewModel.NextButtonBitmap));
        }

        private void AudioBandViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case nameof(AudioBandViewModel.IsPlaying):
                    UpdateControlSvgs();
                    break;
                default: break;
            }
        }

        private void PlayPauseButtonOnClick(object sender, EventArgs eventArgs)
        {
            _audioBandViewModel.IsPlaying = !_audioBandViewModel.IsPlaying;
        }

        private void PreviousButtonOnClick(object sender, EventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void NextButtonOnClick(object sender, EventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            UpdateAlbumArt();
            UpdateControlSvgs();
        }

        private void UpdateAlbumArt()
        {
            var height = mainTable.GetRowHeights().Take(2).Sum();
            mainTable.ColumnStyles[0].SizeType = SizeType.Absolute;
            mainTable.ColumnStyles[0].Width = height;

            var newAlbumArt = new Bitmap(height, height);
            using (var graphics = Graphics.FromImage(newAlbumArt))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(_albumArt, 0, 0, newAlbumArt.Width, newAlbumArt.Height);
            }

            _audioBandViewModel.AlbumArt = newAlbumArt;
        }

        private void UpdateControlSvgs()
        {
            // Issues with svg
            const int padding = 3;
            var height = buttonsTable.GetRowHeights()[0] - padding;

            SvgDocument playPauseSvg = _audioBandViewModel.IsPlaying ? _pauseButtonSvg : _playButtonSvg;
            playPauseSvg.Width = playPauseButton.Width;
            playPauseSvg.Height = height;
            _audioBandViewModel.PlayPauseButtonBitmap = DrawSvg(playPauseSvg);

            _nextButtonSvg.Width = nextButton.Width;
            _nextButtonSvg.Height = height;
            _audioBandViewModel.NextButtonBitmap = DrawSvg(_nextButtonSvg);

            _previousButtonSvg.Width = previousButton.Width;
            _previousButtonSvg.Height = height;
            _audioBandViewModel.PreviousButtonBitmap = DrawSvg(_previousButtonSvg);
        }

        private Bitmap DrawSvg(SvgDocument svg)
        {
            var bmp = new Bitmap((int)svg.Width.Value, (int)svg.Height.Value);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.High;
                svg.Draw(graphics);
                return bmp;
            }
        }
    }
}
