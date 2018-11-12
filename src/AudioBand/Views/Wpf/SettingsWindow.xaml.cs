using AudioBand.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AudioBand.Views.Wpf
{
    internal partial class SettingsWindow
    {
        internal SettingsWindow(SettingsWindowVM vm)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            InitializeComponent();
            DataContext = vm;
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

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private async void ResetSettingOnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var res = await ShowConfirmationDialog("Reset Setting", "Are you sure you want to reset this setting to the default values?");
            if (res != MessageDialogResult.Affirmative)
            {
                return;
            }

            var resettableObj = e.Parameter as IResettableObject;
            resettableObj.Reset();
        }

        private async Task<MessageDialogResult> ShowConfirmationDialog(string title, string message)
        {
            var dialogSettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
                AnimateHide = false,
                AnimateShow = false
            };

            return await this.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative, dialogSettings);
        }
    }
}
