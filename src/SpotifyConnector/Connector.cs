using AudioBand.Connector;
using SpotifyAPI.Local;
using System;
using System.Threading;
using System.Threading.Tasks;

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

        private SpotifyLocalAPI _spotifyClient;

        public Task ActivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!(SpotifyLocalAPI.IsSpotifyRunning() && SpotifyLocalAPI.IsSpotifyWebHelperRunning() && _spotifyClient.Connect()))
            {
                Console.WriteLine("Cannot connect to spotify. " +
                                  $"Running? {SpotifyLocalAPI.IsSpotifyRunning()} | " +
                                  $"Web Helper running?  {SpotifyLocalAPI.IsSpotifyWebHelperRunning()}");
                TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs { TrackName = "Spotify not available" });
                return Task.CompletedTask;
            }

            _spotifyClient = new SpotifyLocalAPI();
            return Task.CompletedTask;
        }

        public Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _spotifyClient.Dispose();
            return Task.CompletedTask;
        }

        public Task PlayTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            TrackPlaying?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public Task PauseTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            TrackPaused?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public Task PreviousTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs
            {
                TrackName = "previous track"
            });

            return Task.CompletedTask;
        }

        public Task NextTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs
            {
                TrackName = "next track"
            });

            return Task.CompletedTask;
        }
    }
}
