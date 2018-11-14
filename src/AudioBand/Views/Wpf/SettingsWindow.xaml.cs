using AudioBand.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using AudioBand.Commands;

namespace AudioBand.Views.Wpf
{
    internal partial class SettingsWindow
    {
        private bool _shouldSave;
        private readonly SettingsWindowVM _vm;

        public RelayCommand CancelCloseCommand { get; }
        public RelayCommand SaveCloseCommand { get; }

        public EventHandler Saved;

        internal SettingsWindow(SettingsWindowVM vm)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            CancelCloseCommand = new RelayCommand(CancelCloseCommandOnExecute);
            SaveCloseCommand =  new RelayCommand(SaveCloseCommandOnExecute);

            InitializeComponent();
            DataContext = _vm = vm;
            vm.CustomLabelsVM.DialogService = new DialogService(this);
            vm.BeginEdit();
        }


        private static readonly HashSet<string> _externalAssemblies = new HashSet<string>
        {
            "MahApps.Metro.IconPacks.Modern",
            "ColorPickerWPF"
        };
        // Problem loading some dlls from the code base
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // name is in this format Xceed.Wpf.Toolkit, Version=3.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4
            var asmName = args.Name.Substring(0, args.Name.IndexOf(','));
            if (!_externalAssemblies.Contains(asmName))
            {
                return null;
            }

            var filename = Path.Combine(DirectoryHelper.BaseDirectory, asmName + ".dll");
            return File.Exists(filename) ? Assembly.LoadFrom(filename) : null;
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

            Saved?.Invoke(this, EventArgs.Empty);
        }
    }
}
