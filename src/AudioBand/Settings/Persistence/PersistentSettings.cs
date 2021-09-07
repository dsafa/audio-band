using AudioBand.Logging;
using AudioBand.Models;
using AudioBand.Settings.Migrations;
using Nett;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
// Alias the current settings version
using CurrentSettings = AudioBand.Settings.Models.V4.SettingsV4;

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
            var json = JsonConvert.SerializeObject(settings);
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
            var userProfiles = new List<UserProfile>();
            var fileNames = Directory.GetFiles(ProfilesDirectory).Where(x => x.EndsWith(".profile.json")).ToArray();

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
            var profiles = userProfiles.ToArray();
            var fileNames = Directory.GetFiles(ProfilesDirectory).Where(x => x.EndsWith(".profile.json")).ToArray();

            // This will clear profiles if they got deleted.
            for (int i = 0; i < fileNames.Length; i++)
            {
                File.Delete(fileNames[i]);
            }

            for (int i = 0; i < profiles.Length; i++)
            {
                var json = JsonConvert.SerializeObject(profiles[i]);
                var path = Path.Combine(ProfilesDirectory, $"{profiles[i].Name}.json");

                File.WriteAllText(path, json);
            }
        }
    }
}
