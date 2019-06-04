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
            { "artist", parameters => new SongArtistPlaceholder(parameters) },
            { "song", parameters => new SongNamePlaceholder(parameters) },
            { "album", parameters => new AlbumNamePlaceholder(parameters) },
            { "time", parameters => new SongProgressPlaceholder(parameters) },
            { "length", parameters => new SongLengthPlaceholder(parameters) },
        };

        private delegate TextPlaceholder PlaceholderFactoryFunc(IEnumerable<TextPlaceholderParameter> parameters);

        /// <summary>
        /// Gets all available tags for text placeholders.
        /// </summary>
        public static IEnumerable<string> Tags { get; } = PlaceholderMap.Keys;

        /// <summary>
        /// Attempts to create a <see cref="TextPlaceholder"/> based on the given tag.
        /// </summary>
        /// <param name="tag">The tag for the placeholder.</param>
        /// <param name="parameters">The parameters for the placeholder.</param>
        /// <param name="placeholder">The text placeholder.</param>
        /// <returns>True if the tag matches a placeholder.</returns>
        public static bool TryGetPlaceholder(string tag, IEnumerable<TextPlaceholderParameter> parameters, out TextPlaceholder placeholder)
        {
            if (PlaceholderMap.TryGetValue(tag, out var func))
            {
                placeholder = func(parameters);
                return true;
            }

            placeholder = null;
            return false;
        }
    }
}
