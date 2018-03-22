using System;

namespace AudioBand.Connector
{
    public interface IAudioConnector
    {
        /// <summary>
        /// User changed the state
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
        /// The current progress of the audio track
        /// </summary>
        IObservable<int> TrackProgress { get; }

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
