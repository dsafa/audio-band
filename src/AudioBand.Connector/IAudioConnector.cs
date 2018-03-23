using System;
using System.ComponentModel.Composition;
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
        /// Album art has changed
        /// </summary>
        event EventHandler<AlbumArtChangedEventArgs> AlbumArtChanged;

        /// <summary>
        /// Track is now playing
        /// </summary>
        event EventHandler TrackPlaying;

        /// <summary>
        /// Track is paused
        /// </summary>
        event EventHandler TrackPaused;

        /// <summary>
        /// Track progress has changed
        /// </summary>
        event EventHandler<int> TrackProgressChanged;

        /// <summary>
        /// User requested to play the track. This should trigger <see cref="TrackPlaying"/> if successful
        /// </summary>
        Task PlayTrackAsync();

        /// <summary>
        /// User requested to pause the track. This should trigger <see cref="TrackPaused"/> if successful
        /// </summary>
        Task PauseTrackAsync();

        /// <summary>
        /// User requested the previous track
        /// </summary>
        Task PreviousTrackAsync();

        /// <summary>
        /// User requested the next track
        /// </summary>
        Task NextTrackAsync();
    }
}
