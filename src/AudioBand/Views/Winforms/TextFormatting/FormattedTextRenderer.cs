using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using TextAlignment = AudioBand.Models.CustomLabel.TextAlignment;

namespace AudioBand.Views.Winforms.TextFormatting
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
                UpdatePlaceholderValue(FormattedTextFlags.Artist, value);
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
                UpdatePlaceholderValue(FormattedTextFlags.Song, value);
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
                UpdatePlaceholderValue(FormattedTextFlags.Album, value);
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
                UpdatePlaceholderValue(FormattedTextFlags.CurrentTime, value.ToString(TimeFormat));
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
                UpdatePlaceholderValue(FormattedTextFlags.SongLength, value.ToString(TimeFormat));
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
                    if (!textChunk.Type.HasFlag(FormattedTextFlags.Colored))
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
        public FormattedTextFlags Flagses { get; private set; }

        /// <summary>
        /// Draws the formatted text with the current values.
        /// </summary>
        /// <returns>A bitmap of the rendered text.</returns>
        public Bitmap Draw(double dpi)
        {
            SizeF size = default(SizeF);
            using (var temp = new Bitmap(1, 1))
            {
                temp.SetResolution((float)dpi, (float)dpi);
                using (var measureGraphics = Graphics.FromImage(temp))
                {
                    size = Measure(measureGraphics);
                }
            }

            if (size.Width < 1 || size.Height < 1)
            {
                return new Bitmap(100, 20);
            }

            var width = (int)Math.Ceiling(size.Width);
            var height = (int)Math.Ceiling(size.Height);
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            bitmap.SetResolution((float)dpi, (float)dpi);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                Draw(graphics, false);
            }

            return bitmap;
        }

        /// <summary>
        /// Measures the size that the text would be.
        /// </summary>
        /// <param name="scaling">The scaling factor.</param>
        /// <returns>The bounds of the text.</returns>
        public SizeF Measure(Graphics measureGraphics)
        {
            return Draw(measureGraphics, true);
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
                ParsePlaceholder(text.ToString(), out string value, out FormattedTextFlags type, out Color c);
                Chunks.Add(new TextChunk(value, type, c));
                Flagses |= type;
            }
            else
            {
                Chunks.Add(new TextChunk(text.ToString(), FormattedTextFlags.Normal, DefaultColor));
            }

            text.Clear();
        }

        private void ParsePlaceholder(string formatString, out string value, out FormattedTextFlags flags, out Color color)
        {
            var match = PlaceholderPattern.Match(formatString);
            flags = FormattedTextFlags.Normal;

            if (!match.Success)
            {
                value = "! invalid format !";
                flags = FormattedTextFlags.Normal;
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
                            flags |= FormattedTextFlags.Bold;
                            break;
                        case ItalicsStyle:
                            flags |= FormattedTextFlags.Italic;
                            break;
                        case UnderlineStyle:
                            flags |= FormattedTextFlags.Underline;
                            break;
                    }
                }
            }

            switch (match.Groups["tag"].Value)
            {
                case ArtistPlaceholder:
                    value = Artist;
                    flags |= FormattedTextFlags.Artist;
                    break;
                case SongNamePlaceholder:
                    value = SongName;
                    flags |= FormattedTextFlags.Song;
                    break;
                case AlbumNamePlaceholder:
                    value = AlbumName;
                    flags |= FormattedTextFlags.Album;
                    break;
                case CurrentTimePlaceholder:
                    value = SongProgress.ToString(TimeFormat);
                    flags |= FormattedTextFlags.CurrentTime;
                    break;
                case SongLengthPlaceholder:
                    value = SongLength.ToString(TimeFormat);
                    flags |= FormattedTextFlags.SongLength;
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
                    flags |= FormattedTextFlags.Colored;
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

        private SizeF Draw(Graphics graphics, bool measure)
        {
            var totalTextSize = default(SizeF);
            var x = 0f;

            if (Chunks?.Count == 0)
            {
                return totalTextSize;
            }

            foreach (var textChunk in Chunks)
            {
                var fontSize = FontSize;
                var font = new Font(FontFamily, fontSize, GetFontStyle(textChunk.Type), GraphicsUnit.Point);
                var textSize = graphics.MeasureString(textChunk.Text, font, new SizeF(1000, 1000), StringFormat.GenericTypographic);

                if (!measure)
                {
                    textChunk.Draw(graphics, font, x, 0);
                }

                totalTextSize.Width += textSize.Width;
                totalTextSize.Height = textSize.Height;
                x += textSize.Width;
            }

            return totalTextSize;
        }

        private FontStyle GetFontStyle(FormattedTextFlags formattedTextFlags)
        {
            var fontStyle = FontStyle.Regular;
            if (formattedTextFlags.HasFlag(FormattedTextFlags.Bold))
            {
                fontStyle |= FontStyle.Bold;
            }

            if (formattedTextFlags.HasFlag(FormattedTextFlags.Italic))
            {
                fontStyle |= FontStyle.Italic;
            }

            if (formattedTextFlags.HasFlag(FormattedTextFlags.Underline))
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

        private void UpdatePlaceholderValue(FormattedTextFlags type, string value)
        {
            foreach (var textChunk in Chunks)
            {
                if (textChunk.Type.HasFlag(type))
                {
                    textChunk.Text = value;
                }
            }
        }
    }
}
