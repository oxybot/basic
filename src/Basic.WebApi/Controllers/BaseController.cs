// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Provides API to retrieve and manage data associated to a specific entity.
/// </summary>
/// <remarks>
/// This class doesn't contain an <c>Edit</c> method. See <see cref="BaseModelController{TModel, TForList, TForView, TForEdit}"/>.
/// </remarks>
[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseController"/> class.
    /// </summary>
    /// <param name="context">The datasource context.</param>
    /// <param name="mapper">The configured automapper.</param>
    /// <param name="logger">The associated logger.</param>
    protected BaseController(Context context, IMapper mapper, ILogger logger)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
        this.Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets the datasource context.
    /// </summary>
    protected Context Context { get; }

    /// <summary>
    /// Gets the configured automapper.
    /// </summary>
    protected IMapper Mapper { get; }

    /// <summary>
    /// Gets the associated logger.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Retrieves the <see cref="User"/> instance associated with the connected user.
    /// </summary>
    /// <returns>A <see cref="User"/> instance; or <c>null</c>.</returns>
    protected User GetConnectedUser()
    {
        var userIdClaim = this.User.Claims.SingleOrDefault(c => c.Type == "sid:guid");
        if (userIdClaim == null)
        {
            return null;
        }

        var userId = Guid.Parse(userIdClaim.Value);
        var user = this.Context.Set<User>()
            .Include(u => u.Roles)
            .SingleOrDefault(u => u.Identifier == userId);

        return user;
    }
}
