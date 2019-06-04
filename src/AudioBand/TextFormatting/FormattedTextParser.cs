using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// Renders formatted text.
    /// </summary>
    internal class FormattedTextParser
    {
        private const char PlaceholderStartToken = '{';
        private const char PlaceholderEndToken = '}';
        private const string BoldStyle = "*";
        private const string ItalicsStyle = "&";
        private const string UnderlineStyle = "_";

        private const string Styles = BoldStyle + ItalicsStyle + UnderlineStyle;
        private static readonly string Tags = string.Join("|", TextPlaceholderFactory.Tags);
        private static readonly Regex PlaceholderPattern = new Regex($@"(?<style>[{Styles}])*(?<tag>({Tags}))(:(?<color>#[A-Fa-f0-9]{{6,8}}))?", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly ColorConverter ColorConverter = new ColorConverter();

        /// <summary>
        /// Parses the formatted text string.
        /// </summary>
        /// <param name="format">The formatted string.</param>
        /// <param name="defaultColor">The default text color to use.</param>
        /// <returns>A list of text segments.</returns>
        public static IEnumerable<TextSegment> ParseFormattedString(string format, Color defaultColor)
        {
            // Build up segments, each chunk is a sparately formatted piece of text
            var segments = new List<TextSegment>();
            var currentText = new StringBuilder();

            // go through building up segments character by character
            for (int i = 0; i < format.Length; i++)
            {
                switch (format[i])
                {
                    // If we see the start of the format, get a chunk until the end token
                    case PlaceholderStartToken:
                        // Add what we have so far
                        AddSegment(segments, currentText, false, defaultColor);

                        // Skip the start token
                        i++;

                        // Add everything until the end token or end of the string
                        while (i < format.Length && format[i] != PlaceholderEndToken)
                        {
                            currentText.Append(format[i]);
                            i++;
                        }

                        // If we reached the end of the string before an end token
                        if (i == format.Length)
                        {
                            currentText.Insert(0, PlaceholderStartToken);
                            AddSegment(segments, currentText, false, defaultColor);
                        }
                        else
                        {
                            // Full placeholder
                            AddSegment(segments, currentText, true, defaultColor);
                        }

                        break;
                    default:
                        currentText.Append(format[i]);
                        break;
                }
            }

            AddSegment(segments, currentText, false, defaultColor);
            return segments;
        }

        private static void AddSegment(List<TextSegment> segments, StringBuilder text, bool isPlaceholder, Color defaultColor)
        {
            if (text.Length == 0)
            {
                return;
            }

            if (isPlaceholder)
            {
                if (TryParsePlaceholder(text.ToString(), defaultColor, out TextPlaceholder placeholder, out FormattedTextFlags flags, out Color c))
                {
                    segments.Add(new PlaceholderTextSegment(placeholder, flags, c));
                }
                else
                {
                    segments.Add(new StaticTextSegment("!Invalid format!", FormattedTextFlags.Normal, Colors.Red));
                }
            }
            else
            {
                segments.Add(new StaticTextSegment(text.ToString(), FormattedTextFlags.Normal, defaultColor));
            }

            text.Clear();
        }

        private static bool TryParsePlaceholder(string placeholderString, Color defaultColor, out TextPlaceholder placeholder, out FormattedTextFlags flags, out Color color)
        {
            var match = PlaceholderPattern.Match(placeholderString);
            flags = FormattedTextFlags.Normal;

            if (!match.Success)
            {
                placeholder = null;
                flags = FormattedTextFlags.Normal;
                color = defaultColor;
                return false;
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

            if (!TextPlaceholderFactory.TryGetPlaceholder(match.Groups["tag"].Value, null, out placeholder))
            {
                color = Colors.Red;
                return false;
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
                    color = defaultColor;
                }
            }
            else
            {
                color = defaultColor;
            }

            return true;
        }
    }
}
