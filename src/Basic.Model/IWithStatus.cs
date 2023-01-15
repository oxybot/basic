// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;

namespace Basic.Model;

/// <summary>
/// Indicates that the associated model instances are linked to a workflow.
/// </summary>
/// <typeparam name="TStatus">The concrete type of status.</typeparam>
public interface IWithStatus<TStatus>
    where TStatus : BaseModelStatus
{
    /// <summary>
    /// Gets the history of status associated with a model instance.
    /// </summary>
    public ICollection<TStatus> Statuses { get; }
}
