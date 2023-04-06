// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model;

/// <summary>
/// Represents a status as part of a workflow that could be applied to an entity.
/// </summary>
public class Status : BaseModel, IComparable<Status>
{
    /// <summary>
    /// The identifier of the requested status.
    /// </summary>
    public static readonly Guid Requested = new("52bc6354-d8ef-44e2-87ca-c64deeeb22e8");

    /// <summary>
    /// The identifier of the approved status.
    /// </summary>
    public static readonly Guid Approved = new("4151c014-ddde-43e4-aa7e-b98a339bbe74");

    /// <summary>
    /// The identifier of the rejected status.
    /// </summary>
    public static readonly Guid Rejected = new("e7f8dcc7-57d5-4e74-ac38-1fbd5153996c");

    /// <summary>
    /// The identifier of the canceled status.
    /// </summary>
    public static readonly Guid Canceled = new("fdac7cc3-3fe0-4e59-ab16-aeaec008f940");

    /// <summary>
    /// Gets or sets the name of the status - as displayed.
    /// </summary>
    [Required]
    [Unique]
    [MaxLength(50)]
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the description of the status, if any.
    /// </summary>
    [MaxLength(255)]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the status is associated with active entities.
    /// </summary>
    [Required]
    public bool IsActive { get; set; }

    /// <inheritdoc />
    public int CompareTo(Status other)
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
