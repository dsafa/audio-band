using System;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Default custom label service.
    /// </summary>
    public class CustomLabelService : ICustomLabelService
    {
        /// <inheritdoc/>
        public event EventHandler<CustomLabelVM> CustomLabelAdded; // Maybe use pub/sub instead

        /// <inheritdoc/>
        public event EventHandler<CustomLabelVM> CustomLabelRemoved;

        /// <inheritdoc/>
        public void AddCustomTextLabel(CustomLabelVM label)
        {
            CustomLabelAdded?.Invoke(this, label);
        }

        /// <inheritdoc/>
        public void RemoveCustomTextLabel(CustomLabelVM label)
        {
            CustomLabelRemoved?.Invoke(this, label);
        }
    }
}
