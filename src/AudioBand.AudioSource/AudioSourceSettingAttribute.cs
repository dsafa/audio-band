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
        /// Expose this property as a setting with a given name
        /// </summary>
        /// <param name="name">Name of setting that will be shown</param>
        public AudioSourceSettingAttribute(string name)
        {
            Name = name;
        }

        internal SettingValidationResult Validate(object instance, object value, string propertyName)
        {
            if (ValidatorName == null)
            {
                return new SettingValidationResult(true);
            }

            var method = instance.GetType().GetMethod(ValidatorName);
            return (SettingValidationResult) method.Invoke(instance, new[] {value, propertyName});
        }
    }

    /// <summary>
    /// Class that represents the results of validation
    /// </summary>
    public class SettingValidationResult
    {
        /// <summary>
        /// The value is valid
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Associated error message if the value was not valid
        /// </summary>
        public string ErrorMessage { get; }

        public SettingValidationResult(bool isValid, string errorMessage = null)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }
    }
}
