// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace Basic.WebApi.Models
{
    /// <summary>
    /// Represents a bad request result as provided to the user.
    /// </summary>
    /// <example>
    /// <code>
    /// {
    ///   "": [ "Global error" ],
    ///   "field1": [ "The Field 1 is required" ],
    ///   "field2": [ "The Field 2 shoulc be a positive value" ],
    /// }
    /// </code>
    /// </example>
    public class InvalidResult : Dictionary<string, IEnumerable<string>>
    {
    }
}
