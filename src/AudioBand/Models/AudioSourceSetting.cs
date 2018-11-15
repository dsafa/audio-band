using System;
using System.ComponentModel;
using AudioBand.AudioSource;

namespace AudioBand.Models
{
    /// <summary>
    /// A key / value pair that represents a single setting for an audio source
    /// </summary>
    internal class AudioSourceSetting : ModelBase
    {
        private string _name;
        private object _value;

        /// <summary>
        /// Name of the setting
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Value of the setting serialized as a string
        /// </summary>
        public object Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}