// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data of a user.
    /// </summary>
    public class AttachmentForEdit : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the display name of the attachment.
        /// </summary>
        [SwaggerSchema("The display name of the attachment")]
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the attachment content.
        /// </summary>
        [SwaggerSchema(Format = "image")]
        [Required]
        public Base64File AttachmentContent { get; set; }
    }
}
