using System.Text;

namespace AudioBand.Extensions
{
    /// <summary>
    /// Extensions for <see cref="object"/>.
    /// </summary>
    internal static class ObjectExtensions
    {
        /// <summary>
        /// Gets the string containing all the properties and their values.
        /// </summary>
        /// <param name="o">Object.</param>
        /// <returns>A string with the object's properties and values.</returns>
        public static string FormattedString(this object o)
        {
            var sb = new StringBuilder();
            sb.Append("{ ");
            foreach (var prop in o.GetType().GetProperties())
            {
                sb.Append($"{prop.Name}: {prop.GetValue(o)}, ");
            }

            sb.Append("}");

            return sb.ToString();
        }
    }
}
