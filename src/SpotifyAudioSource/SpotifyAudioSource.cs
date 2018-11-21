using AudioBand.AudioSource;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Net.Http;
using System.Runtime.CompilerServices;
using SpotifyAPI;
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
        public IAudioSourceLogger Logger { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;
        public event EventHandler TrackPlaying;
        public event EventHandler TrackPaused;
        public event EventHandler<TimeSpan> TrackProgressChanged;

        [AudioSourceSetting("Spotify Client ID")]
        public string ClientId
        {
            get => _clientId;
            set
            {
                if (value == _clientId) return;

                _clientId = value;
                Authorize();
            }
        }

        [AudioSourceSetting("Spotify Client secret")]
        public string ClientSecret
        {
            get => _clientSecret;
            set
            {
                if (value == _clientSecret) return;

                _clientSecret = value;
                Authorize();
            }
        }

        [AudioSourceSetting("Callback Port", Priority = 10)]
        public uint LocalPort
        {
            get => _localPort;
            set
            {
                if (value == _localPort) return;
                _localPort = value;
            }
        }

        [AudioSourceSetting("Spotify Refresh Token", Options = SettingOptions.ReadOnly | SettingOptions.Hidden,
            Priority = 9)]
        public string RefreshToken
        {
            get => _refreshToken;
            set
            {
                if (value == _refreshToken) return;

                _refreshToken = value;
                OnPropertyChanged();
            }
        }

        [AudioSourceSetting("Use Proxy")]
        public bool UseProxy
        {
            get => _useProxy;
            set
            {
                if (value == _useProxy) return;
                _useProxy = value;
                UpdateProxy();
            }
        }

        [AudioSourceSetting("Proxy Host")]
        public string ProxyHost
        {
            get => _proxyConfig.Host;
            set
            {
                if (value == _proxyConfig.Host) return;
                _proxyConfig.Host = value;
                UpdateProxy();
            }
        }

        [AudioSourceSetting("Proxy Port")]
        public uint ProxyPort
        {
            get => (uint)_proxyConfig.Port;
            set
            {
                if (value == _proxyConfig.Port) return;
                _proxyConfig.Port = (int)value; // may overflow
                UpdateProxy();
            }
        }

        [AudioSourceSetting("Proxy Username")]
        public string ProxyUserName
        {
            get => _proxyConfig.Username;
            set
            {
                if (value == _proxyConfig.Username) return;
                _proxyConfig.Username = value;
                UpdateProxy();
            }
        }

        [AudioSourceSetting("Proxy Password", Options = SettingOptions.Sensitive)]
        public string ProxyPassword
        {
            get => _proxyConfig.Password;
            set
            {
                if (value == _proxyConfig.Password) return;
                _proxyConfig.Password = value;
                UpdateProxy();
            }
        }

        private readonly SpotifyControls _spotifyControls = new SpotifyControls();
        private readonly Stopwatch _trackProgressStopwatch = new Stopwatch();
        private readonly Timer _checkSpotifyTimer = new Timer(200);
        private readonly Timer _progressTimer = new Timer(500);
        private readonly Timer _refreshTimer = new Timer(50 * 60 * 1000);
        private readonly ProxyConfig _proxyConfig = new ProxyConfig();
        private HttpClient _httpClient = new HttpClient();
        private SpotifyWebAPI _spotifyApi = new SpotifyWebAPI();
        private string _lastSpotifyWindowTitle = "";
        private string _currentTrackId;
        private string _clientSecret;
        private string _clientId;
        private string _refreshToken;
        private uint _localPort= 80;
        private TimeSpan _baseTrackProgress;
        private TimeSpan _currentTrackLength;
        private AuthorizationCodeAuth _auth;
        private bool _isActive;
        private bool _useProxy;

        public SpotifyAudioSource()
        {
            _checkSpotifyTimer.AutoReset = false;
            _checkSpotifyTimer.Elapsed += CheckSpotifyTimerOnElapsed;
            _progressTimer.AutoReset = false;
            _progressTimer.Elapsed += ProgressTimerOnElapsed;
            _refreshTimer.AutoReset = true;
            _refreshTimer.Elapsed += RefreshTimerOnElapsed;
        }

        public Task ActivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _checkSpotifyTimer.Start();
            _refreshTimer.Start();

            _isActive = true;
            Authorize();
            return Task.CompletedTask;
        }

        public Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _isActive = false;

            _checkSpotifyTimer.Stop();
            _progressTimer.Stop();
            _refreshTimer.Stop();

            return Task.CompletedTask;
        }

        private void Authorize()
        { 
            if (string.IsNullOrEmpty(ClientId) || string.IsNullOrEmpty(ClientSecret) || !_isActive)
            {
                return;
            }

            Logger.Debug("Connecting to spotify");
            var url = "http://localhost:" + LocalPort;
            _auth?.Stop();
            _auth = new AuthorizationCodeAuth(ClientId, ClientSecret, url, url,
                Scope.UserModifyPlaybackState | Scope.UserReadPlaybackState | Scope.UserReadCurrentlyPlaying);

            if (string.IsNullOrEmpty(RefreshToken))
            {
                _auth.Start();
                _auth.AuthReceived += OnAuthReceived;
                _auth.OpenBrowser();
            }
            else
            {
                Logger.Debug("Reusing old refresh token");
                RefreshAccessToken(); // fire and forget
            }
        }

        private async void OnAuthReceived(object sender, AuthorizationCode payload)
        {
            _auth.Stop();
            if (payload.Error != null)
            {
                Logger.Warn($"Error with authorization: {payload.Error}");
                return;
            }

            Logger.Debug("Authorization recieved");

            var token = await _auth.ExchangeCode(payload.Code);
            RefreshToken = token.RefreshToken;

            Logger.Debug($"Received access token. Expires in: {TimeSpan.FromSeconds(token.ExpiresIn)}. Token: {token.AccessToken.Substring(0, 20)}...");

            _spotifyApi.TokenType = token.TokenType;
            _spotifyApi.AccessToken = token.AccessToken;
        }

        private void UpdateProxy()
        {
            if (!UseProxy) return;

            Logger.Debug("Updating proxy configuration");
            _spotifyApi = new SpotifyWebAPI(_proxyConfig)
            {
                AccessToken = _spotifyApi.AccessToken,
                TokenType = _spotifyApi.TokenType,
            };

            _httpClient = new HttpClient(new HttpClientHandler{ Proxy = _proxyConfig.CreateWebProxy(), UseProxy = true});
        }

        private async Task<PlaybackContext> GetPlayback()
        {
            try
            {
                var playback = await _spotifyApi.GetPlaybackAsync();
                if (playback.HasError())
                {
                    Logger.Warn($"Error while trying to get playback. Code: {playback.Error.Status}. Message: {playback.Error.Message}");
                    if (playback.Error.Status == (int)HttpStatusCode.Unauthorized)
                    {
                        Logger.Debug($"Access token: {_spotifyApi.AccessToken.Substring(0, 20)}...");
                    }
                }

                return playback;
            }
            catch (Exception e)
            {
                Logger.Error(e);

                await Task.Delay(TimeSpan.FromSeconds(1));
                return null;
            }
        }

        private async Task NotifyTrackUpdate(FullTrack track)
        {
            if (track.Id == _currentTrackId) return;

            _currentTrackId = track.Id;

            var albumArtImage = await GetAlbumArt(new Uri(track.Album.Images[0].Url));
            var trackName = track.Name;
            var artist = track.Artists[0].Name;
            _currentTrackLength = TimeSpan.FromMilliseconds(track.DurationMs);

            var trackUpdateInfo = new TrackInfoChangedEventArgs
            {
                Artist = artist,
                TrackName = trackName,
                AlbumArt = albumArtImage,
                Album = track.Album.Name,
                TrackLength = _currentTrackLength
            };

            TrackInfoChanged?.Invoke(this, trackUpdateInfo);
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
                    return null;
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
                if (!_isActive || string.IsNullOrEmpty(_spotifyApi.AccessToken)) return;

                var currentSpotifyWindowTitle = _spotifyControls.GetSpotifyWindowTitle();
                if (string.IsNullOrEmpty(currentSpotifyWindowTitle))
                {
                    _lastSpotifyWindowTitle = "";
                    _progressTimer.Stop();
                    return;
                }

                if (currentSpotifyWindowTitle == _lastSpotifyWindowTitle) return;

                // Spotify window title has changed, so either the track changed or its changed from playing to pause and vice versa
                Logger.Debug("Fetching playback status from spotify");

                var playback = await GetPlayback();
                if (playback?.Item == null)
                {
                    // Playback can be null if there are no devices playing
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    _lastSpotifyWindowTitle = "";
                    return;
                }
                Logger.Debug("Received playback");

                var track = playback.Item;
                await NotifyTrackUpdate(track);

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

                // Sometimes the web api is not up to date quickly so we should double check what the api returns against the window title.
                // If the title is different, then we don't update the current spotify window title and the next check should call the api again
                var titleMatches = isPlaying && currentSpotifyWindowTitle == $"{track.Artists[0].Name} - {track.Name}";
                var statusMatches = !isPlaying && _spotifyControls.IsPaused();
                if (titleMatches || statusMatches)
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

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task RefreshAccessToken()
        {
            try
            {
                Logger.Debug("Refreshing access token");

                var token = await _auth.RefreshToken(RefreshToken);
                if (token.HasError())
                {
                    Logger.Warn($"Error with refresh token. Requesting new token. Error: {token.Error}|{token.ErrorDescription}");
                    RefreshToken = null;
                    Authorize();
                    return;
                }

                var expiresIn = TimeSpan.FromSeconds(token.ExpiresIn);
                Logger.Debug($"Received new access token. Expires in: {expiresIn} (At {DateTime.Now + expiresIn}). Token: {token.AccessToken.Substring(0, 20)}...");

                _spotifyApi.AccessToken = token.AccessToken;
                _spotifyApi.TokenType = token.TokenType;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private async void RefreshTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (_auth == null || _spotifyApi == null) return;

            Logger.Debug("Access token about to expire.");
            await RefreshAccessToken();
        }
    }
}
