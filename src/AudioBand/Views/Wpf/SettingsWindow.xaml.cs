using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms.Integration;
using AudioBand.Commands;
using AudioBand.ViewModels;
using PubSub.Extension;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// The code behind for the settings window.
    /// </summary>
    public partial class SettingsWindow : ISettingsWindow
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
        /// <param name="audioBandVM">The audioband view model</param>
        /// <param name="albumArtPopupVM">The album art popup view model</param>
        /// <param name="albumArtVM">The album art view model</param>
        /// <param name="customLabelsVM">The custom labels view model</param>
        /// <param name="aboutVm">The about dialog view model</param>
        /// <param name="nextButtonVM">The next button view model</param>
        /// <param name="playPauseButtonVM">The play/pause button view model</param>
        /// <param name="previousButtonVM">The previous button view model</param>
        /// <param name="progressBarVM">The progress bar view model</param>
        public SettingsWindow(
            SettingsWindowVM vm,
            AudioBandVM audioBandVM,
            AlbumArtPopupVM albumArtPopupVM,
            AlbumArtVM albumArtVM,
            CustomLabelsVM customLabelsVM,
            AboutVM aboutVm,
            NextButtonVM nextButtonVM,
            PlayPauseButtonVM playPauseButtonVM,
            PreviousButtonVM previousButtonVM,
            ProgressBarVM progressBarVM)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            ElementHost.EnableModelessKeyboardInterop(this);

            CancelCloseCommand = new RelayCommand(CancelCloseCommandOnExecute);
            SaveCloseCommand = new RelayCommand(SaveCloseCommandOnExecute);

            AudioBandVM = audioBandVM;
            AlbumArtPopupVM = albumArtPopupVM;
            AlbumArtVM = albumArtVM;
            CustomLabelsVM = customLabelsVM;
            AboutVM = aboutVm;
            NextButtonVM = nextButtonVM;
            PlayPauseButtonVM = playPauseButtonVM;
            PreviousButtonVM = previousButtonVM;
            ProgressBarVM = progressBarVM;

            InitializeComponent();
            DataContext = vm;
        }

        /// <inheritdoc/>
        public event EventHandler Saved;

        /// <inheritdoc/>
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
        public AudioBandVM AudioBandVM { get; private set; }

        /// <inheritdoc/>
        public AlbumArtPopupVM AlbumArtPopupVM { get; private set; }

        /// <inheritdoc/>
        public AlbumArtVM AlbumArtVM { get; private set; }

        /// <inheritdoc/>
        public CustomLabelsVM CustomLabelsVM { get; private set; }

        /// <inheritdoc/>
        public AboutVM AboutVm { get; private set; }

        /// <inheritdoc/>
        public NextButtonVM NextButtonVM { get; private set; }

        /// <inheritdoc/>
        public PlayPauseButtonVM PlayPauseButtonVM { get; private set; }

        /// <inheritdoc/>
        public PreviousButtonVM PreviousButtonVM { get; private set; }

        /// <inheritdoc/>
        public ProgressBarVM ProgressBarVM { get; private set; }

        /// <inheritdoc/>
        public AboutVM AboutVM { get; private set; }

        /// <inheritdoc/>
        public ObservableCollection<AudioSourceSettingsVM> AudioSourceSettingsVM => new ObservableCollection<AudioSourceSettingsVM>();

        /// <inheritdoc/>
        public void ShowWindow()
        {
            Show();
        }

        /// <inheritdoc/>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;

            if (_shouldSave)
            {
                this.Publish(EditMessage.AcceptEdits);
                Saved?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                this.Publish(EditMessage.CancelEdits);
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
