using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace AudioBand.Connector
{
    [InheritedExport(typeof(IAudioConnector))]
    public interface IAudioConnector
    {
        /// <summary>
        /// Name of the connector which is displayed
        /// </summary>
        string ConnectorName { get; }

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
        /// Connector is selected as the audio source
        /// </summary>
        Task ActivateAsync(ILogger logger, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Connector is no longer the audio source
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
