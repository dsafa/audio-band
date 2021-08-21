using System.Windows.Data;

namespace AudioBand.UI
{
    /// <summary>
    /// Static class that contains converts.
    /// </summary>
    public static class Converters
    {
        /// <summary>
        /// Gets a <see cref="PathToImageSourceConverter"/>.
        /// </summary>
        public static PathToImageSourceConverter PathToImageSource { get; } = new PathToImageSourceConverter();

        /// <summary>
        /// Gets a <see cref="ColorToBrushConverter"/>.
        /// </summary>
        public static ColorToBrushConverter ColorToBrush { get; } = new ColorToBrushConverter();

        /// <summary>
        /// Gets a <see cref="BoolToVisibilityConverter"/>.
        /// </summary>
        public static BoolToVisibilityConverter BoolToVisibility { get; } = new BoolToVisibilityConverter();

        /// <summary>
        /// Gets a <see cref="DoubleToCornerRadiusConverter"/>.
        /// </summary>
        public static DoubleToCornerRadiusConverter DoubleToCornerRadius { get; } = new DoubleToCornerRadiusConverter();

        /// <summary>
        /// Gets a <see cref="TimeSpanToMsConverter"/>.
        /// </summary>
        public static TimeSpanToMsConverter TimeSpanToMs { get; } = new TimeSpanToMsConverter();

        /// <summary>
        /// Gets a <see cref="TextAlignmentConverter"/>.
        /// </summary>
        public static TextAlignmentConverter TextAlignment { get; } = new TextAlignmentConverter();

        /// <summary>
        /// Gets a <see cref="ColorToNameConverter"/>.
        /// </summary>
        public static ColorToNameConverter ColorToName { get; } = new ColorToNameConverter();

        /// <summary>
        /// Gets a <see cref="StringToVisibilityConverter"/>.
        /// </summary>
        public static StringToVisibilityConverter StringToVisibility { get; } = new StringToVisibilityConverter();

        /// <summary>
        /// Gets a <see cref="ObjectToTypeConverter"/>.
        /// </summary>
        public static ObjectToTypeConverter ObjectToType { get; } = new ObjectToTypeConverter();

        /// <summary>
        /// Gets a <see cref="StringFormatConverter"/>.
        /// </summary>
        public static StringFormatConverter StringFormat { get; } = new StringFormatConverter();

        /// <summary>
        /// Gets a <see cref="StringToFontFamilyConverter"/>.
        /// </summary>
        public static StringToFontFamilyConverter StringToFontFamily { get; } = new StringToFontFamilyConverter();

        /// <summary>
        /// Gets a <see cref="CoerceNumberConverter"/>.
        /// </summary>
        public static CoerceNumberConverter CoerceNumber { get; } = new CoerceNumberConverter();

        /// <summary>
        /// Gets a <see cref="EmptyStringToBoolConverter"/>.
        /// </summary>
        public static EmptyStringToBoolConverter EmptyStringToBool { get; } = new EmptyStringToBoolConverter();

        /// <summary>
        /// Gets a brush to color converter.
        /// </summary>
        public static IValueConverter BrushToColor { get; } = new ReverseConverter(new ColorToBrushConverter());

        /// <summary>
        /// Gets a <see cref="FlagToBoolConverter"/>.
        /// </summary>
        public static FlagToBoolConverter HasFlag { get; } = new FlagToBoolConverter();

        /// <summary>
        /// Gets a <see cref="InverseBoolConverter"/>.
        /// </summary>
        public static InverseBoolConverter InverseBool { get; } = new InverseBoolConverter();
    }
}
