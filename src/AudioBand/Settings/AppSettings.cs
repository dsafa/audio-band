using Nett;
using NLog;
using System;
using System.Drawing;
using System.IO;
using AudioBand.Models;

namespace AudioBand.Settings
{
    internal class AppSettings
    {
        private static readonly string SettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioBand");
        private static readonly string SettingsFilePath = Path.Combine(SettingsDirectory, "audioband.settings");
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly TomlSettings _tomlSettings;
        private readonly Models.v2.Settings _settings;

        public string Version => _settings.Version;
        public string AudioSource
        {
            get => _settings.AudioSource;
            set => _settings.AudioSource = value;
        }

        public AlbumArtPopup AlbumArtPopup { get; } = new AlbumArtPopup();
        public AlbumArt AlbumArt { get; } = new AlbumArt();
        public AudioBand.Models.AudioBand AudioBand { get; } = new AudioBand.Models.AudioBand();
        public CustomLabelsCollection CustomLabels { get; } = new CustomLabelsCollection();
        public NextButton NextButton { get; } = new NextButton();
        public PreviousButton PreviousButton { get; } = new PreviousButton();
        public PlayPauseButton PlayPauseButton { get; } = new PlayPauseButton();
        public ProgressBar ProgressBar { get; } = new ProgressBar();

        public AppSettings()
        {
            _settings = new Models.v2.Settings();

            //_tomlSettings = TomlSettings.Create(cfg =>
            //{
            //    cfg.ConfigureType<Color>(type => type.WithConversionFor<TomlString>(convert => convert
            //        .ToToml(ColorTranslator.ToHtml)
            //        .FromToml(tomlString => ColorTranslator.FromHtml(tomlString.Value))));
            //});
               
            //if (!Directory.Exists(SettingsDirectory))
            //{
            //    Directory.CreateDirectory(SettingsDirectory);
            //}

            //if (!File.Exists(SettingsFilePath))
            //{
            //    _settings = new Models.v2.Settings();

            //    Toml.WriteFile(_settings, SettingsFilePath, _tomlSettings);
            //}
            //else
            //{
            //    _settings = Toml.ReadFile<Models.v2.Settings>(SettingsFilePath, _tomlSettings);
            //}
        }

        public void Save()
        {
            try
            {
                //Toml.WriteFile(_settings, SettingsFilePath, _tomlSettings);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }
    }
}
