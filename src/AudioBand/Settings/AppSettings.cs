using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AudioBand.Logging;
using AudioBand.Models;
using AudioBand.Settings.MappingProfiles;
using AudioBand.Settings.Migrations;
using AudioBand.Settings.Models.v3;
using AutoMapper;
using Nett;
using NLog;

namespace AudioBand.Settings
{
    /// <summary>
    /// Manages application settings.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        private static readonly Dictionary<string, Type> SettingsTable = new Dictionary<string, Type>()
        {
            { "0.1", typeof(AudioBand.Settings.Models.V1.AudioBandSettings) },
            { "2", typeof(AudioBand.Settings.Models.V2.Settings) },
            { "3", typeof(SettingsV3) },
        };

        private static readonly string CurrentVersion = "3";
        private static readonly string SettingsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AudioBand");
        private static readonly string SettingsFilePath = Path.Combine(SettingsDirectory, "audioband.settings");
        private static readonly ILogger Logger = AudioBandLogManager.GetLogger<AppSettings>();
        private static readonly MapperConfiguration ProfileMappingConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<UserProfileToSettingsProfile>());
        private SettingsV3 _settings;
        private Dictionary<string, UserProfile> _profiles = new Dictionary<string, UserProfile>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
        public AppSettings()
        {
            InitSettings();
            InitProfiles();

            if (_settings.AudioSourceSettings == null)
            {
                _settings.AudioSourceSettings = new List<AudioSourceSettings>();
            }

            SelectProfile(_settings.CurrentProfileName);
        }

        /// <summary>
        /// Occurs when the current profile changes
        /// </summary>
        public event EventHandler ProfileChanged;

        /// <summary>
        /// Gets or sets the name of the current audio source.
        /// </summary>
        public string AudioSource
        {
            get => _settings.AudioSource;
            set => _settings.AudioSource = value;
        }

        /// <summary>
        /// Gets the saved audio source settings.
        /// </summary>
        public List<AudioSourceSettings> AudioSourceSettings => _settings.AudioSourceSettings;

        /// <summary>
        /// Gets the current profile.
        /// </summary>
        public UserProfile CurrentProfile { get; private set; }

        /// <summary>
        /// Gets a list of available profiles.
        /// </summary>
        public IEnumerable<UserProfile> Profiles => _profiles.Values;

        /// <summary>
        /// Selects a new profile.
        /// </summary>
        /// <param name="profileName">The name of the profile to switch to.</param>
        public void SelectProfile(string profileName)
        {
            Debug.Assert(_profiles.ContainsKey(profileName), $"Selecting non existent profile {profileName}");

            CurrentProfile = _profiles[profileName];
            ProfileChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Creates a new profile.
        /// </summary>
        /// <param name="profileName">The name of the new profile.</param>
        public void CreateProfile(string profileName)
        {
            if (profileName == null)
            {
                throw new ArgumentNullException(nameof(profileName));
            }

            if (_profiles.ContainsKey(profileName))
            {
                throw new ArgumentException("Profile name already exists", nameof(profileName));
            }

            _profiles.Add(profileName, UserProfile.CreateInitialProfile());
        }

        /// <summary>
        /// Deletes the profile.
        /// </summary>
        /// <param name="profileName">The name of the profile to delete.</param>
        public void DeleteProfile(string profileName)
        {
            if (profileName == null)
            {
                throw new ArgumentNullException(nameof(profileName));
            }

            if (_profiles.Count == 1)
            {
                throw new InvalidOperationException("Must have at least one profile");
            }

            if (!_profiles.ContainsKey(profileName))
            {
                throw new ArgumentException($"Profile {profileName} does not exist", nameof(profileName));
            }

            _profiles.Remove(profileName);
        }

        /// <summary>
        /// Renames the current profile.
        /// </summary>
        /// <param name="newProfileName">The new profile name.</param>
        public void RenameCurrentProfile(string newProfileName)
        {
            if (newProfileName == null)
            {
                throw new ArgumentNullException(nameof(newProfileName));
            }

            if (CurrentProfile == null)
            {
                throw new InvalidOperationException("No profile selected. Current profile is null");
            }

            if (_profiles.ContainsKey(newProfileName))
            {
                throw new ArgumentException("Profile already exists", nameof(newProfileName));
            }

            _settings.CurrentProfileName = newProfileName;
            CurrentProfile.Name = newProfileName;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            try
            {
                // Write back to the settings object before saving.
                var mapper = ProfileMappingConfiguration.CreateMapper();
                _settings.CurrentProfileName = CurrentProfile.Name;
                _settings.Profiles = _profiles.ToDictionary(kvp => kvp.Key, kvp => mapper.Map<UserProfile, ProfileV3>(kvp.Value));
                Toml.WriteFile(_settings, SettingsFilePath, TomlHelper.DefaultSettings);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        /// <inheritdoc />
        public void ImportProfilesFromPath(string path)
        {
            // Todo: deal with different profile versions?
            var profilesToImport = Toml.ReadFile<ProfileExportV3>(path, TomlHelper.DefaultSettings);
            var mapper = ProfileMappingConfiguration.CreateMapper();
            foreach (var keyVal in profilesToImport.Profiles)
            {
                var key = GetUniqueProfileName(keyVal.Key);
                _profiles[key] = mapper.Map<ProfileV3, UserProfile>(keyVal.Value);
            }
        }

        /// <inheritdoc />
        public void ExportProfilesToPath(string path)
        {
            var exportObject = new ProfileExportV3 { Profiles = _settings.Profiles };
            Toml.WriteFile(exportObject, path, TomlHelper.DefaultSettings);
        }

        private void LoadSettingsFromPath(string path)
        {
            var tomlFile = Toml.ReadFile(path, TomlHelper.DefaultSettings);
            var version = tomlFile["Version"].Get<string>();

            // Create backup
            if (version != CurrentVersion)
            {
                Toml.WriteFile(tomlFile, Path.Combine(SettingsDirectory, $"audioband.settings.{version}"), TomlHelper.DefaultSettings);
                _settings = Migration.MigrateSettings<SettingsV3>(tomlFile.Get(SettingsTable[version]), version, CurrentVersion);
                Save();
            }
            else
            {
                // Fix any missing values
                var initial = new SettingsV3();
                var settings = tomlFile.Get<SettingsV3>();
                new MapperConfiguration(cfg => cfg.AddProfile<SettingsV3Profile>()).CreateMapper().Map(settings, initial);
                _settings = initial;
            }
        }

        private void CreateDefaultSettingsFile()
        {
            var settingsProfile = ProfileMappingConfiguration.CreateMapper().Map<UserProfile, ProfileV3>(UserProfile.CreateInitialProfile());

            _settings = new SettingsV3()
            {
                AudioSource = null,
                AudioSourceSettings = new List<AudioSourceSettings>(),
                Profiles = new Dictionary<string, ProfileV3> { { SettingsV3.DefaultProfileName, settingsProfile } },
                CurrentProfileName = SettingsV3.DefaultProfileName,
            };
            Save();
        }

        private string GetUniqueProfileName(string name)
        {
            string newName = name;
            int count = 0;
            while (_profiles.ContainsKey(newName))
            {
                newName += count;
            }

            return newName;
        }

        private void InitSettings()
        {
            if (!Directory.Exists(SettingsDirectory))
            {
                Directory.CreateDirectory(SettingsDirectory);
            }

            if (!File.Exists(SettingsFilePath))
            {
                CreateDefaultSettingsFile();
                return;
            }

            try
            {
                LoadSettingsFromPath(SettingsFilePath);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Unable to load settings");
                var backupPath = Path.Combine(SettingsDirectory, "audioband.settings.backup-" + DateTime.Now.Ticks);
                File.Copy(SettingsFilePath, backupPath, true);
                Logger.Info("Creating new default settings. Backup created at {backup}", backupPath);
                CreateDefaultSettingsFile();
            }
        }

        private void InitProfiles()
        {
            var mapper = ProfileMappingConfiguration.CreateMapper();
            _profiles = _settings.Profiles.ToDictionary(
                keyValPair => keyValPair.Key,
                keyValPair => mapper.Map<ProfileV3, UserProfile>(keyValPair.Value));
        }
    }
}
