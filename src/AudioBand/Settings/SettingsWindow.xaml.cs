using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        internal event EventHandler<TextLabelChangedEventArgs> NewLabelCreated;
        internal event EventHandler<TextLabelChangedEventArgs> LabelDeleted;

        private Appearance Appearance { get; set; }
        public IEnumerable<TextAlignment> TextAlignValues { get; } = Enum.GetValues(typeof(TextAlignment)).Cast<TextAlignment>();
        public ObservableCollection<TextAppearance> TextAppearancesCollection { get; set; }
        private List<TextAppearance> _deletedTextAppearances;
        private List<TextAppearance> _addedTextAppearances;

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

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            Saved?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            CancelEdit();
            Close();
        }

        private void NewLabelButtonOnClick(object sender, RoutedEventArgs e)
        {
            var appearance = new TextAppearance { Name = "New label" };
            CreateLabel(appearance);

            _addedTextAppearances.Add(appearance);
            appearance.BeginEdit();
        }

        private void DeleteLabelOnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var appearance = button.DataContext as TextAppearance;
            DeleteLabel(appearance);
            _deletedTextAppearances.Add(appearance);
        }

        private void StartEdit()
        {
            Appearance.AlbumArtPopupAppearance.BeginEdit();
            Appearance.TextAppearances.ForEach(a => a.BeginEdit());
            Appearance.AlbumArtAppearance.BeginEdit();
            Appearance.AudioBandAppearance.BeginEdit();
            Appearance.NextSongButtonAppearance.BeginEdit();
            Appearance.PreviousSongButtonAppearance.BeginEdit();
            Appearance.PlayPauseButtonAppearance.BeginEdit();
            Appearance.ProgressBarAppearance.BeginEdit();

            _addedTextAppearances = new List<TextAppearance>();
            _deletedTextAppearances = new List<TextAppearance>();
        }

        private void CancelEdit()
        {
            Appearance.AlbumArtPopupAppearance.CancelEdit();
            Appearance.TextAppearances.ForEach(a => a.CancelEdit());
            Appearance.AlbumArtAppearance.CancelEdit();
            Appearance.AudioBandAppearance.CancelEdit();
            Appearance.NextSongButtonAppearance.CancelEdit();
            Appearance.PreviousSongButtonAppearance.CancelEdit();
            Appearance.PlayPauseButtonAppearance.CancelEdit();
            Appearance.ProgressBarAppearance.CancelEdit();

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
    }

    internal class TextLabelChangedEventArgs : EventArgs
    {
        public TextAppearance Appearance { get; }

        public TextLabelChangedEventArgs(TextAppearance appearance)
        {
            Appearance = appearance;
        }
    }
}
