// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Basic.WebApi.Framework;

/// <summary>
/// Raises a <c>403 - Forbidden</c> http error code.
/// </summary>
/// <remarks>
/// This exception doesn't contains a constructor with a message to avoid information leakage.
/// </remarks>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Message and error code are fixed to avoid information leakage")]
public class ForbiddenRequestException : BadHttpRequestException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenRequestException"/> class.
    /// </summary>
    public ForbiddenRequestException()
        : base("Forbidden", StatusCodes.Status403Forbidden)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenRequestException"/> class.
    /// </summary>
    /// <param name="inner">The underlying error description.</param>
    public ForbiddenRequestException(Exception inner)
        : base("Forbidden", StatusCodes.Status403Forbidden, inner)
    {
    }
}
