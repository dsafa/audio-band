using System.Windows;
using System.Windows.Media;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for ColorPickerDialog.xaml
    /// </summary>
    public partial class ColorPickerDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPickerDialog"/> class.
        /// </summary>
        public ColorPickerDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the color of the picker.
        /// </summary>
        public Color Color
        {
            get => Picker.Color;
            set => Picker.SetColor(value);
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
