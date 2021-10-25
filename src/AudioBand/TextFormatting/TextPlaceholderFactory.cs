using AudioBand.AudioSource;
using System.Collections.Generic;

namespace AudioBand.TextFormatting
{
    /// <summary>
    /// Factory class that creates text placeholders.
    /// </summary>
    public static class TextPlaceholderFactory
    {
        private static readonly Dictionary<string, PlaceholderFactoryFunc> PlaceholderMap = new Dictionary<string, PlaceholderFactoryFunc>()
        {
            { "artist", (parameters, session) => new SongArtistPlaceholder(parameters, session) },
            { "song", (parameters, session) => new SongNamePlaceholder(parameters, session) },
            { "album", (parameters, session) => new AlbumNamePlaceholder(parameters, session) },
            { "time", (parameters, session) => new SongProgressPlaceholder(parameters, session) },
            { "length", (parameters, session) => new SongLengthPlaceholder(parameters, session) },
            { "remaining", (parameters, session) => new RemainingTimePlaceholder(parameters, session) },
            { "volume", (parameters, session) => new VolumePlaceholder(parameters, session) },
        };

        private delegate TextPlaceholder PlaceholderFactoryFunc(IEnumerable<TextPlaceholderParameter> parameters, IAudioSession session);

        /// <summary>
        /// Gets all available tags for text placeholders.
        /// </summary>
        public static IEnumerable<string> Tags { get; } = PlaceholderMap.Keys;

        /// <summary>
        /// Attempts to create a <see cref="TextPlaceholder"/> based on the given tag.
        /// </summary>
        /// <param name="tag">The tag for the placeholder.</param>
        /// <param name="parameters">The parameters for the placeholder.</param>
        /// <param name="session">The audio session.</param>
        /// <param name="placeholder">The text placeholder.</param>
        /// <returns>True if the tag matches a placeholder.</returns>
        public static bool TryGetPlaceholder(string tag, IEnumerable<TextPlaceholderParameter> parameters, IAudioSession session, out TextPlaceholder placeholder)
        {
            if (PlaceholderMap.TryGetValue(tag, out var func))
            {
                placeholder = func(parameters, session);
                return true;
            }

            placeholder = null;
            return false;
        }
    }
}
