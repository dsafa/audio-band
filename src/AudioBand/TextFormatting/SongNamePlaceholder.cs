using System.Collections.Generic;
using AudioBand.AudioSource;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A <see cref="TextPlaceholder"/> with a value based on the current song name.
    /// </summary>
    public class SongNamePlaceholder : TextPlaceholder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SongNamePlaceholder"/> class.
        /// </summary>
        /// <param name="parameters">The placeholder parameters.</param>
        /// <param name="session">The audio session.</param>
        public SongNamePlaceholder(IEnumerable<TextPlaceholderParameter> parameters, IAudioSession session)
            : base(parameters, session)
        {
            AddSessionPropertyFilter(nameof(IAudioSession.SongName));
        }

        /// <inheritdoc />
        public override string GetText()
        {
            return Session.SongName;
        }

        /// <inheritdoc />
        protected override void OnAudioSessionPropertyChanged(string propertyName)
        {
            RaiseTextChanged();
        }
    }
}
