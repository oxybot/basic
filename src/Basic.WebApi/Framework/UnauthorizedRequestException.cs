// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace Basic.WebApi.Framework
{
    /// <summary>
    /// Raises a <c>401 - Unauthorized</c> http error code.
    /// </summary>
    /// <remarks>
    /// This exception doesn't contains a constructor with a message to avoid information leakage.
    /// </remarks>
    public class UnauthorizedRequestException : BadHttpRequestException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedRequestException"/> class.
        /// </summary>
        public UnauthorizedRequestException()
            : base("Unauthorized", 401) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedRequestException"/> class.
        /// </summary>
        /// <param name="inner">The underlying error description.</param>
        public UnauthorizedRequestException(Exception inner)
            : base("Unauthorized", 401, inner) { }
    }
}
