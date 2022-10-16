// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the summarised data of a client.
    /// </summary>
    public class ClientForList : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the client.
        /// </summary>
        [Key]
        [SwaggerSchema("The unique identifier of the client")]
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the name of the client as displayed in the interface.
        /// </summary>
        [Required]
        [Display(Prompt = "Contoso", Description = "The usage name of the client")]
        [SwaggerSchema("The usage name of the client")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the name of the client as displayed in official papers.
        /// </summary>
        [Required]
        [Display(Prompt = "Contoso Limited")]
        [SwaggerSchema("The name of the client as displayed in official papers")]
        public string FullName { get; set; }
    }
}
