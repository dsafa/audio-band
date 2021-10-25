using AudioBand.AudioSource;
using System.Collections.Generic;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A <see cref="TextPlaceholder"/> with a value based on the current song name.
    /// </summary>
    public class VolumePlaceholder : TextPlaceholder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolumePlaceholder"/> class.
        /// </summary>
        /// <param name="parameters">The placeholder parameters.</param>
        /// <param name="session">The audio session.</param>
        public VolumePlaceholder(IEnumerable<TextPlaceholderParameter> parameters, IAudioSession session)
            : base(parameters, session)
        {
            AddSessionPropertyFilter(nameof(IAudioSession.Volume));
        }

        /// <inheritdoc />
        public override string GetText()
        {
            return Session.Volume.ToString();
        }

        /// <inheritdoc />
        protected override void OnAudioSessionPropertyChanged(string propertyName)
        {
            RaiseTextChanged();
        }
    }
}
