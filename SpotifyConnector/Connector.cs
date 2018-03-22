using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioBand.Connector;

namespace SpotifyConnector
{
    public class Connector : IAudioConnector
    {
        public string ConnectorName { get; } = "Spotify";
        public IObservable<int> TrackProgress { get; } = new SpotifyTrackProgress();

        public void ChangeState(AudioState newAudioState)
        {
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs
            {
                TrackName = newAudioState.ToString()
            });   
        }

        public void PreviousTrack()
        {
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs
            {
                TrackName = "previous track"
            });
        }

        public void NextTrack()
        {
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs
            {
                TrackName = "next track"
            });
        }

        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;
        public event EventHandler<AlbumArtChangedEventArgs> AlbumArtChanged;
        public event EventHandler<AudioStateChangedEventArgs> AudioStateChanged;
    }
}
