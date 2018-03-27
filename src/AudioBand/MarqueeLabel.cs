using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace AudioBand
{
    public class MarqueeLabel : Label
    {
        // Have some issues getting the correct size of the control
        public int MaxWidth { get; set; }

        private bool _scrolling;
        private int _textWidth;
        private int _nowPlayingXPos;
        private int _duplicateXPos;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private readonly Timer _nowPlayingTimer = new Timer { Interval = 20 };
        private const int TextMargin = 10;

        public MarqueeLabel()
        {
            _nowPlayingTimer.Tick += NowPlayingTimerOnTick;
            TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, EventArgs eventArgs)
        {
            using (var graphics = CreateGraphics())
            {
                var size = graphics.MeasureString(Text, Font);
                if (size.Width <= MaxWidth)
                {
                    _scrolling = false;
                    _nowPlayingTimer.Stop();
                    _tokenSource.Cancel();
                    return;
                }

                _textWidth = (int) size.Width + 1;
            }

            _nowPlayingXPos = MaxWidth / 4;
            _duplicateXPos = _nowPlayingXPos + _textWidth + TextMargin;
            _scrolling = true;

            _tokenSource = new CancellationTokenSource();
            _nowPlayingTimer.Start();
        }

        private void NowPlayingTimerOnTick(object sender, EventArgs eventArgs)
        {
            BeginInvoke(new Action(() =>
            {
                if (_nowPlayingXPos <= -_textWidth - TextMargin)
                {
                    _nowPlayingXPos = _textWidth + TextMargin;
                }

                if (_duplicateXPos <= -_textWidth - TextMargin)
                {
                    _duplicateXPos = _nowPlayingXPos + _textWidth + TextMargin;
                }

                _nowPlayingXPos--;
                _duplicateXPos--;
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
    }
}
