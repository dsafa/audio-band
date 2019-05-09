using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;
using AudioBand.Models;
using AudioBand.Settings;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model base for playback button.
    /// </summary>
    /// <typeparam name="TButton">The playback button model.</typeparam>
    public abstract class PlaybackButtonVMBase<TButton> : ViewModelBase<TButton>
        where TButton : PlaybackButtonBase, new()
    {
        private static readonly List<ButtonContentType> ContentTypes = Enum.GetValues(typeof(ButtonContentType)).Cast<ButtonContentType>().ToList();

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaybackButtonVMBase{TButton}"/> class.
        /// </summary>
        /// <param name="appSettings">The appSettings.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="buttonModel">The button model.</param>
        protected PlaybackButtonVMBase(IAppSettings appSettings, IDialogService dialogService, TButton buttonModel)
            : base(buttonModel)
        {
            DialogService = dialogService;
            AppSettings = appSettings;

            AppSettings.ProfileChanged += AppsSettingsOnProfileChanged;
        }

        /// <summary>
        /// Gets or sets the default image path.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.ImagePath))]
        public string ImagePath
        {
            get => Model.ImagePath;
            set => SetProperty(nameof(Model.ImagePath), value);
        }

        /// <summary>
        /// Gets or sets the hovered image path.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.HoveredImagePath))]
        public string HoveredImagePath
        {
            get => Model.HoveredImagePath;
            set => SetProperty(nameof(Model.HoveredImagePath), value);
        }

        /// <summary>
        /// Gets or sets the clicked image path.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.ClickedImagePath))]
        public string ClickedImagePath
        {
            get => Model.ClickedImagePath;
            set => SetProperty(nameof(Model.ClickedImagePath), value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the button is visible.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        /// <summary>
        /// Gets or sets the width of the button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.Width))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        /// <summary>
        /// Gets or sets the height of the button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.Height))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        /// <summary>
        /// Gets or sets the x position of the button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.XPosition))]
        public int XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        /// <summary>
        /// Gets or sets the y position of the button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.YPosition))]
        public int YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.BackgroundColor))]
        public Color BackgroundColor
        {
            get => Model.BackgroundColor;
            set => SetProperty(nameof(Model.BackgroundColor), value);
        }

        /// <summary>
        /// Gets or sets the hovered background color.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.HoveredBackgroundColor))]
        public Color HoveredBackgroundColor
        {
            get => Model.HoveredBackgroundColor;
            set => SetProperty(nameof(Model.HoveredBackgroundColor), value);
        }

        /// <summary>
        /// Gets or sets the clicked background color.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.ClickedBackgroundColor))]
        public Color ClickedBackgroundColor
        {
            get => Model.ClickedBackgroundColor;
            set => SetProperty(nameof(Model.ClickedBackgroundColor), value);
        }

        /// <summary>
        /// Gets the button content types.
        /// </summary>
        public IEnumerable<ButtonContentType> ButtonContentTypes => ContentTypes;

        /// <summary>
        /// Gets or sets the content type for the button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.ContentType))]
        public ButtonContentType ContentType
        {
            get => Model.ContentType;
            set => SetProperty(nameof(Model.ContentType), value);
        }

        /// <summary>
        /// Gets or sets the font family for the button text.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.TextFontFamily))]
        public string TextFontFamily
        {
            get => Model.TextFontFamily;
            set => SetProperty(nameof(Model.TextFontFamily), value);
        }

        /// <summary>
        /// Gets or sets the text for button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.Text))]
        public string Text
        {
            get => Model.Text;
            set => SetProperty(nameof(Model.Text), value);
        }

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.TextColor))]
        public Color TextColor
        {
            get => Model.TextColor;
            set => SetProperty(nameof(Model.TextColor), value);
        }

        /// <summary>
        /// Gets or sets the text hovered color.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.TextHoveredColor))]
        public Color TextHoveredColor
        {
            get => Model.TextHoveredColor;
            set => SetProperty(nameof(Model.TextHoveredColor), value);
        }

        /// <summary>
        /// Gets or sets the text clicked color.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.TextClickedColor))]
        public Color TextClickedColor
        {
            get => Model.TextClickedColor;
            set => SetProperty(nameof(Model.TextClickedColor), value);
        }

        /// <summary>
        /// Gets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; }

        /// <summary>
        /// Gets the app settings.
        /// </summary>
        protected IAppSettings AppSettings { get; }

        /// <summary>
        /// Gets the button model when profile changes.
        /// </summary>
        /// <returns>The new button model.</returns>
        protected abstract TButton GetReplacementModel();

        private void AppsSettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(GetReplacementModel());
        }
    }
}
