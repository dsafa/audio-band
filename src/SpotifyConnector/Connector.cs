using AudioBand.Connector;
using SpotifyAPI.Local;
using System;
using System.Threading;
using System.Threading.Tasks;
using SpotifyAPI.Local.Enums;

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
            _spotifyClient = new SpotifyLocalAPI();

            if (!(SpotifyLocalAPI.IsSpotifyRunning() && SpotifyLocalAPI.IsSpotifyWebHelperRunning() && _spotifyClient.Connect()))
            {
                Console.WriteLine("Cannot connect to spotify. " +
                                  $"Running? {SpotifyLocalAPI.IsSpotifyRunning()} | " +
                                  $"Web Helper running?  {SpotifyLocalAPI.IsSpotifyWebHelperRunning()}");
                TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs { TrackName = "Spotify not available" });
                return Task.CompletedTask;
            }

            _spotifyClient.ListenForEvents = true;
            var status = _spotifyClient.GetStatus();

            var track = status.Track;
            AlbumArtChanged?.Invoke(this, new AlbumArtChangedEventArgs { AlbumArt = track.GetAlbumArt(AlbumArtSize.Size160) });
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs
            {
                TrackName = track.TrackResource.Name,
                Artist = track.ArtistResource.Name
            });

            var playing = status.Playing;
            if (playing)
            {
                TrackPlaying?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                TrackPaused?.Invoke(this, EventArgs.Empty);
            }

            TrackProgressChanged?.Invoke(this, (int)status.PlayingPosition);

            _spotifyClient.OnPlayStateChange += SpotifyClientOnOnPlayStateChange;
            _spotifyClient.OnTrackChange += SpotifyClientOnOnTrackChange;
            _spotifyClient.OnTrackTimeChange += SpotifyClientOnOnTrackTimeChange;

            return Task.CompletedTask;
        }

        private void SpotifyClientOnOnTrackTimeChange(object sender, TrackTimeChangeEventArgs trackTimeChangeEventArgs)
        {
            TrackProgressChanged?.Invoke(this, (int)trackTimeChangeEventArgs.TrackTime);
        }

        private void SpotifyClientOnOnTrackChange(object sender, TrackChangeEventArgs trackChangeEventArgs)
        {
            var track = trackChangeEventArgs.NewTrack;
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs
            {
                TrackName = track.ArtistResource.Name,
                Artist = track.ArtistResource.Name,
            });
            AlbumArtChanged?.Invoke(this, new AlbumArtChangedEventArgs { AlbumArt = track.GetAlbumArt(AlbumArtSize.Size160) });
        }

        private void SpotifyClientOnOnPlayStateChange(object sender, PlayStateEventArgs playStateEventArgs)
        {
            if (playStateEventArgs.Playing)
            {
                TrackPlaying?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                TrackPaused?.Invoke(this, EventArgs.Empty);
            }
        }

        public Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _spotifyClient?.Dispose();
            return Task.CompletedTask;
        }

        public async Task PlayTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _spotifyClient.Play();
        }

        public async Task PauseTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _spotifyClient.Pause();
        }

        public Task PreviousTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _spotifyClient.Previous();
            return Task.CompletedTask;
        }

        public Task NextTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _spotifyClient.Skip();
            return Task.CompletedTask;
        }
    }
}
