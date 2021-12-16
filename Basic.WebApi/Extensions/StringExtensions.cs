namespace System
{
    public static class StringExtensions
    {
        public static string ToJsonFieldName(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            else if (value.Length ==1)
            {
                return value.ToLowerInvariant();
            }

            string start = value[..1];
            string remaining = value[1..];

            return start.ToLowerInvariant() + remaining;
        }
    }
}
