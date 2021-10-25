using AudioBand.AudioSource;
using AudioBand.Extensions;
using System;
using System.Collections.Generic;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A <see cref="TextPlaceholder"/> with the value of the remaining time in the song.
    /// </summary>
    public class RemainingTimePlaceholder : TextPlaceholder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemainingTimePlaceholder"/> class.
        /// </summary>
        /// <param name="parameters">The placeholder parameters.</param>
        /// <param name="audioSession">The audio session.</param>
        public RemainingTimePlaceholder(IEnumerable<TextPlaceholderParameter> parameters, IAudioSession audioSession)
            : base(parameters, audioSession)
        {
            AddSessionPropertyFilter(nameof(IAudioSession.SongProgress));
            AddSessionPropertyFilter(nameof(IAudioSession.SongLength));
        }

        /// <inheritdoc />
        public override string GetText()
        {
            if (Session.SongLength < Session.SongProgress)
            {
                return TimeSpan.Zero.Format();
            }

            return (Session.SongLength - Session.SongProgress).Format();
        }

        /// <inheritdoc />
        protected override void OnAudioSessionPropertyChanged(string propertyName)
        {
            RaiseTextChanged();
        }
    }
}
