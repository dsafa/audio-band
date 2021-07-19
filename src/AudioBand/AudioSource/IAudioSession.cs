using System;
using System.ComponentModel;
using System.Drawing;

namespace AudioBand.AudioSource
{
    /// <summary>
    /// Encapsulates the the audio session.
    /// </summary>
    public interface IAudioSession : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the current audio source.
        /// </summary>
        IInternalAudioSource CurrentAudioSource { get; set; }

        /// <summary>
        /// Gets a value indicating whether the audio source is currently playing.
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Gets the current song artist.
        /// </summary>
        string SongArtist { get; }

        /// <summary>
        /// Gets the current song name.
        /// </summary>
        string SongName { get; }

        /// <summary>
        /// Gets the current album name.
        /// </summary>
        string AlbumName { get; }

        /// <summary>
        /// Gets the current album.
        /// </summary>
        Image AlbumArt { get; }

        /// <summary>
        /// Gets the current song progress.
        /// </summary>
        TimeSpan SongProgress { get; }

        /// <summary>
        /// Gets the current song length.
        /// </summary>
        TimeSpan SongLength { get; }

        /// <summary>
        /// Gets a value indicating whether shuffle is currently on.
        /// </summary>
        bool IsShuffleOn { get; }

        /// <summary>
        /// Gets a value indicating what the current volume is.
        /// </summary>
        int Volume { get; }

        /// <summary>
        /// Gets the current repeat mode.
        /// </summary>
        RepeatMode RepeatMode { get; }
    }
}
