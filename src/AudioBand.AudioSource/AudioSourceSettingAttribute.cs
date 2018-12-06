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
        /// Initializes a new instance of the <see cref="AudioSourceSettingAttribute"/> class
        /// with the setting name.
        /// </summary>
        /// <param name="name">Name of the setting.</param>
        public AudioSourceSettingAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the validator function.
        /// <para/>
        /// The validation function should have the signature (<see langword="object"/> valueToValidate, <see langword="string"/> nameOfPropertyBeingSet) -> <see cref="SettingValidationResult"/>.
        /// </summary>
        public string ValidatorName { get; set; }

        /// <summary>
        /// Gets or sets the name of the setting.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SettingOptions"/> flags for the setting.
        /// </summary>
        public SettingOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the priority when changing more that one setting.
        /// <para/>
        /// A higher value is higher priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the description of the setting.
        /// </summary>
        public string Description { get; set; }
    }
}
