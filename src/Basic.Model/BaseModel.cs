// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents the basic set of data for any entity.
    /// </summary>
    public abstract class BaseModel
    {
        /// <summary>
        /// Gets or sets the identifier of the entity.
        /// </summary>
        [Key]
        public Guid Identifier { get; set; }
    }
}
