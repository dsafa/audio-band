using System;
using System.Runtime.Serialization;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Provides data for a <see cref="IAudioSource.SettingChanged"/> event.
    /// </summary>
    [DataContract]
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

        // For serialization
        private SettingChangedEventArgs()
        {
        }

        /// <summary>
        /// Gets or sets the name of the setting's property that changed.
        /// </summary>
        [DataMember]
        public string PropertyName { get; set; }
    }
}
