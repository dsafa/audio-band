using System.Windows;
using MahApps.Metro.Controls;

namespace AudioBand.Views.Wpf.Behaviours
{
    public static class NumericInput
    {
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.RegisterAttached("Type", typeof(NumericInputType), typeof(NumericInput), new FrameworkPropertyMetadata(NumericInputType.Size));

        public static void SetType(NumericUpDown control, NumericInputType inputType)
        {
            control.SetValue(TypeProperty, inputType);
        }

        public static NumericInputType GetType(NumericUpDown control)
        {
            return (NumericInputType) control.GetValue(TypeProperty);
        }
    }

    public enum NumericInputType
    {
        Size,
        Position,
        FontSize
    }
}
