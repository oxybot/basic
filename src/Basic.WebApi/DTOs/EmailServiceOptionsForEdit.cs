// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.Services;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the form entry associated with the <see cref="EmailServiceOptions"/> class.
    /// </summary>
    public class EmailServiceOptionsForEdit : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the server name of the SMTP server.
        /// </summary>
        [Required]
        [Description("The dns name or ip of the SMTP server used to send notifications")]
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the port used to connect to the server.
        /// </summary>
        [Required]
        [Description("The port associated with the SMTP service")]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the connection to the server uses SSL.
        /// </summary>
        [Required]
        public bool Secure { get; set; }

        /// <summary>
        /// Gets or sets the display name of the sender.
        /// </summary>
        [Required]
        public string SenderName { get; set; }

        /// <summary>
        /// Gets or sets the email of the sender.
        /// </summary>
        [Required]
        public string SenderEmail { get; set; }

        /// <summary>
        /// Gets or sets the base url for the front project.
        /// </summary>
        [Required]
        [SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "Part of user options")]
        public string FrontBaseUrl { get; set; }
    }
}
