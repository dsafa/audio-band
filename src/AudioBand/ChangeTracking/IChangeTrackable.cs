using System;
using System.ComponentModel;

namespace AudioBand.ChangeTracking
{
    /// <summary>
    /// Interface that marks an object as being able to participate in the change tracking system.
    /// </summary>
    public interface IChangeTrackable : IEditableObject
    {
        /// <summary>
        /// Occurs when the <see cref="IsEditing"/> property value has changed.
        /// </summary>
        event EventHandler IsEditingChanged;

        /// <summary>
        /// Gets a value indicating whether the current object is being edited.
        /// </summary>
        bool IsEditing { get; }
    }
}
