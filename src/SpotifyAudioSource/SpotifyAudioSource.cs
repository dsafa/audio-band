using AudioBand.AudioSource;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SpotifyAudioSource
{
    public class SpotifyAudioSource : IAudioSource
    {
        public string Name { get; } = "Spotify";

        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;
        public event EventHandler TrackPlaying;
        public event EventHandler TrackPaused;
        public event EventHandler<double> TrackProgressChanged;

        private SpotifyLocalAPI _spotifyClient;
        private int _trackLength;
        private Timer _checkForSpotifytimer;
        private bool _hasConnected;
        private bool _isOpen;
        private IAudioSourceLogger _logger;

        public Task ActivateAsync(IAudioSourceContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger = context.Logger;
            SetupClient();

            _checkForSpotifytimer = new Timer
            {
                Interval = 1000,
                AutoReset = false
            };
            _checkForSpotifytimer.Elapsed += CheckForSpotifytimerOnElapsed;
            _checkForSpotifytimer.Start();

            return Task.CompletedTask;
        }

        private void CheckForSpotifytimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                // If spotify was connected to and not anymore
                if (_hasConnected && !SpotifyRunning())
                {
                    ResetState();
                    _isOpen = false;
                    _logger.Debug("Spotify is not running anymore");
                    return;
                }

                if (_hasConnected && SpotifyRunning())
                {
                    if (_isOpen)
                    {
                        return;
                    }

                    // Spotify was reopened. It may fail to update in which case we will retry next timer tick
                    _isOpen = UpdateSongInfo();
                    _logger.Debug("Spotify reopened");
                    return;
                }

                // Spotify isnt running so don't try to connect
                if (!SpotifyRunning())
                {
                    return;
                }

                // Spotify was never connected to
                Connect();
                _logger.Debug("Connected to spotify");
            }
            catch (Exception)
            {
                // Random http 403 errors
            }
            finally
            {
                _checkForSpotifytimer.Enabled = true;
            }
        }

        public Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _spotifyClient?.Dispose();
            _hasConnected = false;
            if (_checkForSpotifytimer != null)
            {
                _checkForSpotifytimer.Elapsed -= CheckForSpotifytimerOnElapsed;
                _checkForSpotifytimer.Dispose();
            }

            ResetState();
            return Task.CompletedTask;
        }

        public async Task PlayTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _spotifyClient.Play().ConfigureAwait(false);
        }

        public async Task PauseTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _spotifyClient.Pause().ConfigureAwait(false);
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
                AlbumArt = track.GetAlbumArt(AlbumArtSize.Size640)
            });
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

        private double CalculateTrackPercentange(double trackTime)
        {
            if (_trackLength == 0)
            {
                return 0;
            }
            return trackTime / _trackLength * 100;
        }

        private void Connect()
        {
            if (!_spotifyClient.Connect())
            {
                _hasConnected = false;
                _isOpen = false;
                return;
            }

            _isOpen = UpdateSongInfo();
            _spotifyClient.ListenForEvents = true;
            _hasConnected = true;
        }

        private bool UpdateSongInfo()
        {
            var status = _spotifyClient.GetStatus();

            var track = status?.Track;
            if (track == null)
            {
                return false;
            }
            _trackLength = track.Length;

            TrackProgressChanged?.Invoke(this, CalculateTrackPercentange(status.PlayingPosition));
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs
            {
                TrackName = track.TrackResource.Name,
                Artist = track.ArtistResource.Name,
                AlbumArt = track.GetAlbumArt(AlbumArtSize.Size640)
            });

            if (status.Playing)
            {
                TrackPlaying?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                TrackPaused?.Invoke(this, EventArgs.Empty);
            }

            return true;
        }

        private void SetupClient()
        {
            _spotifyClient = new SpotifyLocalAPI();

            _spotifyClient.OnPlayStateChange += SpotifyClientOnOnPlayStateChange;
            _spotifyClient.OnTrackChange += SpotifyClientOnOnTrackChange;
            _spotifyClient.OnTrackTimeChange += SpotifyClientOnOnTrackTimeChange;
        }

        private bool SpotifyRunning()
        {
            return SpotifyLocalAPI.IsSpotifyRunning() && SpotifyLocalAPI.IsSpotifyWebHelperRunning();
        }

        private void ResetState()
        {
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs { TrackName = "", AlbumArt = null });
            TrackPaused?.Invoke(this, EventArgs.Empty);
            TrackProgressChanged?.Invoke(this, 0);
        }

        ~SpotifyAudioSource()
        {
            if (_checkForSpotifytimer != null)
            {
                _checkForSpotifytimer.Elapsed -= CheckForSpotifytimerOnElapsed;
                _checkForSpotifytimer.Dispose();
            }
        }
    }
}
