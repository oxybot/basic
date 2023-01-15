// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.Model;

namespace Basic.DataAccess;

/// <summary>
/// Extension methods for the <see cref="Context"/> class.
/// </summary>
public static class ContextExtensions
{
    /// <summary>
    /// Retrieves a specific status.
    /// </summary>
    /// <param name="context">The current datasource context.</param>
    /// <param name="displayName">The display name of the status to retrieve.</param>
    /// <returns>The <see cref="Status"/> instance identified by <paramref name="displayName"/>.</returns>
    public static Status GetStatus(this Context context, string displayName)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        return context.Set<Status>()
            .Single(s => s.DisplayName == displayName);
    }
}
