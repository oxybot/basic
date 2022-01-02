﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents a user of the application.
    /// </summary>
    public class User : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            this.Events = new List<Event>();
            this.Balances = new List<Balance>();
        }

        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user, if any.
        /// </summary>
        /// <remarks>
        /// A user without a password can't connect to the application.
        /// </remarks>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the title of the user.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the avatar of the user.
        /// </summary>
        public TypedFile Avatar { get; set; }

        /// <summary>
        /// Gets the associated events.
        /// </summary>
        public ICollection<Event> Events { get; }

        /// <summary>
        /// Gets the associated balances.
        /// </summary>
        public ICollection<Balance> Balances { get; }
    }
}
