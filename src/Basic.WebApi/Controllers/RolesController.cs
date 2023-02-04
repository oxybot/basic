// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basic.WebApi.Controllers;

/// <summary>
/// Provides API to retrieve roles data.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class RolesController : BaseController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RolesController"/> class.
    /// </summary>
    /// <param name="context">The datasource context.</param>
    /// <param name="mapper">The configured automapper.</param>
    /// <param name="logger">The associated logger.</param>
    public RolesController(Context context, IMapper mapper, ILogger<RolesController> logger)
        : base(context, mapper, logger)
    {
    }

    /// <summary>
    /// Retrieves all roles.
    /// </summary>
    /// <returns>The list of roles.</returns>
    [HttpGet]
    [Produces("application/json")]
    public ListResult<RoleForList> GetAll()
    {
        var entities = this.Context.Set<Role>()
            .ToList()
            .Select(e => this.Mapper.Map<RoleForList>(e));

        return new ListResult<RoleForList>(entities)
        {
            Total = entities.Count(),
        };
    }
}
