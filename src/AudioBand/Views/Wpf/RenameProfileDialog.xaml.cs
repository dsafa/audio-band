using System;
using AudioBand.ViewModels;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Dialog to rename a profile.
    /// </summary>
    public partial class RenameProfileDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenameProfileDialog"/> class.
        /// </summary>
        /// <param name="vm">The view model.</param>
        public RenameProfileDialog(RenameProfileDialogVM vm)
        {
            InitializeComponent();
            DataContext = vm;

            vm.Accepted += Accepted;
            vm.Canceled += Canceled;
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
