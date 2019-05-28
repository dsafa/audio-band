using System;

namespace AudioBand.Extensions
{
    /// <summary>
    /// Marks enum field as ignored.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DescriptorIgnoreAttribute : Attribute
    {
    }
}
