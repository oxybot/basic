﻿// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Basic.WebApi.Services;

/// <summary>
/// Defines the configuration options to send notification emails.
/// </summary>
/// <remarks>
/// This configuration is used for notifications.
/// </remarks>
public class EmailServiceOptions
{
    /// <summary>
    /// Gets the default section used to extract the options in the configuration.
    /// </summary>
    public const string Section = "EmailService";

    /// <summary>
    /// Gets or sets the server name of the SMTP server.
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    /// Gets or sets the port used to connect to the server.
    /// </summary>
    public int Port { get; set; } = 587;

    /// <summary>
    /// Gets or sets a value indicating whether the connection to the server uses SSL.
    /// </summary>
    public bool Secure { get; set; }

    /// <summary>
    /// Gets or sets the display name of the sender.
    /// </summary>
    public string SenderName { get; set; }

    /// <summary>
    /// Gets or sets the email of the sender.
    /// </summary>
    public string SenderEmail { get; set; }

    /// <summary>
    /// Gets or sets the base url for the front project.
    /// </summary>
    [SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "Part of user options")]
    public string FrontBaseUrl { get; set; }
}
