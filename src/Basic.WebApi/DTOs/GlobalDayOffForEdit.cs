// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data of a global day off definition.
    /// </summary>
    public class GlobalDayOffForEdit : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the date of the day-off.
        /// </summary>
        [Required]
        public DateOnly? Date { get; set; }

        /// <summary>
        /// Gets or sets the description of the day off, if any.
        /// </summary>
        public string Description { get; set; }
    }
}
