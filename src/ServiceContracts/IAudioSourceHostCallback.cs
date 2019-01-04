using System;
using System.ServiceModel;
using AudioBand.AudioSource;

namespace ServiceContracts
{
    /// <summary>
    /// Call back for <see cref="IAudioSourceHost"/>.
    /// </summary>
    public interface IAudioSourceHostCallback
    {
        /// <summary>
        /// Contract method for <see cref="IAudioSource.SettingChanged"/>.
        /// </summary>
        /// <param name="args"></param>
        [OperationContract(IsOneWay = true)]
        void SettingChanged(SettingChangedEventArgs args);

        /// <summary>
        /// Contract method for <see cref="IAudioSource.TrackInfoChanged"/>.
        /// </summary>
        /// <param name="args"></param>
        [OperationContract(IsOneWay = true)]
        void TrackInfoChanged(TrackInfoChangedEventArgs args);

        /// <summary>
        /// Contract method for <see cref="IAudioSource.TrackPlaying"/>.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void TrackPlaying();

        /// <summary>
        /// Contract method for <see cref="IAudioSource.TrackPaused"/>.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void TrackPaused();

        /// <summary>
        /// Contract method for <see cref="IAudioSource.TrackProgressChanged"/>.
        /// </summary>
        /// <param name="progress"></param>
        [OperationContract(IsOneWay = true)]
        void TrackProgressChanged(TimeSpan progress);
    }
}
