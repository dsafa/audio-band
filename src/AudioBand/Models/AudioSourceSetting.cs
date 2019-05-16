using AudioBand.AudioSource;

namespace AudioBand.Models
{
    /// <summary>
    /// A key / value pair that represents a single setting for an audio source.
    /// </summary>
    public class AudioSourceSetting : ModelBase
    {
        private string _name;
        private object _value;

        /// <summary>
        /// Gets or sets the name of the setting provided by the <see cref="IAudioSource"/>.
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <summary>
        /// Gets or sets the value of the setting.
        /// </summary>
        /// <value>
        /// The value of the setting, either provided by the <see cref="IAudioSource"/> or the user.
        /// The default value is set by the <see cref="IAudioSource"/>.
        /// </value>
        public object Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}
