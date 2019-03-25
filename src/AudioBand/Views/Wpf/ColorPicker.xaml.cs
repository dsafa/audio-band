using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AudioBand.ViewModels;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        /// <summary>
        /// Dependency property for <see cref="Color"/>.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorPicker));

        /// <summary>
        /// Dependency property for <see cref="DialogService"/>
        /// </summary>
        public static readonly DependencyProperty DialogServiceProperty = DependencyProperty.Register(nameof(DialogService), typeof(IDialogService), typeof(ColorPicker));

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

        /// <summary>
        /// Gets or sets the dialog service.
        /// </summary>
        public IDialogService DialogService
        {
            get => (IDialogService)GetValue(DialogServiceProperty);
            set => SetValue(DialogServiceProperty, value);
        }

        private void OpenColorPickerOnClick(object sender, RoutedEventArgs e)
        {
            var res = DialogService?.ShowColorPickerDialog(Color);
            if (res.HasValue)
            {
                Color = res.Value;
            }
        }
    }
}
