using System.Windows;
using MahApps.Metro.Controls;

namespace AudioBand.Settings
{
    public static class AttachedProperties
    {
        public static readonly DependencyProperty NumericInputTypeProperty =
            DependencyProperty.RegisterAttached("NumericInputType", typeof(NumericInputType), typeof(AttachedProperties), new FrameworkPropertyMetadata(NumericInputType.Size));

        public static void SetNumericInputType(NumericUpDown control, NumericInputType inputType)
        {
            control.SetValue(NumericInputTypeProperty, inputType);
        }

        public static NumericInputType GetNumericInputType(NumericUpDown control)
        {
            return (NumericInputType) control.GetValue(NumericInputTypeProperty);
        }
    }

    public enum NumericInputType
    {
        Size,
        Position,
        FontSize
    }
}
