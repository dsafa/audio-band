using AudioBand.AudioSource;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Net;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using Image = System.Drawing.Image;

namespace SpotifyAudioSource
{
    public class SpotifyAudioSource : IAudioSource
    {
        public string Name { get; } = "Spotify";

        [AudioSourceSetting("Spotify Client ID")]
        public string ClientId
        {
            get => _clientId;
            set
            {
                _clientId = value;
                UpdateSecrets();
            }
        }

        [AudioSourceSetting("Spotify Client secret")]
        public string ClientSecret
        {
            get => _clientSecret;
            set
            {
                _clientSecret = value;
                UpdateSecrets();
            }
        }

        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;
        public event EventHandler TrackPlaying;
        public event EventHandler TrackPaused;
        public event EventHandler<TimeSpan> TrackProgressChanged;

        private readonly SpotifyControls _spotifyControls = new SpotifyControls();
        private string _currentSpotifyWindowTitle;
        private string _clientSecret = "";
        private string _clientId = "";
        private SpotifyWebAPI _spotifyApi;
        private Timer _checkSpotifyTimer;
        private Timer _progressTimer;
        private IAudioSourceLogger _logger;
        private bool _isAuthorizing;
        private TimeSpan _initialTrackProgress;
        private Stopwatch _trackProgressStopwatch = new Stopwatch();

        public Task ActivateAsync(IAudioSourceContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger = context.Logger;

            return Task.CompletedTask;
        }

        private void UpdateSecrets()
        { 
            // Try to prevent multiple popups for authorization
            if (string.IsNullOrEmpty(ClientId) || string.IsNullOrEmpty(ClientSecret) || _isAuthorizing || _spotifyApi != null)
            {
                return;
            }

            _isAuthorizing = true;
            var auth = new AuthorizationCodeAuth(ClientId, ClientSecret, "http://localhost", "http://localhost", 
                Scope.UserModifyPlaybackState | Scope.UserReadPlaybackState | Scope.UserReadCurrentlyPlaying);
            auth.AuthReceived += OnAuthReceived;
            auth.Start();
            auth.OpenBrowser();
        }

        private async void OnAuthReceived(object sender, AuthorizationCode payload)
        {
            var auth = (AuthorizationCodeAuth)sender;
            auth.Stop();
            _isAuthorizing = false;

            _logger.Debug("Authorization recieved");
            if (payload.Error != null)
            {
                _logger.Warn($"Error with authorization: {payload.Error}");
                return;
            }

            var token = await auth.ExchangeCode(payload.Code);

            _spotifyApi = new SpotifyWebAPI
            {
                AccessToken = token.AccessToken,
                TokenType = token.TokenType
            };

            await UpdateStatusFromSpotify();

            _checkSpotifyTimer?.Stop();
            _checkSpotifyTimer = new Timer(200);
            _checkSpotifyTimer.Elapsed += CheckSpotifyTimerOnElapsed;
            _checkSpotifyTimer.Start();
        }

        private async Task<(string Artist, string TrackName, bool IsPlaying)> UpdateStatusFromSpotify()
        {
            var playback = await _spotifyApi.GetPlaybackAsync();
            var item = playback.Item;

            var albumArtImage = await GetAlbumArt(new Uri(item.Album.Images[0].Url));
            var trackName = item.Name;
            var artist = item.Artists[0].Name;
            var trackInfo = new TrackInfoChangedEventArgs
            {
                Artist = artist,
                TrackName = trackName,
                AlbumArt = albumArtImage
            };
            TrackInfoChanged?.Invoke(this, trackInfo);

            _initialTrackProgress = TimeSpan.FromSeconds(item.DurationMs);
            TrackProgressChanged?.Invoke(this, _initialTrackProgress);

            var isPlaying = playback.IsPlaying;
            if (isPlaying)
            {
                TrackPlaying?.Invoke(this, EventArgs.Empty);

                // we use a timer to estimate the track progress instead of hitting the api very often. might be changed.
                _trackProgressStopwatch.Restart();
                _progressTimer?.Stop();
                _progressTimer = new Timer(500);
                _progressTimer.Elapsed += ProgressTimerOnElapsed;
                _progressTimer.Start();
            }
            else
            {
                TrackPaused?.Invoke(this, EventArgs.Empty);
                _progressTimer?.Stop();
            }

            return (artist, trackName, isPlaying);
        }

        private void ProgressTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var elapsed = _trackProgressStopwatch.Elapsed;
            TrackProgressChanged?.Invoke(this, _initialTrackProgress + elapsed);
        }

        private async Task<Image> GetAlbumArt(Uri albumArtUrl)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    var rawData = await webClient.DownloadDataTaskAsync(albumArtUrl);
                    using (var stream = new MemoryStream(rawData))
                    {
                        return Image.FromStream(stream);
                    }
                }
            }
            catch (WebException e)
            {
                _logger.Error($"{e} {e.InnerException?.Message}");
                return null;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return null;
            }
        }

        // We will check the spotify window title for changes and only call the api when it changes.
        // The spotify window title is a fixed value when it is paused and the artist - song when playing
        private async void CheckSpotifyTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                var spotifyWindowTitle = _spotifyControls.GetSpotifyWindowTitle();
                if (spotifyWindowTitle == _currentSpotifyWindowTitle)
                {
                    return;
                }

                // Something has changed, refresh status
                var trackInfo = await UpdateStatusFromSpotify();

                // Sometimes the web api is not up to date quickly so we should double check against the the last title so see it its changed, only when playing
                if (_currentSpotifyWindowTitle != $"{trackInfo.Artist} - {trackInfo.TrackName}" && trackInfo.IsPlaying)
                {
                    // if its not the case, then it was updated succesfully. otherwise the next timer tick should check again
                    _currentSpotifyWindowTitle = spotifyWindowTitle;
                }

            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
            }
            finally
            {
                _checkSpotifyTimer.Enabled = true;
            }
        }

        public Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_checkSpotifyTimer != null)
            {
                _checkSpotifyTimer.Elapsed -= CheckSpotifyTimerOnElapsed;
                _checkSpotifyTimer.Dispose();
            }

            SetBlankState();
            return Task.CompletedTask;
        }

        public Task PlayTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _spotifyControls.Play();
            return Task.CompletedTask;
        }

        public Task PauseTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _spotifyControls.Pause();
            return Task.CompletedTask;
        }

        public Task PreviousTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _spotifyControls.Previous();
            return Task.CompletedTask;
        }

        public Task NextTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _spotifyControls.Next();
            return Task.CompletedTask;
        }

        public bool ValidateSettingChange(string settingName, object newValue)
        {
            return true;
        }

        private void SetBlankState()
        {
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs());
            TrackPaused?.Invoke(this, EventArgs.Empty);
            TrackProgressChanged?.Invoke(this, new TimeSpan());
        }

        ~SpotifyAudioSource()
        {
            if (_checkSpotifyTimer != null)
            {
                _checkSpotifyTimer.Elapsed -= CheckSpotifyTimerOnElapsed;
                _checkSpotifyTimer.Dispose();
            }
        }
    }
}
