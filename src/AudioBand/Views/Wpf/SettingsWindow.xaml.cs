using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using AudioBand.Commands;
using AudioBand.Messages;
using AudioBand.ViewModels;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// The code behind for the settings window.
    /// </summary>
    public partial class SettingsWindow
    {
        private static readonly HashSet<string> LateBindAssemblies = new HashSet<string>
        {
            "MahApps.Metro.IconPacks.Material",
            "MahApps.Metro",
            "FluentWPF",
            "System.Windows.Interactivity",
        };

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
            : base(messageBus)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            ElementHost.EnableModelessKeyboardInterop(this);

            OpenAboutDialogCommand = new RelayCommand(OpenAboutCommandOnExecute);

            InitializeComponent();
            DataContext = vm;
            _dialogService = dialogService;
            _messageBus = messageBus;
            _messageBus.Subscribe<SettingsWindowMessage>(SettingsWindowMessageHandler);

            Activated += OnActivated;
            Closing += OnClosing;
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
    }
}
