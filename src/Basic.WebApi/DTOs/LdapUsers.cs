// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

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
            this.ListOfLdapUsers = new List<ExternalUser>();
        }

        /// <summary>
        /// Gets the list of ldap users.
        /// </summary>
        [Required]
        public ICollection<ExternalUser> ListOfLdapUsers { get; }

        /// <summary>
        /// Gets or sets the number of occurrence of the specific research.
        /// </summary>
        [Required]
        public int OccurrencesNumber { get; set; }
    }
}
