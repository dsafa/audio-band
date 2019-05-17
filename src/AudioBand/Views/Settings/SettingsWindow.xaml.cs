using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AudioBand.Commands;
using AudioBand.Messages;
using AudioBand.ViewModels;

namespace AudioBand.Views.Settings
{
    /// <summary>
    /// The code behind for the settings window.
    /// </summary>
    public partial class SettingsWindow
    {
        private readonly IDialogService _dialogService;
        private readonly IMessageBus _messageBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindow"/> class
        /// with the settings viewmodel.
        /// </summary>
        /// <param name="vm">The settings window viewmodel.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="messageBus">The message bus.</param>
        public SettingsWindow(SettingsWindowVM vm, IDialogService dialogService, IMessageBus messageBus)
        {
            OpenAboutDialogCommand = new RelayCommand(OpenAboutCommandOnExecute);

            InitializeComponent();
            DataContext = vm;
            _dialogService = dialogService;
            _messageBus = messageBus;
            _messageBus.Subscribe<SettingsWindowMessage>(SettingsWindowMessageHandler);

            Activated += OnActivated;
            Closing += OnClosing;
            Loaded += OnLoaded;
            vm.PropertyChanged += ViewModelOnPropertyChanged;
        }

        /// <summary>
        /// Gets the command to open the about dialog.
        /// </summary>
        public RelayCommand OpenAboutDialogCommand { get; }

        /// <summary>
        /// Manually handle tab navigation. Tab not working maybe because accelerators aren't translated.
        /// </summary>
        /// <param name="e">Key event.</param>
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

        private void OnActivated(object sender, EventArgs e)
        {
            _messageBus.Publish(FocusChangedMessage.FocusCaptured);
        }

        private void OpenAboutCommandOnExecute(object o)
        {
            _dialogService.ShowAboutDialog();
        }

        private void SettingsWindowMessageHandler(SettingsWindowMessage msg)
        {
            switch (msg)
            {
                case SettingsWindowMessage.CloseWindow:
                    Hide();
                    break;
                case SettingsWindowMessage.OpenWindow:
                    Show();
                    Activate();
                    break;
            }
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SettingsWindowVM.SelectedViewModel))
            {
                return;
            }

            ContentScrollView.ScrollToTop();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NativeMethods.FixWindowComposition(this);
        }
    }
}
