using System;
using System.ServiceModel;
using ServiceContracts;

namespace AudioBand.AudioSource
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    internal class AudioSourceHostCallback : IAudioSourceHostCallback
    {
        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        public event EventHandler TrackPlaying;

        public event EventHandler TrackPaused;

        public event EventHandler<TimeSpan> TrackProgressChanged;

        void IAudioSourceHostCallback.SettingChanged(SettingChangedInfo info)
        {
            SettingChanged?.Invoke(this, (SettingChangedEventArgs)info);
        }

        void IAudioSourceHostCallback.TrackInfoChanged(TrackInfo info)
        {
            TrackInfoChanged?.Invoke(this, (TrackInfoChangedEventArgs)info);
        }

        void IAudioSourceHostCallback.TrackPaused()
        {
            TrackPaused?.Invoke(this, EventArgs.Empty);
        }

        void IAudioSourceHostCallback.TrackPlaying()
        {
            TrackPlaying?.Invoke(this, EventArgs.Empty);
        }

        void IAudioSourceHostCallback.TrackProgressChanged(TimeSpan progress)
        {
            TrackProgressChanged?.Invoke(this, progress);
        }
    }
}
