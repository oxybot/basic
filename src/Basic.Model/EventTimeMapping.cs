// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Basic.Model;

/// <summary>
/// Represents the various possibility to compute time booking.
/// </summary>
public enum EventTimeMapping
{
    /// <summary>
    /// Indicates that the associated time booking is to be considered as time-off.
    /// </summary>
    /// <remarks>
    /// <para>Can be used to flag holidays, sickness or other special leaves.</para>
    /// <para>Will be accounted by business hours.</para>
    /// <para>A user should not have two time-offs events at the same time.</para>
    /// </remarks>
    TimeOff = 0,

    /// <summary>
    /// Indicates that the associated time booking is not to be considered as time-off but as
    /// normal activities.
    /// </summary>
    /// <remarks>
    /// <para>Can be used to flag special events like remote work...</para>
    /// <para>Will be accounted by business hours.</para>
    /// </remarks>
    Active,

    /// <summary>
    /// Indicates that the associated time booking is linked to normal activities that
    /// doesn't have to be accounted.
    /// </summary>
    /// <remarks>
    /// <para>Can be used to flag special events like travel, on-calls...</para>
    /// <para>Will be displayed on the calendard based on their start and end dates.</para>
    /// </remarks>
    Informational,
}
