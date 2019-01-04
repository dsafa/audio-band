using System;
using System.ServiceModel;
using AudioBand.AudioSource;

namespace ServiceContracts
{
    public interface IAudioSourceHostCallback
    {
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
