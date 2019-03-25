using System;
using System.Collections.ObjectModel;
using AudioBand.ViewModels;

namespace AudioBand.Views.Wpf
{
    /// <summary>
    /// Interface for a settings window
    /// </summary>
    public interface ISettingsWindow
    {
        /// <summary>
        /// Occurs when settings are saved.
        /// </summary>
        event EventHandler Saved;

        /// <summary>
        /// Occurs when settings are canceled.
        /// </summary>
        event EventHandler Canceled;

        AudioBandVM AudioBandVM { get; }

        AlbumArtPopupVM AlbumArtPopupVM { get; }

        AlbumArtVM AlbumArtVM { get; }

        CustomLabelsVM CustomLabelsVM { get; }

        AboutVM AboutVm { get; }

        NextButtonVM NextButtonVM { get; }

        PlayPauseButtonVM PlayPauseButtonVM { get; }

        PreviousButtonVM PreviousButtonVM { get; }

        ProgressBarVM ProgressBarVM { get; }

        AboutVM AboutVM { get; }

        ObservableCollection<AudioSourceSettingsVM> AudioSourceSettingsVM { get; }

        void ShowWindow();
    }
}
