﻿using System;
using System.ServiceModel;
using AudioBand.ServiceContracts;

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

        /// <summary>
        /// Occurs when <see cref="IAudioSourceHostCallback.VolumeChanged(float)"/> is called;
        /// </summary>
        public event EventHandler<float> VolumeChanged;

        /// <summary>
        /// Occurs when <see cref="IAudioSourceHostCallback.TrackRatingChanged(TrackRating)"/> is called.
        /// </summary>
        public event EventHandler<TrackRating> TrackRatingChanged;

        /// <inheritdoc/>
        void IAudioSourceHostCallback.VolumeChanged(float volume)
        {
            VolumeChanged?.Invoke(this, volume);
        }

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

        /// <inheritdoc/>
        void IAudioSourceHostCallback.TrackRatingChanged(TrackRating rating)
        {
            TrackRatingChanged?.Invoke(this, rating);
        }
    }
}
