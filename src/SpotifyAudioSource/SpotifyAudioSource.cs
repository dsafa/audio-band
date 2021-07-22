using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using AudioBand.AudioSource;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Http;
using static SpotifyAPI.Web.PlayerCurrentPlaybackRequest;
using static SpotifyAPI.Web.PlayerSetRepeatRequest;
using Image = System.Drawing.Image;
using Timer = System.Timers.Timer;

namespace SpotifyAudioSource
{
    /// <summary>
    /// Audio source for spotify.
    /// </summary>
    public class SpotifyAudioSource : IAudioSource
    {
        private readonly SpotifyControls _spotifyControls = new SpotifyControls();
        private readonly Timer _checkSpotifyTimer = new Timer(1000);
        private SpotifyClientConfig _spotifyConfig;
        private ISpotifyClient _spotifyClient;
        private HttpClient _httpClient = new HttpClient();
        private string _currentItemId;
        private string _currentTrackName;
        private bool _currentIsPlaying;
        private int _currentProgress;
        private int _currentVolumePercent;
        private bool _currentShuffle;
        private string _currentRepeat;
        private string _clientSecret;
        private string _clientId;
        private string _refreshToken;
        private int _pollingInterval = 1000;
        private bool _useProxy;
        private string _proxyHost;
        private int _localPort = 80;
        private int _proxyPort;
        private string _proxyUserName;
        private string _proxyPassword;
        private bool _isActive;
        private int _retryAttempts;
        private const int _maxRetries = 25;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpotifyAudioSource"/> class.
        /// </summary>
        public SpotifyAudioSource()
        {
            _checkSpotifyTimer.AutoReset = false;
            _checkSpotifyTimer.Elapsed += CheckSpotifyTimerOnElapsed;
        }

        /// <inheritdoc />
        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        /// <inheritdoc />
        public event EventHandler<TimeSpan> TrackProgressChanged;

        /// <inheritdoc />
        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        /// <inheritdoc />
        public event EventHandler<float> VolumeChanged;

        /// <inheritdoc />
        public event EventHandler<bool> ShuffleChanged;

        /// <inheritdoc />
        public event EventHandler<RepeatMode> RepeatModeChanged;

        /// <inheritdoc />
        public event EventHandler<bool> IsPlayingChanged;

        /// <inheritdoc />
        public string Name { get; } = "Spotify";

        /// <inheritdoc />
        public IAudioSourceLogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the spotify client id.
        /// </summary>
        [AudioSourceSetting("Spotify Client ID")]
        public string ClientId
        {
            get => _clientId;
            set
            {
                if (value == _clientId)
                {
                    return;
                }

                _clientId = value;
                Authorize();
            }
        }

        /// <summary>
        /// Gets or sets the spotify client secret.
        /// </summary>
        [AudioSourceSetting("Spotify Client secret")]
        public string ClientSecret
        {
            get => _clientSecret;
            set
            {
                if (value == _clientSecret)
                {
                    return;
                }

                _clientSecret = value;
                Authorize();
            }
        }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        [AudioSourceSetting("Spotify Refresh Token", Options = SettingOptions.ReadOnly | SettingOptions.Hidden, Priority = 9)]
        public string RefreshToken
        {
            get => _refreshToken;
            set
            {
                if (value == _refreshToken)
                {
                    return;
                }

                _refreshToken = value;
                OnSettingChanged("Spotify Refresh Token");
            }
        }

        /// <summary>
        /// Gets or sets the Polling Interval.
        /// </summary>
        [AudioSourceSetting("Spotify Polling Interval")]
        public int PollingInterval
        {
            get => _pollingInterval;
            set
            {
                if (value == _pollingInterval)
                {
                    return;
                }

                _pollingInterval = value;
                OnSettingChanged("Spotify Polling Interval");
                UpdatePollingInterval();
            }
        }

