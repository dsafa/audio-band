using System.Collections.ObjectModel;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Container for all the view models
    /// </summary>
    public class ViewModelContainer : IViewModelContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelContainer"/> class.
        /// </summary>
        /// <param name="audioBandVm">Audioband view model.</param>
        /// <param name="albumArtPopupVm">Album art popup view model.</param>
        /// <param name="albumArtVm">Album art view model.</param>
        /// <param name="customLabelsVm">Custom labels view model.</param>
        /// <param name="nextButtonVm">Next button view model.</param>
        /// <param name="playPauseButtonVm">Play pause button view model.</param>
        /// <param name="previousButtonVm">Previous button view model.</param>
        /// <param name="progressBarVm">Progress bar view model.</param>
        public ViewModelContainer(
            AudioBandVM audioBandVm,
            AlbumArtPopupVM albumArtPopupVm,
            AlbumArtVM albumArtVm,
            CustomLabelsVM customLabelsVm,
            NextButtonVM nextButtonVm,
            PlayPauseButtonVM playPauseButtonVm,
            PreviousButtonVM previousButtonVm,
            ProgressBarVM progressBarVm)
        {
            AudioBandVM = audioBandVm;
            AlbumArtPopupVM = albumArtPopupVm;
            AlbumArtVM = albumArtVm;
            CustomLabelsVM = customLabelsVm;
            NextButtonVM = nextButtonVm;
            PlayPauseButtonVM = playPauseButtonVm;
            PreviousButtonVM = previousButtonVm;
            ProgressBarVM = progressBarVm;;
        }

        /// <inheritdoc />
        public AudioBandVM AudioBandVM { get; }

        /// <inheritdoc />
        public AlbumArtPopupVM AlbumArtPopupVM { get; }

        /// <inheritdoc />
        public AlbumArtVM AlbumArtVM { get; }

        /// <inheritdoc />
        public CustomLabelsVM CustomLabelsVM { get; }

        /// <inheritdoc />
        public NextButtonVM NextButtonVM { get; }

        /// <inheritdoc />
        public PlayPauseButtonVM PlayPauseButtonVM { get; }

        /// <inheritdoc />
        public PreviousButtonVM PreviousButtonVM { get; }

        /// <inheritdoc />
        public ProgressBarVM ProgressBarVM { get; }

        /// <inheritdoc />
        public ObservableCollection<AudioSourceSettingsVM> AudioSourceSettingsVM { get; } = new ObservableCollection<AudioSourceSettingsVM>();
    }
}
