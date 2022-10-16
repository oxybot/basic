using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.Models
{
    /// <summary>
    /// Represents the definition of a DTO field.
    /// </summary>
    public class DefinitionField
    {
        /// <summary>
        /// Gets or sets the technical name of the field.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display name of the field.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the description associated to the field.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of field.
        /// </summary>
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the field is required.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the placeholder text associated to the field, if any.
        /// </summary>
        public string Placeholder { get; set; }

        /// <summary>
        /// Gets or sets the group's name associated with the field, if any.
        /// </summary>
        public string Group { get; set; }
    }
}
