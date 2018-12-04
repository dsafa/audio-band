using System;
using System.Drawing;

namespace AudioBand.Settings
{
    /// <summary>
    /// Conversion helpers for serialization.
    /// </summary>
    internal static class SerializationConversions
    {
        /// <summary>
        /// Convert a font to a string representation.
        /// </summary>
        /// <param name="font">Font to convert.</param>
        /// <returns>The font serialized as a string.</returns>
        public static string FontToString(Font font)
        {
            return string.Join(";", font.Name, font.Size.ToString(), font.Style.ToString(), font.Unit.ToString());
        }

        /// <summary>
        /// Converts the string from <see cref="FontToString(Font)"/> back to a <see cref="Font"/>.
        /// </summary>
        /// <param name="fontString">String representation of the font.</param>
        /// <returns>The font.</returns>
        public static Font StringToFont(string fontString)
        {
            var vals = fontString.Split(';');
            return new Font(vals[0], float.Parse(vals[1]), StringToEnum<FontStyle>(vals[2]), StringToEnum<GraphicsUnit>(vals[3]));
        }

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
    }
}
