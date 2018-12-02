using System;
using System.ComponentModel.Composition;
using System.Threading;
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
        /// Called when the audio source is active.
        /// </summary>
        Task ActivateAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Called when the audio source is no longer active.
        /// </summary>
        Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Called when there is a request to start playback.
        /// </summary>
        Task PlayTrackAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Called when there is a request to stop playback.
        /// </summary>
        Task PauseTrackAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Called when there is a request to skip to the previous track.
        /// </summary>
        Task PreviousTrackAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Called when there is a request to skip to the next track.
        /// </summary>
        Task NextTrackAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
