using System.Windows;
using System.Windows.Media;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for ColorPickerDialog.xaml
    /// </summary>
    public partial class ColorPickerDialog
    {
        public Color Color
        {
            get => Picker.Color;
            set => Picker.SetColor(value);
        }

        public ColorPickerDialog()
        {
            InitializeComponent();
        }

        private void OkButtonOnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButtonOnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
