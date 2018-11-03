using System;
using System.Drawing;

namespace AudioBand.Settings
{
    internal static class SerializationConversions
    {
        public static string FontToString(Font font)
        {
            return String.Join(";", font.Name, font.Size.ToString(), font.Style.ToString(), font.Unit.ToString());
        }

        public static Font StringToFont(string fontString)
        {
            var vals = fontString.Split(';');
            return new Font(vals[0], float.Parse(vals[1]), StringToEnum<FontStyle>(vals[2]), StringToEnum<GraphicsUnit>(vals[3]));
        }

        public static T StringToEnum<T>(string val)
        {
            return (T)Enum.Parse(typeof(T), val);
        }

        public static string EnumToString<T>(T value)
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException($"Value is not an enum {value} | {typeof(T)}");
            }

            return Enum.GetName(typeof(T), value);
        }
    }
}
