using System;
using System.ServiceModel;
using AudioBand.AudioSource;

namespace ServiceContracts
{
    /// <summary>
    /// Service contract for the audio source listner which is the server.
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IAudioSourceHost))]
    public interface IAudioSourceListener
    {
        [OperationContract(IsOneWay = true)]
        void OpenSession();

        [OperationContract(IsOneWay = true)]
        void SettingChanged(SettingChangedEventArgs args);

        [OperationContract(IsOneWay = true)]
        void TrackInfoChanged(TrackInfoChangedEventArgs args);

        [OperationContract(IsOneWay = true)]
        void TrackPlaying();

        [OperationContract(IsOneWay = true)]
        void TrackPaused();

        [OperationContract(IsOneWay = true)]
        void TrackProgressChanged(TimeSpan progress);
    }
}
