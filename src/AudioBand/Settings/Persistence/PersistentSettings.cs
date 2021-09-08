using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AudioBand.Logging;
using AudioBand.Models;
using AudioBand.Settings.Migrations;
using Nett;
using Newtonsoft.Json;
using NLog;

// Alias the current settings version
using OldTomlSettings = AudioBand.Settings.Models.V4.SettingsV4;

namespace AudioBand.Settings.Persistence
{
    /// <summary>
    /// Reads and writes persisted settings. The current type of the settings will change depending on the latest version.
    /// </summary>
    public class PersistentSettings : IPersistentSettings
    {
        private static readonly string MainDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioBand");
        private static readonly string ProfilesDirectory = Path.Combine(MainDirectory, "Profiles");
        private static readonly string SettingsFilePath = Path.Combine(MainDirectory, "settings.json");
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<PersistentSettings>();

        private static readonly Dictionary<string, Type> SettingsTypeTable = new Dictionary<string, Type>()
        {
            { "0.1", typeof(Models.V1.AudioBandSettings) },
            { "2", typeof(Models.V2.SettingsV2) },
            { "3", typeof(Models.V3.SettingsV3) },
            { "4", typeof(Models.V4.SettingsV4) },
        };

        /// <inheritdoc />
        public void CheckAndConvertOldSettings()
        {
            var oldSettingsFilePath = Path.Combine(MainDirectory, "audioband.settings");

            if (!File.Exists(oldSettingsFilePath))
            {
                return;
            }

            // Make a backup so we can delete later
            File.Copy(oldSettingsFilePath, Path.Combine(MainDirectory, "old-audioband-settings.backup"));
            var oldSettings = LoadOldSettings(oldSettingsFilePath);

            if (!File.Exists(SettingsFilePath))
            {
                WriteSettings(new Settings()
                {
                    CurrentAudioSource = oldSettings.AudioSource,
                    CurrentProfileName = oldSettings.CurrentProfileName,
                    AudioBandSettings = oldSettings.AudioBandSettings,
                    AudioSourceSettings = oldSettings.AudioSourceSettings
                });
            }

            if (!Directory.Exists(ProfilesDirectory) || GetAllProfileFiles().Length <= 0)
            {
                WriteProfiles(oldSettings.Profiles);
            }

            File.Delete(oldSettingsFilePath);
        }

        /// <inheritdoc />
        public Settings ReadSettings()
        {
            if (!File.Exists(SettingsFilePath))
            {
                Directory.CreateDirectory(MainDirectory);
                var settings = new Settings();

                WriteSettings(settings);
                return settings;
            }

            var content = File.ReadAllText(SettingsFilePath);

            try
            {
                return JsonConvert.DeserializeObject<Settings>(content);
            }
            catch (Exception e)
            {
                // take a backup of the file and reset settings
                Logger.Error(e);
                var backupPath = Path.Combine(MainDirectory, $"settings.json.backup-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");
                File.WriteAllText(backupPath, content);

                var newSettings = new Settings();
                var json = JsonConvert.SerializeObject(newSettings);
                File.WriteAllText(SettingsFilePath, json);
                return newSettings;
            }
        }

        /// <inheritdoc />
        public void WriteSettings(Settings settings)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(SettingsFilePath, json);
        }

        /// <inheritdoc />
        public UserProfile ReadProfile(string path)
        {
            var json = File.ReadAllText(path);

            try
            {
                return JsonConvert.DeserializeObject<UserProfile>(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public IEnumerable<UserProfile> ReadProfiles()
        {
            if (!Directory.Exists(ProfilesDirectory))
            {
                Directory.CreateDirectory(ProfilesDirectory);
            }

            var userProfiles = new List<UserProfile>();
            var fileNames = GetAllProfileFiles();

            for (int i = 0; i < fileNames.Length; i++)
            {
                try
                {
                    var json = File.ReadAllText(fileNames[i]);
                    userProfiles.Add(JsonConvert.DeserializeObject<UserProfile>(json));
                }
                catch (Exception)
                {
                    // file might not be a userprofile, just skip it
                    continue;
                }
            }

            return userProfiles;
        }

        /// <inheritdoc />
        public void WriteProfiles(IEnumerable<UserProfile> userProfiles)
        {
            if (!Directory.Exists(ProfilesDirectory))
            {
                Directory.CreateDirectory(ProfilesDirectory);
            }

            var profiles = userProfiles.ToArray();

            for (int i = 0; i < profiles.Length; i++)
            {
                var json = JsonConvert.SerializeObject(profiles[i], Formatting.Indented);
                var path = Path.Combine(ProfilesDirectory, $"{profiles[i].Name}.profile.json");

                File.WriteAllText(path, json);
            }
        }

        /// <inheritdoc />
        public void DeleteProfile(string profileName)
        {
            try
            {
                File.Delete($"{profileName}.profile.json");
            }
            catch (Exception) { }
        }

        private string[] GetAllProfileFiles()
            => Directory.GetFiles(ProfilesDirectory).Where(x => x.EndsWith(".profile.json")).ToArray();

        private OldTomlSettings LoadOldSettings(string path)
        {
            try
            {
                var tomlFile = Toml.ReadFile(path, TomlHelper.DefaultSettings);
                var version = tomlFile["Version"].Get<string>();

                if (version != "4")
                {
                    Toml.WriteFile(tomlFile, Path.Combine(MainDirectory, $"old-audioband.settings.{version}"), TomlHelper.DefaultSettings);
                    return SettingsMigration.MigrateSettings<OldTomlSettings>(tomlFile.Get(SettingsTypeTable[version]), version, "4");
                }

                return tomlFile.Get<OldTomlSettings>();
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to read old settings file: {e.Message}");
                return new OldTomlSettings();
            }
        }
    }
}
