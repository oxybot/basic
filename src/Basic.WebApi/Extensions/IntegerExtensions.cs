namespace System
{
    /// <summary>
    /// Extension methods for <see cref="Int32"/> instances.
    /// </summary>
    internal static class IntegerExtensions
    {
        public static bool IsEven(this int reference)
        {
            return reference % 2 == 0;
        }
    }
}
