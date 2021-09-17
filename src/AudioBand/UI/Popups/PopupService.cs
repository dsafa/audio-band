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
        private ResourceDictionary _textParts = new ResourceDictionary();

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupService"/> class.
        /// </summary>
        /// <param name="appSettings">The app's settings.</param>
        /// <param name="viewModels">The viewmodels.</param>
        public PopupService(IAppSettings appSettings, IViewModelContainer viewModels)
        {
            _appSettings = appSettings;
            _viewModels = viewModels;

            _textParts.Source = new Uri("/AudioBand;component/UI/Resources/Strings.xaml", UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Shows a popup.
        /// </summary>
        /// <param name="title">The title string key of the popup.</param>
        /// <param name="description">The text to show to the user.</param>
        /// <param name="duration">The duration to show the popup for (max 60 seconds).</param>
        public void ShowPopup(string title, string description, TimeSpan duration)
        {
            // First one is an edge-case, handled in here separately.
            if (title == "UpdatePopupTitle")
            {
                if (_appSettings.AudioBandSettings.ShowPopupOnAvailableUpdate)
                {
                    return;
                }
            }
            else if (!_appSettings.AudioBandSettings.ShowInformationPopups)
            {
                return;
            }

            duration = duration.TotalSeconds < 3 ? TimeSpan.FromSeconds(3) : duration;
            duration = duration.TotalSeconds > 60 ? TimeSpan.FromSeconds(60) : duration;

            if (!_textParts.Contains(title) || !_textParts.Contains(description))
            {
                _viewModels.PopupViewModel.ActivateNewPopup("Text Error", $"The keys '{title}' and/or '{description}' could not be found in Strings.xaml. (Notify a developer)", duration);
            }

            _viewModels.PopupViewModel.ActivateNewPopup(_textParts[title].ToString(), _textParts[description].ToString(), duration);
        }

        /// <summary>
        /// Shows a popup.
        /// </summary>
        /// <param name="title">The title of the popup.</param>
        /// <param name="description">The text to show to the user.</param>
        public void ShowPopup(string title, string description) => ShowPopup(title, description, TimeSpan.FromSeconds(7));
    }
}
