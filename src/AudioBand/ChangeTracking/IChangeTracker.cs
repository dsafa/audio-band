using System;
using System.ComponentModel;

namespace AudioBand.ChangeTracking
{
    /// <summary>
    /// An object that maintains change tracking for <see cref="IChangeTrackable"/> objects.
    /// </summary>
    public interface IChangeTracker
    {
        /// <summary>
        /// Occurs when the change tracking state is changed.
        /// </summary>
        event EventHandler TrackingStateChanged;

        /// <summary>
        /// Gets a value indicating whether any associated <see cref="IChangeTrackable"/> object has <see cref="IChangeTrackable.IsEditing"/> set to true.
        /// </summary>
        bool IsAnyEditing { get; }

        /// <summary>
        /// Adds a <see cref="IChangeTrackable"/> object to the tracking scope.
        /// </summary>
        /// <param name="toTrack">The object add to tracking.</param>
        void Add(IChangeTrackable toTrack);

        /// <summary>
        /// Removes a <see cref="IChangeTrackable"/> object from the tracking scope.
        /// </summary>
        /// <param name="toTrack">The object to remove from tracking.</param>
        void Remove(IChangeTrackable toTrack);

        /// <summary>
        /// Calls <see cref="IEditableObject.BeginEdit"/> for all tracked objects.
        /// </summary>
        void BeginEditAll();

        /// <summary>
        /// Calls <see cref="IEditableObject.CancelEdit"/> for all tracked objects.
        /// </summary>
        void CancelEditAll();

        /// <summary>
        /// Calls <see cref="IEditableObject.EndEdit"/> for all tracked objects.
        /// </summary>
        void EndEditAll();
    }
}
