using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace AudioBand
{
    public struct NowPlayingText
    {
        public string Artist { get; set; }
        public string TrackName { get; set; }
    }

    public class NowPlayingDisplay : Control
    {
        public NowPlayingText NowPlayingText
        {
            get => _nowPlayingText;
            set
            {
                if (value.Equals(_nowPlayingText))
                {
                    return;
                }

                _nowPlayingText = value;
                OnNowPlayingTextChanged();
                Refresh();
            }
        }

        public Font ArtistFont { get; set; }
        public Color ArtistColor { get; set; }

        private NowPlayingText _nowPlayingText;
        private bool _scrolling;
        private int _nowPlayingTextWidth;
        private int _nowPlayingXPos;
        private int _duplicateXPos;
        private readonly Timer _nowPlayingTimer = new Timer { Interval = 20 };
        private const int TextMargin = 60; //Spacing between scrolling text
        private Rectangle _clipRectangle; // Real size

        public NowPlayingDisplay()
        {
            DoubleBuffered = true;
            UpdateClipRectangle();
            _nowPlayingTimer.Tick += NowPlayingTimerOnTick;
        }

        private void OnNowPlayingTextChanged()
        {
            using (var graphics = CreateGraphics())
            {
                var textSize = MeasureNowPlayingText(graphics);
                _nowPlayingTextWidth = (int) textSize.Width + 1;

                if (textSize.Width <= _clipRectangle.Width)
                {
                    _scrolling = false;
                    _nowPlayingTimer.Stop();
                    return;
                }
            }

            _nowPlayingXPos = _clipRectangle.Width / 4;
            _duplicateXPos = _nowPlayingXPos + _nowPlayingTextWidth + TextMargin;
            _scrolling = true;

            _nowPlayingTimer.Start();
        }

        private void NowPlayingTimerOnTick(object sender, EventArgs eventArgs)
        {
            BeginInvoke(new Action(() =>
            {
                UpdateTextPositions();
                Refresh();
            }));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            if (!_scrolling)
            {
                DrawNowPlayingText(e.Graphics, null);
                return;
            }

            DrawNowPlayingText(graphics, _nowPlayingXPos);
            DrawNowPlayingText(graphics, _duplicateXPos);

            var edgeBrush = new LinearGradientBrush(e.ClipRectangle, BackColor, BackColor, LinearGradientMode.Horizontal)
            {
                InterpolationColors = new ColorBlend(4)
                {
                    Colors = new[] {BackColor, Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), BackColor},
                    Positions = new[] {0, 0.1f, 0.9f, 1}
                }
            };
            graphics.FillRectangle(edgeBrush, e.ClipRectangle);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _nowPlayingTimer.Stop();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateClipRectangle();
        }

        private void UpdateClipRectangle()
        {
            _clipRectangle = RectangleToClient(ClientRectangle);
        }

        private void UpdateTextPositions()
        {
            if (_nowPlayingXPos + _nowPlayingTextWidth + TextMargin < 0)
            {
                _nowPlayingXPos = _duplicateXPos + _nowPlayingTextWidth + TextMargin;
            }

            if (_duplicateXPos + _nowPlayingTextWidth + TextMargin < 0)
            {
                _duplicateXPos = _nowPlayingXPos + _nowPlayingTextWidth + TextMargin;
            }

            _nowPlayingXPos--;
            _duplicateXPos--;
        }

        private void DrawNowPlayingText(Graphics graphics, float? x)
        {
            if (x == null)
            {
                var size = MeasureNowPlayingText(graphics);
                x = _clipRectangle.Width / 2 - size.Width / 2;
            }

            var artistText = NowPlayingText.Artist;
            var trackNameText = NowPlayingText.TrackName;
            var artistTextSize = graphics.MeasureString(artistText, ArtistFont);
            var y = 0;

            graphics.DrawString(artistText, ArtistFont, new SolidBrush(ArtistColor), x.Value, y);
            graphics.DrawString(trackNameText, Font, new SolidBrush(ForeColor), x.Value + artistTextSize.Width, y);
        }

        private SizeF MeasureNowPlayingText(Graphics graphics)
        {
            var artistSize = graphics.MeasureString(_nowPlayingText.Artist, ArtistFont);
            var trackNameSize = graphics.MeasureString(_nowPlayingText.TrackName, Font);
            return artistSize + trackNameSize;
        }
    }
}
