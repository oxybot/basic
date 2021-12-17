using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.Models
{
    [SwaggerSchema(Title = "SimpleClientContract")]
    public class SimpleClientContractDTO : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the contract.
        /// </summary>
        [SwaggerSchema("The unique identifier of the contract")]
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
        /// Gets or sets the internal code associated to the contract.
        /// </summary>
        [Required]
        public string InternalCode { get; set; }

        /// <summary>
        /// Gets or sets the title of the contract.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the signature date of the contract.
        /// </summary>
        [SwaggerSchema(Format = "date")]
        public DateTime? SignatureDate { get; set; }
    }
}
