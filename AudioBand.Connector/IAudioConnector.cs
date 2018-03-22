using System;
using System.ComponentModel.Composition;

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
        /// The current progress of the audio track
        /// </summary>
        IObservable<int> TrackProgress { get; }

        /// <summary>
        /// User changed the state. This should trigger the <see cref="AudioStateChanged"/> event if successful
        /// </summary>
        /// <param name="newAudioState">The new state</param>
        void ChangeState(AudioState newAudioState);

        /// <summary>
        /// User requested the previous track
        /// </summary>
        void PreviousTrack();

        /// <summary>
        /// User requested the next track
        /// </summary>
        void NextTrack();

        /// <summary>
        /// Track information has changed
        /// </summary>
        event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        /// <summary>
        /// Album art has changed
        /// </summary>
        event EventHandler<AlbumArtChangedEventArgs> AlbumArtChanged;

        /// <summary>
        /// Audio state has changed
        /// </summary>
        event EventHandler<AudioStateChangedEventArgs> AudioStateChanged;
    }
}
