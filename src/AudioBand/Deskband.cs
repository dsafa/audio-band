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
    /// Entry point of the application
    /// </summary>
    [ComVisible(true)]
    [Guid("957D8782-5B07-4126-9B24-1E917BAAAD64")]
    [CSDeskBandRegistration(Name = "Audio Band", ShowDeskBand = true)]
    public class Deskband : CSDeskBandWin
    {
        private MainControl _mainControl;
        private Container _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="Deskband"/> class.
        /// </summary>
        public Deskband()
        {
            // Assign a fake main window since some libraries require one
            if (System.Windows.Application.Current?.MainWindow == null)
            {
                new System.Windows.Application().MainWindow = new Window();
            }

            AudioBandLogManager.Initialize();
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => AudioBandLogManager.GetLogger("AudioBand").Error((Exception)args.ExceptionObject, "Unhandled Exception");
            ConfigureDependencies();
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
            _mainControl = null;
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
                _container.Register<ISettingsWindow, SettingsWindow>(Lifestyle.Transient);

                _container.Register<AboutVM>();
                _container.Register<AlbumArtVM>();
                _container.Register<AlbumArtPopupVM>();
                _container.Register<AudioBandVM>();
                _container.Register<CustomLabelVM>();
                _container.Register<CustomLabelsVM>();
                _container.Register<NextButtonVM>();
                _container.Register<PlayPauseButtonVM>();
                _container.Register<PreviousButtonVM>();
                _container.Register<ProgressBarVM>();
                _container.Register<SettingsWindowVM>();

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
