// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data of a user.
    /// </summary>
    public class MyUserForView : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        [SwaggerSchema("The display name of the user")]
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the user name of the user.
        /// </summary>
        [SwaggerSchema("The user name of the user")]
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the e-mail name of the user.
        /// </summary>
        [SwaggerSchema("The e-mail of the user")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the title of the user.
        /// </summary>
        [SwaggerSchema("The title of the user")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the avatar of the user.
        /// </summary>
        [Display(Order = 1)]
        [SwaggerSchema(Format = "image")]
        public Base64File Avatar { get; set; }

        /// <summary>
        /// Gets or sets the external identifier of the user, if any.
        /// </summary>
        [SwaggerSchema("The external identifier of the user")]
        public string ExternalIdentifier { get; set; }
    }
}
