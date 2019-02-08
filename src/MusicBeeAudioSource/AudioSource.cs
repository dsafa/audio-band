using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using AudioBand.AudioSource;
using Timer = System.Timers.Timer;

namespace MusicBeeAudioSource
{
    public class AudioSource : IAudioSource
    {
        private static readonly string[] TimeFormats = new string[] { @"m\:s", @"h\:m\:s" };
        private MusicBeeIPC _ipc;
        private Timer _checkMusicBeeTimer;
        private bool _isPlaying;
        private string _currentId;
        private int _volume;

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

        public event EventHandler<TrackInfoChangedEventArgs> TrackInfoChanged;

        public event EventHandler TrackPlaying;

        public event EventHandler TrackPaused;

        public event EventHandler<TimeSpan> TrackProgressChanged;

#pragma warning disable 00067 // The event is never used
        public event EventHandler<SettingChangedEventArgs> SettingChanged;
#pragma warning restore 00067 // The event is never used

        public event EventHandler<float> VolumeChanged;

        public string Name => "Music Bee";

        public IAudioSourceLogger Logger { get; set; }

        public Task ActivateAsync()
        {
            _checkMusicBeeTimer.Start();
            return Task.CompletedTask;
        }

        public Task DeactivateAsync()
        {
            _checkMusicBeeTimer.Stop();
            return Task.CompletedTask;
        }

        public Task NextTrackAsync()
        {
            _ipc.NextTrack();
            return Task.CompletedTask;
        }

        public Task PauseTrackAsync()
        {
            _ipc.Pause();
            return Task.CompletedTask;
        }

        public Task PlayTrackAsync()
        {
            _ipc.Play();
            return Task.CompletedTask;
        }

        public Task PreviousTrackAsync()
        {
            _ipc.PreviousTrack();
            return Task.CompletedTask;
        }

        public Task SetVolumeAsync(float newVolume)
        {
            _ipc.SetVolume((int)(newVolume * 100));
            return Task.CompletedTask;
        }

        public Task SetPlaybackProgress(TimeSpan newProgress)
        {
            _ipc.SetPosition((int)newProgress.TotalMilliseconds);
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
                NotifyVolume();

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

        private void NotifyVolume()
        {
            var volume = _ipc.GetVolume();

            if (volume == _volume)
            {
                return;
            }

            _volume = volume;
            VolumeChanged?.Invoke(this, _volume / 100f);
        }
    }
}
