using System;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Provides data for a <see cref="IAudioSource.SettingChanged"/> event.
    /// </summary>
    public class SettingChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingChangedEventArgs"/> class
        /// with the setting's name.
        /// </summary>
        /// <param name="settingName">Name of the setting that changed.</param>
        public SettingChangedEventArgs(string settingName)
        {
            SettingName = settingName;
        }

        /// <summary>
        /// Gets or sets the name of the setting's property that changed.
        /// </summary>
        public string SettingName { get; set; }
    }
}
