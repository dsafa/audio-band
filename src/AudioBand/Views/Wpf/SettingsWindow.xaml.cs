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

        public RelayCommand CancelCloseCommand { get; }
        public RelayCommand SaveCloseCommand { get; }

        public EventHandler Saved;
        public EventHandler Canceled;

        internal SettingsWindow(SettingsWindowVM vm)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            CancelCloseCommand = new RelayCommand(CancelCloseCommandOnExecute);
            SaveCloseCommand =  new RelayCommand(SaveCloseCommandOnExecute);

            InitializeComponent();
            DataContext = vm;
        }

        private static readonly HashSet<string> _bindingHelpAssemblies = new HashSet<string>
        {
            "MahApps.Metro.IconPacks.Material",
            "ColorPickerWPF"
        };

        // Problem with late binding? Fuslogvw shows its not probing the original location.
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // name is in this format Xceed.Wpf.Toolkit, Version=3.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4
            var asmName = args.Name.Substring(0, args.Name.IndexOf(','));
            if (!_bindingHelpAssemblies.Contains(asmName))
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
                Saved?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Canceled?.Invoke(this, EventArgs.Empty);
            }

            _shouldSave = false;
            Hide();
        }
    }
}
