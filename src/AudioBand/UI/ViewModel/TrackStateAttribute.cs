using System;

namespace AudioBand.UI
{
    /// <summary>
    /// Marks the property for state tracking.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TrackStateAttribute : Attribute
    {
    }
}
