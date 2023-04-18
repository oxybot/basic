// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents the detail of a balance.
    /// </summary>
    public class BalanceItem : BaseModel
    {
        /// <summary>
        /// Gets or sets the parent entity.
        /// </summary>
        [Required]
        public virtual Balance Balance { get; set; }

        /// <summary>
        /// Gets or sets the display order of this item.
        /// </summary>
        [Required]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the description of the detailed item.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the associated value.
        /// </summary>
        [Required]
        public decimal Value { get; set; }
    }
}
