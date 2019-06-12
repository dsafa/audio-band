using System.Collections.Generic;
using AudioBand.AudioSource;
using AudioBand.Extensions;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A <see cref="TextPlaceholder"/> with the value based on the songs total time.
    /// </summary>
    public class SongLengthPlaceholder : TextPlaceholder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SongLengthPlaceholder"/> class.
        /// </summary>
        /// <param name="parameters">The placeholder parameters.</param>
        /// <param name="audioSession">The audio session.</param>
        public SongLengthPlaceholder(IEnumerable<TextPlaceholderParameter> parameters, IAudioSession audioSession)
            : base(parameters, audioSession)
        {
            AddSessionPropertyFilter(nameof(IAudioSession.SongLength));
        }

        /// <inheritdoc />
        public override string GetText()
        {
            return Session.SongLength.Format();
        }

        /// <inheritdoc />
        protected override void OnAudioSessionPropertyChanged(string propertyName)
        {
            RaiseTextChanged();
        }
    }
}
