namespace AudioBand.ValueConverters
{
    /// <summary>
    /// Static class for multi value converters.
    /// </summary>
    public static class MultiConverters
    {
        /// <summary>
        /// Gets a <see cref="ComparisonConverter"/>.
        /// </summary>
        public static ComparisonConverter Comparison { get; } = new ComparisonConverter();

        /// <summary>
        /// Gets a <see cref="MultiplierConverter"/>.
        /// </summary>
        public static MultiplierConverter Multiply { get; } = new MultiplierConverter();

        /// <summary>
        /// Gets a <see cref="PointConverter"/>.
        /// </summary>
        public static PointConverter Point { get; } = new PointConverter();
    }
}
