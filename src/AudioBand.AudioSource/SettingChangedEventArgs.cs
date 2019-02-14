using System;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Provides data for a <see cref="IAudioSource.SettingChanged"/> event.
    /// </summary>
    [Serializable]
    public sealed class SettingChangedEventArgs : EventArgs
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
        /// Gets or sets the name of the setting that changed.
        /// </summary>
        /// <value>The name of the setting.</value>
        public string SettingName { get; set; }
    }
}
