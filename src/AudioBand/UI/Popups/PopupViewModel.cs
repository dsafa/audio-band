using System;
using System.Timers;
using System.Windows.Input;
using AudioBand.Commands;

namespace AudioBand.UI
{
    /// <summary>
    /// View model for popups.
    /// </summary>
    public class PopupViewModel : ViewModelBase
    {
        private bool _isPopupOpen;
        private string _title;
        private string _description;
        private Timer _timer = new Timer();

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupViewModel"/> class.
        /// </summary>
        public PopupViewModel()
        {
            _timer.Elapsed += OnTimerElapsed;
            CloseCommand = new RelayCommand(ClosePopupCommand);
        }

        /// <summary>
        /// Gets the command to close the popup.
        /// </summary>
        public ICommand CloseCommand { get; }

        /// <summary>
        /// Gets or sets whether the popup is currently showing or not.
        /// </summary>
        public bool IsInformationPopupOpen
        {
            get => _isPopupOpen;
            set => SetProperty(ref _isPopupOpen, value);
        }

        /// <summary>
        /// Gets or sets the title of the popup.
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// Gets or sets the description of the popup.
        /// </summary>
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        /// <summary>
        /// Activates a new popup with the given information.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="duration">How long to display it for.</param>
        public void ActivateNewPopup(string title, string description, TimeSpan duration)
        {
            _timer.Stop();
            IsInformationPopupOpen = false;

            Title = title;
            Description = description;

            _timer.Interval = duration.TotalMilliseconds;
            IsInformationPopupOpen = true;
            _timer.Start();
        }

        private void ClosePopupCommand()
        {
            IsInformationPopupOpen = false;
            _timer.Stop();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e) => ClosePopupCommand();
    }
}
