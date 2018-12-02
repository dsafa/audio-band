using System;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Provides data for a <see cref="IAudioSource.SettingChanged"/> event.
    /// </summary>
    public class SettingChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Name of the setting's property that changed.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="SettingChangedEventArgs"/> with the setting name.
        /// </summary>
        /// <param name="propertyName">Name of the setting's property that changed.</param>
        public SettingChangedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
