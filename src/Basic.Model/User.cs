// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents a user of the application.
    /// </summary>
    public class User : BaseModel, IWithAttachments<UserAttachment>, IComparable<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            this.Roles = new List<Role>();
            this.Events = new List<Event>();
            this.Balances = new List<Balance>();
            this.Schedules = new List<Schedule>();
            this.Attachments = new List<UserAttachment>();
        }

        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active or not.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the e-mail of the user.
        /// </summary>
        [MaxLength(255)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user, if any.
        /// </summary>
        [MaxLength(255)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the salt value generated for this user.
        /// </summary>
        /// <remarks>
        /// This value is 'internal' and should not be displayed or changed by the user.
        /// </remarks>
        [MaxLength(255)]
        public string Salt { get; set; }

        /// <summary>
        /// Gets or sets the title of the user.
        /// </summary>
        [MaxLength(255)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the avatar of the user.
        /// </summary>
        public TypedFile Avatar { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user in the external authenticator.
        /// </summary>
        [MaxLength(255)]
        public string ExternalIdentifier { get; set; }

        /// <summary>
        /// Gets the associated roles.
        /// </summary>
        public virtual ICollection<Role> Roles { get; }

        /// <summary>
        /// Gets the associated events.
        /// </summary>
        public virtual ICollection<Event> Events { get; }

        /// <summary>
        /// Gets the associated balances.
        /// </summary>
        public virtual ICollection<Balance> Balances { get; }

        /// <summary>
        /// Gets the associated working schedules.
        /// </summary>
        public virtual ICollection<Schedule> Schedules { get; }

        /// <summary>
        /// Gets the list of the attachments.
        /// </summary>
        public virtual ICollection<UserAttachment> Attachments { get; }

        /// <summary>
        /// Gets the list of the tokens.
        /// </summary>
        public virtual ICollection<Token> Tokens { get; }

        /// <inheritdoc />
        public int CompareTo(User other)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                return string.Compare(this.DisplayName, other.DisplayName, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
