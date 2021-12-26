namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents a link from a DTO to any other entity.
    /// </summary>
    public class EntityReference
    {
        /// <summary>
        /// Gets or sets the identifier of the referenced entity.
        /// </summary>
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the display name of the referenced entity.
        /// </summary>
        public string DisplayName { get; set; }
    }
}
