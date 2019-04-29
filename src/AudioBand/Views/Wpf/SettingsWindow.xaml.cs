using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using AudioBand.Commands;
using AudioBand.Messages;
using AudioBand.ViewModels;
using PubSub.Extension;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// The code behind for the settings window.
    /// </summary>
    public partial class SettingsWindow : ISettingsWindow
    {
        private static readonly HashSet<string> LateBindAssemblies = new HashSet<string>
        {
            "MahApps.Metro.IconPacks.Material",
            "MahApps.Metro",
            "FluentWPF",
            "System.Windows.Interactivity"
        };

        private bool _shouldSave;
        private readonly IDialogService _dialogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindow"/> class
        /// with the settings viewmodel.
        /// </summary>
        /// <param name="vm">The settings window viewmodel.</param>
        /// <param name="audioBandVM">The audioband view model</param>
        /// <param name="albumArtPopupVM">The album art popup view model</param>
        /// <param name="albumArtVM">The album art view model</param>
        /// <param name="customLabelsVM">The custom labels view model</param>
        /// <param name="nextButtonVM">The next button view model</param>
        /// <param name="playPauseButtonVM">The play/pause button view model</param>
        /// <param name="previousButtonVM">The previous button view model</param>
        /// <param name="progressBarVM">The progress bar view model</param>
        /// <param name="dialogService">The dialog service</param>
        public SettingsWindow(
            SettingsWindowVM vm,
            AudioBandVM audioBandVM,
            AlbumArtPopupVM albumArtPopupVM,
            AlbumArtVM albumArtVM,
            CustomLabelsVM customLabelsVM,
            NextButtonVM nextButtonVM,
            PlayPauseButtonVM playPauseButtonVM,
            PreviousButtonVM previousButtonVM,
            ProgressBarVM progressBarVM,
            IDialogService dialogService)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            ElementHost.EnableModelessKeyboardInterop(this);

            CancelCloseCommand = new RelayCommand(CancelCloseCommandOnExecute);
            SaveCloseCommand = new RelayCommand(SaveCloseCommandOnExecute);
            OpenAboutDialogCommand = new RelayCommand(OpenAboutCommandOnExecute);

            AudioBandVM = audioBandVM;
            AlbumArtPopupVM = albumArtPopupVM;
            AlbumArtVM = albumArtVM;
            CustomLabelsVM = customLabelsVM;
            NextButtonVM = nextButtonVM;
            PlayPauseButtonVM = playPauseButtonVM;
            PreviousButtonVM = previousButtonVM;
            ProgressBarVM = progressBarVM;

            InitializeComponent();
            DataContext = vm;
            _dialogService = dialogService;

            Activated += OnActivated;
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

        /// <summary>
        /// Gets the command to open the about dialog
        /// </summary>
        public RelayCommand OpenAboutDialogCommand { get; }

        /// <inheritdoc/>
        public AudioBandVM AudioBandVM { get; private set; }

        /// <inheritdoc/>
        public AlbumArtPopupVM AlbumArtPopupVM { get; private set; }

        /// <inheritdoc/>
        public AlbumArtVM AlbumArtVM { get; private set; }

        /// <inheritdoc/>
        public CustomLabelsVM CustomLabelsVM { get; private set; }

        /// <inheritdoc/>
        public NextButtonVM NextButtonVM { get; private set; }

        /// <inheritdoc/>
        public PlayPauseButtonVM PlayPauseButtonVM { get; private set; }

        /// <inheritdoc/>
        public PreviousButtonVM PreviousButtonVM { get; private set; }

        /// <inheritdoc/>
        public ProgressBarVM ProgressBarVM { get; private set; }

        /// <inheritdoc/>
        public ObservableCollection<AudioSourceSettingsVM> AudioSourceSettingsVM { get; } = new ObservableCollection<AudioSourceSettingsVM>();

        /// <inheritdoc/>
        public void ShowWindow()
        {
            _shouldSave = false;
            Show();
            Activate();
        }

        /// <inheritdoc/>
        protected override void OnClosing(CancelEventArgs e)
        {
            // Hide instead of closing the window
            e.Cancel = true;

            if (_shouldSave)
            {
                this.Publish(EndEditMessage.AcceptEdits);
                Saved?.Invoke(this, EventArgs.Empty);
                Hide();
                return;
            }

            if (_dialogService.ShowConfirmationDialog(ConfirmationDialogType.DiscardChanges))
            {
                this.Publish(EndEditMessage.CancelEdits);
                Canceled?.Invoke(this, EventArgs.Empty);
                Hide();
            }
        }

        /// <summary>
        /// Manually handle tab navigation since deskband focus is weird
        /// </summary>
        /// <param name="e">Key event</param>
        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            base.OnPreviewKeyUp(e);
            if (e.Key != Key.Tab)
            {
                return;
            }

            var focusedElement = Keyboard.FocusedElement as UIElement;
            FocusNavigationDirection direction = Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)
                ? FocusNavigationDirection.Previous
                : FocusNavigationDirection.Next;

            focusedElement?.MoveFocus(new TraversalRequest(direction));
        }

        // Problem with late binding. Fuslogvw shows its not probing the original location.
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // name is in this format Xceed.Wpf.Toolkit, Version=3.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4
            var asmName = args.Name.Substring(0, args.Name.IndexOf(','));
            if (!LateBindAssemblies.Contains(asmName))
            {
                return null;
            }

            var filename = Path.Combine(DirectoryHelper.BaseDirectory, asmName + ".dll");
            return File.Exists(filename) ? Assembly.LoadFrom(filename) : null;
        }

        private void OnActivated(object sender, EventArgs e)
        {
            this.Publish(FocusChangedMessage.FocusCaptured);
        }

        private void SaveCloseCommandOnExecute(object o)
        {
            _shouldSave = true;
            Close();
        }

        private void CancelCloseCommandOnExecute(object o)
        {
            Close();
        }

        private void OpenAboutCommandOnExecute(object o)
        {
            _dialogService.ShowAboutDialog();
        }
    }
}
