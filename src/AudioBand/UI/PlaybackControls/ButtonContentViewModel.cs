using System.Collections.Generic;
using System.Windows.Media;
using AudioBand.Extensions;
using AudioBand.Models;

namespace AudioBand.UI
{
    /// <summary>
    /// View model for button content.
    /// </summary>
    public class ButtonContentViewModel : ViewModelBase
    {
        private readonly ButtonContent _model = new ButtonContent();
        private readonly ButtonContent _backup = new ButtonContent();
        private readonly ButtonContent _resetState;
        private readonly ButtonContent _originalSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonContentViewModel"/> class.
        /// </summary>
        /// <param name="source">The button content model.</param>
        /// <param name="resetState">The state used for resetting.</param>
        /// <param name="dialogService">The dialog service.</param>
        public ButtonContentViewModel(ButtonContent source, ButtonContent resetState, IDialogService dialogService)
        {
            _resetState = resetState;
            _originalSource = source;
            DialogService = dialogService;

            MapSelf(source, _model);
        }

        /// <summary>
        /// Gets the button content types.
        /// </summary>
        public static IEnumerable<EnumDescriptor<ButtonContentType>> ButtonContentTypes { get; } = typeof(ButtonContentType).GetEnumDescriptors<ButtonContentType>();

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        [TrackState]
        public ButtonContentType ContentType
        {
            get => _model.ContentType;
            set => SetProperty(_model, nameof(_model.ContentType), value);
        }

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        [TrackState]
        public string ImagePath
        {
            get => _model.ImagePath;
            set => SetProperty(_model, nameof(_model.ImagePath), value);
        }

        /// <summary>
        /// Gets or sets the path of the image when hovered.
        /// </summary>
        [TrackState]
        public string HoveredImagePath
        {
            get => _model.HoveredImagePath;
            set => SetProperty(_model, nameof(_model.HoveredImagePath), value);
        }

        /// <summary>
        /// Gets or sets the path of the image when clicked.
        /// </summary>
        [TrackState]
        public string ClickedImagePath
        {
            get => _model.ClickedImagePath;
            set => SetProperty(_model, nameof(_model.ClickedImagePath), value);
        }

        /// <summary>
        /// Gets or sets the font family for the text.
        /// </summary>
        [TrackState]
        public string FontFamily
        {
            get => _model.FontFamily;
            set => SetProperty(_model, nameof(_model.FontFamily), value);
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [TrackState]
        public string Text
        {
            get => _model.Text;
            set => SetProperty(_model, nameof(_model.Text), value);
        }

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        [TrackState]
        public Color TextColor
        {
            get => _model.TextColor;
            set => SetProperty(_model, nameof(_model.TextColor), value);
        }

        /// <summary>
        /// Gets or sets the text color when hovered.
        /// </summary>
        [TrackState]
        public Color HoveredTextColor
        {
            get => _model.HoveredTextColor;
            set => SetProperty(_model, nameof(_model.HoveredTextColor), value);
        }

        /// <summary>
        /// Gets or sets the text color when clicked.
        /// </summary>
        [TrackState]
        public Color ClickedTextColor
        {
            get => _model.ClickedTextColor;
            set => SetProperty(_model, nameof(_model.ClickedTextColor), value);
        }

        /// <summary>
        /// Gets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; }

        /// <inheritdoc />
        protected override void OnReset()
        {
            base.OnReset();
            MapSelf(_resetState, _model);
        }

        /// <inheritdoc />
        protected override void OnBeginEdit()
        {
            base.OnBeginEdit();
            MapSelf(_model, _backup);
        }

        /// <inheritdoc />
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            MapSelf(_backup, _model);
        }

        /// <inheritdoc />
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            MapSelf(_model, _originalSource);
        }
    }
}
