using System;
using System.ComponentModel;

namespace AudioSourceHost
{
    public static class TypeConvertHelper
    {
        /// <summary>
        /// Converts a value to a specified type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="type">The target type.</param>
        /// <returns>The converted value.</returns>
        public static object ConvertToType(object value, Type type)
        {
            if (value != null && value.GetType() != type)
            {
                var converter = TypeDescriptor.GetConverter(value.GetType());
                if (converter.CanConvertTo(type))
                {
                    return converter.ConvertTo(value, type);
                }

                if (TryChangeType(value, type, out var converted))
                {
                    return converted;
                }

                throw new ArgumentException($"Unable to convert value to desired type: value: `{value}`, value type: `{value.GetType()}`, target type: `{type}`");
            }

            return value;
        }

        private static bool TryChangeType(object value, Type type, out object converted)
        {
            try
            {
                converted = Convert.ChangeType(value, type);
                return true;
            }
            catch (Exception)
            {
                converted = null;
                return false;
            }
        }
    }
}
