using System;

namespace SonosAudioSource
{
	public class TrackInfo
	{
		public TrackInfo(string artist, string album, string title, string albumArtUri, TimeSpan trackLength, TimeSpan trackProgress)
		{
			Artist = artist;
			Album = album;
			Title = title;
			AlbumArtUri = albumArtUri;
			TrackLength = trackLength;
			TrackProgress = trackProgress;
		}

		public string Album { get; private set; }

		public string Artist { get; private set; }

		public string Title { get; private set; }

		public string AlbumArtUri { get; private set; }

		public TimeSpan TrackLength { get; private set; }

		public TimeSpan TrackProgress { get; private set; }
	}
}
