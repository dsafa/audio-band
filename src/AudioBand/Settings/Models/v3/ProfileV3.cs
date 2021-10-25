#pragma warning disable SA1300
using AudioBand.Models;
using System.Collections.Generic;

namespace AudioBand.Settings.Models.V3
{
    /// <summary>
    /// Represents a profile for v3 of settings.
    /// </summary>
    public class ProfileV3
    {
        /// <summary>
        /// Gets or sets the audio band settings.
        /// </summary>
        public GeneralSettings GeneralSettings { get; set; }

        /// <summary>
        /// Gets or sets the previous button settings.
        /// </summary>
        public PreviousButton PreviousButtonSettings { get; set; }

        /// <summary>
        /// Gets or sets the play pause button settings.
        /// </summary>
        public PlayPauseButton PlayPauseButtonSettings { get; set; }

        /// <summary>
        /// Gets or sets the next button settings.
        /// </summary>
        public NextButton NextButtonSettings { get; set; }

        /// <summary>
        /// Gets or sets the repeat mode button settings.
        /// </summary>
        public RepeatModeButton RepeatModeButtonSettings { get; set; }

        /// <summary>
        /// Gets or sets the shuffle button settings.
        /// </summary>
        public ShuffleModeButton ShuffleModeButtonSettings { get; set; }

        /// <summary>
        /// Gets or sets the progressbar settings.
        /// </summary>
        public ProgressBar ProgressBarSettings { get; set; }

        /// <summary>
        /// Gets or sets the album art settings.
        /// </summary>
        public AlbumArt AlbumArtSettings { get; set; }

        /// <summary>
        /// Gets or sets the album art popup settings.
        /// </summary>
        public AlbumArtPopup AlbumArtPopupSettings { get; set; }

        /// <summary>
        /// Gets or sets the custom label settings.
        /// </summary>
        public List<CustomLabel> CustomLabelSettings { get; set; }
    }
}
