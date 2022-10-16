// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data of a client.
    /// </summary>
    public class ClientForView : BaseEntityDTO
    {
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

        /// <summary>
        /// Gets or sets the first line of the client address.
        /// </summary>
        [Display(GroupName = "Address")]
        [SwaggerSchema("The first line of the client address")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the first line of the client address.
        /// </summary>
        [Display(GroupName = "Address")]
        [SwaggerSchema("The second line of the client address")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the postal code of the client address.
        /// </summary>
        [Display(GroupName = "Address")]
        [SwaggerSchema("The postal code of the client address")]
        public string AddressPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the city of the client address.
        /// </summary>
        [Display(GroupName = "Address")]
        [SwaggerSchema("The city of the client address")]
        public string AddressCity { get; set; }

        /// <summary>
        /// Gets or sets the country of the client address.
        /// </summary>
        [Display(GroupName = "Address")]
        [SwaggerSchema("The country of the client address")]
        public string AddressCountry { get; set; }
    }
}
