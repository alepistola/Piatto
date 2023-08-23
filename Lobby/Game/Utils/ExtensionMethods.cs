namespace ExtensionMethods
{
    public static class Extensions
    {
        public static string StringJoin(this IEnumerable<string> values, string separator)
        {
            return string.Join(separator, values);
        }
    }
}