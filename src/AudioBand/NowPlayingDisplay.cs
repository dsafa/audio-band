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
                CheckNowPlayingText();
                Refresh();
            }
        }

        public Font ArtistFont
        {
            get => _artistFont;
            set
            {
                if (Equals(value, _artistFont))
                {
                    return;
                }

                _artistFont = value;
                CheckNowPlayingText();
                Refresh();
            }
        }

        public Color ArtistColor
        {
            get => _artistColor;
            set
            {
                if (value.Equals(_artistColor))
                {
                    return;
                }

                _artistColor = value;
                UpdateBrushes(_nowPlayingTimer.Enabled);
                Refresh();
            }
        }

        public Font TrackNameFont
        {
            get => _trackNameFont;
            set
            {
                if (Equals(value, _trackNameFont))
                {
                    return;
                }

                _trackNameFont = value;
                CheckNowPlayingText();
                Refresh();
            }
        }

        public Color TrackNameColor
        {
            get => _trackNameColor;
            set
            {
                if (value.Equals(_trackNameColor))
                {
                    return;
                }

                _trackNameColor = value;
                UpdateBrushes(_nowPlayingTimer.Enabled);
                Refresh();
            }
        }

        private NowPlayingText _nowPlayingText;
        private bool _scrolling;
        private int _nowPlayingTextWidth;
        private int _nowPlayingXPos;
        private int _duplicateXPos;
        private readonly Timer _nowPlayingTimer = new Timer { Interval = 20 };
        private const int TextMargin = 60; //Spacing between scrolling text
        private Font _artistFont = new Font("Segoe UI", 8.5f);
        private Color _artistColor = Color.SlateGray;
        private Brush _artistBrush = new SolidBrush(Color.SlateGray);
        private Font _trackNameFont = new Font("Segoe UI", 8.5f);
        private Color _trackNameColor = Color.White;
        private Brush _songBrush = new SolidBrush(Color.White);

        public NowPlayingDisplay()
        {
            DoubleBuffered = true;
            _nowPlayingTimer.Tick += NowPlayingTimerOnTick;
        }

        private void CheckNowPlayingText()
        {
            using (var graphics = CreateGraphics())
            {
                var textSize = MeasureNowPlayingText(graphics);
                _nowPlayingTextWidth = (int) textSize.Width + 1;

                if (textSize.Width <= ClientRectangle.Width)
                {
                    _scrolling = false;
                    _nowPlayingTimer.Stop();
                    UpdateBrushes(false);
                    return;
                }
            }

            if (_nowPlayingTimer.Enabled)
            {
                return;
            }

            _nowPlayingXPos = ClientRectangle.Width / 4;
            _duplicateXPos = _nowPlayingXPos + _nowPlayingTextWidth + TextMargin;
            _scrolling = true;

            UpdateBrushes(true);
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
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _nowPlayingTimer.Stop();
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
                x = ClientRectangle.Width / 2 - size.Width / 2;
            }

            var artistText = NowPlayingText.Artist;
            var trackNameText = NowPlayingText.TrackName;
            var artistTextSize = graphics.MeasureString(artistText, ArtistFont);
            var y = 0;

            graphics.DrawString(artistText, ArtistFont, _artistBrush, x.Value, y);
            graphics.DrawString(trackNameText, TrackNameFont, _songBrush, x.Value + artistTextSize.Width, y);
        }

        private SizeF MeasureNowPlayingText(Graphics graphics)
        {
            var artistSize = graphics.MeasureString(_nowPlayingText.Artist, ArtistFont);
            var trackNameSize = graphics.MeasureString(_nowPlayingText.TrackName, Font);
            return artistSize + trackNameSize;
        }

        private void UpdateBrushes(bool fade)
        {
            if (!fade)
            {
                _artistBrush = new SolidBrush(ArtistColor);
                _songBrush = new SolidBrush(TrackNameColor);
                return;
            }

            _artistBrush = CreateFadeBrush(ArtistColor);
            _songBrush = CreateFadeBrush(TrackNameColor);
        }

        private LinearGradientBrush CreateFadeBrush(Color color)
        {
            return new LinearGradientBrush(ClientRectangle, color, color, LinearGradientMode.Horizontal)
            {
                InterpolationColors = new ColorBlend(4)
                {
                    Colors = new[] { Color.FromArgb(0, 0, 0, 0), color, color, Color.FromArgb(0, 0, 0, 0) },
                    Positions = new[] { 0, 0.1f, 0.9f, 1 }
                }
            };
        }
    }
}
