using System;
using System.ServiceModel;
using ServiceContracts;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Callback class for <see cref="IAudioSourceHostCallback"/>.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class AudioSourceHostCallback : IAudioSourceHostCallback
    {
        /// <summary>
        /// Occurs when <see cref="IAudioSourceHostCallback.SettingChanged(SettingChangedInfo)"/> is called.
        /// </summary>
        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        /// <summary>
        /// Occurs when <see cref="IAudioSourceHostCallback.TrackInfoChanged(TrackInfo)"/> is called.
        /// </summary>
        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        /// <summary>
        /// Occurs when <see cref="IAudioSourceHostCallback.TrackPlaying"/> is called.
        /// </summary>
        public event EventHandler TrackPlaying;

        /// <summary>
        /// Occurs when <see cref="IAudioSourceHostCallback.TrackPaused"/> is called.
        /// </summary>
        public event EventHandler TrackPaused;

        /// <summary>
        /// Occurs when <see cref="IAudioSourceHostCallback.TrackProgressChanged(TimeSpan)"/> is called.
        /// </summary>
        public event EventHandler<TimeSpan> TrackProgressChanged;

        /// <inheritdoc/>
        void IAudioSourceHostCallback.SettingChanged(SettingChangedInfo info)
        {
            SettingChanged?.Invoke(this, (SettingChangedEventArgs)info);
        }

        /// <inheritdoc/>
        void IAudioSourceHostCallback.TrackInfoChanged(TrackInfo info)
        {
            TrackInfoChanged?.Invoke(this, (TrackInfoChangedEventArgs)info);
        }

        /// <inheritdoc/>
        void IAudioSourceHostCallback.TrackPaused()
        {
            TrackPaused?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        void IAudioSourceHostCallback.TrackPlaying()
        {
            TrackPlaying?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        void IAudioSourceHostCallback.TrackProgressChanged(TimeSpan progress)
        {
            TrackProgressChanged?.Invoke(this, progress);
        }
    }
}
