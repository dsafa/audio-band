namespace AudioBand.AudioSource
{
    /// <summary>
    /// Class that represents the results of validation
    /// </summary>
    internal class SettingValidationResult
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

        public static SettingValidationResult Fail => new SettingValidationResult(false);
        public static SettingValidationResult Success => new SettingValidationResult(true);
    }
}
