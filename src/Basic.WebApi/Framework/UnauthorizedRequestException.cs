// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Basic.WebApi.Framework;

/// <summary>
/// Raises a <c>401 - Unauthorized</c> http error code.
/// </summary>
/// <remarks>
/// This exception doesn't contains a constructor with a message to avoid information leakage.
/// </remarks>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Message and error code are fixed to avoid information leakage")]
public class UnauthorizedRequestException : BadHttpRequestException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedRequestException"/> class.
    /// </summary>
    public UnauthorizedRequestException()
        : base("Unauthorized", StatusCodes.Status401Unauthorized)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedRequestException"/> class.
    /// </summary>
    /// <param name="inner">The underlying error description.</param>
    public UnauthorizedRequestException(Exception inner)
        : base("Unauthorized", StatusCodes.Status401Unauthorized, inner)
    {
    }
}
