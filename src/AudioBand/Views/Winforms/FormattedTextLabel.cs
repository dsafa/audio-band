using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TextAlignment= AudioBand.Models.CustomLabel.TextAlignment;
using Timer = System.Windows.Forms.Timer;

namespace AudioBand.Views.Winforms
{
    /// <summary>
    /// A text label that supports custom formatting.
    /// </summary>
    public class FormattedTextLabel : AudioBandControl
    {
        private const int TickRateMs = 20;
        private const int TickPerS = 1000 / TickRateMs;
        private const int TextMargin = 60; // Spacing between scrolling text
        private const int WidthPadding = 4;
        private readonly Timer _scrollingTimer = new Timer { Interval = TickRateMs };
        private readonly FormattedTextRenderer _renderer;
        private float _textXPos;
        private float _duplicateXPos; // Draw 2 labels so that there wont be a gap between
        private int _textWidth;

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
        private float _scrollDelta;
        private Bitmap _renderedText;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormattedTextLabel"/> class
        /// with initial display values.
        /// </summary>
        /// <param name="format">The text format.</param>
        /// <param name="defaultColor">The default color.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="alignment">The text alignment.</param>
        public FormattedTextLabel(string format, Color defaultColor, float fontSize, string fontFamily, TextAlignment alignment)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            _renderer = new FormattedTextRenderer(format, defaultColor, fontSize, fontFamily, alignment);
            _scrollingTimer.Tick += ScrollingTimerOnTick;
        }

        /// <summary>
        /// Gets or sets the format of the label.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public string Format
        {
            get => _format;
            set
            {
                _format = value;
                _renderer.Format = value;
                Redraw();
            }
        }

        /// <summary>
        /// Gets or sets the default color of the text.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public Color DefaultColor
        {
            get => _defaultColor;
            set
            {
                _defaultColor = value;
                _renderer.DefaultColor = value;
                Redraw();
            }
        }

        /// <summary>
        /// Gets or sets the font size of the text.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public float FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                _renderer.FontSize = value;
                Redraw();
            }
        }

        /// <summary>
        /// Gets or sets the font family of the text.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public string FontFamily
        {
            get => _fontFamily;
            set
            {
                _fontFamily = value;
                _renderer.FontFamily = value;
                Redraw();
            }
        }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public TextAlignment Alignment
        {
            get => _alignment;
            set
            {
                _alignment = value;
                _renderer.Alignment = value;
                Redraw();
            }
        }

        /// <summary>
        /// Gets or sets the current artist to display.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public string Artist
        {
            get => _artist;
            set
            {
                _artist = value;
                _renderer.Artist = value;

                if (_renderer.Formats.HasFlag(FormattedTextRenderer.TextFormat.Artist))
                {
                    Redraw();
                }
            }
        }

        /// <summary>
        /// Gets or sets the current song name.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public string SongName
        {
            get => _songName;
            set
            {
                _songName = value;
                _renderer.SongName = value;

                if (_renderer.Formats.HasFlag(FormattedTextRenderer.TextFormat.Song))
                {
                    Redraw();
                }
            }
        }

        /// <summary>
        /// Gets or sets the current album name.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public string AlbumName
        {
            get => _albumName;
            set
            {
                _albumName = value;
                _renderer.AlbumName = value;

                if (_renderer.Formats.HasFlag(FormattedTextRenderer.TextFormat.Album))
                {
                    Redraw();
                }
            }
        }

        /// <summary>
        /// Gets or sets the current song progress.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public TimeSpan SongProgress
        {
            get => _songProgress;
            set
            {
                _songProgress = value;
                _renderer.SongProgress = value;

                if (_renderer.Formats.HasFlag(FormattedTextRenderer.TextFormat.CurrentTime))
                {
                    Redraw();
                }
            }
        }

        /// <summary>
        /// Gets or sets the current song length.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public TimeSpan SongLength
        {
            get => _songLength;
            set
            {
                _songLength = value;
                _renderer.SongLength = value;

                if (_renderer.Formats.HasFlag(FormattedTextRenderer.TextFormat.SongLength))
                {
                    Redraw();
                }
            }
        }

        /// <summary>
        /// Gets or sets the scroll speed of the label.
        /// </summary>
        [Bindable(BindableSupport.Yes)]
        public int ScrollSpeed
        {
            get => _scrollSpeed;
            set
            {
                _scrollSpeed = value;
                _scrollDelta = (float)_scrollSpeed / TickPerS;
                InvokeRefresh();
            }
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            // measure
            _textWidth = _renderer.Measure(ScalingFactor).Width;
            var textTooLong = _textWidth > ClientRectangle.Width - WidthPadding; // extra padding in case

            // to long and not scrolling -> start scrolling
            if (textTooLong && !_scrollingTimer.Enabled)
            {
                _duplicateXPos = _textXPos + _textWidth + TextMargin;
                _scrollingTimer.Start();
            }
            else if (!textTooLong && _scrollingTimer.Enabled)
            {
                // text no longer too long -> stop scrolling
                _scrollingTimer.Stop();
                _textXPos = GetNormalTextPos();
            }

            // if not scrolling get the x position for the text based on alignment
            if (!_scrollingTimer.Enabled)
            {
                _textXPos = GetNormalTextPos();
            }

            Draw(e.Graphics, (int)_textXPos);

            if (_scrollingTimer.Enabled)
            {
                Draw(e.Graphics, (int)_duplicateXPos);
            }
        }

        /// <inheritdoc/>
        protected override void OnDpiChanged()
        {
            base.OnDpiChanged();
            Redraw();
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _scrollingTimer.Stop();
        }

        private void ScrollingTimerOnTick(object sender, EventArgs eventArgs)
        {
            UpdateTextPositions();
            InvokeRefresh();
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

            _textXPos -= _scrollDelta;
            _duplicateXPos -= _scrollDelta;
        }

        /// <summary>
        /// Get the text position when not scrolling.
        /// </summary>
        /// <returns>The x position.</returns>
        private int GetNormalTextPos()
        {
            switch (Alignment)
            {
                case TextAlignment.Left:
                    return 0;
                case TextAlignment.Center:
                    return (ClientRectangle.Width - WidthPadding - _textWidth) / 2;
                case TextAlignment.Right:
                    return ClientRectangle.Width - WidthPadding - _textWidth;
            }

            return 0;
        }

        private void Redraw()
        {
            _renderedText = _renderer.Draw(ScalingFactor);
            InvokeRefresh();
        }

        private void Draw(Graphics g, int xpos)
        {
            var rect = ClientRectangle;
            rect.X = xpos;
            g.DrawImage(_renderedText, xpos, 0, _renderedText.Width, _renderedText.Height);
        }
    }
}
