// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents days-off that apply to every users.
    /// </summary>
    /// <remarks>
    /// Can be used to represent public holidays for instance.
    /// </remarks>
    [Index(nameof(Date), IsUnique = true)]
    public class GlobalDayOff : BaseModel
    {
        /// <summary>
        /// Gets or sets the reference date.
        /// </summary>
        [Required]
        public DateOnly Date { get; set; }

        /// <summary>
        /// Gets or sets the associated description, if any.
        /// </summary>
        [MaxLength(255)]
        public string Description { get; set; }
    }
}
