using System;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Represents a service to add and remove new custom labels
    /// </summary>
    public interface ICustomLabelService
    {
        /// <summary>
        /// Occurs when a custom label is added.
        /// </summary>
        event EventHandler<CustomLabelVM> CustomLabelAdded;

        /// <summary>
        /// Occurs when a custom label is removed.
        /// </summary>
        event EventHandler<CustomLabelVM> CustomLabelRemoved;

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
