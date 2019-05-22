using System;
using System.Collections.Generic;
using System.Windows.Media;
using AudioBand.Extensions;
using AudioBand.Models;

namespace AudioBand.ViewModels
{
    /// <summary>
    /// View model for button content.
    /// </summary>
    public class ButtonContentViewModel : ViewModelBase<ButtonContent>
    {
        private readonly ButtonContent _resetContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonContentViewModel"/> class.
        /// </summary>
        /// <param name="model">The button content model.</param>
        /// <param name="resetContent">The state used for resetting.</param>
        /// <param name="dialogService">The dialog service.</param>
        public ButtonContentViewModel(ButtonContent model, ButtonContent resetContent, IDialogService dialogService)
            : base(model)
        {
            _resetContent = resetContent;
            DialogService = dialogService;
        }

        /// <summary>
        /// Gets the button content types.
        /// </summary>
        public static IEnumerable<EnumDescriptor<ButtonContentType>> ButtonContentTypes { get; } = typeof(ButtonContentType).GetEnumDescriptors<ButtonContentType>();

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonContent.ContentType))]
        public ButtonContentType ContentType
        {
            get => Model.ContentType;
            set => SetProperty(nameof(Model.ContentType), value);
        }

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonContent.ImagePath))]
        public string ImagePath
        {
            get => Model.ImagePath;
            set => SetProperty(nameof(Model.ImagePath), value);
        }

        /// <summary>
        /// Gets or sets the path of the image when hovered.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonContent.HoveredImagePath))]
        public string HoveredImagePath
        {
            get => Model.HoveredImagePath;
            set => SetProperty(nameof(Model.HoveredImagePath), value);
        }

        /// <summary>
        /// Gets or sets the path of the image when clicked.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonContent.ClickedImagePath))]
        public string ClickedImagePath
        {
            get => Model.ClickedImagePath;
            set => SetProperty(nameof(Model.ClickedImagePath), value);
        }

        /// <summary>
        /// Gets or sets the font family for the text.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonContent.FontFamily))]
        public string FontFamily
        {
            get => Model.FontFamily;
            set => SetProperty(nameof(Model.FontFamily), value);
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonContent.Text))]
        public string Text
        {
            get => Model.Text;
            set => SetProperty(nameof(Model.Text), value);
        }

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonContent.TextColor))]
        public Color TextColor
        {
            get => Model.TextColor;
            set => SetProperty(nameof(Model.TextColor), value);
        }

        /// <summary>
        /// Gets or sets the text color when hovered.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonContent.HoveredTextColor))]
        public Color HoveredTextColor
        {
            get => Model.HoveredTextColor;
            set => SetProperty(nameof(Model.HoveredTextColor), value);
        }

        /// <summary>
        /// Gets or sets the text color when clicked.
        /// </summary>
        [PropertyChangeBinding(nameof(ButtonContent.ClickedTextColor))]
        public Color ClickedTextColor
        {
            get => Model.ClickedTextColor;
            set => SetProperty(nameof(Model.ClickedTextColor), value);
        }

        /// <summary>
        /// Gets the dialog service.
        /// </summary>
        public IDialogService DialogService { get; }

        /// <inheritdoc />
        protected override void OnReset()
        {
            BeginEdit();
            ResetObject(_resetContent, Model);
        }
    }
}
