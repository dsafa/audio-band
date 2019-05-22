namespace AudioBand.ViewModels
{
    /// <summary>
    /// Container for all the view models.
    /// </summary>
    public class ViewModelContainer : IViewModelContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelContainer"/> class.
        /// </summary>
        /// <param name="audioBandViewModel">Audioband view model.</param>
        /// <param name="albumArtPopupViewModel">Album art popup view model.</param>
        /// <param name="albumArtViewModel">Album art view model.</param>
        /// <param name="customLabelsViewModel">Custom labels view model.</param>
        /// <param name="nextButtonViewModel">Next button view model.</param>
        /// <param name="playPauseButtonVm">Play pause button view model.</param>
        /// <param name="repeatModeButtonViewModel">Repeat mode button view model.</param>
        /// <param name="previousButtonViewModel">Previous button view model.</param>
        /// <param name="progressBarViewModel">Progress bar view model.</param>
        /// <param name="audioSourceSettingsViewModel">Audio source settings view model.</param>
        public ViewModelContainer(
            AudioBandViewModel audioBandViewModel,
            AlbumArtPopupViewModel albumArtPopupViewModel,
            AlbumArtViewModel albumArtViewModel,
            CustomLabelsViewModel customLabelsViewModel,
            NextButtonViewModel nextButtonViewModel,
            PlayPauseButtonVM playPauseButtonVm,
            RepeatModeButtonViewModel repeatModeButtonViewModel,
            PreviousButtonViewModel previousButtonViewModel,
            ProgressBarViewModel progressBarViewModel,
            AudioSourceSettingsViewModel audioSourceSettingsViewModel)
        {
            AudioBandViewModel = audioBandViewModel;
            AlbumArtPopupViewModel = albumArtPopupViewModel;
            AlbumArtViewModel = albumArtViewModel;
            CustomLabelsViewModel = customLabelsViewModel;
            NextButtonViewModel = nextButtonViewModel;
            PlayPauseButtonVM = playPauseButtonVm;
            RepeatModeButtonViewModel = repeatModeButtonViewModel;
            PreviousButtonViewModel = previousButtonViewModel;
            ProgressBarViewModel = progressBarViewModel;
            AudioSourceSettingsViewModel = audioSourceSettingsViewModel;
        }

        /// <inheritdoc />
        public AudioBandViewModel AudioBandViewModel { get; }

        /// <inheritdoc />
        public AlbumArtPopupViewModel AlbumArtPopupViewModel { get; }

        /// <inheritdoc />
        public AlbumArtViewModel AlbumArtViewModel { get; }

        /// <inheritdoc />
        public CustomLabelsViewModel CustomLabelsViewModel { get; }

        /// <inheritdoc />
        public NextButtonViewModel NextButtonViewModel { get; }

        /// <inheritdoc />
        public PlayPauseButtonVM PlayPauseButtonVM { get; }

        /// <inheritdoc />
        public PreviousButtonViewModel PreviousButtonViewModel { get; }

        /// <inheritdoc />
        public RepeatModeButtonViewModel RepeatModeButtonViewModel { get; }

        /// <inheritdoc />
        public ProgressBarViewModel ProgressBarViewModel { get; }

        /// <inheritdoc />
        public AudioSourceSettingsViewModel AudioSourceSettingsViewModel { get; }
    }
}
