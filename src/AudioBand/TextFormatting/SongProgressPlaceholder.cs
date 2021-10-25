using AudioBand.AudioSource;
using AudioBand.Extensions;
using System.Collections.Generic;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A <see cref="TextPlaceholder"/> with a value based on the current song progress.
    /// </summary>
    public class SongProgressPlaceholder : TextPlaceholder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SongProgressPlaceholder"/> class.
        /// </summary>
        /// <param name="parameters">The placeholder parameters.</param>
        /// <param name="session">The audio session.</param>
        public SongProgressPlaceholder(IEnumerable<TextPlaceholderParameter> parameters, IAudioSession session)
            : base(parameters, session)
        {
            AddSessionPropertyFilter(nameof(IAudioSession.SongProgress));
        }

        /// <inheritdoc />
        public override string GetText()
        {
            return Session.SongProgress.Format();
        }

        /// <inheritdoc />
        protected override void OnAudioSessionPropertyChanged(string propertyName)
        {
            RaiseTextChanged();
        }
    }
}
