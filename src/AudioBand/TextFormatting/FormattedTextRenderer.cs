using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using AudioBand.Extensions;
using TextAlignment = AudioBand.Models.CustomLabel.TextAlignment;

namespace AudioBand.TextFormatting
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

        private const string Styles = BoldStyle + ItalicsStyle + UnderlineStyle;
        private const string Tags = ArtistPlaceholder + "|" + SongNamePlaceholder + "|" + AlbumNamePlaceholder + "|" + CurrentTimePlaceholder + "|" + SongLengthPlaceholder;
        private static readonly Regex PlaceholderPattern = new Regex($@"(?<style>[{Styles}])*(?<tag>({Tags}))(:(?<color>#[A-Fa-f0-9]{{6}}))?", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly ColorConverter ColorConverter = new ColorConverter();

        private readonly object _textSegmentLock = new object();
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
        public FormattedTextRenderer(string format, Color defaultColor)
        {
            Format = format;
            DefaultColor = defaultColor;

            Parse();
        }

        /// <summary>
        /// Gets or sets the list of text segments.
        /// </summary>
        public ObservableCollection<TextSegment> TextSegments { get; set; }

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
                UpdatePlaceholderValue(FormattedTextFlags.CurrentTime, value.Format());
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
                UpdatePlaceholderValue(FormattedTextFlags.SongLength, value.Format());
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
                foreach (var textChunk in TextSegments)
                {
                    if (!textChunk.Type.HasFlag(FormattedTextFlags.Colored))
                    {
                        textChunk.Color = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the format flags.
        /// </summary>
        public FormattedTextFlags Flags { get; private set; }

        private void Parse()
        {
            // Build up segments, each chunk is a sparately formatted piece of text
            var segments = new List<TextSegment>();
            var currentText = new StringBuilder();

            if (Format == null)
            {
                return;
            }

            // go through building up segments character by character
            for (int i = 0; i < Format.Length; i++)
            {
                switch (Format[i])
                {
                    // If we see the start of the format, get a chunk until the end token
                    case PlaceholderStartToken:
                        // Add what we have so far
                        AddSegment(segments, currentText, false);

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
                            AddSegment(segments, currentText, false);
                        }
                        else
                        {
                            // Full placeholder
                            AddSegment(segments, currentText, true);
                        }

                        break;
                    default:
                        currentText.Append(Format[i]);
                        break;
                }
            }

            AddSegment(segments, currentText, false);

            lock (_textSegmentLock)
            {
                TextSegments = new ObservableCollection<TextSegment>(segments);
            }
        }

        private void AddSegment(List<TextSegment> segments, StringBuilder text, bool placeholder)
        {
            if (text.Length == 0)
            {
                return;
            }

            if (placeholder)
            {
                ParsePlaceholder(text.ToString(), out string value, out FormattedTextFlags type, out Color c);
                segments.Add(new TextSegment(value, type, c));
                Flags |= type;
            }
            else
            {
                segments.Add(new TextSegment(text.ToString(), FormattedTextFlags.Normal, DefaultColor));
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
                    value = SongProgress.Format();
                    flags |= FormattedTextFlags.CurrentTime;
                    break;
                case SongLengthPlaceholder:
                    value = SongLength.Format();
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
                    color = (Color)ColorConverter.ConvertFrom(match.Groups["color"].Value);
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

        private void UpdatePlaceholderValue(FormattedTextFlags type, string value)
        {
            lock (_textSegmentLock)
            {
                foreach (var textChunk in TextSegments)
                {
                    if (textChunk.Type.HasFlag(type))
                    {
                        textChunk.Text = value;
                    }
                }
            }
        }
    }
}
