using AudioBand.Extensions;
using System.Collections.Generic;

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
