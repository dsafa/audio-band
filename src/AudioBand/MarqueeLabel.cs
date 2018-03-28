using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace AudioBand
{
    public class MarqueeLabel : Label
    {
        private bool _scrolling;
        private int _textWidth;
        private int _nowPlayingXPos;
        private int _duplicateXPos;
        private readonly Timer _nowPlayingTimer = new Timer { Interval = 20 };
        private const int TextMargin = 10;
        private Rectangle _clipRectangle;

        public MarqueeLabel()
        {
            UpdateClipRectangle();
            _nowPlayingTimer.Tick += NowPlayingTimerOnTick;
            TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, EventArgs eventArgs)
        {
            using (var graphics = CreateGraphics())
            {
                var size = graphics.MeasureString(Text, Font);
                _textWidth = (int) size.Width + 1;

                if (size.Width <= _clipRectangle.Width)
                {
                    _scrolling = false;
                    _nowPlayingTimer.Stop();
                    return;
                }
            }

            _nowPlayingXPos = _clipRectangle.Width / 4;
            _duplicateXPos = _nowPlayingXPos + _textWidth + TextMargin;
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
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            if (!_scrolling)
            {
                base.OnPaint(e);
                return;
            }

            var graphics = e.Graphics;

            var textBrush = new SolidBrush(ForeColor);
            graphics.Clear(BackColor);
            graphics.DrawString(Text, Font, textBrush , _nowPlayingXPos, e.ClipRectangle.Y);
            graphics.DrawString(Text, Font, textBrush, _duplicateXPos, e.ClipRectangle.Y);

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

            using (var graphics = CreateGraphics())
            {
                var size = graphics.MeasureString(Text, Font);
                _textWidth = (int)size.Width + 1;
            }
        }

        private void UpdateClipRectangle()
        {
            _clipRectangle = RectangleToClient(ClientRectangle);
        }

        private void UpdateTextPositions()
        {
            if (_nowPlayingXPos + _textWidth + TextMargin < 0)
            {
                _nowPlayingXPos = _duplicateXPos + _textWidth + TextMargin;
            }

            if (_duplicateXPos + _textWidth + TextMargin < 0)
            {
                _duplicateXPos = _nowPlayingXPos + _textWidth + TextMargin;
            }

            _nowPlayingXPos--;
            _duplicateXPos--;
        }
    }
}
