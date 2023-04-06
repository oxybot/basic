// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;

namespace Basic.WebApi.Framework;

/// <summary>
/// Raises a <c>400</c> http error code.
/// </summary>
/// <remarks>
/// This exception doesn't contains a constructor with a message to avoid information leakage.
/// </remarks>
[SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Message and error code are fixed to avoid information leakage")]
public class InvalidModelStateException : BadHttpRequestException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidModelStateException"/> class.
    /// </summary>
    /// <param name="modelState">The current modal state.</param>
    public InvalidModelStateException(ModelStateDictionary modelState)
        : base("Bad Request", StatusCodes.Status400BadRequest)
    {
        this.ModelState = modelState ?? throw new ArgumentNullException(nameof(modelState));
    }

    /// <summary>
    /// Gets the model state used as a reference for the Json payload.
    /// </summary>
    public ModelStateDictionary ModelState { get; }
}