        /// <summary>
        /// Gets or sets the port for the local webserver used for authentication with spotify.
        /// </summary>
        [AudioSourceSetting("Callback Port", Priority = 10)]
        public int LocalPort
        {
            get => _localPort;
            set
            {
                if (value == _localPort)
                {
                    return;
                }

                _localPort = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use a proxy.
        /// </summary>
        [AudioSourceSetting("Use Proxy", Priority = 15)]
        public bool UseProxy
        {
            get => _useProxy;
            set
            {
                if (value == _useProxy)
                {
                    return;
                }

                _useProxy = value;
                UpdateProxy();
            }
        }

        /// <summary>
        /// Gets or sets the proxy host.
        /// </summary>
        [AudioSourceSetting("Proxy Host", Priority = 20)]
        public string ProxyHost
        {
            get => _proxyHost;
            set
            {
                if (value == _proxyHost)
                {
                    return;
                }

                _proxyHost = value;
                UpdateProxy();
            }
        }

        /// <summary>
        /// Gets or sets the proxy port.
        /// </summary>
        [AudioSourceSetting("Proxy Port", Priority = 20)]
        public int ProxyPort
        {
            get => _proxyPort;
            set
            {
                if (value == _proxyPort)
                {
                    return;
                }

                _proxyPort = value;
                UpdateProxy();
            }
        }

        /// <summary>
        /// Gets or sets the proxy username.
        /// </summary>
        [AudioSourceSetting("Proxy Username", Priority = 20)]
        public string ProxyUserName
        {
            get => _proxyUserName;
            set
            {
                if (value == _proxyUserName)
                {
                    return;
                }

                _proxyUserName = value;
                UpdateProxy();
            }
        }

        /// <summary>
        /// Gets or sets the proxy password.
        /// </summary>
        [AudioSourceSetting("Proxy Password", Options = SettingOptions.Sensitive, Priority = 20)]
        public string ProxyPassword
        {
            get => _proxyPassword;
            set
            {
                if (value == _proxyPassword)
                {
                    return;
                }

                _proxyPassword = value;
                UpdateProxy();
            }
        }

        /// <inheritdoc />
        public Task ActivateAsync()
        {
            _checkSpotifyTimer.Start();

            _isActive = true;
            Authorize();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task DeactivateAsync()
        {
            _isActive = false;

            _checkSpotifyTimer.Stop();
            _currentItemId = null;
            _currentTrackName = null;
            Logger.Debug("Spotify has been deactivated.");

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task PlayTrackAsync()
        {
            // Play/pause/next/back controls use the desktop rather than the api
            // because it is quicker and player controls require spotify premium.
            _spotifyControls.Play();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task PauseTrackAsync()
        {
            _spotifyControls.Pause();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task PreviousTrackAsync()
        {
            _spotifyControls.Previous();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task NextTrackAsync()
        {
            _spotifyControls.Next();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task SetVolumeAsync(float newVolume)
        {
            var volume = (int) newVolume * 100;
            await LogPlayerCommandIfFailed(() 
                => _spotifyClient.Player.SetVolume(new PlayerVolumeRequest(volume)), "SetVolume");
        }

        /// <inheritdoc />
        public async Task SetPlaybackProgressAsync(TimeSpan newProgress)
        {
            await LogPlayerCommandIfFailed(()
                => _spotifyClient.Player.SeekTo(new PlayerSeekToRequest((long)newProgress.TotalMilliseconds)), "SetPlaybackProgress");
        }

        /// <inheritdoc />
        public async Task SetShuffleAsync(bool shuffleOn)
        {
            await LogPlayerCommandIfFailed(()
                => _spotifyClient.Player.SetShuffle(new PlayerShuffleRequest(shuffleOn)), "SetShuffle");
        }

        /// <inheritdoc />
        public async Task SetRepeatModeAsync(RepeatMode newRepeatMode)
        {
            await LogPlayerCommandIfFailed(()
            => _spotifyClient.Player.SetRepeat(new PlayerSetRepeatRequest(ToRepeatState(newRepeatMode))), "SetRepeatMode");
        }

        private RepeatMode ToRepeatMode(State state)
        {
            switch (state)
            {
                case State.Off:
                    return RepeatMode.Off;
                case State.Context:
                    return RepeatMode.RepeatContext;
                case State.Track:
                    return RepeatMode.RepeatTrack;
                default:
                    Logger.Warn($"No case for {state}");
                    return RepeatMode.Off;
            }
        }

        private RepeatMode ToRepeatMode(string state)
        {
            switch (state)
            {
                case "off":
                    return RepeatMode.Off;
                case "context":
                    return RepeatMode.RepeatContext;
                case "track":
                    return RepeatMode.RepeatTrack;
                default:
                    Logger.Warn($"No case for {state}");
                    return RepeatMode.Off;
            }
        }

        private State ToRepeatState(RepeatMode mode)
        {
            switch (mode)
            {
                case RepeatMode.Off:
                    return State.Off;
                case RepeatMode.RepeatContext:
                    return State.Context;
                case RepeatMode.RepeatTrack:
                    return State.Track;
                default:
                    Logger.Warn($"No case for {mode}");
                    return State.Off;
            }
        }

        private void Authorize()
        {
            if (!_isActive)
            {
                return;
            }
            else if (string.IsNullOrEmpty(ClientId) || string.IsNullOrEmpty(ClientSecret))
            {
                Logger.Error($"Cannot connect to Spotify because either ClientId or ClientSecret is empty.");
                return;
            }
            else if (!string.IsNullOrEmpty(RefreshToken))
            {
                Logger.Debug("Using RefreshToken from previous auth to connect to Spotify.");
                RefreshAccessTokenOnClient().GetAwaiter().GetResult();
            }
            else
            {
                Logger.Debug("Connecting to Spotify through own application.");
                var address = new Uri("http://localhost:80");

                var server = new EmbedIOAuthServer(address, 80);
                server.Start().GetAwaiter().GetResult();

                server.AuthorizationCodeReceived += async (sender, response) =>
                {
                    await server.Stop();

                    var config = SpotifyClientConfig.CreateDefault();
                    var tokenResponse = await new OAuthClient(config).RequestToken(
                        new AuthorizationCodeTokenRequest(ClientId, ClientSecret, response.Code, address)
                    );

                    _spotifyConfig = SpotifyClientConfig.CreateDefault().WithAuthenticator(new AuthorizationCodeAuthenticator(ClientId, ClientSecret, tokenResponse));
                    _spotifyClient = new SpotifyClient(_spotifyConfig);
                };

                var request = new LoginRequest(address, ClientId, LoginRequest.ResponseType.Code)
                {
                    Scope = new [] { Scopes.UserReadCurrentlyPlaying, Scopes.UserReadPlaybackState, Scopes.UserReadPlaybackPosition, Scopes.UserModifyPlaybackState }
                };

                BrowserUtil.Open(request.ToUri());
            }
        }

        private void UpdateSpotifyHttpClient()
        {
            if (string.IsNullOrEmpty(ProxyHost) || ProxyPort == 0
            || string.IsNullOrEmpty(ProxyUserName) || string.IsNullOrEmpty(ProxyPassword))
            {
                return;
            }

            var httpClient = new NetHttpClient(new ProxyConfig(ProxyHost, ProxyPort)
                {
                    User = ProxyUserName,
                    Password = ProxyPassword
                });

            _spotifyConfig.WithHTTPClient(httpClient);
        }

        private void UpdateProxy()
        {
            Logger.Debug("Updating proxy configuration.");

            UpdateSpotifyHttpClient();
            if (_spotifyConfig is null)
                return;
            _spotifyClient = new SpotifyClient(_spotifyConfig);
        }

        private async Task<CurrentlyPlayingContext> GetPlayback()
        {
            try
            {
                var playback = await _spotifyClient.Player.GetCurrentPlayback(new PlayerCurrentPlaybackRequest(AdditionalTypes.All));

                return playback;
            }
            catch (HttpRequestException e)
            {
                // Gets thrown when internet cuts out, will retry every 10 seconds.
                if (_retryAttempts >= _maxRetries)
                {
                    Logger.Info($"Retried {_maxRetries} times, but user still has no internet connection. Disabling Spotify.");
                    await DeactivateAsync();
                    return null;
                }

                Logger.Info("Tried to update Spotify Playback, but user has no internet connection.");
                await Task.Delay(10000);
                _retryAttempts++;
                return await GetPlayback();
            }
            catch (APIUnauthorizedException e)
            {
                await RefreshAccessTokenOnClient();
                return await GetPlayback();
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        private async Task NotifyTrackUpdate(FullTrack track)
        {
            // Local files have no id so we use name
            if (track.Id == _currentItemId && track.Name == _currentTrackName)
            {
                return;
            }

            _currentItemId = track.Id;
            _currentTrackName = track.Name;

            string albumArtUrl = track.Album?.Images.FirstOrDefault()?.Url;
            Image albumArtImage = albumArtUrl is null ? null : await GetAlbumArt(new Uri(albumArtUrl));

            string artists = string.Join(", ", track.Artists?.Select(a => a?.Name));
            var trackLength = TimeSpan.FromMilliseconds(track.DurationMs);

            var trackUpdateInfo = new TrackInfoChangedEventArgs
            {
                Artist = artists,
                TrackName = track.Name,
                AlbumArt = albumArtImage,
                Album = track.Album?.Name,
                TrackLength = trackLength,
            };

            TrackInfoChanged?.Invoke(this, trackUpdateInfo);
        }

        private async Task NotifyEpisodeUpdate(FullEpisode episode)
        {
            _currentItemId = episode.Id;
            _currentTrackName = episode.Name;

            var imageUrl = episode.Images?.FirstOrDefault()?.Url;

            var trackUpdateInfo = new TrackInfoChangedEventArgs
            {
                Artist = episode.Show.Publisher,
                TrackName = episode.Name,
                AlbumArt = await GetPodcastAlbumArtAsync(imageUrl),
                Album = episode.Show.Name,
                TrackLength = TimeSpan.FromMilliseconds(episode.DurationMs),
            };

            TrackInfoChanged?.Invoke(this, trackUpdateInfo);
        }

        private void NotifyPlayState(CurrentlyPlayingContext context)
        {
            bool isPlaying = context.IsPlaying;
            if (isPlaying == _currentIsPlaying)
            {
                return;
            }

            _currentIsPlaying = isPlaying;
            IsPlayingChanged?.Invoke(this, _currentIsPlaying);
        }

        private void NotifyTrackProgress(CurrentlyPlayingContext context)
        {
            int progress = context.ProgressMs;
            if (progress == _currentProgress)
            {
                return;
            }

            _currentProgress = progress;
            TrackProgressChanged?.Invoke(this, TimeSpan.FromMilliseconds(_currentProgress));
        }

        private void NotifyVolume(CurrentlyPlayingContext context)
        {
            if (context.Device == null)
            {
                return;
            }

            int vol = context.Device.VolumePercent.HasValue ? context.Device.VolumePercent.Value : 0;
            if (vol == _currentVolumePercent)
            {
                return;
            }

            _currentVolumePercent = vol;
            VolumeChanged?.Invoke(this, _currentVolumePercent / 100f);
        }

        private void NotifyShuffle(CurrentlyPlayingContext context)
        {
            bool shuffle = context.ShuffleState;
            if (shuffle == _currentShuffle)
            {
                return;
            }

            _currentShuffle = shuffle;
            ShuffleChanged?.Invoke(this, _currentShuffle);
        }

        private void NotifyRepeat(CurrentlyPlayingContext context)
        {
            string repeat = context.RepeatState;
            if (repeat == _currentRepeat)
            {
                return;
            }

            _currentRepeat = repeat;
            RepeatModeChanged?.Invoke(this, ToRepeatMode(_currentRepeat));
        }

        private async Task<Image> GetAlbumArt(Uri albumArtUrl)
        {
            if (albumArtUrl == null)
            {
                return null;
            }

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

        private async Task<Image> GetPodcastAlbumArtAsync(string imageUrl = null)
        {
            try
            {
                HttpResponseMessage response;
                
                if (string.IsNullOrEmpty(imageUrl))
                {
                    response = await _httpClient.GetAsync("https://i.imgur.com/FZG4OtK.png");
                }
                else
                {
                    response = await _httpClient.GetAsync(new Uri(imageUrl));
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

        private async void CheckSpotifyTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            // Spotify api does not provide a way to get realtime player status updates, so we have to resort to polling.
            try
            {
                if (!_isActive)
                {
                    return;
                }

                var currentSpotifyWindowTitle = _spotifyControls.GetSpotifyWindowTitle();
                if (string.IsNullOrEmpty(currentSpotifyWindowTitle))
                {
                    // reduce number of calls when paused since we have to poll for track changes.
                    _checkSpotifyTimer.Interval = 3000;
                }
                else
                {
                    _checkSpotifyTimer.Interval = PollingInterval;
                }

                var playback = await GetPlayback();
                if (playback == null)
                {
                    return;
                }

                NotifyPlayState(playback);
                NotifyTrackProgress(playback);
                NotifyVolume(playback);
                NotifyShuffle(playback);
                NotifyRepeat(playback);

                if (playback.Item == null)
                {
                    // Playback can be null if there are no devices playing
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    return;
                }
                else if (playback.Item.Type == ItemType.Track)
                {
                    await NotifyTrackUpdate(playback.Item as FullTrack);
                }
                else
                {
                    await NotifyEpisodeUpdate(playback.Item as FullEpisode);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            finally
            {
                // check again in case
                if (_isActive)
                {
                    _checkSpotifyTimer.Enabled = true;
                }
            }
        }

        private async Task RefreshAccessTokenOnClient()
        {
            if (string.IsNullOrEmpty(RefreshToken))
            {
                Logger.Warn("RefreshToken missing. Please restart and go through the authorization process again.");
                await DeactivateAsync();
                return;
            }

            var request = new AuthorizationCodeRefreshRequest(ClientId, ClientSecret, RefreshToken);
            var response = await new OAuthClient().RequestToken(request);

            _spotifyClient = new SpotifyClient(response.AccessToken);

            var expiresIn = TimeSpan.FromSeconds(response.ExpiresIn);
            Logger.Debug($"Received new access token. Expires in: {expiresIn} (At {DateTime.Now + expiresIn})");
        }

        private void OnSettingChanged(string settingName)
        {
            SettingChanged?.Invoke(this, new SettingChangedEventArgs(settingName));
        }

        private async Task LogPlayerCommandIfFailed(Func<Task<bool>> command, [CallerMemberName] string caller = null)
        {
            var hasError = await command();
            if (hasError)
            {
                Logger.Warn($"Something went wrong with player command [{caller}].");
            }
        }
    
        private void UpdatePollingInterval()
        {
            _checkSpotifyTimer.Stop();
            _checkSpotifyTimer.Interval = PollingInterval;
            _checkSpotifyTimer.Start();
        }
    }
}
