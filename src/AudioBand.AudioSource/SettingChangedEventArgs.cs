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
        /// with the setting's property name.
        /// </summary>
        /// <param name="propertyName">Name of the setting's property that changed.</param>
        public SettingChangedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Gets or sets the name of the setting's property that changed.
        /// </summary>
        public string PropertyName { get; set; }
    }
}
