using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AudioBand.Models;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using TextAlignment = AudioBand.Models.CustomLabel.TextAlignment;

#pragma warning disable 1591

namespace AudioBand.ViewModels
{
    /// <summary>
    /// The view model for a custom label.
    /// </summary>
    public class CustomLabelVM : ViewModelBase<CustomLabel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLabelVM"/> class.
        /// </summary>
        /// <param name="model">The custom label</param>
        /// <param name="dialogService">The dialog service</param>
        public CustomLabelVM(CustomLabel model, IDialogService dialogService)
            : base(model)
        {
            DialogService = dialogService;
        }

        [PropertyChangeBinding(nameof(CustomLabel.Name))]
        public string Name
        {
            get => Model.Name;
            set => SetProperty(nameof(Model.Name), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.FontFamily))]
        public string FontFamily
        {
            get => Model.FontFamily;
            set => SetProperty(nameof(Model.FontFamily), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.FontSize))]
        public float FontSize
        {
            get => Model.FontSize;
            set => SetProperty(nameof(Model.FontSize), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.Color))]
        public Color Color
        {
            get => Model.Color;
            set => SetProperty(nameof(Model.Color), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.FormatString))]
        public string FormatString
        {
            get => Model.FormatString;
            set => SetProperty(nameof(Model.FormatString), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.Alignment))]
        public TextAlignment TextAlignment
        {
            get => Model.Alignment;
            set => SetProperty(nameof(Model.Alignment), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.IsVisible))]
        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetProperty(nameof(Model.IsVisible), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.Width))]
        [AlsoNotify(nameof(Size))]
        public int Width
        {
            get => Model.Width;
            set => SetProperty(nameof(Model.Width), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.Height))]
        [AlsoNotify(nameof(Size))]
        public int Height
        {
            get => Model.Height;
            set => SetProperty(nameof(Model.Height), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.XPosition))]
        [AlsoNotify(nameof(Location))]
        public int XPosition
        {
            get => Model.XPosition;
            set => SetProperty(nameof(Model.XPosition), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.YPosition))]
        [AlsoNotify(nameof(Location))]
        public int YPosition
        {
            get => Model.YPosition;
            set => SetProperty(nameof(Model.YPosition), value);
        }

        [PropertyChangeBinding(nameof(CustomLabel.ScrollSpeed))]
        public int ScrollSpeed
        {
            get => Model.ScrollSpeed;
            set => SetProperty(nameof(Model.ScrollSpeed), value);
        }

        /// <summary>
        /// Gets the location of the custom label.
        /// </summary>
        /// <remarks>This property exists so the designer can bind to it.</remarks>
        public Point Location => new Point(XPosition, YPosition);

        /// <summary>
        /// Gets the size of the custom label.
        /// </summary>
        /// <remarks>This property exists so the designer can bind to it.</remarks>
        public Size Size => new Size(Width, Height);

        /// <summary>
        /// Gets the values of <see cref="CustomLabel.TextAlignment"/>.
        /// </summary>
        public IEnumerable<TextAlignment> TextAlignValues { get; } = Enum.GetValues(typeof(TextAlignment)).Cast<TextAlignment>();

        /// <summary>
        /// Gets the dialog service
        /// </summary>
        public IDialogService DialogService { get; private set; }
    }
}
#pragma warning restore 1591
