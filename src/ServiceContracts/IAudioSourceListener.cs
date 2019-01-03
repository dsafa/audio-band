using System;
using System.ServiceModel;
using AudioBand.AudioSource;

namespace ServiceContracts
{
    /// <summary>
    /// This is a callback for the <see cref="IAudioSourceHost"/>.
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IAudioSourceHost))]
    public interface IAudioSourceListener
    {
        [OperationContract]
        void BeginSession();

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
