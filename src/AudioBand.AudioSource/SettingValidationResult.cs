namespace AudioBand.AudioSource
{
    /// <summary>
    /// Represents the results of validation.
    /// </summary>
    internal class SettingValidationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingValidationResult"/> class
        /// with a value indicating the validity and an optional error message.
        /// </summary>
        /// <param name="isValid">A value indicating whether the result is valid.</param>
        /// <param name="errorMessage">An optional error message.</param>
        public SettingValidationResult(bool isValid, string errorMessage = null)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets a <see cref="SettingValidationResult"/> representing a failure with no error message.
        /// </summary>
        public static SettingValidationResult Fail => new SettingValidationResult(false);

        /// <summary>
        /// Gets a <see cref="SettingValidationResult"/> representing a success.
        /// </summary>
        public static SettingValidationResult Success => new SettingValidationResult(true);

        /// <summary>
        /// Gets a value indicating whether the setting is valid.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Gets the associated error message if the value was not valid.
        /// </summary>
        public string ErrorMessage { get; }
    }
}
