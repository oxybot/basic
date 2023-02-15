// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace Basic.WebApi.Framework;

/// <summary>
/// Raises a <c>403 - Forbidden</c> http error code.
/// </summary>
/// <remarks>
/// This exception doesn't contains a constructor with a message to avoid information leakage.
/// </remarks>
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
