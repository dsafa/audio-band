using System.Collections.Generic;
using AudioBand.Extensions;

namespace AudioBand.Models
{
    /// <summary>
    /// Static properties for <see cref="PositionAnchor"/>.
    /// </summary>
    public static class PositionAnchors
    {
        /// <summary>
        /// Gets the values for a <see cref="PositionAnchor"/>.
        /// </summary>
        public static IEnumerable<EnumDescriptor<PositionAnchor>> Values { get; } = typeof(PositionAnchor).GetEnumDescriptors<PositionAnchor>();
    }
}
