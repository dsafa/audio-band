using System.Windows;

namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Helpers for value conversion.
    /// </summary>
    internal static class ValueConverterHelper
    {
        /// <summary>
        /// Provides a standard guard clause for the converter value.
        /// </summary>
        /// <typeparam name="TType">Expected type.</typeparam>
        /// <param name="value">The value passed to the converter.</param>
        /// <returns>True if valid, false otherwise.</returns>
        internal static bool IsValid<TType>(object value)
        {
            return value != null
                   && value != DependencyProperty.UnsetValue
                   && value.GetType() == typeof(TType);
        }
    }
}
