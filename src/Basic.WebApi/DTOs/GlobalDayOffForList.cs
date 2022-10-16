// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data of a global day off definition.
    /// </summary>
    public class GlobalDayOffForList : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the day off.
        /// </summary>
        [Key]
        [SwaggerSchema("The unique identifier of the category")]
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the date of the day-off.
        /// </summary>
        [Required]
        public DateOnly Date { get; set; }

        /// <summary>
        /// Gets or sets the description of the day off, if any.
        /// </summary>
        public string Description { get; set; }
    }
}
