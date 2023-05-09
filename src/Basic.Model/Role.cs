// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Basic.Model;

/// <summary>
/// Represents the possible roles in the application.
/// </summary>
public class Role : BaseModel
{
    /// <summary>
    /// The right to view and manage all data related to clients.
    /// </summary>
    public const string Clients = "clients";

    /// <summary>
    /// The right to view all data related to clients.
    /// </summary>
    public const string ClientsRO = "clients-ro";

    /// <summary>
    /// The right to view and manage personal schedules and associated events information.
    /// </summary>
    public const string Schedules = "schedules";

    /// <summary>
    /// The right to view personal schedules and associated events information.
    /// </summary>
    public const string SchedulesRO = "schedules-ro";

    /// <summary>
    /// The right to view and manage the users.
    /// </summary>
    public const string Users = "users";

    /// <summary>
    /// The right to change the global options.
    /// </summary>
    public const string Options = "options";

    /// <summary>
    /// The right to view and manage the beta features for a development purpose.
    /// </summary>
    public const string Beta = "beta";

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class.
    /// </summary>
    public Role()
    {
    }

    /// <summary>
    /// Gets or sets the code of the role.
    /// </summary>
    [Required]
    [Unique]
    [MaxLength(50)]
    public string Code { get; set; }
}
