using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using AudioBand.AudioSource;
using AudioBand.Models;
using AudioBand.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using TextAlignment = AudioBand.ViewModels.TextAlignment;

namespace AudioBand.Views.Wpf
{
    internal partial class SettingsWindow
    {
        internal event EventHandler Saved;
        internal event EventHandler<TextLabelChangedEventArgs> NewLabelCreated;
        internal event EventHandler<TextLabelChangedEventArgs> LabelDeleted;

        public IEnumerable<TextAlignment> TextAlignValues { get; } = Enum.GetValues(typeof(TextAlignment)).Cast<TextAlignment>();
        public ObservableCollection<TextAppearance> TextAppearancesCollection { get; set; }
        public About About { get; } = new About();
        public AudioSourceSettingsCollectionViewModel AudioSourceSettingsViewModel { get; }

        private List<TextAppearance> _deletedTextAppearances;
        private List<TextAppearance> _addedTextAppearances;
        private bool _cancelEdit = true;
        private Appearance Appearance { get; set; }

        internal SettingsWindow(Appearance appearance,List<IAudioSource> audioSources, List<AudioSourceSettingsCollection> audioSourceSettings)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            Appearance = appearance;
            TextAppearancesCollection = new ObservableCollection<TextAppearance>(appearance.TextAppearances);
            AudioSourceSettingsViewModel = new AudioSourceSettingsCollectionViewModel(audioSources, audioSourceSettings);

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
            var res = await ShowConfirmationDialog("Delete label", "Are you sure you want to delete this label?");
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

        private void AlwaysCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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
