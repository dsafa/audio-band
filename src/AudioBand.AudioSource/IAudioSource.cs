using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Provides information and notifies changes from an audio source and exposes playback controls.
    /// </summary>
    [InheritedExport(typeof(IAudioSource))]
    public interface IAudioSource
    {
        /// <summary>
        /// Occurs when a setting has changed internally.
        /// </summary>
        event EventHandler<SettingChangedEventArgs> SettingChanged;

        /// <summary>
        /// Occurs when track information has changed.
        /// </summary>
        event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        /// <summary>
        /// Occurs when the playback state has changed to playing.
        /// </summary>
        event EventHandler TrackPlaying;

        /// <summary>
        /// Occurs when the playback state has changed to paused.
        /// </summary>
        event EventHandler TrackPaused;

        /// <summary>
        /// Occurs when the current track progress has changed.
        /// </summary>
        event EventHandler<TimeSpan> TrackProgressChanged;

        /// <summary>
        /// Occurs when the volume of the audio source changes. The range of the new volume is between 0.0 and 1.0 inclusive.
        /// </summary>
        event EventHandler<float> VolumeChanged;

        /// <summary>
        /// Gets the name of the audio source.
        /// </summary>
        /// <value>The name of the audio source.</value>
        string Name { get; }

        /// <summary>
        /// Gets or sets the <see cref="IAudioSourceLogger"/> used for logging.
        /// </summary>
        /// <value>Audio source logger that will be injected.</value>
        IAudioSourceLogger Logger { get; set; }

        /// <summary>
        /// Called when the audio source becomes active.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous activate operation.</returns>
        Task ActivateAsync();

        /// <summary>
        /// Called when the audio source is no longer active.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous deactivate operation.</returns>
        Task DeactivateAsync();

        /// <summary>
        /// Called when there is a request to start playback.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous play operation.</returns>
        Task PlayTrackAsync();

        /// <summary>
        /// Called when there is a request to stop playback.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous pause operation.</returns>
        Task PauseTrackAsync();

        /// <summary>
        /// Called when there is a request to skip to the previous track.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous skip to previous track operation.</returns>
        Task PreviousTrackAsync();

        /// <summary>
        /// Called when there is a request to skip to the next track.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous skip to next track operation.</returns>
        Task NextTrackAsync();

        /// <summary>
        /// Called when there is a request to change the volume.
        /// </summary>
        /// <param name="newVolume">The new volume to set. The range is between 0.0 and 1.0 inclusive.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous set volume operation.</returns>
        Task SetVolumeAsync(float newVolume);

        /// <summary>
        /// Called when there is a request to change to current playback progress.
        /// </summary>
        /// <param name="newProgress">The new time to seek to.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous set playback progress operation.</returns>
        Task SetPlaybackProgress(TimeSpan newProgress);
    }
}
