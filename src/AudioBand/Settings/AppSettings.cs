using AudioBand.Models;
using AudioBand.Settings.Migrations;
using Nett;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AudioBand.Settings.Models.v2;
using AudioSourceSettings = AudioBand.Models.AudioSourceSettings;

namespace AudioBand.Settings
{
    internal class AppSettings
    {
        private static readonly Dictionary<string, Type> SettingsTable = new Dictionary<string, Type>()
        {
            {"0.1", typeof(Settings.Models.v1.AudioBandSettings)},
            {"2", typeof(Settings.Models.v2.Settings)}
        };
        private static string CurrentVersion = "2";
        private static readonly string SettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioBand");
        private static readonly string SettingsFilePath = Path.Combine(SettingsDirectory, "audioband.settings");
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly TomlSettings _tomlSettings;
        private Models.v2.Settings _settings;

        public string Version => _settings.Version;

        public string AudioSource
        {
            get => _settings.AudioSource;
            set => _settings.AudioSource = value;
        }

        public AlbumArtPopup AlbumArtPopup { get; set; }

        public AlbumArt AlbumArt { get; set; }

        public AudioBand.Models.AudioBand AudioBand { get;set; }

        public List<CustomLabel> CustomLabels { get; set; }

        public NextButton NextButton { get; set; }

        public PreviousButton PreviousButton { get; set; }

        public PlayPauseButton PlayPauseButton { get; set; }

        public ProgressBar ProgressBar { get; set; }

        public List<AudioSourceSettings> AudioSourceSettings { get; set; }

        public AppSettings()
        {
            _tomlSettings = TomlSettings.Create(cfg =>
            {
                cfg.ConfigureType<Color>(type => type.WithConversionFor<TomlString>(convert => convert
                    .ToToml(ColorTranslator.ToHtml)
                    .FromToml(tomlString => ColorTranslator.FromHtml(tomlString.Value))));
                cfg.ConfigureType<CustomLabel.TextAlignment>(type => type.WithConversionFor<TomlString>(convert => convert
                    .ToToml(SerializationConversions.EnumToString)
                    .FromToml(str => SerializationConversions.StringToEnum<CustomLabel.TextAlignment>(str.Value))));
            });

            if (!Directory.Exists(SettingsDirectory))
            {
                Directory.CreateDirectory(SettingsDirectory);
            }

            if (!File.Exists(SettingsFilePath))
            {
                CreateDefault();
                Toml.WriteFile(_settings, SettingsFilePath, _tomlSettings);
            }
            else
            {
                LoadSettings();
            }

            GetModels();
        }

        public void Save()
        {
            try
            {
                _settings.AlbumArtPopupSettings = ToSetting<AlbumArtPopupSettings>(AlbumArtPopup);
                _settings.AlbumArtSettings = ToSetting<AlbumArtSettings>(AlbumArt);
                _settings.AudioBandSettings = ToSetting<AudioBandSettings>(AudioBand);
                _settings.CustomLabelSettings = ToSetting<List<CustomLabelSettings>>(CustomLabels);
                _settings.NextButtonSettings = ToSetting<NextButtonSettings>(NextButton);
                _settings.PreviousButtonSettings = ToSetting<PreviousButtonSettings>(PreviousButton);
                _settings.PlayPauseButtonSettings = ToSetting<PlayPauseButtonSettings>(PlayPauseButton);
                _settings.ProgressBarSettings = ToSetting<ProgressBarSettings>(ProgressBar);
                _settings.AudioSourceSettings = ToSetting<List<Models.v2.AudioSourceSettings>>(AudioSourceSettings);
                Toml.WriteFile(_settings, SettingsFilePath, _tomlSettings);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private void GetModels()
        {
            AlbumArtPopup = ToModel<AlbumArtPopup>(_settings.AlbumArtPopupSettings);
            AlbumArt = ToModel<AlbumArt>(_settings.AlbumArtSettings);
            AudioBand = ToModel<AudioBand.Models.AudioBand>(_settings.AudioBandSettings);
            CustomLabels = ToModel<List<CustomLabel>>(_settings.CustomLabelSettings);
            NextButton = ToModel<NextButton>(_settings.NextButtonSettings);
            PreviousButton = ToModel<PreviousButton>(_settings.PreviousButtonSettings);
            PlayPauseButton = ToModel<PlayPauseButton>(_settings.PlayPauseButtonSettings);
            ProgressBar = ToModel<ProgressBar>(_settings.ProgressBarSettings);
            AudioSourceSettings = ToModel<List<AudioSourceSettings>>(_settings.AudioSourceSettings) ?? new List<AudioSourceSettings>();
        }

        private void LoadSettings()
        {
            try
            {
                var file = Toml.ReadFile(SettingsFilePath, _tomlSettings);
                var version = file["Version"].Get<string>();
                if (version != CurrentVersion)
                {
                    Toml.WriteFile(file, Path.Combine(SettingsDirectory, $"audioband.settings.{version}"), _tomlSettings);
                }
                _settings = Migration.MigrateSettings<Settings.Models.v2.Settings>(file.Get(SettingsTable[version]), version, CurrentVersion);
            }
            catch (Exception e)
            {
                Logger.Error("Error loading settings: " + e);
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
                Logger.Error($"Cannot convert setting {setting} to model {typeof(TModel)}");
                throw;
            }
        }

        private T ToSetting<T>(object model)
        {
            try
            {
                return SettingsMapper.ToModel<T>(model);
            }
            catch (Exception e)
            {
                Logger.Error($"Cannot model to settings {model} target: {typeof(T)}");
                throw;
            }
        }

        private void CreateDefault()
        {
            _settings = new Models.v2.Settings
            {
                AudioSourceSettings = new List<Models.v2.AudioSourceSettings>(),
                AudioBandSettings = ToSetting<Models.v2.AudioBandSettings>(new AudioBand.Models.AudioBand()),
                AlbumArtSettings = ToSetting<AlbumArtSettings>(new AlbumArt()),
                AudioSource = null,
                AlbumArtPopupSettings = ToSetting<AlbumArtPopupSettings>(new AlbumArtPopup()),
                PlayPauseButtonSettings = ToSetting<PlayPauseButtonSettings>(new PlayPauseButton()),
                NextButtonSettings = ToSetting<NextButtonSettings>(new NextButton()),
                PreviousButtonSettings = ToSetting<PreviousButtonSettings>(new PreviousButton()),
                ProgressBarSettings = ToSetting<ProgressBarSettings>(new ProgressBar()),
                CustomLabelSettings = new List<CustomLabelSettings> {ToSetting<CustomLabelSettings>(new CustomLabel())}
            };
        }
    }
}
