using Basic.Model;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the event category data.
    /// </summary>
    public class EventCategoryForEdit : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the display name of the category.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a balance is required to book time on this category.
        /// </summary>
        [Required]
        [Display(Description = "With a balance, this type of events will be capped for each user to the maximum of their balance.")]
        [SwaggerSchema("With a balance, this type of events will be capped for each user to the maximum of their balance.")]
        public bool? RequireBalance { get; set; }

        /// <summary>
        /// Gets or sets how the time booked on this category should be considered.
        /// </summary>
        [Required]
        [Display(Description = "Choose 'Time-off' for an event that is linked to the absence of the user.")]
        [SwaggerSchema(Format = "ref/eventtimemapping", Description = "Choose 'Time-off' for an event that is linked to the absence of the user.")]
        public EventTimeMapping? Mapping { get; set; }

        /// <summary>
        /// Gets or sets the css class associated to the category.
        /// </summary>
        [RequiredWhen(nameof(Mapping), EventTimeMapping.Active)]
        [SwaggerSchema(Format = "color")]
        public string ColorClass { get; set; }
    }
}
