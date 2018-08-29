using AudioBand.AudioSource;
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

        private Timer _checkSpotifyTimer;
        private IAudioSourceLogger _logger;
        private readonly SpotifyControls _spotifyControls = new SpotifyControls();
        private const string NotPlayingTitle = "Spotify"; // Window title when no song is playing

        private string _currentSong;
        private string _currentArtist;
        private bool _currentIsPlaying;
        private bool _spotifyRunning = true;

        public Task ActivateAsync(IAudioSourceContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger = context.Logger;

            _checkSpotifyTimer = new Timer
            {
                Interval = 100,
                AutoReset = false
            };
            _checkSpotifyTimer.Elapsed += CheckSpotifyTimerOnElapsed;
            _checkSpotifyTimer.Start();

            return Task.CompletedTask;
        }

        private void CheckSpotifyTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                var spotifyTitle = _spotifyControls.GetSpotifyTitle();

                // Spotify not open
                if (string.IsNullOrEmpty(spotifyTitle))
                {
                    // Already blanked
                    if (!_spotifyRunning)
                    {
                        return;
                    }

                    SetBlankState();

                    _spotifyRunning = false;
                    return;
                }

                _spotifyRunning = true;

                if (spotifyTitle == NotPlayingTitle)
                {
                    // already paused
                    if (!_currentIsPlaying)
                    {
                        return;
                    }

                    TrackPaused?.Invoke(this, EventArgs.Empty);
                    _currentIsPlaying = false;
                    return;
                }

                ExtractTitle(spotifyTitle);
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

        private void SetBlankState()
        {
            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs());
            TrackPaused?.Invoke(this, EventArgs.Empty);
            TrackProgressChanged?.Invoke(this, 0);

            _currentIsPlaying = false;
            _currentArtist = null;
            _currentSong = null;
        }

        private void ExtractTitle(string title)
        {
            // Spotify title is in the form of <artist> - <title>
            var parts = title.Split(new string[]{" - "}, StringSplitOptions.None);
            if (parts.Length != 2)
            {
                _logger.Warn($"Spotify title invalid: {title}");
                return;
            }

            var artist = parts[0];
            var song = parts[1];

            if (artist != _currentArtist || song != _currentSong)
            {
                TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs{TrackName = song, Artist = artist});
            }

            _currentArtist = artist;
            _currentSong = song;

            // Already playing
            if (_currentIsPlaying)
            {
                return;
            }

            TrackPlaying?.Invoke(this, EventArgs.Empty);
            _currentIsPlaying = true;
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
