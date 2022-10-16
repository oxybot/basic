// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data of an agreement.
    /// </summary>
    public class AgreementForList : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the agreement.
        /// </summary>
        [Key]
        [SwaggerSchema("The unique identifier of the agreement")]
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the internal code of the agreement.
        /// </summary>
        [Required]
        public string InternalCode { get; set; }

        /// <summary>
        /// Gets or sets the title of the agreement.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the reference to the associated client.
        /// </summary>
        [SwaggerSchema(Format = "ref/client")]
        public EntityReference Client { get; set; }

        /// <summary>
        /// Gets or sets the signature date of the agreement.
        /// </summary>
        [SwaggerSchema(Format = "date")]
        public DateTime? SignatureDate { get; set; }
    }
}
