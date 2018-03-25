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
        private int _trackLength;

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
            _trackLength = track.Length;
            TrackProgressChanged?.Invoke(this, CalculateTrackPercentange(status.PlayingPosition));
            AlbumArtChanged?.Invoke(this, new AlbumArtChangedEventArgs { AlbumArt = track.GetAlbumArt(AlbumArtSize.Size640) });
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs
            {
                TrackName = track.TrackResource.Name,
                Artist = track.ArtistResource.Name
            });

            if (status.Playing)
            {
                TrackPlaying?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                TrackPaused?.Invoke(this, EventArgs.Empty);
            }

            _spotifyClient.OnPlayStateChange += SpotifyClientOnOnPlayStateChange;
            _spotifyClient.OnTrackChange += SpotifyClientOnOnTrackChange;
            _spotifyClient.OnTrackTimeChange += SpotifyClientOnOnTrackTimeChange;

            return Task.CompletedTask;
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

        private void SpotifyClientOnOnTrackTimeChange(object sender, TrackTimeChangeEventArgs trackTimeChangeEventArgs)
        {
            
            TrackProgressChanged?.Invoke(this, CalculateTrackPercentange(trackTimeChangeEventArgs.TrackTime));
        }

        private void SpotifyClientOnOnTrackChange(object sender, TrackChangeEventArgs trackChangeEventArgs)
        {
            var track = trackChangeEventArgs.NewTrack;
            _trackLength = track.Length;
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs
            {
                TrackName = track.TrackResource.Name,
                Artist = track.ArtistResource.Name,
            });
            AlbumArtChanged?.Invoke(this, new AlbumArtChangedEventArgs { AlbumArt = track.GetAlbumArt(AlbumArtSize.Size640) });
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

        private int CalculateTrackPercentange(double trackTime)
        {
            return (int)(trackTime / _trackLength * 100);
        }
    }
}
