using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using AudioBand.AudioSource;
using AudioBand.Commands;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for the play/pause button.
    /// </summary>
    public class PlayPauseButtonVM : ViewModelBase<PlayPauseButton>
    {
        private static readonly List<ButtonContentType> ContentTypes = Enum.GetValues(typeof(ButtonContentType)).Cast<ButtonContentType>().ToList();
        private readonly IAppSettings _appsettings;
        private bool _isPlaying;
        private IAudioSource _audioSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayPauseButtonVM"/> class.
        /// </summary>
        /// <param name="appsettings">App settings.</param>
        /// <param name="dialogService">The dialog service.</param>
        public PlayPauseButtonVM(IAppSettings appsettings, IDialogService dialogService)
            : base(appsettings.PlayPauseButton)
        {
            DialogService = dialogService;
            _appsettings = appsettings;
            PlayPauseTrackCommand = new AsyncRelayCommand<object>(PlayPauseTrackCommandOnExecute);

            _appsettings.ProfileChanged += AppsettingsOnProfileChanged;
        }

        /// <summary>
        /// Gets or sets the play image path.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonImagePath))]
        public string PlayImagePath
        {
            get => Model.PlayButtonImagePath;
            set => SetProperty(nameof(Model.PlayButtonImagePath), value);
        }

        /// <summary>
        /// Gets or sets the path of the play image when hovered.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonHoveredImagePath))]
        public string PlayHoveredImagePath
        {
            get => Model.PlayButtonHoveredImagePath;
            set => SetProperty(nameof(Model.PlayButtonHoveredImagePath), value);
        }

        /// <summary>
        /// Gets or sets the path of the play image when clicked.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonClickedImagePath))]
        public string PlayClickedImagePath
        {
            get => Model.PlayButtonClickedImagePath;
            set => SetProperty(nameof(Model.PlayButtonClickedImagePath), value);
        }

        /// <summary>
        /// Gets or sets the path of the pause image.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonImagePath))]
        public string PauseImagePath
        {
            get => Model.PauseButtonImagePath;
            set => SetProperty(nameof(Model.PauseButtonImagePath), value);
        }

        /// <summary>
        /// Gets or sets the path of the pause image when hovered.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonHoveredImagePath))]
        public string PauseHoveredImagePath
        {
            get => Model.PauseButtonHoveredImagePath;
            set => SetProperty(nameof(Model.PauseButtonHoveredImagePath), value);
        }

        /// <summary>
        /// Gets or sets the path of the pause image when clicked.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonClickedImagePath))]
        public string PauseClickedImagePath
        {
            get => Model.PauseButtonClickedImagePath;
            set => SetProperty(nameof(Model.PauseButtonClickedImagePath), value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the play pause button is visible.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        /// <summary>
        /// Gets or sets the width of the button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.Width))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        /// <summary>
        /// Gets or sets the height of the button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.Height))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        /// <summary>
        /// Gets or sets the x position.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.XPosition))]
        public int XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        /// <summary>
        /// Gets or sets the y position.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.YPosition))]
        public int YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        /// <summary>
        /// Gets or sets the default background color.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.DefaultBackgroundColor))]
        public Color DefaultBackgroundColor
        {
            get => Model.DefaultBackgroundColor;
            set => SetProperty(nameof(Model.DefaultBackgroundColor), value);
        }

        /// <summary>
        /// Gets or sets the background color when hovered.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.HoveredBackgroundColor))]
        public Color HoveredBackgroundColor
        {
            get => Model.HoveredBackgroundColor;
            set => SetProperty(nameof(Model.HoveredBackgroundColor), value);
        }

        /// <summary>
        /// Gets or sets the background color when clicked.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.ClickedBackgroundColor))]
        public Color ClickedBackgroundColor
        {
            get => Model.ClickedBackgroundColor;
            set => SetProperty(nameof(Model.ClickedBackgroundColor), value);
        }

        /// <summary>
        /// Gets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; }

        /// <summary>
        /// Gets or sets the <see cref="ButtonContentType"/> for the play button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonContentType))]
        public ButtonContentType PlayButtonContentType
        {
            get => Model.PlayButtonContentType;
            set => SetProperty(nameof(Model.PlayButtonContentType), value);
        }

        /// <summary>
        /// Gets or sets the <see cref="ButtonContentType"/> for the pause button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonContentType))]
        public ButtonContentType PauseButtonContentType
        {
            get => Model.PauseButtonContentType;
            set => SetProperty(nameof(Model.PauseButtonContentType), value);
        }

        /// <summary>
        /// Gets or sets the font family for the play button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonTextFontFamily))]
        public string PlayButtonTextFontFamily
        {
            get => Model.PlayButtonTextFontFamily;
            set => SetProperty(nameof(Model.PlayButtonTextFontFamily), value);
        }

        /// <summary>
        /// Gets or sets the font family for the pause button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonTextFontFamily))]
        public string PauseButtonTextFontFamily
        {
            get => Model.PauseButtonTextFontFamily;
            set => SetProperty(nameof(Model.PauseButtonTextFontFamily), value);
        }

        /// <summary>
        /// Gets or sets the text for the play button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonText))]
        public string PlayButtonText
        {
            get => Model.PlayButtonText;
            set => SetProperty(nameof(Model.PlayButtonText), value);
        }

        /// <summary>
        /// Gets or sets the text for the pause button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonText))]
        public string PauseButtonText
        {
            get => Model.PauseButtonText;
            set => SetProperty(nameof(Model.PauseButtonText), value);
        }

        /// <summary>
        /// Gets or sets the text color for the play button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonTextColor))]
        public Color PlayButtonTextColor
        {
            get => Model.PlayButtonTextColor;
            set => SetProperty(nameof(Model.PlayButtonTextColor), value);
        }

        /// <summary>
        /// Gets or sets the text color for the play button when hovered.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonTextHoverColor))]
        public Color PlayButtonTextHoverColor
        {
            get => Model.PlayButtonTextHoverColor;
            set => SetProperty(nameof(Model.PlayButtonTextHoverColor), value);
        }

        /// <summary>
        /// Gets or sets the text color for the play button when clicked.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PlayButtonTextClickedColor))]
        public Color PlayButtonTextClickedColor
        {
            get => Model.PlayButtonTextClickedColor;
            set => SetProperty(nameof(Model.PlayButtonTextClickedColor), value);
        }

        /// <summary>
        /// Gets or sets the text color for the pause button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonTextColor))]
        public Color PauseButtonTextColor
        {
            get => Model.PauseButtonTextColor;
            set => SetProperty(nameof(Model.PauseButtonTextColor), value);
        }

        /// <summary>
        /// Gets or sets the text color for the pause button when hovered.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonTextHoverColor))]
        public Color PauseButtonTextHoverColor
        {
            get => Model.PauseButtonTextHoverColor;
            set => SetProperty(nameof(Model.PauseButtonTextHoverColor), value);
        }

        /// <summary>
        /// Gets or sets the text color for the pause button when clicked.
        /// </summary>
        [PropertyChangeBinding(nameof(PlayPauseButton.PauseButtonTextClickedColor))]
        public Color PauseButtonTextClickedColor
        {
            get => Model.PauseButtonTextClickedColor;
            set => SetProperty(nameof(Model.PauseButtonTextClickedColor), value);
        }

        /// <summary>
        /// Sets the audio source.
        /// </summary>
        public IAudioSource AudioSource
        {
            set => UpdateAudioSource(value);
        }

        /// <summary>
        /// Gets the play pause command.
        /// </summary>
        public ICommand PlayPauseTrackCommand { get; }

        /// <summary>
        /// Gets a value indicating whether a track is playing.
        /// </summary>
        [AlsoNotify(nameof(IsPlayButtonShown))]
        public bool IsPlaying
        {
            get => _isPlaying;
            private set => SetProperty(ref _isPlaying, value, false);
        }

        /// <summary>
        /// Gets a value indicating whether the button is playing.
        /// </summary>
        public bool IsPlayButtonShown => !_isPlaying;

        /// <summary>
        /// Gets the button content types.
        /// </summary>
        public IEnumerable<ButtonContentType> ButtonContentTypes => ContentTypes;

        private void AppsettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(_appsettings.PlayPauseButton);
        }

        private void UpdateAudioSource(IAudioSource audioSource)
        {
            if (_audioSource != null)
            {
                _audioSource.IsPlayingChanged -= AudioSourceOnIsPlayingChanged;
            }

            _audioSource = audioSource;
            if (_audioSource == null)
            {
                IsPlaying = false;
                return;
            }

            _audioSource.IsPlayingChanged += AudioSourceOnIsPlayingChanged;
        }

        private void AudioSourceOnIsPlayingChanged(object sender, bool e)
        {
            IsPlaying = e;
        }

        private async Task PlayPauseTrackCommandOnExecute(object arg)
        {
            if (_audioSource == null)
            {
                return;
            }

            if (IsPlaying)
            {
                await _audioSource.PauseTrackAsync();
            }
            else
            {
                await _audioSource.PlayTrackAsync();
            }
        }
    }
}
