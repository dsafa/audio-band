using System.Windows;
using AudioBand.Messages;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml.
    /// </summary>
    public partial class AboutDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutDialog"/> class.
        /// </summary>
        /// <param name="messageBus">The message bus.</param>
        public AboutDialog(IMessageBus messageBus)
            : base(messageBus)
        {
            InitializeComponent();
        }

        private void OkButtonOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
