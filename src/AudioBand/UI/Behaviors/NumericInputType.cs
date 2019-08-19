namespace AudioBand.UI
{
    /// <summary>
    /// The type of the numericupdown.
    /// </summary>
    public enum NumericInputType
    {
        /// <summary>
        /// Greater than 0.
        /// </summary>
        Size,

        /// <summary>
        /// Any integer value
        /// </summary>
        Position,

        /// <summary>
        /// Floating point greater than 0.
        /// </summary>
        FontSize,

        /// <summary>
        /// Integer greater than 0.
        /// </summary>
        Positive,

        /// <summary>
        /// Any integer.
        /// </summary>
        Integer,

        /// <summary>
        /// Integer 0 or greater.
        /// </summary>
        WholeNumber,
    }
}
