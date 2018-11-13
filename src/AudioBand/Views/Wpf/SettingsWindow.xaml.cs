using AudioBand.ViewModels;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using AudioBand.Commands;

namespace AudioBand.Views.Wpf
{
    internal partial class SettingsWindow
    {
        private bool _shouldSave;
        private SettingsWindowVM _vm;

        public RelayCommand CancelCloseCommand { get; }
        public RelayCommand SaveCloseCommand { get; }

        internal SettingsWindow(SettingsWindowVM vm)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            CancelCloseCommand = new RelayCommand(CancelCloseCommandOnExecute);
            SaveCloseCommand =  new RelayCommand(SaveCloseCommandOnExecute);

            InitializeComponent();
            DataContext = _vm = vm;
            vm.CustomLabelsVM.DialogService = new DialogService(this);
        }

        // Problem loading some dlls
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (!args.Name.StartsWith("Xceed.Wpf.Toolkit") && !args.Name.StartsWith("MahApps.Metro.IconPacks.Modern"))
            {
                return null;
            }

            var dir = DirectoryHelper.BaseDirectory;
            // name is in this format Xceed.Wpf.Toolkit, Version=3.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4
            var asmName = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";
            var filename = Path.Combine(dir, asmName);

            return !File.Exists(filename) ? null : Assembly.LoadFrom(filename);
        }

        private void SaveCloseCommandOnExecute(object o)
        {
            _shouldSave = true;
            Close();
        }

        private void CancelCloseCommandOnExecute(object o)
        {
            _shouldSave = false;
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;

            if (_shouldSave)
            {
                _vm.EndEdit();
            }
            else
            {
                _vm.CancelEdit();
            }

            _shouldSave = false;
            _vm.BeginEdit();
            Hide();
        }
    }
}
