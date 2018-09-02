using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                UpdatePlaceholderValue(TextFormat.CurrentTime, value.ToString());
            }
        }

        public TimeSpan SongLength
        {
            get => _songLength;
            set
            {
                _songLength = value;
                UpdatePlaceholderValue(TextFormat.SongLength, value.ToString());
            }
        }

        public int TextLength { get; private set; }

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

        private string _format;
        private string _artist;
        private string _songName;
        private string _albumName;
        private TimeSpan _songProgress;
        private TimeSpan _songLength;
        private Color _defaultColor;

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
            }
            else
            {
                Chunks.Add(new TextChunk(text.ToString(), TextFormat.Normal, DefaultColor));
            }

            text.Clear();
        }

        private void ParsePlaceholder(string format, out string value, out TextFormat type, out Color color)
        {
            // either artist or artist:#ffffffff
            var split = format.Split(new[] {":"}, 2, StringSplitOptions.None);

            switch (split[0])
            {
                case ArtistPlaceholder:
                    value = Artist;
                    type = TextFormat.Artist;
                    break;
                case SongNamePlaceholder:
                    value = SongName;
                    type = TextFormat.Song;
                    break;
                case AlbumNamePlaceholder:
                    value = AlbumName;
                    type = TextFormat.Album;
                    break;
                case CurrentTimePlaceholder:
                    value = SongProgress.ToString();
                    type = TextFormat.CurrentTime;
                    break;
                case SongLengthPlaceholder:
                    value = SongLength.ToString(); //TODO time format
                    type = TextFormat.SongLength;
                    break;
                default:
                    value = "! invalid format !";
                    type = TextFormat.Normal;
                    break;
            }

            if (split.Length == 1)
            {
                color = DefaultColor;
                return;
            }

            try
            {
                color = ColorTranslator.FromHtml(split[1]);
                type |= TextFormat.Colored;
            }
            catch (Exception)
            {
                color = DefaultColor;
            }
        }

        public void Draw(Graphics graphics, int x)
        {
            var font = new Font(FontFamily, FontSize, FontStyle.Regular, GraphicsUnit.Point);

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

            // Use as text location
            TextLength = 0;
            foreach (var textChunk in Chunks)
            {
                var textLength = TextRenderer.MeasureText(textChunk.Text, font).Width - padding;
                textChunk.Draw(graphics, font, x, 0);

                TextLength += textLength;
                x += textLength;
            }
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
                TextRenderer.DrawText(g, Text, font, new Point(x, y), Color);
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
            Colored = 32
        }
    }
}
