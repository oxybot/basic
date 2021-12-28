using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the data of a user.
    /// </summary>
    public class UserForList : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        [Key]
        [SwaggerSchema("The unique identifier of the user")]
        public Guid Identifier { get; set; }

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
        /// Gets or sets the title of the user.
        /// </summary>
        [SwaggerSchema("The title of the user")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the avatar of the user.
        /// </summary>
        public Base64File Avatar { get; set; }
    }
}
