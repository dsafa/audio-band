using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using AudioBand.AudioSource;
using AudioBand.Logging;
using AudioBand.Messages;
using AudioBand.Settings;
using AudioBand.Settings.Persistence;
using AudioBand.UI;
using CSDeskBand;
using NLog;
using SimpleInjector;

namespace AudioBand
{
    /// <summary>
    /// Entry point of the application.
    /// </summary>
    [ComVisible(true)]
    [Guid("957D8782-5B07-4126-9B24-1E917BAAAD64")]
    [CSDeskBandRegistration(Name = "Audio Band", ShowDeskBand = true)]
    public class Deskband : CSDeskBandWpf
    {
        private static readonly HashSet<string> LateBindAssemblies = new HashSet<string>
        {
            "MahApps.Metro.IconPacks.Material",
            "MahApps.Metro",
            "FluentWPF",
            "System.Windows.Interactivity",
        };

        private AudioBandToolbar _audioBandToolbar;
        private Container _container;
        private Window _settingsWindow;
        private ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Deskband"/> class.
        /// </summary>
        public Deskband()
        {
            // Fluentwpf requires an application window
            if (System.Windows.Application.Current == null)
            {
                new System.Windows.Application().MainWindow = new Window();
            }

            var initialSize = new DeskBandSize(50, 30);
            Options.HorizontalSize = initialSize;
            Options.MinHorizontalSize = initialSize;
            AudioBandLogManager.Initialize();
            _logger = AudioBandLogManager.GetLogger("AudioBand");
            _logger.Info("Starting AudioBand. Version: {version}, OS: {os}", GetType().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion, Environment.OSVersion);

            StartupCheck();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;

            ConfigureDependencies();

            _settingsWindow = _container.GetInstance<SettingsWindow>();
            _audioBandToolbar = _container.GetInstance<AudioBandToolbar>();

            _container.GetInstance<IMessageBus>().Subscribe<FocusChangedMessage>(FocusCaptured);
        }

        /// <inheritdoc />
        protected override UIElement UIElement => _audioBandToolbar;

        /// <inheritdoc/>
        protected override void DeskbandOnClosed()
        {
            base.DeskbandOnClosed();
            _audioBandToolbar = null;
            _settingsWindow = null;
        }

        // Problem with late binding. Fuslogvw shows its not probing the original location.
        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            // name is in this format Xceed.Wpf.Toolkit, Version=3.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4
            var asmName = args.Name.Split(',')[0];
            if (!LateBindAssemblies.Contains(asmName))
            {
                return null;
            }

            var filename = Path.Combine(DirectoryHelper.BaseDirectory, asmName + ".dll");
            return File.Exists(filename) ? Assembly.LoadFrom(filename) : null;
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            GlobalSettings.Default.UnhandledException = true;
            GlobalSettings.Default.Save();

            AudioBandLogManager.GetLogger("AudioBand").Error((Exception)args.ExceptionObject, "Unhandled Exception");
        }

        private void ConfigureDependencies()
        {
            try
            {
                _container = new Container();
                _container.RegisterInstance(Options);
                _container.RegisterInstance(TaskbarInfo);
                _container.Register<IMessageBus, MessageBus>(Lifestyle.Singleton);
                _container.Register<IAudioSourceManager, AudioSourceManager>(Lifestyle.Singleton);
                _container.Register<IAppSettings, AppSettings>(Lifestyle.Singleton);
                _container.Register<IDialogService, DialogService>(Lifestyle.Singleton);
                _container.Register<IViewModelContainer, ViewModelContainer>(Lifestyle.Singleton);
                _container.Register<IAudioSession, AudioSession>(Lifestyle.Singleton);
                _container.Register<IPersistSettings, PersistSettings>(Lifestyle.Singleton);

                _container.Register<AboutDialogViewModel>(Lifestyle.Singleton);
                _container.Register<AlbumArtViewModel>(Lifestyle.Singleton);
                _container.Register<AlbumArtPopupViewModel>(Lifestyle.Singleton);
                _container.Register<GlobalSettingsViewModel>(Lifestyle.Singleton);
                _container.Register<GeneralSettingsViewModel>(Lifestyle.Singleton);
                _container.Register<CustomLabelsViewModel>(Lifestyle.Singleton);
                _container.Register<NextButtonViewModel>(Lifestyle.Singleton);
                _container.Register<PlayPauseButtonViewModel>(Lifestyle.Singleton);
                _container.Register<PreviousButtonViewModel>(Lifestyle.Singleton);
                _container.Register<ProgressBarViewModel>(Lifestyle.Singleton);
                _container.Register<SettingsWindowViewModel>(Lifestyle.Singleton);
                _container.Register<AudioSourceSettingsViewModel>(Lifestyle.Singleton);
                _container.Register<RepeatModeButtonViewModel>(Lifestyle.Singleton);
                _container.Register<ShuffleModeButtonViewModel>(Lifestyle.Singleton);
                _container.Register<VolumeButtonViewModel>(Lifestyle.Singleton);

                _container.Verify();
            }
            catch (Exception e)
            {
                AudioBandLogManager.GetLogger("AudioBand").Error(e);
                throw;
            }
        }

        private void FocusCaptured(FocusChangedMessage msg)
        {
            // Capture focus so that tab button is captured
            if (msg == FocusChangedMessage.FocusCaptured)
            {
                UpdateFocus(true);
            }
        }

        private void StartupCheck()
        {
            if (!GlobalSettings.Default.UnhandledException)
            {
                return;
            }

            // Unhandled exception from last run. Prevent audioband from starting in case there is a crash loop.
            _logger.Info("Startup prevented due to previous unhandled exception. Open audioband again to ignore.");
            GlobalSettings.Default.UnhandledException = false;
            GlobalSettings.Default.Save();

            // Exception should make explorer remove the deskband from being automatically started.
            throw new Exception("Startup prevented due to previous unhandled exception.");
        }
    }
}
