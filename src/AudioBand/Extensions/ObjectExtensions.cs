using System.Text;

namespace AudioBand.Extensions
{
    internal static class ObjectExtensions
    {
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
