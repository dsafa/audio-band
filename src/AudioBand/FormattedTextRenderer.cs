using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using AudioBand.Settings;
using AudioBand.ViewModels;
using NLog;

namespace AudioBand
{
    public class FormattedTextRenderer
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

        public List<TextChunk> Chunks { get; set; }

        public string Format
        {
            get => _format;
            set
            {
                _format = value;
                Parse();
            }
        }

        public string Artist
        {
            get => _artist;
            set
            {
                _artist = value;
                UpdatePlaceholderValue(TextFormat.Artist, value);
            }
        }

        public string SongName
        {
            get => _songName;
            set
            {
                _songName = value;
                UpdatePlaceholderValue(TextFormat.Song, value);
            }
        }

        public string AlbumName
        {
            get => _albumName;
            set
            {
                _albumName = value;
                UpdatePlaceholderValue(TextFormat.Album, value);
            }
        }

        public TimeSpan SongProgress
        {
            get => _songProgress;
            set
            {
                _songProgress = value;
                UpdatePlaceholderValue(TextFormat.CurrentTime, value.ToString(TimeFormat));
            }
        }

        public TimeSpan SongLength
        {
            get => _songLength;
            set
            {
                _songLength = value;
                UpdatePlaceholderValue(TextFormat.SongLength, value.ToString(TimeFormat));
            }
        }

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

        public float FontSize { get; set; }

        public string FontFamily { get; set; }

        public TextAlignment Alignment { get; set; }

        public TextFormat Formats { get; private set; }

        private string _format;
        private string _artist;
        private string _songName;
        private string _albumName;
        private TimeSpan _songProgress;
        private TimeSpan _songLength;
        private Color _defaultColor;
        private const string TimeFormat = @"m\:ss";

        private const string Styles = BoldStyle + ItalicsStyle + UnderlineStyle;
        private const string Tags = ArtistPlaceholder + "|" + SongNamePlaceholder + "|" + AlbumNamePlaceholder + "|" + CurrentTimePlaceholder + "|" + SongLengthPlaceholder;
        private static readonly Regex PlaceholderPattern = new Regex($@"(?<style>[{Styles}])?(?<tag>({Tags}))(:(?<color>#[A-Fa-f0-9]{{6}}))?", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public FormattedTextRenderer(string format, Color defaultColor, float fontSize, string fontFamily, TextAlignment alignment)
        {
            Format = format;
            DefaultColor = defaultColor;
            FontSize = fontSize;
            FontFamily = fontFamily;
            Alignment = alignment;

            Parse();
        }

        private void Parse()
        {
            // Build up chunks, each chunk is a sparately formatted piece of text
            Chunks = new List<TextChunk>();
            var currentText = new StringBuilder();

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
                switch (match.Groups["style"].Value)
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
                    default:
                        break;
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
                catch (Exception e)
                {
                    color = DefaultColor;
                }
            }
            else
            {
                color = DefaultColor;
            }
        }

        public Bitmap Draw()
        {
            var size = Measure();
            if (size.Width < 1 || size.Height < 1)
            {
                return new Bitmap(100, 20);
            }

            var bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                Draw(graphics, false);
            }

            return bitmap;
        }

        public Size Measure()
        {
            return Draw(null, true);
        }

        private Size Draw(Graphics graphics, bool measure)
        {
            var totalTextSize = new Size();
            var x = 0;

            AddChunk(new StringBuilder(" ", 1), false); // Padding because the last character is being cut off
            foreach (var textChunk in Chunks)
            {
                var font = new Font(FontFamily, FontSize, FontStyle.Regular, GraphicsUnit.Point);
                if (textChunk.Type.HasFlag(TextFormat.Bold))
                {
                    font = new Font(FontFamily, FontSize, FontStyle.Bold, GraphicsUnit.Point);
                }
                else if (textChunk.Type.HasFlag(TextFormat.Italic))
                {
                    font = new Font(FontFamily, FontSize, FontStyle.Italic, GraphicsUnit.Point);
                }
                else if (textChunk.Type.HasFlag(TextFormat.Underline))
                {
                    font = new Font(FontFamily, FontSize, FontStyle.Underline, GraphicsUnit.Point);
                }

                var textSize = TextRenderer.MeasureText(textChunk.Text, font, new Size(1000, 1000), TextFormatFlags.NoPrefix);
                if (textSize.Width > 0 )
                {
                    // keep padding in last item
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

        // A chunk of text that has its own custom rendering
        public class TextChunk
        {
            public string Text { get; set; }
            public TextFormat Type { get; }
            public Color Color { get; set; }


            public TextChunk(string text, TextFormat type, Color color)
            {
                Text = text;
                Type = type;
                Color = color;
            }

            public void Draw(Graphics g, Font font, int x, int y)
            {
                TextRenderer.DrawText(g, Text, font, new Point(x, y), Color, TextFormatFlags.NoPrefix);
            }
        }

        [Flags]
        public enum TextFormat
        {
            Normal = 0,
            Artist = 1,
            Song = 2,
            Album = 4,
            CurrentTime = 8,
            SongLength = 16,
            Colored = 32,
            Bold = 64,
            Italic = 128,
            Underline = 256
        }
    }
}
