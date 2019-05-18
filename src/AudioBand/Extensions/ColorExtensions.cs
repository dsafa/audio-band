using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace AudioBand.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Color"/>.
    /// </summary>
    internal static class ColorExtensions
    {
        private static readonly List<(Color color, string name)> _colors;

        static ColorExtensions()
        {
            _colors = typeof(Colors).GetProperties()
                .Where(c => c.PropertyType == typeof(Color))
                .Select(p => ((Color)p.GetValue(null), p.Name))
                .ToList();
        }

        /// <summary>
        /// Get the known name of the color if available otherwise get the hex code.
        /// </summary>
        /// <param name="color">Color to get the name of.</param>
        /// <returns>The known name or hex value as a string.</returns>
        public static string GetColorName(this Color color)
        {
            var matchingColorNames = _colors.Where(x => Color.AreClose(x.color, color)).Select(x => x.name);
            return matchingColorNames.FirstOrDefault() ?? new ColorConverter().ConvertToString(color);
        }

        /// <summary>
        /// Converts a windows ui color to a wpf color.
        /// </summary>
        /// <param name="color">The windows color.</param>
        /// <returns>The wpf color.</returns>
        public static Color ToWpfColor(this Windows.UI.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
