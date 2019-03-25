using System;
using System.Collections.Generic;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Default custom label service.
    /// </summary>
    public class CustomLabelService : ICustomLabelService
    {
        private List<CustomLabelVM> _labels = new List<CustomLabelVM>();

        /// <inheritdoc/>
        public event EventHandler<CustomLabelVM> CustomLabelAdded;

        /// <inheritdoc/>
        public event EventHandler<CustomLabelVM> CustomLabelRemoved;

        /// <inheritdoc/>
        public IEnumerable<CustomLabelVM> Labels => _labels;

        /// <inheritdoc/>
        public void AddCustomTextLabel(CustomLabelVM label)
        {
            _labels.Add(label);
            CustomLabelAdded?.Invoke(this, label);
        }

        /// <inheritdoc/>
        public void RemoveCustomTextLabel(CustomLabelVM label)
        {
            _labels.Remove(label);
            CustomLabelRemoved?.Invoke(this, label);
        }
    }
}
