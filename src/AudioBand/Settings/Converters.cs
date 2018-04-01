using System;
using System.Drawing;

namespace AudioBand.Settings
{
    internal static class Converters
    {
        public static string FontToString(Font font)
        {
            return String.Join(";", font.Name, font.Size.ToString(), font.Style.ToString(), font.Unit.ToString());
        }

        public static Font StringToFont(string fontString)
        {
            var vals = fontString.Split(';');
            return new Font(vals[0], float.Parse(vals[1]), ToEnum<FontStyle>(vals[2]), ToEnum<GraphicsUnit>(vals[3]));
        }

        private static T ToEnum<T>(string val)
        {
            return (T)Enum.Parse(typeof(T), val);
        }
    }
}
