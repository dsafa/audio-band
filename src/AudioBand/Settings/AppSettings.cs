using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AudioBand.Models;
using AudioBand.Settings.Persistence;

namespace AudioBand.Settings
{
    /// <summary>
    /// Manages application settings.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        private readonly IPersistSettings _persistSettings;
        private Dictionary<string, UserProfile> _profiles = new Dictionary<string, UserProfile>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
        /// <param name="persistSettings">The settings persistence object.</param>
        public AppSettings(IPersistSettings persistSettings)
        {
            _persistSettings = persistSettings;

            var dto = _persistSettings.ReadSettings();
            AudioSource = dto.AudioSource;
            AudioSourceSettings = dto.AudioSourceSettings?.ToList() ?? new List<AudioSourceSettings>();
            CheckAndLoadProfiles(dto);
            SelectProfile(dto.CurrentProfileName);
        }

        /// <summary>
        /// Occurs when the current profile changes
        /// </summary>
        public event EventHandler ProfileChanged;

        /// <summary>
        /// Gets or sets the name of the current audio source.
        /// </summary>
        public string AudioSource { get; set; }

        /// <summary>
        /// Gets the saved audio source settings.
        /// </summary>
        public List<AudioSourceSettings> AudioSourceSettings { get; }

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
            Save();
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

            _profiles.Add(profileName, UserProfile.CreateDefaultProfile(profileName));
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

            CurrentProfile.Name = newProfileName;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            var dto = new PersistedSettingsDto
            {
                AudioSource = AudioSource,
                CurrentProfileName = CurrentProfile.Name,
                Profiles = _profiles.Values,
                AudioSourceSettings = AudioSourceSettings,
            };
            _persistSettings.WriteSettings(dto);
        }

        /// <inheritdoc />
        public void ImportProfilesFromPath(string path)
        {
            var profiles = _persistSettings.ReadProfiles(path).ToList();

            // Check imported profiles for duplicate names before adding them to the dict.
            foreach (var profile in profiles)
            {
                var name = UserProfile.GetUniqueProfileName(_profiles.Keys, profile.Name);
                _profiles[name] = profile;
                _profiles[name].Name = name;
            }
        }

        /// <inheritdoc />
        public void ExportProfilesToPath(string path)
        {
            _persistSettings.WriteProfiles(_profiles.Values, path);
        }

        private void CheckAndLoadProfiles(PersistedSettingsDto dto)
        {
            /* If there are no profiles, create new ones, they're automatically saved later.
             * Second line of if statement is for people who have reinstalled audioband
             * while their last version was pre-profiles (v0.9.6) update */
            if (dto.Profiles == null || !dto.Profiles.Any()
            || (dto.Profiles.Count() == 1 && dto.Profiles.First().Name == "Default Profile"))
            {
                dto.CurrentProfileName = UserProfile.DefaultProfileName;

                _profiles = new Dictionary<string, UserProfile>();
                var profiles = UserProfile.CreateDefaultProfiles();

                for (int i = 0; i < profiles.Length; i++)
                {
                    _profiles.Add(profiles[i].Name, profiles[i]);
                }

                return;
            }

            _profiles = dto.Profiles.ToDictionary(profile => profile.Name, profile => profile);

            if (dto.CurrentProfileName == null || !_profiles.ContainsKey(dto.CurrentProfileName))
            {
                dto.CurrentProfileName = _profiles.First().Key;
            }
        }
    }
}
