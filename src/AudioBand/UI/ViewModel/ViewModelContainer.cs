namespace AudioBand.UI
{
    /// <summary>
    /// Container for all the view models.
    /// </summary>
    public class ViewModelContainer : IViewModelContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelContainer"/> class.
        /// </summary>
        /// <param name="globalSettingsViewModel">Global Audioband settings view model.</param>
        /// <param name="generalSettingsViewModel">Audioband view model.</param>
        /// <param name="albumArtPopupViewModel">Album art popup view model.</param>
        /// <param name="albumArtViewModel">Album art view model.</param>
        /// <param name="customLabelsViewModel">Custom labels view model.</param>
        /// <param name="nextButtonViewModel">Next button view model.</param>
        /// <param name="playPauseButtonViewModel">Play pause button view model.</param>
        /// <param name="repeatModeButtonViewModel">Repeat mode button view model.</param>
        /// <param name="shuffleModeButtonViewModel">Shuffle mode button view model.</param>
        /// <param name="previousButtonViewModel">Previous button view model.</param>
        /// <param name="progressBarViewModel">Progress bar view model.</param>
        /// <param name="popupViewModel">Popup view model.</param>
        /// <param name="audioSourceSettingsViewModel">Audio source settings view model.</param>
        public ViewModelContainer(
            GlobalSettingsViewModel globalSettingsViewModel,
            GeneralSettingsViewModel generalSettingsViewModel,
            AlbumArtPopupViewModel albumArtPopupViewModel,
            AlbumArtViewModel albumArtViewModel,
            CustomLabelsViewModel customLabelsViewModel,
            NextButtonViewModel nextButtonViewModel,
            PlayPauseButtonViewModel playPauseButtonViewModel,
            RepeatModeButtonViewModel repeatModeButtonViewModel,
            ShuffleModeButtonViewModel shuffleModeButtonViewModel,
            PreviousButtonViewModel previousButtonViewModel,
            ProgressBarViewModel progressBarViewModel,
            PopupViewModel popupViewModel,
            AudioSourceSettingsViewModel audioSourceSettingsViewModel)
        {
            GlobalSettingsViewModel = globalSettingsViewModel;
            GeneralSettingsViewModel = generalSettingsViewModel;
            AlbumArtPopupViewModel = albumArtPopupViewModel;
            AlbumArtViewModel = albumArtViewModel;
            CustomLabelsViewModel = customLabelsViewModel;
            NextButtonViewModel = nextButtonViewModel;
            PlayPauseButtonViewModel = playPauseButtonViewModel;
            RepeatModeButtonViewModel = repeatModeButtonViewModel;
            ShuffleModeButtonViewModel = shuffleModeButtonViewModel;
            PreviousButtonViewModel = previousButtonViewModel;
            ProgressBarViewModel = progressBarViewModel;
            PopupViewModel = popupViewModel;
            AudioSourceSettingsViewModel = audioSourceSettingsViewModel;
        }

        /// <inheritdoc />
        public GlobalSettingsViewModel GlobalSettingsViewModel { get; }

        /// <inheritdoc />
        public GeneralSettingsViewModel GeneralSettingsViewModel { get; }

        /// <inheritdoc />
        public AlbumArtPopupViewModel AlbumArtPopupViewModel { get; }

        /// <inheritdoc />
        public AlbumArtViewModel AlbumArtViewModel { get; }

        /// <inheritdoc />
        public CustomLabelsViewModel CustomLabelsViewModel { get; }

        /// <inheritdoc />
        public NextButtonViewModel NextButtonViewModel { get; }

        /// <inheritdoc />
        public PlayPauseButtonViewModel PlayPauseButtonViewModel { get; }

        /// <inheritdoc />
        public PreviousButtonViewModel PreviousButtonViewModel { get; }

        /// <inheritdoc />
        public RepeatModeButtonViewModel RepeatModeButtonViewModel { get; }

        /// <inheritdoc />
        public ShuffleModeButtonViewModel ShuffleModeButtonViewModel { get; }

        /// <inheritdoc />
        public ProgressBarViewModel ProgressBarViewModel { get; }

        /// <inheritdoc />
        public PopupViewModel PopupViewModel { get; }

        /// <inheritdoc />
        public AudioSourceSettingsViewModel AudioSourceSettingsViewModel { get; }
    }
}
