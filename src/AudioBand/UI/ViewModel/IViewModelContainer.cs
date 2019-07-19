namespace AudioBand.UI
{
    /// <summary>
    /// Interface for a settings window.
    /// </summary>
    public interface IViewModelContainer
    {
        /// <summary>
        /// Gets the viewmodel for audioband toolbar.
        /// </summary>
        GeneralSettingsViewModel GeneralSettingsViewModel { get; }

        /// <summary>
        /// Gets the viewmodel for the album art popup.
        /// </summary>
        AlbumArtPopupViewModel AlbumArtPopupViewModel { get; }

        /// <summary>
        /// Gets the viewmodel for the album art.
        /// </summary>
        AlbumArtViewModel AlbumArtViewModel { get; }

        /// <summary>
        /// Gets the viewmodel for custom labels collection.
        /// </summary>
        CustomLabelsViewModel CustomLabelsViewModel { get; }

        /// <summary>
        /// Gets the view model for the next button.
        /// </summary>
        NextButtonViewModel NextButtonViewModel { get; }

        /// <summary>
        /// Gets the view model for the play/pause button.
        /// </summary>
        PlayPauseButtonViewModel PlayPauseButtonViewModel { get; }

        /// <summary>
        /// Gets the view model for the previous button.
        /// </summary>
        PreviousButtonViewModel PreviousButtonViewModel { get; }

        /// <summary>
        /// Gets the view model for the repeat mode button.
        /// </summary>
        RepeatModeButtonViewModel RepeatModeButtonViewModel { get; }

        /// <summary>
        /// Gets the view model for the shuffle mode button.
        /// </summary>
        ShuffleModeButtonViewModel ShuffleModeButtonViewModel { get; }

        /// <summary>
        /// Gets the view model for the progress bar.
        /// </summary>
        ProgressBarViewModel ProgressBarViewModel { get; }

        /// <summary>
        /// Gets the collection for view models for the audio source settings.
        /// </summary>
        AudioSourceSettingsViewModel AudioSourceSettingsViewModel { get; }
    }
}
