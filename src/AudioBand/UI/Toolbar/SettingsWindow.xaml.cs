using AudioBand.Commands;
using AudioBand.Messages;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace AudioBand.UI
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
        /// <param name="viewModel">The settings window viewmodel.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="messageBus">The message bus.</param>
        public SettingsWindow(SettingsWindowViewModel viewModel, IDialogService dialogService, IMessageBus messageBus)
        {
            OpenAboutDialogCommand = new RelayCommand(OpenAboutCommandOnExecute);

            InitializeComponent();
            DataContext = viewModel;
            _dialogService = dialogService;
            _messageBus = messageBus;
            _messageBus.Subscribe<SettingsWindowMessage>(SettingsWindowMessageHandler);

            Activated += OnActivated;
            Closing += OnClosing;
            Loaded += OnLoaded;
            viewModel.PropertyChanged += ViewModelOnPropertyChanged;
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

            switch (e.Key)
            {
                case Key.Tab:
                {
                    var focusedElement = Keyboard.FocusedElement as UIElement;
                    FocusNavigationDirection direction = Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)
                        ? FocusNavigationDirection.Previous
                        : FocusNavigationDirection.Next;

                    focusedElement?.MoveFocus(new TraversalRequest(direction));
                    break;
                }
                case Key.W:
                {
                    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                    {
                        Hide();
                    }
                    break;
                }
            }
        }

        private void OnActivated(object sender, EventArgs e)
        {
            _messageBus.Publish(FocusChangedMessage.FocusCaptured);
        }

        private void OpenAboutCommandOnExecute()
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
            if (e.PropertyName != nameof(SettingsWindowViewModel.SelectedViewModel))
            {
                return;
            }

            ContentScrollView.ScrollToTop();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Windows 10 1903 acrylic fix
            var win1903 = new Version(10, 0, 18362);
            if (Environment.OSVersion.Version < win1903)
            {
                return;
            }

            ((HwndSource)PresentationSource.FromVisual(this)).AddHook(Hook);
        }

        private IntPtr Hook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            const int WM_ENTERSIZEMOVE = 0x0231;
            const int WM_EXITSIZEMOVE = 0x0232;

            switch (msg)
            {
                case WM_ENTERSIZEMOVE:
                    NativeMethods.DisableAcrylic(this);
                    break;
                case WM_EXITSIZEMOVE:
                    NativeMethods.EnableAcrylic(this);
                    break;
            }

            return IntPtr.Zero;
        }
    }
}
