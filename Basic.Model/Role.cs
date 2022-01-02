using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents the possible roles in the application.
    /// </summary>
    public class Role : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        public Role()
        {
            this.Users = new List<User>();
        }

        /// <summary>
        /// Gets or sets the code of the role.
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Gets the users with this role assigned.
        /// </summary>
        public virtual ICollection<User> Users { get; }
    }
}
