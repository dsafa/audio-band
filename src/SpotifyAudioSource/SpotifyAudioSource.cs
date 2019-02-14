using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using AudioBand.AudioSource;
using SpotifyAPI;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using Image = System.Drawing.Image;
using Timer = System.Timers.Timer;

namespace SpotifyAudioSource
{
    public class SpotifyAudioSource : IAudioSource
    {
        private readonly SpotifyControls _spotifyControls = new SpotifyControls();
        private readonly Timer _checkSpotifyTimer = new Timer(1000);
        private readonly ProxyConfig _proxyConfig = new ProxyConfig();
        private HttpClient _httpClient = new HttpClient();
        private SpotifyWebAPI _spotifyApi = new SpotifyWebAPI();
        private string _currentTrackId;
        private string _currentTrackName;
        private bool _currentIsPlaying;
        private int _currentProgress;
        private int _currentVolumePercent;
        private bool _currentShuffle;
        private RepeatState _currentRepeat;
        private string _clientSecret;
        private string _clientId;
        private string _refreshToken;
        private uint _localPort = 80;
        private AuthorizationCodeAuth _auth;
        private bool _isActive;
        private bool _useProxy;

        public SpotifyAudioSource()
        {
            _checkSpotifyTimer.AutoReset = false;
            _checkSpotifyTimer.Elapsed += CheckSpotifyTimerOnElapsed;
        }

        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        public event EventHandler<TimeSpan> TrackProgressChanged;

        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        public event EventHandler<float> VolumeChanged;

        public event EventHandler<bool> ShuffleChanged;

        public event EventHandler<RepeatMode> RepeatModeChanged;

        public event EventHandler<bool> IsPlayingChanged;

        public string Name { get; } = "Spotify";

        public IAudioSourceLogger Logger { get; set; }

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

        [AudioSourceSetting("Callback Port", Priority = 10)]
        public uint LocalPort
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

        [AudioSourceSetting("Use Proxy")]
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

        [AudioSourceSetting("Proxy Host")]
        public string ProxyHost
        {
            get => _proxyConfig.Host;
            set
            {
                if (value == _proxyConfig.Host)
                {
                    return;
                }

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
                if (value == _proxyConfig.Port)
                {
                    return;
                }

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
                if (value == _proxyConfig.Username)
                {
                    return;
                }

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
                if (value == _proxyConfig.Password)
                {
                    return;
                }

                _proxyConfig.Password = value;
                UpdateProxy();
            }
        }

        public Task ActivateAsync()
        {
            _checkSpotifyTimer.Start();

            _isActive = true;
            Authorize();
            return Task.CompletedTask;
        }

        public Task DeactivateAsync()
        {
            _isActive = false;

            _checkSpotifyTimer.Stop();
            _currentTrackId = null;

            return Task.CompletedTask;
        }

        public Task PlayTrackAsync()
        {
            _spotifyControls.Play();
            return Task.CompletedTask;
        }

        public Task PauseTrackAsync()
        {
            _spotifyControls.Pause();
            return Task.CompletedTask;
        }

        public Task PreviousTrackAsync()
        {
            _spotifyControls.Previous();
            return Task.CompletedTask;
        }

        public Task NextTrackAsync()
        {
            _spotifyControls.Next();
            return Task.CompletedTask;
        }

        public async Task SetVolumeAsync(float newVolume)
        {
            await _spotifyApi.SetVolumeAsync((int)(newVolume * 100));
        }

        public async Task SetPlaybackProgressAsync(TimeSpan newProgress)
        {
            await _spotifyApi.SeekPlaybackAsync((int)newProgress.TotalMilliseconds);
        }

        public async Task SetShuffleAsync(bool shuffleOn)
        {
            await _spotifyApi.SetShuffleAsync(shuffleOn);
        }

        public async Task SetRepeatModeAsync(RepeatMode newRepeatMode)
        {
            await _spotifyApi.SetRepeatModeAsync(ToRepeatState(newRepeatMode));
        }

        private static RepeatMode ToRepeatMode(RepeatState state)
        {
            switch (state)
            {
                case RepeatState.Context:
                    return RepeatMode.RepeatContext;
                case RepeatState.Off:
                    return RepeatMode.Off;
                case RepeatState.Track:
                    return RepeatMode.RepeatTrack;
                default:
                    throw new InvalidOperationException($"No case for {state}");
            }
        }

        private static RepeatState ToRepeatState(RepeatMode mode)
        {
            switch (mode)
            {
                case RepeatMode.Off:
                    return RepeatState.Off;
                case RepeatMode.RepeatContext:
                    return RepeatState.Context;
                case RepeatMode.RepeatTrack:
                    return RepeatState.Track;
                default:
                    throw new InvalidOperationException($"No case for {mode}");
            }
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
            _auth = new AuthorizationCodeAuth(ClientId, ClientSecret, url, url, Scope.UserModifyPlaybackState | Scope.UserReadPlaybackState | Scope.UserReadCurrentlyPlaying);

            if (string.IsNullOrEmpty(RefreshToken))
            {
                _auth.Start();
                _auth.AuthReceived += OnAuthReceived;
                _auth.OpenBrowser();
            }
            else
            {
                Logger.Debug("Reusing old refresh token");
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                RefreshAccessToken(); // fire and forget
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
            UpdateAccessToken(token);
        }

        private void UpdateProxy()
        {
            if (!UseProxy)
            {
                return;
            }

            Logger.Debug("Updating proxy configuration");

            _spotifyApi = new SpotifyWebAPI(_proxyConfig)
            {
                AccessToken = _spotifyApi.AccessToken,
                TokenType = _spotifyApi.TokenType,
            };

            _httpClient = new HttpClient(new HttpClientHandler { Proxy = _proxyConfig.CreateWebProxy(), UseProxy = true });
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
                        await RefreshAccessToken();
                        return null;
                    }
                }

