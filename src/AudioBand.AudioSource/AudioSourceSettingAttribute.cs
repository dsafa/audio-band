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
        /// Name that will be seen by user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets whether or not the setting is visible to the user.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Expose this property as a setting with a given name
        /// </summary>
        /// <param name="name">Name of setting that will be shown</param>
        public AudioSourceSettingAttribute(string name)
        {
            Name = name;
        }
    }
}
