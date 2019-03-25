using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AudioBand.Logging;
using AudioBand.Models;
using AudioBand.Settings.Migrations;
using AudioBand.Settings.Models.V2;
using Nett;
using NLog;
using AudioSourceSettings = AudioBand.Models.AudioSourceSettings;

namespace AudioBand.Settings
{
    /// <summary>
    /// Manages application settings.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        private static readonly Dictionary<string, Type> SettingsTable = new Dictionary<string, Type>()
        {
            { "0.1", typeof(Settings.Models.V1.AudioBandSettings) },
            { "2", typeof(Settings.Models.V2.Settings) }
        };

        private static readonly string CurrentVersion = "2";
        private static readonly string SettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioBand");
        private static readonly string SettingsFilePath = Path.Combine(SettingsDirectory, "audioband.settings");
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<AppSettings>();
        private readonly TomlSettings _tomlSettings;
        private Models.V2.Settings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the name of the current audio source.
        /// </summary>
        public string AudioSource
        {
            get => _settings.AudioSource;
            set => _settings.AudioSource = value;
        }

        /// <summary>
        /// Gets the saved album art popup model.
        /// </summary>
        public AlbumArtPopup AlbumArtPopup { get; private set; }

        /// <summary>
        /// Gets the saved album art model.
        /// </summary>
        public AlbumArt AlbumArt { get; private set; }

        /// <summary>
        /// Gets the saved audio band model.
        /// </summary>
        public AudioBand.Models.AudioBand AudioBand { get; private set; }

        /// <summary>
        /// Gets the saved labels.
        /// </summary>
        public List<CustomLabel> CustomLabels { get; private set; }

        /// <summary>
        /// Gets the saved button model.
        /// </summary>
        public NextButton NextButton { get; private set; }

        /// <summary>
        /// Gets the saved previous button model.
        /// </summary>
        public PreviousButton PreviousButton { get; private set; }

        /// <summary>
        /// Gets the saved play pause button model.
        /// </summary>
        public PlayPauseButton PlayPauseButton { get; private set; }

        /// <summary>
        /// Gets the saved progress bar model.
        /// </summary>
        public ProgressBar ProgressBar { get; private set; }

        /// <summary>
        /// Gets the saved audio source settings.
        /// </summary>
        public List<AudioSourceSettings> AudioSourceSettings { get; private set; }

        /// <summary>
        /// Save the settings.
        /// </summary>
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
                _settings.AudioSourceSettings = ToSetting<List<Models.V2.AudioSourceSettings>>(AudioSourceSettings);
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

                _settings = Migration.MigrateSettings<Settings.Models.V2.Settings>(file.Get(SettingsTable[version]), version, CurrentVersion);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error loading settings");
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
                Logger.Error(e, "Cannot convert setting {name} to model of type {type}", setting, typeof(TModel));
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
                Logger.Error(e, "Cannot convert model {@model} to setting of type {type}", model, typeof(T));
                throw;
            }
        }

        private void CreateDefault()
        {
            _settings = new Models.V2.Settings
            {
                AudioSourceSettings = new List<Models.V2.AudioSourceSettings>(),
                AudioBandSettings = ToSetting<Models.V2.AudioBandSettings>(new AudioBand.Models.AudioBand()),
                AlbumArtSettings = ToSetting<AlbumArtSettings>(new AlbumArt()),
                AudioSource = null,
                AlbumArtPopupSettings = ToSetting<AlbumArtPopupSettings>(new AlbumArtPopup()),
                PlayPauseButtonSettings = ToSetting<PlayPauseButtonSettings>(new PlayPauseButton()),
                NextButtonSettings = ToSetting<NextButtonSettings>(new NextButton()),
                PreviousButtonSettings = ToSetting<PreviousButtonSettings>(new PreviousButton()),
                ProgressBarSettings = ToSetting<ProgressBarSettings>(new ProgressBar()),
                CustomLabelSettings = new List<CustomLabelSettings> { ToSetting<CustomLabelSettings>(new CustomLabel()) }
            };
        }
    }
}
