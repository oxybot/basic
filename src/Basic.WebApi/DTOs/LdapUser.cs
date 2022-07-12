using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents a ldap user of the application.
    /// </summary>
    public class LdapUser : BaseEntityDTO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LdapUser"/> class.
        /// </summary>
        public LdapUser()
        {
            this.Importable = true;
        }

        /// <summary>
        /// Gets or sets the display name of the ldap user.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the username of the ldap user.
        /// </summary>
        [Required]
        public string UserName { get; set; }
        
        /// <summary>
        /// Gets or sets the e-mail of the ldap user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the title of the ldap user.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the avatar of the ldap user.
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Gets or sets importable status of the ldap user.
        /// </summary>
        public bool Importable { get; set; }
    }
}
