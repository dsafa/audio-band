using AudioBand.Models;
using AudioBand.Settings.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AudioBand.Settings
{
    /// <summary>
    /// Manages application settings.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        private readonly IPersistentSettings _persistSettings;
        private Dictionary<string, UserProfile> _profiles = new Dictionary<string, UserProfile>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
        /// <param name="persistSettings">The settings persistence object.</param>
        public AppSettings(IPersistentSettings persistSettings)
        {
            _persistSettings = persistSettings;
            _persistSettings.CheckAndConvertOldSettings();
            var settings = _persistSettings.ReadSettings();

            AudioSource = settings.CurrentAudioSource;
            AudioSourceSettings = settings.AudioSourceSettings?.ToList() ?? new List<AudioSourceSettings>();
            AudioBandSettings = settings.AudioBandSettings ?? new AudioBandSettings();

            CheckAndLoadProfiles(settings, _persistSettings.ReadProfiles().ToArray());
            var profileName = AudioBandSettings.UseAutomaticIdleProfile && !string.IsNullOrEmpty(AudioBandSettings.LastNonIdleProfileName)
                            ? AudioBandSettings.LastNonIdleProfileName : settings.CurrentProfileName;
            SelectProfile(profileName);
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
            Debug.Assert(_profiles.ContainsKey(profileName), $"Selecting non existent profile {profileName}.");

            if (!string.IsNullOrEmpty(CurrentProfile?.Name))
            {
                AudioBandSettings.LastNonIdleProfileName = CurrentProfile.Name;
            }

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
            if (string.IsNullOrEmpty(profileName))
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
            _persistSettings.DeleteProfile(profileName);
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

            _persistSettings.DeleteProfile(CurrentProfile.Name);
            CurrentProfile.Name = newProfileName;
            Save();
        }

        /// <inheritdoc />
        public void Save()
        {
            _persistSettings.WriteSettings(new Persistence.Settings()
            {
                CurrentAudioSource = AudioSource,
                AudioBandSettings = AudioBandSettings,
                CurrentProfileName = CurrentProfile.Name,
                AudioSourceSettings = AudioSourceSettings
            });

            _persistSettings.WriteProfiles(Profiles);
        }

        /// <inheritdoc />
        public void ImportProfileFromPath(string path)
        {
            var profile = _persistSettings.ReadProfile(path);
            if (profile is null)
            {
                return;
            }

            var name = UserProfile.GetUniqueProfileName(_profiles.Keys, profile.Name);
            _profiles[name] = profile;
            _profiles[name].Name = name;
        }

        private void CheckAndLoadProfiles(Persistence.Settings settings, UserProfile[] profiles)
        {
            /* If there are no profiles, create new ones, they're automatically saved later.
             * Second line of if statement is for people who have reinstalled audioband
             * while their last version was pre-profiles (v0.9.6) update */
            if (profiles.Length == 0
            || (profiles.Length == 1 && profiles[0].Name == "Default Profile"))
            {
                settings.CurrentProfileName = UserProfile.DefaultProfileName;

                _profiles = new Dictionary<string, UserProfile>();
                var defaultProfiles = UserProfile.CreateDefaultProfiles();
                var idleProfile = UserProfile.CreateDefaultIdleProfile();

                _profiles.Add(idleProfile.Name, idleProfile);
                AudioBandSettings.IdleProfileName = UserProfile.DefaultIdleProfileName;

                for (int i = 0; i < defaultProfiles.Length; i++)
                {
                    _profiles.Add(defaultProfiles[i].Name, defaultProfiles[i]);
                }

                return;
            }
            else if (profiles.FirstOrDefault(x => x.Name == AudioBandSettings.IdleProfileName) == null)
            {
                // if the profile doesn't exist, disable idle profile
                AudioBandSettings.IdleProfileName = profiles.First().Name;
                AudioBandSettings.UseAutomaticIdleProfile = false;
            }

            _profiles = profiles.ToDictionary(profile => profile.Name, profile => profile);

            if (settings.CurrentProfileName == null || !_profiles.ContainsKey(settings.CurrentProfileName))
            {
                settings.CurrentProfileName = _profiles.First().Key;
            }
        }
    }
}
