using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for ColorPickerDialog.xaml
    /// </summary>
    public partial class ColorPickerDialog
    {
        private bool _ok;

        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorPickerDialog));
        public static DependencyProperty SuccessCommandProperty = DependencyProperty.Register(nameof(SuccessCommand), typeof(ICommand), typeof(ColorPickerDialog));
        public static DependencyProperty CancelCommandProperty = DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(ColorPickerDialog));

        public Color Color
        {
            get => (Color) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public ICommand SuccessCommand
        {
            get => (ICommand) GetValue(SuccessCommandProperty);
            set => SetValue(SuccessCommandProperty, value);
        }

        public ICommand CancelCommand
        {
            get => (ICommand) GetValue(CancelCommandProperty);
            set => SetValue(CancelCommandProperty, value);
        }

        public ColorPickerDialog(Color color)
        {
            InitializeComponent();
            Picker.Color = color;
            Picker.OnPickColor += PickerOnOnPickColor;
        }

        private void PickerOnOnPickColor(Color color)
        {
            Color = Picker.Color;
        }

        private void OkButtonOnClick(object sender, RoutedEventArgs e)
        {
            _ok = true;
            Close();
        }

        private void CancelButtonOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Success()
        {
            if (SuccessCommand == null)
            {
                return;
            }

            if (SuccessCommand.CanExecute(Color))
            {
                SuccessCommand.Execute(Color);
            }

            DialogResult = true;
        }

        private void Cancelled()
        {
            if (CancelCommand == null)
            {
                return;
            }

            if (CancelCommand.CanExecute(null))
            {
                CancelCommand.Execute(null);
            }

            DialogResult = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (_ok)
            {
                Success();
            }
            else
            {
                Cancelled();
            }
        }
    }
}
