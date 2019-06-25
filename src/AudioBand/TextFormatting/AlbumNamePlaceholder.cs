using System.Collections.Generic;
using AudioBand.AudioSource;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// A <see cref="TextPlaceholder"/> with the value based on the album name.
    /// </summary>
    public class AlbumNamePlaceholder : TextPlaceholder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumNamePlaceholder"/> class.
        /// </summary>
        /// <param name="parameters">The placeholder parameters.</param>
        /// <param name="audioSession">The audio session.</param>
        public AlbumNamePlaceholder(IEnumerable<TextPlaceholderParameter> parameters, IAudioSession audioSession)
            : base(parameters, audioSession)
        {
            AddSessionPropertyFilter(nameof(IAudioSession.AlbumName));
        }

        /// <inheritdoc />
        public override string GetText()
        {
            return Session.AlbumName;
        }

        /// <inheritdoc />
        protected override void OnAudioSessionPropertyChanged(string propertyName)
        {
            RaiseTextChanged();
        }
    }
}
