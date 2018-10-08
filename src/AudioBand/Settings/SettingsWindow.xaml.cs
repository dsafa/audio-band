using AudioBand.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using MahApps.Metro.Controls.Dialogs;
using TextAlignment = AudioBand.ViewModels.TextAlignment;

namespace AudioBand.Settings
{
    internal partial class SettingsWindow
    {
        internal event EventHandler Saved;
        internal event EventHandler<TextLabelChangedEventArgs> NewLabelCreated;
        internal event EventHandler<TextLabelChangedEventArgs> LabelDeleted;

        public IEnumerable<TextAlignment> TextAlignValues { get; } = Enum.GetValues(typeof(TextAlignment)).Cast<TextAlignment>();
        public ObservableCollection<TextAppearance> TextAppearancesCollection { get; set; }
                public About About { get; } = new About();

        private List<TextAppearance> _deletedTextAppearances;
        private List<TextAppearance> _addedTextAppearances;
        private bool _cancelEdit = true;
        private Appearance Appearance { get; set; }

        internal SettingsWindow(Appearance appearance)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            Appearance = appearance;
            TextAppearancesCollection = new ObservableCollection<TextAppearance>(appearance.TextAppearances);

            InitializeComponent();
            DataContext = appearance;

            StartEdit();
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
            base.OnClosing(e);

            // if closed by x button
            if (_cancelEdit)
            {
                CancelEdit();
            }
            else
            {
                Saved?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            _cancelEdit = false;
            Close();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            _cancelEdit = true;
            Close();
        }

        private void NewLabelOnExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var appearance = new TextAppearance { Name = "New label" };
            CreateLabel(appearance);

            _addedTextAppearances.Add(appearance);
            appearance.BeginEdit();
        }

        private void NewLabelCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (TabControl.SelectedItem as TabItem)?.Name == "Labels";
        }

        private async void DeleteLabelOnExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var dialogSettings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
                AnimateHide = false,
                AnimateShow = false
            };

            var res = await this.ShowMessageAsync("Delete label", "Are you sure you want to delete this label?", MessageDialogStyle.AffirmativeAndNegative, dialogSettings);
            if (res != MessageDialogResult.Affirmative)
            {
                return;
            }

            var appearance = e.Parameter as TextAppearance;

            DeleteLabel(appearance);

            // check if we are deleting a new label
            var newAppearance = _addedTextAppearances.FirstOrDefault(a => a.Tag == appearance.Tag);
            if (newAppearance != null)
            {
                // it was a new appearance
                _addedTextAppearances.Remove(newAppearance);
            }
            else
            {
                _deletedTextAppearances.Add(appearance);
            }
        }

        private void StartEdit()
        {
            Appearance.BeginEdit();
            _addedTextAppearances = new List<TextAppearance>();
            _deletedTextAppearances = new List<TextAppearance>();
        }

        private void CancelEdit()
        {
            Appearance.CancelEdit();

            foreach (var addedTextAppearance in _addedTextAppearances)
            {
                DeleteLabel(addedTextAppearance);
            }

            foreach (var deletedTextAppearance in _deletedTextAppearances)
            {
                CreateLabel(deletedTextAppearance);
            }
        }

        private void CreateLabel(TextAppearance appearance)
        {
            Appearance.TextAppearances.Add(appearance);
            TextAppearancesCollection.Add(appearance);

            NewLabelCreated?.Invoke(this, new TextLabelChangedEventArgs(appearance));
        }

        private void DeleteLabel(TextAppearance appearance)
        {
            TextAppearancesCollection.Remove(TextAppearancesCollection.Single(t => t.Tag == appearance.Tag));
            Appearance.TextAppearances.Remove(Appearance.TextAppearances.Single(t => t.Tag == appearance.Tag));

            LabelDeleted?.Invoke(this, new TextLabelChangedEventArgs(appearance));
        }

        private void ShowAboutOnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AboutDialog.IsOpen = true;
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
        }

        private void ResetSettingOnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is IResettableObject resettable)
            {
                resettable.Reset();
            }
        }

        private void AlwaysCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }

    internal class TextLabelChangedEventArgs : EventArgs
    {
        public TextAppearance Appearance { get; }

        public TextLabelChangedEventArgs(TextAppearance appearance)
        {
            Appearance = appearance;
        }
    }

    internal class About
    {
        public string Version { get; } = "AudioBand " + typeof(SettingsWindow).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        public string ProjectLink { get; } = @"https://github.com/dsafa/audio-band";
    }
}
