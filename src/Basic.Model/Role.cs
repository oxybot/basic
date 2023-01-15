// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model;

/// <summary>
/// Represents the possible roles in the application.
/// </summary>
public class Role : BaseModel
{
    /// <summary>
    /// The right to view all data related to clients.
    /// </summary>
    public const string ClientRO = "client-ro";

    /// <summary>
    /// The right to view and manage all data related to clients.
    /// </summary>
    public const string Client = "client";

    /// <summary>
    /// The right to view time management information.
    /// </summary>
    public const string TimeRO = "time-ro";

    /// <summary>
    /// The right to view and manage time management information.
    /// </summary>
    public const string Time = "time";

    /// <summary>
    /// The right to view and manage the users.
    /// </summary>
    public const string User = "user";

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
        this.Users = new List<User>();
    }

    /// <summary>
    /// Gets or sets the code of the role.
    /// </summary>
    [Required]
    [Unique]
    [MaxLength(50)]
    public string Code { get; set; }

    /// <summary>
    /// Gets the users with this role assigned.
    /// </summary>
    public virtual ICollection<User> Users { get; }
}
