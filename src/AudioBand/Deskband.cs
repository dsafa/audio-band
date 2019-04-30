using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using AudioBand.AudioSource;
using AudioBand.Logging;
using AudioBand.Messages;
using AudioBand.Models;
using AudioBand.Resources;
using AudioBand.Settings;
using AudioBand.ViewModels;
using AudioBand.Views.Wpf;
using AudioBandModel.Settings;
using CSDeskBand;
using SimpleInjector;

namespace AudioBand
{
    /// <summary>
    /// Entry point of the application.
    /// </summary>
    [ComVisible(true)]
    [Guid("957D8782-5B07-4126-9B24-1E917BAAAD64")]
    [CSDeskBandRegistration(Name = "Audio Band", ShowDeskBand = true)]
    public class Deskband : CSDeskBandWin
    {
        private MainControl _mainControl;
        private Container _container;
        private Window _settingsWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="Deskband"/> class.
        /// </summary>
        public Deskband()
        {
            // Fluentwpf requires an application window
            if (System.Windows.Application.Current?.MainWindow == null)
            {
                new System.Windows.Application().MainWindow = new Window();
            }

            AudioBandLogManager.Initialize();
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => AudioBandLogManager.GetLogger("AudioBand").Error((Exception)args.ExceptionObject, "Unhandled Exception");
            ConfigureDependencies();

            _settingsWindow = _container.GetInstance<SettingsWindow>();
            _mainControl = _container.GetInstance<MainControl>();
            _container.GetInstance<IMessageBus>().Subscribe<FocusChangedMessage>(FocusCaptured);
        }

        /// <inheritdoc/>
        protected override Control Control => _mainControl;

        /// <inheritdoc/>
        protected override void DeskbandOnClosed()
        {
            base.DeskbandOnClosed();
            _mainControl.CloseAudioband();
            _mainControl.Hide();
            _mainControl.Dispose();
            _mainControl = null;
            _settingsWindow = null;
        }

        private void ConfigureDependencies()
        {
            try
            {
                _container = new Container();
                _container.RegisterInstance(Options);
                _container.RegisterInstance(TaskbarInfo);
                _container.Register<IMessageBus, MessageBus>(Lifestyle.Singleton);
                _container.Register<Track>(Lifestyle.Singleton);
                _container.Register<IAudioSourceManager, AudioSourceManager>(Lifestyle.Singleton);
                _container.Register<IAppSettings, AppSettings>(Lifestyle.Singleton);
                _container.Register<IResourceLoader, ResourceLoader>(Lifestyle.Singleton);
                _container.Register<ICustomLabelService, CustomLabelService>(Lifestyle.Singleton);
                _container.Register<IDialogService, DialogService>(Lifestyle.Singleton);
                _container.Register<IViewModelContainer, ViewModelContainer>(Lifestyle.Singleton);

                _container.Register<AboutVM>(Lifestyle.Singleton);
                _container.Register<AlbumArtVM>(Lifestyle.Singleton);
                _container.Register<AlbumArtPopupVM>(Lifestyle.Singleton);
                _container.Register<AudioBandVM>(Lifestyle.Singleton);
                _container.Register<CustomLabelsVM>(Lifestyle.Singleton);
                _container.Register<NextButtonVM>(Lifestyle.Singleton);
                _container.Register<PlayPauseButtonVM>(Lifestyle.Singleton);
                _container.Register<PreviousButtonVM>(Lifestyle.Singleton);
                _container.Register<ProgressBarVM>(Lifestyle.Singleton);
                _container.Register<SettingsWindowVM>(Lifestyle.Singleton);

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
    }
}
