using AudioBand.AudioSource;
using System.Collections.Generic;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A <see cref="TextPlaceholder"/> with the value based on the song artist.
    /// </summary>
    public class SongArtistPlaceholder : TextPlaceholder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SongArtistPlaceholder"/> class.
        /// </summary>
        /// <param name="parameters">The placeholder parameter.</param>
        /// <param name="audioSession">The audio session.</param>
        public SongArtistPlaceholder(IEnumerable<TextPlaceholderParameter> parameters, IAudioSession audioSession)
            : base(parameters, audioSession)
        {
            AddSessionPropertyFilter(nameof(IAudioSession.SongArtist));
        }

        /// <inheritdoc />
        public override string GetText()
        {
            return Session.SongArtist;
        }

        /// <inheritdoc />
        protected override void OnAudioSessionPropertyChanged(string propertyName)
        {
            RaiseTextChanged();
        }
    }
}
