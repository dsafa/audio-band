using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AudioBand.Logging;
using AudioBand.Models;
using AudioBand.Settings.Migrations;
using Nett;
using NLog;

// Alias the current settings version
using CurrentSettings = AudioBand.Settings.Models.V4.SettingsV4;

namespace AudioBand.Settings.Persistence
{
    /// <summary>
    /// Reads and writes persisted settings. The current type of the settings will change depending on the latest version.
    /// </summary>
    public class PersistSettings : IPersistSettings
    {
        private static readonly Dictionary<string, Type> SettingsTypeTable = new Dictionary<string, Type>()
        {
            { "0.1", typeof(AudioBand.Settings.Models.V1.AudioBandSettings) },
            { "2", typeof(AudioBand.Settings.Models.V2.Settings) },
            { "3", typeof(AudioBand.Settings.Models.V3.SettingsV3) },
            { "4", typeof(AudioBand.Settings.Models.V4.SettingsV4) },
        };

        private static readonly string CurrentSettingsVersion = "4";
        private static readonly string SettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioBand");
        private static readonly string SettingsFilePath = Path.Combine(SettingsDirectory, "audioband.settings");
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<PersistSettings>();

        /// <inheritdoc />
        public void WriteSettings(PersistedSettingsDto settings)
        {
            SerializeSettings(DtoToCurrentSettings(settings));
        }

        /// <inheritdoc />
        public PersistedSettingsDto ReadSettings()
        {
            return CurrentSettingsToDto(InitAndLoadSettings());
        }

        /// <inheritdoc />
        public void WriteProfiles(IEnumerable<UserProfile> profiles, string path)
        {
            // Just write out the settings object stripped of everything else to make it easier to import/export
            // And as a bonus we can reuse the migrations so we can import older settings.
            var exportObject = new CurrentSettings
            {
                Profiles = profiles.ToList(),
            };
            Toml.WriteFile(exportObject, path, TomlHelper.DefaultSettings);
        }

        /// <inheritdoc />
        public IEnumerable<UserProfile> ReadProfiles(string path)
        {
            var tomlFile = Toml.ReadFile(path, TomlHelper.DefaultSettings);
            var version = tomlFile["Version"].Get<string>();
            var settings = SettingsMigration.MigrateSettings<CurrentSettings>(tomlFile.Get(SettingsTypeTable[version]), version, CurrentSettingsVersion);
            return settings.Profiles;
        }

        private CurrentSettings InitAndLoadSettings()
        {
            // Tries to load settings from the path.
            // 1. If no settings exist, then create a new default settings file.
            // 2. If there are errors with loading settings, then backup and create a new settings file.
            // 3. If the settings file is an older version, then run migrations to update to the latest format.
            if (!Directory.Exists(SettingsDirectory))
            {
                Directory.CreateDirectory(SettingsDirectory);
            }

            if (!File.Exists(SettingsFilePath))
            {
                return CreateDefaultSettingsFile();
            }

            try
            {
                return LoadSettingsFromPath(SettingsFilePath);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Unable to load settings");
                var backupPath = Path.Combine(SettingsDirectory, "audioband.settings.backup-" + DateTime.Now.Ticks);
                File.Copy(SettingsFilePath, backupPath, true);
                Logger.Info("Creating new default settings. Backup created at {backup}", backupPath);

                return CreateDefaultSettingsFile();
            }
        }

        private CurrentSettings LoadSettingsFromPath(string path)
        {
            var tomlFile = Toml.ReadFile(path, TomlHelper.DefaultSettings);
            var version = tomlFile["Version"].Get<string>();

            // Create backup before migration
            if (version != CurrentSettingsVersion)
            {
                Toml.WriteFile(tomlFile, Path.Combine(SettingsDirectory, $"audioband.settings.{version}"), TomlHelper.DefaultSettings);
                var settings = SettingsMigration.MigrateSettings<CurrentSettings>(tomlFile.Get(SettingsTypeTable[version]), version, CurrentSettingsVersion);

                SerializeSettings(settings);
                return settings;
            }

            return tomlFile.Get<CurrentSettings>();
        }

        private CurrentSettings CreateDefaultSettingsFile()
        {
            var settings = new CurrentSettings()
            {
                AudioSource = null,
                AudioSourceSettings = new List<AudioSourceSettings>(),
                Profiles = new List<UserProfile> { UserProfile.CreateDefaultProfile(UserProfile.DefaultProfileName) },
                CurrentProfileName = UserProfile.DefaultProfileName,
            };

            SerializeSettings(settings);
            return settings;
        }

        private void SerializeSettings(CurrentSettings settings)
        {
            try
            {
                Toml.WriteFile(settings, SettingsFilePath, TomlHelper.DefaultSettings);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private CurrentSettings DtoToCurrentSettings(PersistedSettingsDto dto)
        {
            return new CurrentSettings
            {
                AudioSource = dto.AudioSource,
                CurrentProfileName = dto.CurrentProfileName,
                Profiles = dto.Profiles.ToList(),
                AudioSourceSettings = dto.AudioSourceSettings.ToList(),
            };
        }

        private PersistedSettingsDto CurrentSettingsToDto(CurrentSettings settings)
        {
            return new PersistedSettingsDto
            {
                AudioSource = settings.AudioSource,
                CurrentProfileName = settings.CurrentProfileName,
                Profiles = settings.Profiles,
                AudioSourceSettings = settings.AudioSourceSettings,
            };
        }
    }
}
