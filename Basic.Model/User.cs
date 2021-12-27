using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents a user of the application.
    /// </summary>
    public class User : BaseModel
    {
        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the user name of the user.
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the title of the user.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the avatar of the user.
        /// </summary>
        public File Avatar { get; set; }
    }
}
