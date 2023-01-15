// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.WebApi.DTOs;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Represents the data provided and excepted while executing a CRUD test.
/// </summary>
/// <typeparam name="TForView">The DTO associated with view.</typeparam>
public class TestCRUDModel<TForView>
    where TForView : BaseEntityDTO
{
    /// <summary>
    /// Gets or sets the content used to create a new instance.
    /// </summary>
    public object CreateContent { get; set; }

    /// <summary>
    /// Gets or sets the content expected after the creation of a new instance
    /// with <see cref="CreateContent"/>.
    /// </summary>
    public TForView CreateExpected { get; set; }

    /// <summary>
    /// Gets or sets the content used to update an existing instance.
    /// </summary>
    /// <remarks>
    /// If <c>null</c> the update part of the test will not be executed.
    /// </remarks>
    public object UpdateContent { get; set; }

    /// <summary>
    /// Gets or sets the content expected after the update of an existing instance
    /// with <see cref="UpdateContent"/>.
    /// </summary>
    public TForView UpdateExpected { get; set; }
}
