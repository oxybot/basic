// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Threading;

namespace Basic.DataAccess;

/// <summary>
/// Implements a singleton observer class to whom a registration can be made to
/// receive notification on any entity change.
/// </summary>
public class EntityChangeObserver
{
    /// <summary>
    /// The single instance of the class.
    /// </summary>
    private static readonly Lazy<EntityChangeObserver> Lazy = new(() => new EntityChangeObserver());

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityChangeObserver"/> class.
    /// </summary>
    private EntityChangeObserver()
    {
    }

    /// <summary>
    /// The event raised when an entity has changed.
    /// </summary>
    public event EventHandler<EntityChangeEventArgs> Changed;

    /// <summary>
    /// Gets the single instance of the class.
    /// </summary>
    public static EntityChangeObserver Instance => Lazy.Value;

    /// <summary>
    /// Raises the <see cref="Changed"/> event for a specific entity.
    /// </summary>
    /// <param name="e">The entry associated with the changed entity.</param>
    public void OnChanged(EntityChangeEventArgs e)
    {
        ThreadPool.QueueUserWorkItem((_) => this.Changed?.Invoke(this, e));
    }
}
