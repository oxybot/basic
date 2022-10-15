namespace System
{
    /// <summary>
    /// Extension methods for <see cref="int"/> instances.
    /// </summary>
    internal static class IntegerExtensions
    {
        public static bool IsEven(this int reference)
        {
            return reference % 2 == 0;
        }
    }
}
