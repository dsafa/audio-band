using AudioBand.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AudioBand.ViewModels;
using MahApps.Metro.Controls;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    internal partial class ColorPicker : UserControl
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorPicker));

        public Color Color
        {
            get => (Color) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public ColorPicker()
        {
            InitializeComponent();
        }

        private void ChangeColorOnClick(object sender, RoutedEventArgs e)
        {
            var res = DialogService.ShowColorPickerDialog(Window.GetWindow(this), Color);
            if (res.HasValue)
            {
                Color = res.Value;
            }
        }
    }
}