                return playback;
            }
            catch (Exception e)
            {
                Logger.Error(e);

                // https://github.com/JohnnyCrazy/SpotifyAPI-NET/issues/303
                await Task.Delay(TimeSpan.FromSeconds(5));
                _spotifyApi = new SpotifyWebAPI(UseProxy ? _proxyConfig : null)
                {
                    AccessToken = _spotifyApi.AccessToken,
                    TokenType = _spotifyApi.TokenType
                };
                return null;
            }
        }

        private async Task NotifyTrackUpdate(FullTrack track)
        {
            // need name because local files dont have ids
            if (track.Id == _currentTrackId && track.Name == _currentTrackName)
            {
                return;
            }

            _currentTrackId = track.Id;
            _currentTrackName = track.Name;

            string albumArtUrl = track.Album?.Images.FirstOrDefault()?.Url;
            Image albumArtImage = null;
            if (albumArtUrl != null)
            {
                albumArtImage = await GetAlbumArt(new Uri(albumArtUrl));
            }

            string album = track.Album?.Name;
            string trackName = track.Name;
            string artist = track.Artists?.FirstOrDefault()?.Name;
            var trackLength = TimeSpan.FromMilliseconds(track.DurationMs);

            var trackUpdateInfo = new TrackInfoChangedEventArgs
            {
                Artist = artist,
                TrackName = trackName,
                AlbumArt = albumArtImage,
                Album = album,
                TrackLength = trackLength
            };

            TrackInfoChanged?.Invoke(this, trackUpdateInfo);
        }

        private void NotifyPlayState(PlaybackContext context)
        {
            bool isPlaying = context.IsPlaying;
            if (isPlaying == _currentIsPlaying)
            {
                return;
            }

            _currentIsPlaying = isPlaying;
            IsPlayingChanged?.Invoke(this, _currentIsPlaying);
        }

        private void NotifyTrackProgress(PlaybackContext context)
        {
            int progress = context.ProgressMs;
            if (progress == _currentProgress)
            {
                return;
            }

            _currentProgress = progress;
            TrackProgressChanged?.Invoke(this, TimeSpan.FromMilliseconds(_currentProgress));
        }

        private void NotifyVolume(PlaybackContext context)
        {
            if (context.Device == null)
            {
                return;
            }

            int vol = context.Device.VolumePercent;
            if (vol == _currentVolumePercent)
            {
                return;
            }

            _currentVolumePercent = vol;
            VolumeChanged?.Invoke(this, _currentVolumePercent / 100f);
        }

        private void NotifyShuffle(PlaybackContext context)
        {
            bool shuffle = context.ShuffleState;
            if (shuffle == _currentShuffle)
            {
                return;
            }

            _currentShuffle = shuffle;
            ShuffleChanged?.Invoke(this, _currentShuffle);
        }

        private void NotifyRepeat(PlaybackContext context)
        {
            RepeatState repeat = context.RepeatState;
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

        // We will check the spotify window title for changes and only call the api when it changes.
        // The spotify window title is a fixed value when it is paused and the artist - song when playing
        private async void CheckSpotifyTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                if (!_isActive || string.IsNullOrEmpty(_spotifyApi.AccessToken))
                {
                    return;
                }

                var currentSpotifyWindowTitle = _spotifyControls.GetSpotifyWindowTitle();
                if (string.IsNullOrEmpty(currentSpotifyWindowTitle))
                {
                    // reduce number of calls when paused
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

                await NotifyTrackUpdate(playback.Item);
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

        private async Task RefreshAccessToken()
        {
            try
            {
                Logger.Debug("Getting new access token");

                var token = await _auth.RefreshToken(RefreshToken);
                if (token.HasError())
                {
                    Logger.Warn($"Error getting new token. Requesting new refresh token. Error: {token.Error}|{token.ErrorDescription}");
                    RefreshToken = null;
                    Authorize();
                    return;
                }

                UpdateAccessToken(token);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private void UpdateAccessToken(Token token)
        {
            if (_spotifyApi == null)
            {
                return;
            }

            _spotifyApi = new SpotifyWebAPI(UseProxy ? _proxyConfig : null)
            {
                AccessToken = token.AccessToken,
                TokenType = token.TokenType
            };

            if (!string.IsNullOrEmpty(token.RefreshToken))
            {
                RefreshToken = token.RefreshToken;
            }

            var expiresIn = TimeSpan.FromSeconds(token.ExpiresIn);
            Logger.Debug($"Received new access token. Expires in: {expiresIn} (At {DateTime.Now + expiresIn}). Token: {token.AccessToken.Substring(0, 20)}...");
        }
    }
}
