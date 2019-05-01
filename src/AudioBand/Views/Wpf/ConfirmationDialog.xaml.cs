using System.Windows;
using AudioBand.Messages;
using AudioBand.ViewModels;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for ConfirmationDialog.xaml.
    /// </summary>
    public partial class ConfirmationDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationDialog"/> class.
        /// </summary>
        /// <param name="messageBus">The message bus to use.</param>
        /// <param name="confirmType">Type of confirmation.</param>
        /// <param name="data">Data for confirmation dialog.</param>
        public ConfirmationDialog(IMessageBus messageBus, ConfirmationDialogType confirmType, object[] data)
            : base(messageBus)
        {
            DialogType = confirmType;
            DialogData = data;
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the type of the confirmation dialog.
        /// </summary>
        public ConfirmationDialogType DialogType { get; set; }

        /// <summary>
        /// Gets or sets the dialog data.
        /// </summary>
        public object[] DialogData { get; set; }

        private void OkButtonOnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButtonOnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
