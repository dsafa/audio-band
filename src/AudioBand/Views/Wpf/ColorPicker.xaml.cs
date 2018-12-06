using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    internal partial class ColorPicker : UserControl
    {
        /// <summary>
        /// Dependency property for <see cref="Color"/>.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorPicker));

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPicker"/> class.
        /// </summary>
        public ColorPicker()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the current color.
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
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
