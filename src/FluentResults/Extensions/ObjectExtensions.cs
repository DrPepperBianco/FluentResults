// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Extension methods for object base type
    /// </summary>
    public static class ObjectExtensions
    {

        internal static string ToLabelValueStringOrEmpty(this object value, string label)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var valueText = value.ToString();

            if (valueText == string.Empty)
            {
                return string.Empty;
            }

            return $"{label}='{valueText}'";
        }
    }
}