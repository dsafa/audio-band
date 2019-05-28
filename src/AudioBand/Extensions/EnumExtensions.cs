using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace AudioBand.Extensions
{
    /// <summary>
    /// Extension methods for enums.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets a list of descriptors for the enum.
        /// </summary>
        /// <param name="enumType">The enum type.</param>
        /// <typeparam name="T">The enum.</typeparam>
        /// <returns>A list of descriptors for the num.</returns>
        public static IEnumerable<EnumDescriptor<T>> GetEnumDescriptors<T>(this Type enumType)
        {
            string GetDescription(FieldInfo fieldInfo)
            {
                var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                return descriptionAttribute?.Description ?? fieldInfo.Name;
            }

            return enumType
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(field => field.GetCustomAttribute<DescriptorIgnoreAttribute>() == null)
                .Select(field => new EnumDescriptor<T>((T)field.GetValue(null), GetDescription(field)));
        }
    }

    /// <summary>
    /// Describes an enum value.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    #pragma warning disable SA1201
    public struct EnumDescriptor<TEnum>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        public TEnum Value { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDescriptor{TEnum}"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="description">The description of the value.</param>
        public EnumDescriptor(TEnum value, string description)
        {
            Value = value;
            Description = description;
        }
    }
}
