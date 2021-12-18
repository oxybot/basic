using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.Models
{
    /// <summary>
    /// Represents the data of an agreement.
    /// </summary>
    [SwaggerSchema(Title = "SimpleAgreement")]
    public class SimpleAgreementDTO : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the agreement.
        /// </summary>
        [SwaggerSchema("The unique identifier of the agreement")]
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated client.
        /// </summary>
        [Required]
        public Guid ClientIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the display name of the associated client.
        /// </summary>
        [Required]
        public string ClientDisplayName { get; set; }

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
        /// Gets or sets the signature date of the agreement.
        /// </summary>
        [SwaggerSchema(Format = "date")]
        public DateTime? SignatureDate { get; set; }
    }
}
