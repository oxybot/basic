namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Indicates that the associated property can or can't be used as a sort option.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SortableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortableAttribute"/> class.
        /// </summary>
        /// <param name="sortable">A value indicating whether the associated property can be used as a sort option.</param>
        public SortableAttribute(bool sortable)
        {
            this.Sortable = sortable;
        }

        /// <summary>
        /// Gets a value indicating whether the associated property can be used as a sort option.
        /// </summary>
        public bool Sortable { get; }
    }
}
