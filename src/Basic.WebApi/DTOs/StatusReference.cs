// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace Basic.WebApi.DTOs;

/// <summary>
/// Represents a link from a DTO to a status.
/// </summary>
public class StatusReference : EntityReference
{
    /// <summary>
    /// Gets or sets the description of the status, if any.
    /// </summary>
    public string Description { get; set; }
}
