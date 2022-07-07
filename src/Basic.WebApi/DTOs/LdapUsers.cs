using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents a user of the application.
    /// </summary>
    public class LdapUsers : BaseEntityDTO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LdapUsers"/> class.
        /// </summary>
        public LdapUsers()
        {
            this.ListOfLdapUsers = new List<LdapUser>();
        }

        /// <summary>
        /// Gets or sets the list of ldap users.
        /// </summary>
        [Required]
        public List<LdapUser> ListOfLdapUsers { get; set; }

        /// <summary>
        /// Gets or sets the number of occurrence of the specific research.
        /// </summary>
        [Required]
        public int OccurrencesNumber { get; set; }

    }
}
