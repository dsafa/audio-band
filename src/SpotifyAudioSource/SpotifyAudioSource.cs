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
        private IHTTPClient _spotifyHttpClient = null;
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
        private bool _useProxy;
        private string _proxyHost;
        private int _localPort = 80;
        private int _proxyPort;
        private string _proxyUserName;
        private string _proxyPassword;
        private bool _isActive;

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

                _proxyPort = value; // may overflow
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
                Logger.Debug($"Connecting to Spotify through Login (PKCE).");

                var (verifier, challenge) = PKCEUtil.GenerateCodes();

                var server = new EmbedIOAuthServer(new Uri("http://localhost:5000/callback"), 5000);

                server.AuthorizationCodeReceived += async (sender, response) =>
                {
                    await server.Stop();
                    PKCETokenResponse initialResponse;

                    try
                    {
                        initialResponse = await new OAuthClient().RequestToken(
                        new PKCETokenRequest("857519a171be4000b821e41844d8070f", response.Code, new Uri("http://localhost:5000/callback"), verifier));
                    }
                    catch (Exception e)
                    {
                        Logger.Debug(e.Message);
                        throw;
                    }

                    var authenticator = new PKCEAuthenticator("857519a171be4000b821e41844d8070f", initialResponse);

                    _spotifyConfig = SpotifyClientConfig.CreateDefault().WithAuthenticator(authenticator);
                    _spotifyClient = new SpotifyClient(_spotifyConfig);
                };

                server.Start().GetAwaiter().GetResult();

                var loginRequest = new LoginRequest(server.BaseUri, "857519a171be4000b821e41844d8070f", LoginRequest.ResponseType.Code)
                    {
                        CodeChallengeMethod = "S256",
                        CodeChallenge = challenge,
                        Scope = new[] { Scopes.UserReadCurrentlyPlaying, Scopes.UserReadPlaybackState, Scopes.UserReadPlaybackPosition, Scopes.UserModifyPlaybackState }
                    };

                BrowserUtil.Open(loginRequest.ToUri());
                return;
            }

            Logger.Debug("Connecting to Spotify through own application.");

            if (UseProxy)
            {
                UpdateSpotifyHttpClient();
            }

            if (_spotifyHttpClient == null)
            {
                _spotifyConfig = SpotifyClientConfig
                    .CreateDefault()
                    .WithAuthenticator(new ClientCredentialsAuthenticator(ClientId, ClientSecret));
            }
            else
            {
                _spotifyConfig = SpotifyClientConfig
                    .CreateDefault()
                    .WithAuthenticator(new ClientCredentialsAuthenticator(ClientId, ClientSecret))
                    .WithHTTPClient(_spotifyHttpClient);
            }

            _spotifyClient = new SpotifyClient(_spotifyConfig);
        }

        private void UpdateSpotifyHttpClient()
        {
            if (string.IsNullOrEmpty(ProxyHost) || ProxyPort == 0
            || string.IsNullOrEmpty(ProxyUserName) || string.IsNullOrEmpty(ProxyPassword))
            {
                return;
            }

            _spotifyHttpClient = new NetHttpClient(new ProxyConfig(ProxyHost, ProxyPort)
                {
                    User = ProxyUserName,
                    Password = ProxyPassword
                });
        }

        private void UpdateProxy()
        {
            Logger.Debug("Updating proxy configuration");

            UpdateSpotifyHttpClient();
            if (_spotifyConfig is null)
                return;
            _spotifyClient = new SpotifyClient(_spotifyConfig);
        }

        private async Task<CurrentlyPlayingContext> GetPlayback()
        {
            try
            {
                /* Hasnt logged in yet */
                if (_spotifyClient is null)
                {
                    return null;
                }
                var playback = await _spotifyClient.Player.GetCurrentPlayback();

                return playback;
            }
            catch (Exception e)
            {
                Logger.Error(e);

                // Currently commented to see if it is still necessary
                // When there is an error, for unknown reasons, the client sometimes stops working properly
                // i.e, it is unable to refresh the token properly. Recreate the client prevents this issue.
                // await Task.Delay(TimeSpan.FromSeconds(5));
                // _spotifyApi = CreateSpotifyClient(_spotifyApi.AccessToken, _spotifyApi.TokenType);
                return null;
            }
        }

        private async Task NotifyTrackUpdate(FullTrack track)
        {
            // need name because local files dont have ids
            if (track.Id == _currentItemId && track.Name == _currentTrackName)
            {
                return;
            }

            _currentItemId = track.Id;
            _currentTrackName = track.Name;

            string albumArtUrl = track.Album?.Images.FirstOrDefault()?.Url;
            Image albumArtImage = null;
            if (albumArtUrl != null)
            {
                albumArtImage = await GetAlbumArt(new Uri(albumArtUrl));
            }

            string album = track.Album?.Name;
            string trackName = track.Name;
            string artists = string.Join(", ", track.Artists?.Select(a => a?.Name));

            var trackLength = TimeSpan.FromMilliseconds(track.DurationMs);

            var trackUpdateInfo = new TrackInfoChangedEventArgs
            {
                Artist = artists,
                TrackName = trackName,
                AlbumArt = albumArtImage,
                Album = album,
                TrackLength = trackLength,
            };

            TrackInfoChanged?.Invoke(this, trackUpdateInfo);
        }

        private async Task NotifyEpisodeUpdate(FullEpisode episode)
        {
            if (episode.Id == _currentItemId)
            {
                return;
            }

            _currentItemId = episode.Id;
            _currentTrackName = "";

            string albumArtUrl = episode.Images.FirstOrDefault()?.Url;
            Image albumArtImage = null;
            if (albumArtUrl != null)
            {
                albumArtImage = await GetAlbumArt(new Uri(albumArtUrl));
            }

            string album = "";
            string trackName = episode.Name;
            string artists = string.Join(", ", episode.Show.Publisher);

            var trackLength = TimeSpan.FromMilliseconds(episode.DurationMs);

            var trackUpdateInfo = new TrackInfoChangedEventArgs
            {
                Artist = artists,
                TrackName = trackName,
                AlbumArt = albumArtImage,
                Album = album,
                TrackLength = trackLength,
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
                    _checkSpotifyTimer.Interval = 1000;
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

                if (playback.Item.Type == ItemType.Track)
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

        private void OnSettingChanged(string settingName)
        {
            SettingChanged?.Invoke(this, new SettingChangedEventArgs(settingName));
        }
        private async Task LogPlayerCommandIfFailed(Func<Task<bool>> command, [CallerMemberName] string caller = null)
        {
            var hasError = await command();
            if (hasError)
            {
                Logger.Warn($"Error with player command [{caller}].");
            }
        }
    }
}
