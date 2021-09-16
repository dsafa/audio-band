using System;
using System.Windows;
using AudioBand.Settings;

namespace AudioBand.UI
{
    /// <summary>
    /// Service used to display short popups to the user to give some information.
    /// </summary>
    public class PopupService
    {
        private IAppSettings _appSettings;
        private IViewModelContainer _viewModels;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupService"/> class.
        /// </summary>
        /// <param name="appSettings">The app's settings.</param>
        /// <param name="viewModels">The viewmodels.</param>
        public PopupService(IAppSettings appSettings, IViewModelContainer viewModels)
        {
            _appSettings = appSettings;
            _viewModels = viewModels;
        }

        /// <summary>
        /// Shows a popup.
        /// </summary>
        /// <param name="title">The title string key of the popup.</param>
        /// <param name="description">The text to show to the user.</param>
        /// <param name="duration">The duration to show the popup for (max 60 seconds).</param>
        public void ShowPopup(string title, string description, TimeSpan duration)
        {
            if (!_appSettings.AudioBandSettings.ShowInformationPopups
            && (title == "UpdateIsAvailableTitle" && !_appSettings.AudioBandSettings.ShowPopupOnAvailableUpdate))
            {
                return;
            }

            duration = duration.TotalSeconds < 3 ? TimeSpan.FromSeconds(3) : duration;
            duration = duration.TotalSeconds > 60 ? TimeSpan.FromSeconds(60) : duration;

            var a = Application.Current.TryFindResource(title);
            _viewModels.PopupViewModel.ActivateNewPopup("Update found!", "An update was found, press the button below to start downloading!", duration);
        }

        /// <summary>
        /// Shows a popup.
        /// </summary>
        /// <param name="title">The title of the popup.</param>
        /// <param name="description">The text to show to the user.</param>
        public void ShowPopup(string title, string description) => ShowPopup(title, description, TimeSpan.FromSeconds(7));
    }
}
