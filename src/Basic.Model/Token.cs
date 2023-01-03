// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents a validated token for an user.
    /// </summary>
    public class Token : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        public Token()
        {
        }

        /// <summary>
        /// Gets or sets the expiration time.
        /// </summary>
        [Required]
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Gets or sets the user's token.
        /// </summary>
        [Required]
        public virtual User User { get; set; }
    }
}
