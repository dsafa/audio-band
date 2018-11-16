using System;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Represents a setting that is exposed to the user to modify
    /// </summary>
    /// <inheritdoc cref="Attribute"/>
    [AttributeUsage(AttributeTargets.Property)]
    public class AudioSourceSettingAttribute : Attribute
    {
        /// <summary>
        /// Name of the validator function
        /// <para/>
        /// The validation function should have the signature (object valueToValidate, string nameOfPropertyBeingSet) -> <see cref="SettingValidationResult"/>
        /// </summary>
        public string ValidatorName { get; set; }

        /// <summary>
        /// Name of the setting.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the <see cref="SettingOptions"/> for the setting.
        /// </summary>
        public SettingOptions Options { get; set; }

        /// <summary>
        /// Expose this property as a setting with a given name
        /// </summary>
        /// <param name="name">Name of the setting.</param>
        public AudioSourceSettingAttribute(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Flags for audio source settings.
    /// </summary>
    [Flags]
    public enum SettingOptions
    {
        /// <summary>
        /// Setting is invisible to the user.
        /// </summary>
        Hidden = 1 << 0,
        
        /// <summary>
        /// Setting cannot be modified by the user.
        /// </summary>
        ReadOnly = 1 << 1,

        /// <summary>
        /// Indicates a sensitive setting such as a password, causing a warning to be given.
        /// </summary>
        Sensitive = 1 << 2,
    }
}
