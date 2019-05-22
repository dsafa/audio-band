using System;
using AudioBand.ViewModels;

namespace AudioBand.Views.Dialogs
{
    /// <summary>
    /// Dialog to rename a profile.
    /// </summary>
    public partial class RenameProfileDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenameProfileDialog"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public RenameProfileDialog(RenameProfileDialogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.Accepted += Accepted;
            viewModel.Canceled += Canceled;
        }

        private void Canceled(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Accepted(object sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
