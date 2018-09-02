using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using AudioBand.ViewModels;
using Timer = System.Windows.Forms.Timer;

namespace AudioBand
{
    public class FormattedTextLabel : Control
    {
        public string Format
        {
            get => _format;
            set
            {
                _format = value;
                _renderer.Format = value;
                Refresh();
            }
        }

        public Color DefaultColor
        {
            get => _defaultColor;
            set
            {
                _defaultColor = value;
                _renderer.DefaultColor = value;
                Refresh();
            }
        }

        public float FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                _renderer.FontSize = value;
                Refresh();
            }
        }

        public string FontFamily
        {
            get => _fontFamily;
            set
            {
                _fontFamily = value;
                _renderer.FontFamily = value;
                Refresh();
            }
        }

        public TextAlignment Alignment
        {
            get => _alignment;
            set
            {
                _alignment = value;
                _renderer.Alignment = value;
                Refresh();
            }
        }

        public string Artist
        {
            get => _artist;
            set
            {
                _artist = value;
                _renderer.Artist = value;
                Refresh();
            }
        }

        public string SongName
        {
            get => _songName;
            set
            {
                _songName = value;
                _renderer.SongName = value;
                Refresh();
            }
        }

        public string AlbumName
        {
            get => _albumName;
            set
            {
                _albumName = value;
                _renderer.AlbumName = value;
                Refresh();
            }
        }

        public TimeSpan SongProgress
        {
            get => _songProgress;
            set
            {
                _songProgress = value;
                _renderer.SongProgress = value;
                Refresh();
            }
        }

        public TimeSpan SongLength
        {
            get => _songLength;
            set
            {
                _songLength = value;
                _renderer.SongLength = value;
                Refresh();
            }
        }

        public int ScrollSpeed
        {
            get => _scrollSpeed;
            set
            {
                _scrollSpeed = value;
                Refresh();
            }
        }

        private readonly Timer _scrollingTimer = new Timer { Interval = 20};
        private const int TextMargin = 60; //Spacing between scrolling text
        private int _textXPos;
        private int _duplicateXPos; // Draw 2 labels so that there wont be a gap between
        private int _textWidth;

        private FormattedTextRenderer _renderer;
        private string _format;
        private Color _defaultColor;
        private float _fontSize;
        private string _fontFamily;
        private TextAlignment _alignment;
        private string _artist;
        private string _songName;
        private string _albumName;
        private TimeSpan _songProgress;
        private TimeSpan _songLength;
        private int _scrollSpeed;

        public FormattedTextLabel(string format, Color defaultColor, float fontSize, string fontFamily, TextAlignment alignment)
        {
            DoubleBuffered = true;
            _renderer = new FormattedTextRenderer(format, defaultColor, fontSize, fontFamily, alignment);
            _scrollingTimer.Tick += ScrollingTimerOnTick;
        }

        private void ScrollingTimerOnTick(object sender, EventArgs eventArgs)
        {
            BeginInvoke(new Action(() =>
            {
                UpdateTextPositions();
                Refresh();
            }));
        }

        private void UpdateTextPositions()
        {
            if (_textXPos + _textWidth + TextMargin < 0)
            {
                _textXPos = _duplicateXPos + _textWidth + TextMargin;
            }

            if (_duplicateXPos + _textWidth + TextMargin < 0)
            {
                _duplicateXPos = _textXPos + _textWidth + TextMargin;
            }

            _textXPos -= ScrollSpeed;
            _duplicateXPos -= ScrollSpeed;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            _renderer.Draw(graphics, _textXPos);
            _textWidth = _renderer.TextLength;

            if (_scrollingTimer.Enabled)
            {
                _renderer.Draw(graphics, _duplicateXPos);
            }

            if (_textWidth > ClientRectangle.Width && !_scrollingTimer.Enabled)
            {
                _scrollingTimer.Start();
            }
            else if (_textWidth <= ClientRectangle.Width && _scrollingTimer.Enabled)
            {
                _scrollingTimer.Stop();
                _textXPos = 0;
            }

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _scrollingTimer.Stop();
        }
    }
}
