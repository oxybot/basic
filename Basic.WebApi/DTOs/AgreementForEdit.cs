using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data of an agreement.
    /// </summary>
    public class AgreementForEdit : BaseEntityDTO
    {
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
        /// Gets or sets the identifier of the associated client.
        /// </summary>
        [Required]
        [Display(Name = "Client")]
        [SwaggerSchema(Format = "ref/client")]
        public Guid ClientIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the signature date of the agreement.
        /// </summary>
        [SwaggerSchema(Format = "date")]
        public DateTime? SignatureDate { get; set; }

        /// <summary>
        /// Gets or sets the private notes associated to the agreement.
        /// </summary>
        public string PrivateNotes { get; set; }

        /// <summary>
        /// Gets the associated items.
        /// </summary>
        [SwaggerSchema(ReadOnly = false)]
        public ICollection<AgreementItemForEditWithIdentifier> Items { get; set; }
    }
}
