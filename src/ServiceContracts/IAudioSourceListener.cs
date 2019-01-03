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
        [OperationContract]
        void SettingChanged(SettingChangedEventArgs args);

        [OperationContract]
        void TrackInfoChanged(TrackInfoChangedEventArgs args);

        [OperationContract]
        void TrackPlaying();

        [OperationContract]
        void TrackPaused();

        [OperationContract]
        void TrackProgressChanged(TimeSpan progress);
    }
}
