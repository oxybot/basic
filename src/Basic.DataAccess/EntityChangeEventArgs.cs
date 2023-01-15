// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace Basic.DataAccess;

/// <summary>
/// Represents a change of entity in the database.
/// </summary>
public class EntityChangeEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityChangeEventArgs"/> class.
    /// </summary>
    /// <param name="entry">The entry associated with the updated entity.</param>
    public EntityChangeEventArgs(EntityEntry entry)
    {
        this.Entry = entry;
    }

    /// <summary>
    /// Gets the entry associated with the updated entity.
    /// </summary>
    public EntityEntry Entry { get; }
}
