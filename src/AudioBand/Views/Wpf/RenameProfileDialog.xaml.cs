using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AudioBand.ViewModels;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interaction logic for RenameProfileDialog.xaml
    /// </summary>
    public partial class RenameProfileDialog
    {
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
