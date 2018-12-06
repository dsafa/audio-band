using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using AudioBand.Commands;
using AudioBand.ViewModels;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// The code behind for the settings window.
    /// </summary>
    internal partial class SettingsWindow
    {
        private static readonly HashSet<string> _bindingHelpAssemblies = new HashSet<string>
        {
            "MahApps.Metro.IconPacks.Material",
            "ColorPickerWPF"
        };

        private bool _shouldSave;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindow"/> class
        /// with the settings viewmodel.
        /// </summary>
        /// <param name="vm">The settings window viewmodel.</param>
        internal SettingsWindow(SettingsWindowVM vm)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            CancelCloseCommand = new RelayCommand(CancelCloseCommandOnExecute);
            SaveCloseCommand = new RelayCommand(SaveCloseCommandOnExecute);

            InitializeComponent();
            DataContext = vm;
        }

        /// <summary>
        /// Occurs when settings are saved.
        /// </summary>
        public event EventHandler Saved;

        /// <summary>
        /// Occurs when settings are canceled.
        /// </summary>
        public event EventHandler Canceled;

        /// <summary>
        /// Gets the command to cancel changes and close.
        /// </summary>
        public RelayCommand CancelCloseCommand { get; }

        /// <summary>
        /// Gets the command to save changes and close.
        /// </summary>
        public RelayCommand SaveCloseCommand { get; }

        /// <inheritdoc/>
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

        // Problem with late binding. Fuslogvw shows its not probing the original location.
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
    }
}
