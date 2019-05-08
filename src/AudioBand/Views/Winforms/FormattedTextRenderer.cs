using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TextAlignment = AudioBand.Models.CustomLabel.TextAlignment;

namespace AudioBand.Views.Winforms
{
    /// <summary>
    /// Renders formatted text.
    /// </summary>
    internal class FormattedTextRenderer
    {
        private const char PlaceholderStartToken = '{';
        private const char PlaceholderEndToken = '}';
        private const string ArtistPlaceholder = "artist";
        private const string SongNamePlaceholder = "song";
        private const string AlbumNamePlaceholder = "album";
        private const string CurrentTimePlaceholder = "time";
        private const string SongLengthPlaceholder = "length";
        private const string BoldStyle = "*";
        private const string ItalicsStyle = "&";
        private const string UnderlineStyle = "_";

        private const string TimeFormat = @"m\:ss";
        private const string Styles = BoldStyle + ItalicsStyle + UnderlineStyle;
        private const string Tags = ArtistPlaceholder + "|" + SongNamePlaceholder + "|" + AlbumNamePlaceholder + "|" + CurrentTimePlaceholder + "|" + SongLengthPlaceholder;
        private static readonly Regex PlaceholderPattern = new Regex($@"(?<style>[{Styles}])*(?<tag>({Tags}))(:(?<color>#[A-Fa-f0-9]{{6}}))?", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        private string _format;
        private string _artist;
        private string _songName;
        private string _albumName;
        private TimeSpan _songProgress;
        private TimeSpan _songLength;
        private Color _defaultColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormattedTextRenderer"/> class.
        /// </summary>
        /// <param name="format">The text format.</param>
        /// <param name="defaultColor">The default color.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="alignment">The text alignment.</param>
        public FormattedTextRenderer(string format, Color defaultColor, float fontSize, string fontFamily, TextAlignment alignment)
        {
            Format = format;
            DefaultColor = defaultColor;
            FontSize = fontSize;
            FontFamily = fontFamily;
            Alignment = alignment;

            Parse();
        }

        /// <summary>
        /// Flags for what the text contains.
        /// </summary>
        [Flags]
        public enum TextFormat
        {
            /// <summary>
            /// Normal text
            /// </summary>
            Normal = 0,

            /// <summary>
            /// Displays the artist
            /// </summary>
            Artist = 1,

            /// <summary>
            /// Displays the song.
            /// </summary>
            Song = 2,

            /// <summary>
            /// Displays the album
            /// </summary>
            Album = 4,

            /// <summary>
            /// Displays the current time.
            /// </summary>
            CurrentTime = 8,

            /// <summary>
            /// Displays the song length.
            /// </summary>
            SongLength = 16,

            /// <summary>
            /// Text is colored.
            /// </summary>
            Colored = 32,

            /// <summary>
            /// Text is bolded.
            /// </summary>
            Bold = 64,

            /// <summary>
            /// Text is italicised.
            /// </summary>
            Italic = 128,

            /// <summary>
            /// Text is underlined.
            /// </summary>
            Underline = 256,
        }

        /// <summary>
        /// Gets or sets the list of text chunks.
        /// </summary>
        public List<TextChunk> Chunks { get; set; }

        /// <summary>
        /// Gets or sets the text format.
        /// </summary>
        public string Format
        {
            get => _format;
            set
            {
                _format = value;
                Parse();
            }
        }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        public string Artist
        {
            get => _artist;
            set
            {
                _artist = value;
                UpdatePlaceholderValue(TextFormat.Artist, value);
            }
        }

        /// <summary>
        /// Gets or sets the song name.
        /// </summary>
        public string SongName
        {
            get => _songName;
            set
            {
                _songName = value;
                UpdatePlaceholderValue(TextFormat.Song, value);
            }
        }

        /// <summary>
        /// Gets or sets the album name.
        /// </summary>
        public string AlbumName
        {
            get => _albumName;
            set
            {
                _albumName = value;
                UpdatePlaceholderValue(TextFormat.Album, value);
            }
        }

        /// <summary>
        /// Gets or sets the song progress.
        /// </summary>
        public TimeSpan SongProgress
        {
            get => _songProgress;
            set
            {
                _songProgress = value;
                UpdatePlaceholderValue(TextFormat.CurrentTime, value.ToString(TimeFormat));
            }
        }

        /// <summary>
        /// Gets or sets the song length.
        /// </summary>
        public TimeSpan SongLength
        {
            get => _songLength;
            set
            {
                _songLength = value;
                UpdatePlaceholderValue(TextFormat.SongLength, value.ToString(TimeFormat));
            }
        }

        /// <summary>
        /// Gets or sets the default color.
        /// </summary>
        public Color DefaultColor
        {
            get => _defaultColor;
            set
            {
                _defaultColor = value;
                foreach (var textChunk in Chunks)
                {
                    if (!textChunk.Type.HasFlag(TextFormat.Colored))
                    {
                        textChunk.Color = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        public float FontSize { get; set; }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        public string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        public TextAlignment Alignment { get; set; }

        /// <summary>
        /// Gets the format flags.
        /// </summary>
        public TextFormat Formats { get; private set; }

        /// <summary>
        /// Draws the formatted text with the current values.
        /// </summary>
        /// <param name="scaling">The scaling factor.</param>
        /// <returns>A bitmap of the rendered text.</returns>
        public Bitmap Draw(double scaling)
        {
            var size = Measure(scaling);
            if (size.Width < 1 || size.Height < 1)
            {
                return new Bitmap(100, 20);
            }

            var bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                Draw(graphics, false, scaling);
            }

            return bitmap;
        }

        /// <summary>
        /// Measures the size that the text would be.
        /// </summary>
        /// <param name="scaling">The scaling factor.</param>
        /// <returns>The bounds of the text.</returns>
        public Size Measure(double scaling)
        {
            return Draw(null, true, scaling);
        }

        private void Parse()
        {
            // Build up chunks, each chunk is a sparately formatted piece of text
            Chunks = new List<TextChunk>();
            var currentText = new StringBuilder();

            if (Format == null)
            {
                return;
            }

            // go through building up chunks character by character
            for (int i = 0; i < Format.Length; i++)
            {
                switch (Format[i])
                {
                    // If we see the start of the format, get a chunk until the end token
                    case PlaceholderStartToken:
                        // Add what we have so far
                        AddChunk(currentText, false);

                        // Skip the start token
                        i++;

                        // Add everything until the end token or end of the string
                        while (i < Format.Length && Format[i] != PlaceholderEndToken)
                        {
                            currentText.Append(Format[i]);
                            i++;
                        }

                        // If we reached the end of the string before an end token
                        if (i == Format.Length)
                        {
                            currentText.Insert(0, PlaceholderStartToken);
                            AddChunk(currentText, false);
                        }
                        else
                        {
                            // Full placeholder
                            AddChunk(currentText, true);
                        }

                        break;
                    default:
                        currentText.Append(Format[i]);
                        break;
                }
            }

            AddChunk(currentText, false);
        }

        private void AddChunk(StringBuilder text, bool placeholder)
        {
            if (text.Length == 0)
            {
                return;
            }

            if (placeholder)
            {
                ParsePlaceholder(text.ToString(), out string value, out TextFormat type, out Color c);
                Chunks.Add(new TextChunk(value, type, c));
                Formats |= type;
            }
            else
            {
                Chunks.Add(new TextChunk(text.ToString(), TextFormat.Normal, DefaultColor));
            }

            text.Clear();
        }

        private void ParsePlaceholder(string formatString, out string value, out TextFormat format, out Color color)
        {
            var match = PlaceholderPattern.Match(formatString);
            format = TextFormat.Normal;

            if (!match.Success)
            {
                value = "! invalid format !";
                format = TextFormat.Normal;
                color = DefaultColor;
                return;
            }

            if (match.Groups["style"].Success)
            {
                var styleGroup = match.Groups["style"];
                for (int i = 0; i < styleGroup.Captures.Count; i++)
                {
                    switch (styleGroup.Captures[i].Value)
                    {
                        case BoldStyle:
                            format |= TextFormat.Bold;
                            break;
                        case ItalicsStyle:
                            format |= TextFormat.Italic;
                            break;
                        case UnderlineStyle:
                            format |= TextFormat.Underline;
                            break;
                    }
                }
            }

            switch (match.Groups["tag"].Value)
            {
                case ArtistPlaceholder:
                    value = Artist;
                    format |= TextFormat.Artist;
                    break;
                case SongNamePlaceholder:
                    value = SongName;
                    format |= TextFormat.Song;
                    break;
                case AlbumNamePlaceholder:
                    value = AlbumName;
                    format |= TextFormat.Album;
                    break;
                case CurrentTimePlaceholder:
                    value = SongProgress.ToString(TimeFormat);
                    format |= TextFormat.CurrentTime;
                    break;
                case SongLengthPlaceholder:
                    value = SongLength.ToString(TimeFormat);
                    format |= TextFormat.SongLength;
                    break;
                default:
                    value = "invalid";
                    break;
            }

            if (match.Groups["color"].Success)
            {
                try
                {
                    color = ColorTranslator.FromHtml(match.Groups["color"].Value);
                    format |= TextFormat.Colored;
                }
                catch (Exception)
                {
                    color = DefaultColor;
                }
            }
            else
            {
                color = DefaultColor;
            }
        }

        private Size Draw(Graphics graphics, bool measure, double scaling)
        {
            var totalTextSize = default(Size);
            var x = 0;

            if (Chunks?.Count == 0)
            {
                return totalTextSize;
            }

            // Add a chunk at the end for padding because the last character is being cut off
            AddChunk(new StringBuilder(" ", 1), false);
            foreach (var textChunk in Chunks)
            {
                var fontSize = (float)(FontSize * scaling);
                var font = new Font(FontFamily, fontSize, GetFontStyle(textChunk.Type), GraphicsUnit.Point);
                var textSize = TextRenderer.MeasureText(textChunk.Text, font, new Size(1000, 1000), TextFormatFlags.NoPrefix);
                if (textSize.Width > 0)
                {
                    // remove padding between items
                    textSize.Width -= MeasurePadding(font);
                }

                if (!measure)
                {
                    textChunk.Draw(graphics, font, x, 0);
                }

                totalTextSize.Width += textSize.Width;
                totalTextSize.Height = textSize.Height;
                x += textSize.Width;
            }

            Chunks.RemoveAt(Chunks.Count - 1);

            return totalTextSize;
        }

        private FontStyle GetFontStyle(TextFormat textFormat)
        {
            var fontStyle = FontStyle.Regular;
            if (textFormat.HasFlag(TextFormat.Bold))
            {
                fontStyle |= FontStyle.Bold;
            }

            if (textFormat.HasFlag(TextFormat.Italic))
            {
                fontStyle |= FontStyle.Italic;
            }

            if (textFormat.HasFlag(TextFormat.Underline))
            {
                fontStyle |= FontStyle.Underline;
            }

            return fontStyle;
        }

        private int MeasurePadding(Font font)
        {
            // Get the extra padding added by TextRenderer.MeasureText https://stackoverflow.com/a/12171682
            // Let width of character = x
            // Let width of padding = y
            // measuring 2 characters -> 2x + y
            // measuring 1 character -> x + y
            // (2x + y) - (x + y) = x = width of one character
            // x + y - x = y = padding
            var twoCharacterWidth = TextRenderer.MeasureText("  ", font).Width;
            var singleCharacterWidth = TextRenderer.MeasureText(" ", font).Width;
            var padding = singleCharacterWidth - (twoCharacterWidth - singleCharacterWidth);

            return padding;
        }

        private void UpdatePlaceholderValue(TextFormat type, string value)
        {
            foreach (var textChunk in Chunks)
            {
                if (textChunk.Type.HasFlag(type))
                {
                    textChunk.Text = value;
                }
            }
        }

        /// <summary>
        /// A chunk of text that has its own custom rendering.
        /// </summary>
        public class TextChunk
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TextChunk"/> class
            /// with text, color and type.
            /// </summary>
            /// <param name="text">The text.</param>
            /// <param name="type">The text type.</param>
            /// <param name="color">The text color.</param>
            public TextChunk(string text, TextFormat type, Color color)
            {
                Text = text;
                Type = type;
                Color = color;
            }

            /// <summary>
            /// Gets or sets the text.
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// Gets the text type.
            /// </summary>
            public TextFormat Type { get; }

            /// <summary>
            /// Gets or sets the chunk color.
            /// </summary>
            public Color Color { get; set; }

            /// <summary>
            /// Draws the chunk onto the graphics.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="font">The font.</param>
            /// <param name="x">The x position of the text.</param>
            /// <param name="y">The y position of the text.</param>
            public void Draw(Graphics g, Font font, int x, int y)
            {
                TextRenderer.DrawText(g, Text, font, new Point(x, y), Color, TextFormatFlags.NoPrefix);
            }
        }
    }
}
