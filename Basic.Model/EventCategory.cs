using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents a category of event.
    /// </summary>
    public class EventCategory : BaseModel
    {
        /// <summary>
        /// Gets or sets the display name of the category.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a balance is required to book time on this category.
        /// </summary>
        public bool RequireBalance { get; set; }

        /// <summary>
        /// Gets or sets the css class associated to this category.
        /// </summary>
        [Required]
        public string ColorClass { get; set; }

        /// <summary>
        /// Gets or sets how the time booked on this category should be considered.
        /// </summary>
        public EventTimeMapping Mapping { get; set; }
    }
}
