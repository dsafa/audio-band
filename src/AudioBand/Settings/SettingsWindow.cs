using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace AudioBand.Settings
{
    public partial class SettingsWindow : MetroForm
    {
        public SettingsWindow()
        {
            InitializeComponent();

            FormClosing += OnFormClosing;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs formClosingEventArgs)
        {
            formClosingEventArgs.Cancel = true;
            Hide();
        }
    }
}
