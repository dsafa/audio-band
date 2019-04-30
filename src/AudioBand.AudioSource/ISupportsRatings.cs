using System;
using System.Threading.Tasks;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Indicates that this audio source supports ratings on tracks.
    /// </summary>
    public interface ISupportsRatings
    {
        /// <summary>
        /// Occurs when the the track's rating has changed.
        /// </summary>
        event EventHandler<TrackRating> TrackRatingChanged;

        /// <summary>
        /// Called when a new rating is being set.
        /// </summary>
        /// <param name="newRating">The new rating of the track.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous set rating operation.</returns>
        Task SetTrackRatingAsync(TrackRating newRating);
    }
}
