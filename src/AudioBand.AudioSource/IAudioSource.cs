using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Provides information from an audio source and exposes controls
    /// </summary>
    [InheritedExport(typeof(IAudioSource))]
    public interface IAudioSource
    {
        /// <summary>
        /// Name of the audio source which is displayed
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Track information has changed
        /// </summary>
        event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        /// <summary>
        /// Track is now playing
        /// </summary>
        event EventHandler TrackPlaying;

        /// <summary>
        /// Track is paused
        /// </summary>
        event EventHandler TrackPaused;

        /// <summary>
        /// Track progress has changed. Track progress is from [0 - 100]
        /// </summary>
        event EventHandler<double> TrackProgressChanged;

        /// <summary>
        /// This audio source has been selected
        /// </summary>
        Task ActivateAsync(IAudioSourceContext context, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// The audio source is no longer active
        /// </summary>
        Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// User requested to play the track. This should trigger <see cref="TrackPlaying"/> if successful
        /// </summary>
        Task PlayTrackAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// User requested to pause the track. This should trigger <see cref="TrackPaused"/> if successful
        /// </summary>
        Task PauseTrackAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// User requested the previous track
        /// </summary>
        Task PreviousTrackAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// User requested the next track
        /// </summary>
        Task NextTrackAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
