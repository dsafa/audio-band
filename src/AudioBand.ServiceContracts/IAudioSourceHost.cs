using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using AudioBand.AudioSource;

namespace AudioBand.ServiceContracts
{
    /// <summary>
    /// Contract for the service that is hosting the audio sources.
    /// </summary>
    [ServiceContract(Namespace = "Audioband", CallbackContract = typeof(IAudioSourceHostCallback))]
    public interface IAudioSourceHost
    {
        /// <summary>
        /// Establishes the callback channel by saving the operation context from this call.
        /// </summary>
        [OperationContract]
        void OpenCallbackChannel();

        /// <summary>
        /// Contract method for <see cref="IAudioSource.Name"/>.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string GetName();

        /// <summary>
        /// Contract method for <see cref="IAudioSource.ActivateAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Task ActivateAsync();

        /// <summary>
        /// Contract method for <see cref="IAudioSource.DeactivateAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Task DeactivateAsync();

        /// <summary>
        /// Contract method for <see cref="IAudioSource.PlayTrackAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Task PlayTrackAsync();

        /// <summary>
        /// Contract method for <see cref="IAudioSource.PauseTrackAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Task PauseTrackAsync();

        /// <summary>
        /// Contract method for <see cref="IAudioSource.PreviousTrackAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Task PreviousTrackAsync();

        /// <summary>
        /// Contract method for <see cref="IAudioSource.NextTrackAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Task NextTrackAsync();

        /// <summary>
        /// Gets the settings that the audio source has.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<AudioSourceSettingInfo> GetAudioSourceSettings();

        /// <summary>
        /// Contract method for <see cref="IAudioSource.SetVolumeAsync(float)"/>.
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        [OperationContract]
        Task SetVolume(float volume);

        /// <summary>
        /// Contract method for <see cref="IAudioSource.SetPlaybackProgress(TimeSpan)"/>.
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        [OperationContract]
        Task SetPlaybackProgress(TimeSpan progress);

        /// <summary>
        /// Update the audio source setting with a new value.
        /// </summary>
        /// <param name="settingName">Setting name.</param>
        /// <param name="value">New value.</param>
        /// <returns></returns>
        [OperationContract(IsOneWay = true)]
        void UpdateSetting(string settingName, object value);

        /// <summary>
        /// Gets the value of a setting.
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        [OperationContract]
        object GetSettingValue(string settingName);
    }
}
