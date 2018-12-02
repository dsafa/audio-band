using AudioBand.AudioSource;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace MusicBeeAudioSource
{
    public class AudioSource : IAudioSource
    {
        private MusicBeeIPC _ipc;
        private Timer _checkMusicBeeTimer;
        private bool _isPlaying;
        private string _currentId;
        private static readonly string[] TimeFormats = new string[] { @"m\:s", @"h\:m\:s" };

        public string Name => "Music Bee";
        public IAudioSourceLogger Logger { get; set; }
        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;
        public event EventHandler TrackPlaying;
        public event EventHandler TrackPaused;
        public event EventHandler<TimeSpan> TrackProgressChanged;
        public event EventHandler<SettingChangedEventArgs> SettingChanged;

        public AudioSource()
        {
            _ipc = new MusicBeeIPC();
            _checkMusicBeeTimer = new Timer(100)
            {
                AutoReset = false,
                Enabled = false
            };
            _checkMusicBeeTimer.Elapsed += CheckMusicBee;
        }

        public Task ActivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _checkMusicBeeTimer.Start();
            return Task.CompletedTask;
        }

        public Task DeactivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _checkMusicBeeTimer.Stop();
            return Task.CompletedTask;
        }

        public Task NextTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _ipc.NextTrack();
            return Task.CompletedTask;
        }

        public Task PauseTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _ipc.Pause();
            return Task.CompletedTask;
        }

        public Task PlayTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _ipc.Play();
            return Task.CompletedTask;
        }

        public Task PreviousTrackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _ipc.PreviousTrack();
            return Task.CompletedTask;
        }

        private void CheckMusicBee(object sender, ElapsedEventArgs eventArgs)
        {
            try
            {
                if (Process.GetProcessesByName("MusicBee").Length == 0)
                {
                    return;
                }

                // The ipc plugin does not load right away
                if (string.IsNullOrEmpty(_ipc.GetPluginVersionStr()))
                {
                    return;
                }

                NotifyState();
                NotifyTrackChange();

                var time = TimeSpan.FromMilliseconds(_ipc.GetPosition());
                TrackProgressChanged?.Invoke(this, time);
            }
            catch (Exception e)
            {
                Logger.Debug(e);
            }
            finally
            {
                _checkMusicBeeTimer.Enabled = true;
            }
        }

        private void NotifyState()
        {
            var isPlaying = _ipc.GetPlayState().HasFlag(MusicBeeIPC.PlayState.Playing);
            if (isPlaying == _isPlaying)
            {
                return;
            }

            if (isPlaying)
            {
                TrackPlaying?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                TrackPaused?.Invoke(this, EventArgs.Empty);
            }

            _isPlaying = isPlaying;
        }

        private void NotifyTrackChange()
        {
            var track = _ipc.GetFileTag(MusicBeeIPC.MetaData.TrackTitle);
            var artist = _ipc.GetFileTag(MusicBeeIPC.MetaData.Artist);

            var id = artist + track;
            if (_currentId == id)
            {
                return;
            }

            _currentId = id;

            var album = _ipc.GetFileTag(MusicBeeIPC.MetaData.Album);

            if (!TimeSpan.TryParseExact(_ipc.GetFileProperty(MusicBeeIPC.FileProperty.Duration), TimeFormats, null, out var trackLength))
            {
                Logger.Warn($"Unable to parse track length: {_ipc.GetFileProperty(MusicBeeIPC.FileProperty.Duration)}");
            }

            Image albumArt = null;
            var bytes = Convert.FromBase64String(_ipc.GetArtwork());
            using (var ms = new MemoryStream(bytes))
            {
                albumArt = new Bitmap(ms);
            }

            TrackInfoChanged?.Invoke(this, new TrackInfoChangedEventArgs
            {
                Album = album,
                Artist = artist,
                AlbumArt = albumArt,
                TrackLength = trackLength,
                TrackName = track
            });
        }
    }
}
