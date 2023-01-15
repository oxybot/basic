// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model;

/// <summary>
/// Represents a physical address.
/// </summary>
[Owned]
public class StreetAddress
{
    /// <summary>
    /// Gets or sets the first line of the address.
    /// </summary>
    [MaxLength(255)]
    public string Line1 { get; set; }

    /// <summary>
    /// Gets or sets the second line of the address, if any.
    /// </summary>
    [MaxLength(255)]
    public string Line2 { get; set; }

    /// <summary>
    /// Gets or sets the postal code of the address.
    /// </summary>
    [MaxLength(50)]
    public string PostalCode { get; set; }

    /// <summary>
    /// Gets or sets the city of the address.
    /// </summary>
    [MaxLength(255)]
    public string City { get; set; }

    /// <summary>
    /// Gets or sets the country of the address.
    /// </summary>
    [MaxLength(255)]
    public string Country { get; set; }
}
