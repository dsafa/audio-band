using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace AudioBand.Extensions
{
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

        public static string GetColorName(this Color color)
        {
            var col = _colors.Where(x => Color.AreClose(x.color, color)).Select(x => x.name).Take(1).ToList();
            return col.Any() ? col[0] : new ColorConverter().ConvertToString(color);
        }
    }
}
