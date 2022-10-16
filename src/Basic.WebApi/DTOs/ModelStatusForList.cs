// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data of a model status.
    /// </summary>
    public class ModelStatusForList : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the status.
        /// </summary>
        [Key]
        [SwaggerSchema("The unique identifier of the status history")]
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the status of the associated entity.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "ref/status")]
        public StatusReference Status { get; set; }

        /// <summary>
        /// Gets or sets the user that did the update of status.
        /// </summary>
        [Required]
        [SwaggerSchema(Format = "ref/user")]
        public UserReference UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the update.
        /// </summary>
        [Required]
        public DateTime UpdatedOn { get; set; }
    }
}
