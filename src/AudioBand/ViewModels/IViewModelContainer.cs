using System;
using System.Collections.ObjectModel;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// Interface for a settings window.
    /// </summary>
    public interface IViewModelContainer
    {
        /// <summary>
        /// Gets the viewmodel for audioband toolbar.
        /// </summary>
        AudioBandVM AudioBandVM { get; }

        /// <summary>
        /// Gets the viewmodel for the album art popup.
        /// </summary>
        AlbumArtPopupVM AlbumArtPopupVM { get; }

        /// <summary>
        /// Gets the viewmodel for the album art.
        /// </summary>
        AlbumArtVM AlbumArtVM { get; }

        /// <summary>
        /// Gets the viewmodel for custom labels collection.
        /// </summary>
        CustomLabelsVM CustomLabelsVM { get; }

        /// <summary>
        /// Gets the view model for the next button.
        /// </summary>
        NextButtonVM NextButtonVM { get; }

        /// <summary>
        /// Gets the view model for the play/pause button.
        /// </summary>
        PlayPauseButtonVM PlayPauseButtonVM { get; }

        /// <summary>
        /// Gets the view model for the previous button.
        /// </summary>
        PreviousButtonVM PreviousButtonVM { get; }

        /// <summary>
        /// Gets the view model for the progress bar.
        /// </summary>
        ProgressBarVM ProgressBarVM { get; }

        /// <summary>
        /// Gets the collection for view models for the audio source settings.
        /// </summary>
        ObservableCollection<AudioSourceSettingsVM> AudioSourceSettingsVM { get; }
    }
}
