using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using AudioBand.ViewModels;
using MahApps.Metro.Controls;
using NLog;
using TextAlignment = AudioBand.ViewModels.TextAlignment;

namespace AudioBand.Settings
{
    internal partial class SettingsWindow
    {
        internal event EventHandler Saved;
        internal Appearance Appearance { get; set; }
        public IEnumerable<TextAlignment> TextAlignValues { get; } = Enum.GetValues(typeof(TextAlignment)).Cast<TextAlignment>();

        internal SettingsWindow(Appearance appearance)
        {
            Appearance = appearance;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            InitializeComponent();
            DataContext = Appearance;
        }

        // Problem loading xceed.wpf.toolkit assembly normally
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (!args.Name.StartsWith("Xceed.Wpf.Toolkit"))
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

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            Saved?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
