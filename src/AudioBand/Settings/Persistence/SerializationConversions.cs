using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace AudioBand.Settings.Persistence
{
    /// <summary>
    /// Conversion helpers for serialization.
    /// </summary>
    internal static class SerializationConversions
    {
        private static readonly Regex ColorStringRegex =
            new Regex(@"#(?<a>[0-9a-fA-F]{2})(?<r>[0-9a-fA-F]{2})(?<g>[0-9a-fA-F]{2})(?<b>[0-9a-fA-F]{2})", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        /// <summary>
        /// Converts a string to an enum.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="val">The string value of an enum value.</param>
        /// <returns>The enum.</returns>
        public static T StringToEnum<T>(string val)
        {
            return (T)Enum.Parse(typeof(T), val);
        }

        /// <summary>
        /// Converts the enum value to a string.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>The string representing its value.</returns>
        public static string EnumToString<T>(T value)
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException($"Value is not an enum {value} | {typeof(T)}");
            }

            return Enum.GetName(typeof(T), value);
        }

        /// <summary>
        /// Converts color to string.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>The color as a hex value.</returns>
        public static string ColorToString(Color color)
        {
            return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        /// <summary>
        /// Converts string to color.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The color from the string.</returns>
        public static Color StringToColor(string s)
        {
            if (s == null)
            {
                return default(Color);
            }

            var match = ColorStringRegex.Match(s);
            if (match.Success)
            {
                byte a = ParseHexFromGroup(match.Groups["a"]);
                byte r = ParseHexFromGroup(match.Groups["r"]);
                byte g = ParseHexFromGroup(match.Groups["g"]);
                byte b = ParseHexFromGroup(match.Groups["b"]);
                return Color.FromArgb(a, r, g, b);
            }

            return Colors.White;
        }

        private static byte ParseHexFromGroup(Group g)
        {
            return byte.Parse(g.Value, NumberStyles.AllowHexSpecifier);
        }
    }
}
