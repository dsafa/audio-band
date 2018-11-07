using AudioBand.Models;
using AudioBand.Settings.Migrations;
using Nett;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace AudioBand.Settings
{
    internal class AppSettings
    {
        private static readonly Dictionary<string, Type> SettingsTable = new Dictionary<string, Type>()
        {
            {"0.1", typeof(Settings.Models.v1.AudioBandSettings)},
            {"0.2", typeof(Settings.Models.v2.Settings)}
        };
        private static string CurrentVersion = "2";
        private static readonly string SettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioBand");
        private static readonly string SettingsFilePath = Path.Combine(SettingsDirectory, "audioband.settings");
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly TomlSettings _tomlSettings;
        private Models.v2.Settings _settings;

        public string Version => _settings.Version;

        public string AudioSource
        {
            get => _settings.AudioSource;
            set => _settings.AudioSource = value;
        }

        public AlbumArtPopup AlbumArtPopup { get; private set; }

        public AlbumArt AlbumArt { get; private set; }

        public AudioBand.Models.AudioBand AudioBand { get; private set; }

        public List<CustomLabel> CustomLabels { get; private set; }

        public NextButton NextButton { get; private set; }

        public PreviousButton PreviousButton { get; private set; }

        public PlayPauseButton PlayPauseButton { get; private set; }

        public ProgressBar ProgressBar { get; private set; }

        public AppSettings()
        {
            _tomlSettings = TomlSettings.Create(cfg =>
            {
                cfg.ConfigureType<Color>(type => type.WithConversionFor<TomlString>(convert => convert
                    .ToToml(ColorTranslator.ToHtml)
                    .FromToml(tomlString => ColorTranslator.FromHtml(tomlString.Value))));
            });

            if (!Directory.Exists(SettingsDirectory))
            {
                Directory.CreateDirectory(SettingsDirectory);
            }

            if (!File.Exists(SettingsFilePath))
            {
                _settings = new Models.v2.Settings();
                Toml.WriteFile(_settings, SettingsFilePath, _tomlSettings);
            }
            else
            {
                LoadSettings();
            }
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

        private void LoadSettings()
        {
            try
            {
                var file = Toml.ReadFile(SettingsFilePath, _tomlSettings);
                var version = file["Version"].Get<string>();
                _settings = Migration.MigrateSettings<Settings.Models.v2.Settings>(file.Get(SettingsTable[version]), version, CurrentVersion);

                AlbumArtPopup = ToModel<AlbumArtPopup>(_settings.AlbumArtPopupSettings);
                AlbumArt = ToModel<AlbumArt>(_settings.AlbumArtSettings);
                AudioBand = ToModel<AudioBand.Models.AudioBand>(_settings.AudioBandSettings);
                CustomLabels = ToModel<List<CustomLabel>>(_settings.CustomLabelSettings);
                NextButton = ToModel<NextButton>(_settings.NextButtonSettings);
                PreviousButton = ToModel<PreviousButton>(_settings.PreviousButtonSettings);
                PlayPauseButton = ToModel<PlayPauseButton>(_settings.PlayPauseButtonSettings);
                ProgressBar = ToModel<ProgressBar>(_settings.ProgressBarSettings);
            }
            catch (Exception e)
            {
                _logger.Error("Error loading settings: " + e);
                throw;
            }
        }

        private TModel ToModel<TModel>(object setting)
        {
            try
            {
                return SettingsMapper.ToModel<TModel>(setting);
            }
            catch (Exception e)
            {
                _logger.Error($"Cannot convert setting {setting} to model {typeof(TModel)}");
                throw;
            }
        }
    }
}
