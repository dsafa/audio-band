﻿using System;
using System.ServiceModel;
using AudioBand.AudioSource;

namespace ServiceContracts
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IAudioSourceServer
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

        [OperationContract]
        void RegisterAudioSource(string name);
    }
}
