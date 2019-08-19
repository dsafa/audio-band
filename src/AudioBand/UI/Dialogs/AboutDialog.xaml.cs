using System.Windows;

namespace AudioBand.UI
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml.
    /// </summary>
    public partial class AboutDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutDialog"/> class.
        /// </summary>
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void OkButtonOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
