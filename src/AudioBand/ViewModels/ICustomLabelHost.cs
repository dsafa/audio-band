namespace AudioBand.ViewModels
{
    /// <summary>
    /// Represents a host for custom labels.
    /// </summary>
    internal interface ICustomLabelHost
    {
        /// <summary>
        /// Adds a new custom label.
        /// </summary>
        /// <param name="label">The new label to add.</param>
        void AddCustomTextLabel(CustomLabelVM label);

        /// <summary>
        /// Removes an existing label.
        /// </summary>
        /// <param name="label">The label to remove.</param>
        void RemoveCustomTextLabel(CustomLabelVM label);
    }
}
