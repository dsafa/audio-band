using System;
using System.ServiceModel;
using AudioBand.AudioSource;

namespace AudioBand.ServiceContracts
{
    /// <summary>
    /// Call back for <see cref="IAudioSourceHost"/>.
    /// </summary>
    public interface IAudioSourceHostCallback
    {
        /// <summary>
        /// Contract method for <see cref="IAudioSource.SettingChanged"/>.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void SettingChanged(SettingChangedInfo info);

        /// <summary>
        /// Contract method for <see cref="IAudioSource.TrackInfoChanged"/>.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void TrackInfoChanged(TrackInfo info);

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
        [OperationContract(IsOneWay = true)]
        void TrackProgressChanged(TimeSpan progress);

        /// <summary>
        /// Contact method for <see cref="IAudioSource.VolumeChanged"/>.
        /// </summary>
        /// <param name="volume">The new volume.</param>
        [OperationContract(IsOneWay = true)]
        void VolumeChanged(float volume);

        /// <summary>
        /// Contract method for <see cref="ISupportsRatings.TrackRatingChanged"/>.
        /// </summary>
        /// <param name="rating"></param>
        [OperationContract(IsOneWay = true)]
        void TrackRatingChanged(TrackRating rating);
    }
}
