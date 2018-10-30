﻿using AudioBand.AudioSource;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Net.Http;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using Image = System.Drawing.Image;

namespace SpotifyAudioSource
{
    public class SpotifyAudioSource : IAudioSource
    {
        public string Name { get; } = "Spotify";
        public IAudioSourceLogger Logger { get; set; }

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

        private const string SpotifyPausedWindowTitle = "Spotify";
        private readonly SpotifyControls _spotifyControls = new SpotifyControls();
        private readonly Stopwatch _trackProgressStopwatch = new Stopwatch();
        private readonly Timer _checkSpotifyTimer = new Timer(200);
        private readonly Timer _progressTimer = new Timer(500);
        private string _lastSpotifyWindowTitle;
        private string _clientSecret = "";
        private string _clientId = "";
        private SpotifyWebAPI _spotifyApi;
        private bool _isAuthorizing;
        private TimeSpan _baseTrackProgress;
        private TimeSpan _currentTrackLength;
        private readonly HttpClient _httpClient = new HttpClient();

        public Task ActivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _checkSpotifyTimer.AutoReset = false;
            _checkSpotifyTimer.Elapsed += CheckSpotifyTimerOnElapsed;
            _progressTimer.AutoReset = false;
            _progressTimer.Elapsed += ProgressTimerOnElapsed;

            UpdateSecrets();
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

            Logger.Debug("Authorization recieved");
            if (payload.Error != null)
            {
                Logger.Warn($"Error with authorization: {payload.Error}");
                return;
            }

            var token = await auth.ExchangeCode(payload.Code);

            _spotifyApi = new SpotifyWebAPI
            {
                AccessToken = token.AccessToken,
                TokenType = token.TokenType
            };

            await UpdateStatusFromSpotify();

            _checkSpotifyTimer.Start();
        }

        private async Task<(string Artist, string TrackName, bool IsPlaying)> UpdateStatusFromSpotify()
        {
            var playback = await _spotifyApi.GetPlayingTrackAsync();
            if (playback?.Item == null)
            {
                Logger.Debug("No playback");
                return (null, null, false);
            }

            var item = playback.Item;

            var albumArtImage = await GetAlbumArt(new Uri(item.Album.Images[0].Url));
            var trackName = item.Name;
            var artist = item.Artists[0].Name;
            _currentTrackLength = TimeSpan.FromMilliseconds(item.DurationMs);
            var trackInfo = new TrackInfoChangedEventArgs
            {
                Artist = artist,
                TrackName = trackName,
                AlbumArt = albumArtImage,
                TrackLength = _currentTrackLength
            };
            TrackInfoChanged?.Invoke(this, trackInfo);

            _baseTrackProgress = TimeSpan.FromMilliseconds(playback.ProgressMs);
            TrackProgressChanged?.Invoke(this, _baseTrackProgress);

            var isPlaying = playback.IsPlaying;
            if (isPlaying)
            {
                TrackPlaying?.Invoke(this, EventArgs.Empty);

                // If the track is playing then we use a timer to estimate the track progress instead of hitting the api every second. might be changed.
                _trackProgressStopwatch.Restart();
                _progressTimer.Start();
            }
            else
            {
                TrackPaused?.Invoke(this, EventArgs.Empty);
                _progressTimer.Stop();
            }

            return (artist, trackName, isPlaying);
        }

        private void ProgressTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var total = _baseTrackProgress + _trackProgressStopwatch.Elapsed;
            if (total > _currentTrackLength)
            {
                total = _currentTrackLength;
            }

            TrackProgressChanged?.Invoke(this, total);
            _progressTimer.Enabled = true;
        }

        private async Task<Image> GetAlbumArt(Uri albumArtUrl)
        {
            try
            {
                var response = await _httpClient.GetAsync(albumArtUrl);
                if (!response.IsSuccessStatusCode)
                {
                    Logger.Warn("Response was not successful when getting album art: " + response);
                }

                var stream = await response.Content.ReadAsStreamAsync();
                return Image.FromStream(stream);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        // We will check the spotify window title for changes and only call the api when it changes.
        // The spotify window title is a fixed value when it is paused and the artist - song when playing
        private async void CheckSpotifyTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                var currentSpotifyWindowTitle = _spotifyControls.GetSpotifyWindowTitle();
                if (currentSpotifyWindowTitle == _lastSpotifyWindowTitle)
                {
                    return;
                }

                // Spotify window title has changed, so either the track changed or its changed from playing to pause and vice versa
                var (currentArtist, currentTrackName, spotifyIsPlaying) = await UpdateStatusFromSpotify();

                // Sometimes the web api is not up to date quickly so we should double check what the api returns against the window title.
                // If the title is different, then we don't update the current spotify window title and the next check should call the api again
                var matches = spotifyIsPlaying
                    ? currentSpotifyWindowTitle == $"{currentArtist} - {currentTrackName}" 
                    : currentSpotifyWindowTitle == SpotifyPausedWindowTitle;
                if (matches)
                {
                    _lastSpotifyWindowTitle = currentSpotifyWindowTitle;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            finally
            {
                _checkSpotifyTimer.Enabled = true;
            }
        }

        public Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _checkSpotifyTimer.Stop();
            _checkSpotifyTimer.Elapsed -= CheckSpotifyTimerOnElapsed;
            _progressTimer.Stop();
            _progressTimer.Elapsed -= ProgressTimerOnElapsed;

            _spotifyApi = null;

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

        private void SetBlankState()
        {
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs());
            TrackPaused?.Invoke(this, EventArgs.Empty);
            TrackProgressChanged?.Invoke(this, new TimeSpan());
        }
    }
}
