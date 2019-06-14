using System;
using System.Collections.Generic;
using System.Linq;

namespace AudioBand.ChangeTracking
{
    /// <summary>
    /// Basic change tracker.
    /// </summary>
    public class ChangeTracker : IChangeTracker
    {
        private readonly HashSet<IChangeTrackable> _trackedObjects = new HashSet<IChangeTrackable>();
        private readonly HashSet<IChangeTrackable> _objectsThatAreEditing = new HashSet<IChangeTrackable>();

        /// <inheritdoc />
        public event EventHandler TrackingStateChanged;

        /// <inheritdoc />
        public bool IsAnyEditing => _objectsThatAreEditing.Any();

        /// <inheritdoc />
        public void Add(IChangeTrackable toTrack)
        {
            _trackedObjects.Add(toTrack);
            toTrack.IsEditingChanged += ChangeTrackableOnIsEditingChanged;
        }

        /// <inheritdoc />
        public void Remove(IChangeTrackable toTrack)
        {
            _trackedObjects.Remove(toTrack);
            toTrack.IsEditingChanged -= ChangeTrackableOnIsEditingChanged;
        }

        /// <inheritdoc />
        public void BeginEditAll()
        {
            foreach (var changeTrackable in _trackedObjects)
            {
                changeTrackable.BeginEdit();
            }
        }

        /// <inheritdoc />
        public void CancelEditAll()
        {
            foreach (var changeTrackable in _trackedObjects)
            {
                changeTrackable.CancelEdit();
            }
        }

        /// <inheritdoc />
        public void EndEditAll()
        {
            foreach (var changeTrackable in _trackedObjects)
            {
                changeTrackable.EndEdit();
            }
        }

        private void ChangeTrackableOnIsEditingChanged(object sender, EventArgs e)
        {
            var trackable = (IChangeTrackable)sender;
            if (trackable.IsEditing)
            {
                _objectsThatAreEditing.Add(trackable);
                if (_objectsThatAreEditing.Count == 1)
                {
                    TrackingStateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                _objectsThatAreEditing.Remove(trackable);
                if (_objectsThatAreEditing.Count == 0)
                {
                    TrackingStateChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
