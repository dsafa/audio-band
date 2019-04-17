using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AudioBand.Logging;
using AudioBand.Models;
using AudioBand.Settings.Migrations;
using AudioBand.Settings.Models.v3;
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
            { "2", typeof(Settings.Models.V2.Settings) },
            { "3", typeof(SettingsV3) }
        };

        private static readonly string CurrentVersion = "3";
        private static readonly string SettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioBand");
        private static readonly string SettingsFilePath = Path.Combine(SettingsDirectory, "audioband.settings");
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<AppSettings>();
        private readonly TomlSettings _tomlSettings;
        private SettingsV3 _settings;
        private UISettingsV3 _currentProfile;

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
                Toml.WriteFile(_settings, SettingsFilePath, _tomlSettings);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private void GetModels()
        {
            _currentProfile = _settings.DefaultProfile;

            AlbumArtPopup = _currentProfile.AlbumArtPopupSettings;
            AlbumArt = _currentProfile.AlbumArtSettings;
            AudioBand = _currentProfile.AudioBandSettings;
            CustomLabels = _currentProfile.CustomLabelSettings;
            NextButton = _currentProfile.NextButtonSettings;
            PreviousButton = _currentProfile.PreviousButtonSettings;
            PlayPauseButton = _currentProfile.PlayPauseButtonSettings;
            ProgressBar = _currentProfile.ProgressBarSettings;
            AudioSourceSettings = _settings?.AudioSourceSettings ?? new List<AudioSourceSettings>();
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

                _settings = Migration.MigrateSettings<SettingsV3>(file.Get(SettingsTable[version]), version, CurrentVersion);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error loading settings");
                throw;
            }
        }

        private void CreateDefault()
        {
            _settings = new SettingsV3()
            {
                AudioSource = null,
                AudioSourceSettings = new List<AudioSourceSettings>(),
                DefaultProfile = new UISettingsV3()
                {
                    AudioBandSettings = new AudioBand.Models.AudioBand(),
                    AlbumArtSettings = new AlbumArt(),
                    AlbumArtPopupSettings = new AlbumArtPopup(),
                    PlayPauseButtonSettings = new PlayPauseButton(),
                    NextButtonSettings = new NextButton(),
                    PreviousButtonSettings = new PreviousButton(),
                    ProgressBarSettings = new ProgressBar(),
                    CustomLabelSettings = new List<CustomLabel> { new CustomLabel() }
                },
                Profiles = new Dictionary<string, UISettingsV3>()
            };
        }
    }
}
