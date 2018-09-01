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

        private readonly Timer _nowPlayingTimer = new Timer { Interval = 20 };
        private const int TextMargin = 60; //Spacing between scrolling text

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

        public FormattedTextLabel(string format, Color defaultColor, float fontSize, string fontFamily, TextAlignment alignment)
        {
            DoubleBuffered = true;
            _renderer = new FormattedTextRenderer(format, defaultColor, fontSize, fontFamily, alignment);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            _renderer.Draw(graphics, 0);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _nowPlayingTimer.Stop();
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
