using AudioBand.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorPicker));
        private readonly RelayCommand<Color> _colorPickedCommand;
        private readonly RelayCommand _dialogCanceledCommaned;

        public Color Color
        {
            get => (Color) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public ColorPicker()
        {
            InitializeComponent();

            _colorPickedCommand = new RelayCommand<Color>(ColorPickedCommandOnExecute);
            _dialogCanceledCommaned = new RelayCommand(o => { });
        }

        private void ChangeColorOnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new ColorPickerDialog(Color)
            {
                SuccessCommand = _colorPickedCommand,
                CancelCommand = _dialogCanceledCommaned,
                Owner = Window.GetWindow(this)
            };

            dialog.ShowDialog();
        }

        private void ColorPickedCommandOnExecute(Color color)
        {
            Color = color;
        }
    }
}
