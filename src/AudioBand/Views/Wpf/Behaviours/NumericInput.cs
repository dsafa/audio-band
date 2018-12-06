using System.Windows;
using MahApps.Metro.Controls;

namespace AudioBand.Views.Wpf.Behaviours
{
    /// <summary>
    /// Attached property to help format numeric input min/max/interval.
    /// </summary>
    public static class NumericInput
    {
        /// <summary>
        /// The type of numeric input.
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.RegisterAttached("Type", typeof(NumericInputType), typeof(NumericInput), new FrameworkPropertyMetadata(NumericInputType.Size));

        /// <summary>
        /// Set the depedency property <see cref="TypeProperty"/>.
        /// </summary>
        /// <param name="control">The numericupdown control.</param>
        /// <param name="inputType">The input type.</param>
        public static void SetType(NumericUpDown control, NumericInputType inputType)
        {
            control.SetValue(TypeProperty, inputType);
        }

        /// <summary>
        /// Get the depedency property <see cref="TypeProperty"/> from the numericupdown control.
        /// </summary>
        /// <param name="control">The numericupdown control.</param>
        /// <returns>The attached <see cref="TypeProperty"/>.</returns>
        public static NumericInputType GetType(NumericUpDown control)
        {
            return (NumericInputType)control.GetValue(TypeProperty);
        }
    }
}
