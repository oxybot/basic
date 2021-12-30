﻿using Basic.Model;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the event category data.
    /// </summary>
    public class EventCategoryForList : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the category.
        /// </summary>
        [Key]
        [SwaggerSchema("The unique identifier of the category")]
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the display name of the category.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a balance is required to book time on this category.
        /// </summary>
        [Required]
        public bool RequireBalance { get; set; }

        /// <summary>
        /// Gets or sets how the time booked on this category should be considered.
        /// </summary>
        [Required]
        public EventTimeMapping Mapping { get; set; }
    }
}
