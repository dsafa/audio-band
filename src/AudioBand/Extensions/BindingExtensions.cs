using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AudioBand.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Binding"/>.
    /// </summary>
    internal static class BindingExtensions
    {
        /// <summary>
        /// Get the value of a binding.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="binding">Binding to get value from.</param>
        /// <returns>Value from the binding.</returns>
        public static T As<T>(this Binding binding)
        {
            if (binding == null)
            {
                throw new ArgumentNullException(nameof(binding));
            }

            BindingManagerBase manager = binding.BindingManagerBase;
            PropertyDescriptor itemProperty = manager.GetItemProperties().Find(binding.BindingMemberInfo.BindingField, true);

            return (T)itemProperty.GetValue(manager.Current);
        }
    }
}
