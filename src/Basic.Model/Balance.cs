// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model;

/// <summary>
/// Represents the user balance for a specific year and a specific category of event.
/// </summary>
public class Balance : BaseModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Balance"/> class.
    /// </summary>
    public Balance()
    {
        this.Details = new List<BalanceItem>();
    }

    /// <summary>
    /// Gets or sets the associated user.
    /// </summary>
    [Required]
    public virtual User User { get; set; }

    /// <summary>
    /// Gets or sets the associated category.
    /// </summary>
    [Required]
    public virtual EventCategory Category { get; set; }

    /// <summary>
    /// Gets or sets the reference year.
    /// </summary>
    [Required]
    public int Year { get; set; }

    /// <summary>
    /// Gets or sets the defined standard allowance for this year, in hours.
    /// </summary>
    [Required]
    public decimal Allowed { get; set; }

    /// <summary>
    /// Gets the details of the balance, if any.
    /// </summary>
    /// <remarks>
    /// If the balance has detailed items, the <see cref="Allowed"/> value is expected
    /// to be the total value of all detailed items.
    /// </remarks>
    public virtual ICollection<BalanceItem> Details { get; }
}
