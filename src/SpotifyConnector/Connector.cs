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

        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;
        public event EventHandler<AlbumArtChangedEventArgs> AlbumArtChanged;
        public event EventHandler TrackPlaying;
        public event EventHandler TrackPaused;
        public event EventHandler<int> TrackProgressChanged;

        public Task PlayTrackAsync()
        {
            TrackPlaying?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public Task PauseTrackAsync()
        {
            TrackPaused?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public Task PreviousTrackAsync()
        {
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs
            {
                TrackName = "previous track"
            });

            return Task.CompletedTask;
        }

        public Task NextTrackAsync()
        {
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs
            {
                TrackName = "next track"
            });

            return Task.CompletedTask;
        }
    }
}
