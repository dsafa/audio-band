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

        /// <inheritdoc />
        public event EventHandler ProfileChanged;

        /// <inheritdoc />
        public string AudioSource { get; set; }

        /// <inheritdoc />
        public List<AudioSourceSettings> AudioSourceSettings { get; }

        /// <inheritdoc />
        public AudioBandSettings AudioBandSettings { get; }

        /// <inheritdoc />
        public UserProfile CurrentProfile { get; private set; }

        /// <inheritdoc />
        public IEnumerable<UserProfile> Profiles => _profiles.Values;

        /// <inheritdoc />
        public void SelectProfile(string profileName)
        {
            Debug.Assert(_profiles.ContainsKey(profileName), $"Selecting non existent profile {profileName}");

            CurrentProfile = _profiles[profileName];
            ProfileChanged?.Invoke(this, EventArgs.Empty);
            Save();
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        private void CheckAndLoadProfiles(PersistedSettingsDto settings)
        {
            /* If there are no profiles, create new ones, they're automatically saved later.
             * Second line of if statement is for people who have reinstalled audioband
             * while their last version was pre-profiles (v0.9.6) update */
            if (settings.Profiles == null || !settings.Profiles.Any()
            || (settings.Profiles.Count() == 1 && settings.Profiles.First().Name == "Default Profile"))
            {
                settings.CurrentProfileName = UserProfile.DefaultProfileName;

                _profiles = new Dictionary<string, UserProfile>();
                var profiles = UserProfile.CreateDefaultProfiles();

                for (int i = 0; i < profiles.Length; i++)
                {
                    _profiles.Add(profiles[i].Name, profiles[i]);
                }

                return;
            }
            else if (settings.Profiles.FirstOrDefault(x => x.Name == UserProfile.IdleProfileName) == null)
            {
                // Add idle profile as the first one, so move all the others down one.
                var existingProfiles = settings.Profiles.ToArray();
                var newProfiles = new UserProfile[existingProfiles.Count() + 1];

                newProfiles[0] = UserProfile.CreateIdleProfile();

                for (int i = 0; i < existingProfiles.Count(); i++)
                {
                    newProfiles[i + 1] = existingProfiles[i];
                }

                settings.Profiles = newProfiles;
            }

            _profiles = settings.Profiles.ToDictionary(profile => profile.Name, profile => profile);

            if (settings.CurrentProfileName == null || !_profiles.ContainsKey(settings.CurrentProfileName))
            {
                settings.CurrentProfileName = _profiles.First().Key;
            }
        }
    }
}
