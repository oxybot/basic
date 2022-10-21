namespace Basic.WebApi.Models
{
    /// <summary>
    /// Defines the model associated with a standard sort and filter options.
    /// </summary>
    public class SortAndFilterModel
    {
        /// <summary>
        /// Gets or sets the filter value.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the property name to sort on.
        /// </summary>
        public string SortKey { get; set; }

        /// <summary>
        /// Gets or sets the order for sort (asc or desc).
        /// </summary>
        public string SortValue { get; set; }
    }
}
