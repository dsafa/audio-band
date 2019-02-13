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
        /// Occurs when the track play state has changed. <see langword="true"/> if playing; <see langword="false"/> otherwise;
        /// </summary>
        event EventHandler<bool> IsPlayingChanged;

        /// <summary>
        /// Occurs when the current track progress has changed.
        /// </summary>
        event EventHandler<TimeSpan> TrackProgressChanged;

        /// <summary>
        /// Occurs when the volume of the audio source changes. The range of the volume is between 0.0 and 1.0 inclusive.
        /// </summary>
        event EventHandler<float> VolumeChanged;

        /// <summary>
        /// Occurs when the shuffle state changes. <see langword="true"/> if shuffle is on; <see langword="false"/> otherwise;
        /// </summary>
        event EventHandler<bool> ShuffleChanged;

        /// <summary>
        /// Occurs when the repeat mode changes.
        /// </summary>
        event EventHandler<RepeatMode> RepeatModeChanged;

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
        Task SetPlaybackProgressAsync(TimeSpan newProgress);

        /// <summary>
        /// Called when there is a request to change the shuffle state.
        /// </summary>
        /// <param name="shuffleOn">The new shuffle state, <see langword="true"/> if shuffle should be on; <see langword="false"/> otherwise.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous set shuffle operation.</returns>
        Task SetShuffleAsync(bool shuffleOn);

        /// <summary>
        /// Called when there is a request to change the repeat mode.
        /// </summary>
        /// <param name="newRepeatMode">The new <see cref="RepeatMode"/>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous set repeat mode operation.</returns>
        Task SetRepeatMode(RepeatMode newRepeatMode);
    }
}
