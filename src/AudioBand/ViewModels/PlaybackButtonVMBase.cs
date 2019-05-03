using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime;
using AudioBand.Models;
using AudioBand.Resources;
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
        private IImage _image;
        private IImage _hoveredImage;
        private IImage _clickedImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaybackButtonVMBase{TButton}"/> class.
        /// </summary>
        /// <param name="appSettings">The appSettings.</param>
        /// <param name="resourceLoader">The resource loader.</param>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="buttonModel">The button model.</param>
        protected PlaybackButtonVMBase(IAppSettings appSettings, IResourceLoader resourceLoader, IDialogService dialogService, TButton buttonModel)
            : base(buttonModel)
        {
            DialogService = dialogService;
            AppSettings = appSettings;
            ResourceLoader = resourceLoader;

            AppSettings.ProfileChanged += AppsSettingsOnProfileChanged;
            Image = GetDefaultImage(); // Abstract method but everything should be initialized already
            HoveredImage = GetHoveredImage();
            ClickedImage = GetClickedImage();
        }

        /// <summary>
        /// Gets or sets the default image.
        /// </summary>
        public IImage Image
        {
            get => _image;
            set => SetProperty(ref _image, value, trackChanges: false);
        }

        /// <summary>
        /// Gets or sets the hovered image.
        /// </summary>
        public IImage HoveredImage
        {
            get => _hoveredImage;
            set => SetProperty(ref _hoveredImage, value, trackChanges: false);
        }

        /// <summary>
        /// Gets or sets the clicked image.
        /// </summary>
        public IImage ClickedImage
        {
            get => _clickedImage;
            set => SetProperty(ref _clickedImage, value, trackChanges: false);
        }

        /// <summary>
        /// Gets or sets the default image path.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.ImagePath))]
        public string ImagePath
        {
            get => Model.ImagePath;
            set
            {
                if (SetProperty(nameof(Model.ImagePath), value))
                {
                    Image = GetDefaultImage();
                }
            }
        }

        /// <summary>
        /// Gets or sets the hovered image path.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.HoveredImagePath))]
        public string HoveredImagePath
        {
            get => Model.HoveredImagePath;
            set
            {
                if (SetProperty(nameof(Model.HoveredImagePath), value))
                {
                    HoveredImage = GetHoveredImage();
                }
            }
        }

        /// <summary>
        /// Gets or sets the clicked image path.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.ClickedImagePath))]
        public string ClickedImagePath
        {
            get => Model.ClickedImagePath;
            set
            {
                if (SetProperty(nameof(Model.ClickedImagePath), value))
                {
                    ClickedImage = GetClickedImage();
                }
            }
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
        [AlsoNotify(nameof(Size))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        /// <summary>
        /// Gets or sets the height of the button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.Height))]
        [AlsoNotify(nameof(Size))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        /// <summary>
        /// Gets or sets the x position of the button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.XPosition))]
        [AlsoNotify(nameof(Location))]
        public int XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        /// <summary>
        /// Gets or sets the y position of the button.
        /// </summary>
        [PropertyChangeBinding(nameof(PlaybackButtonBase.YPosition))]
        [AlsoNotify(nameof(Location))]
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
        /// Gets the location of the button.
        /// </summary>
        /// <remarks>This property exists so the designer can bind to it.</remarks>
        public Point Location => new Point(Model.XPosition, Model.YPosition);

        /// <summary>
        /// Gets the size of the button.
        /// </summary>
        /// <remarks>This property exists so the designer can bind to it.</remarks>
        public Size Size => new Size(Width, Height);

        /// <summary>
        /// Gets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; }

        /// <summary>
        /// Gets the app settings.
        /// </summary>
        protected IAppSettings AppSettings { get; }

        /// <summary>
        /// Gets the resource loader.
        /// </summary>
        protected IResourceLoader ResourceLoader { get; }

        /// <inheritdoc/>
        protected override void OnReset()
        {
            base.OnReset();
            Image = GetDefaultImage();
        }

        /// <inheritdoc/>
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            Image = GetDefaultImage();
        }

        /// <summary>
        /// Gets the button model when profile changes.
        /// </summary>
        /// <returns>The new button model.</returns>
        protected abstract TButton GetReplacementModel();

        /// <summary>
        /// Gets the default image.
        /// </summary>
        /// <returns>The default image.</returns>
        protected abstract IImage GetDefaultImage();

        /// <summary>
        /// Gets the hovered image.
        /// </summary>
        /// <returns>The hovered image.</returns>
        protected abstract IImage GetHoveredImage();

        /// <summary>
        /// Gets the clicked image.
        /// </summary>
        /// <returns>The clicked image.</returns>
        protected abstract IImage GetClickedImage();

        private void AppsSettingsOnProfileChanged(object sender, EventArgs e)
        {
            Debug.Assert(IsEditing == false, "Should not be editing");
            ReplaceModel(GetReplacementModel());
        }
    }
}
