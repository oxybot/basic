// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the summarised data of a role.
    /// </summary>
    public class RoleForList : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        [Key]
        [SwaggerSchema("The unique identifier of the role")]
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the code of the role.
        /// </summary>
        [Required]
        [SwaggerSchema("The code of the role")]
        public string Code { get; set; }
    }
}
