// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Basic.WebApi.Models;
using Basic.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage global days off data.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class GlobalDaysOffController : BaseModelController<GlobalDayOff, GlobalDayOffForList, GlobalDayOffForList, GlobalDayOffForEdit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalDaysOffController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public GlobalDaysOffController(Context context, IMapper mapper, ILogger<GlobalDaysOffController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all days-off.
        /// </summary>
        /// <param name="definitions">The service providing the entity definitions.</param>
        /// <param name="sortAndFilter">The sort and filter options, is any.</param>
        /// <returns>The list of days-off.</returns>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time)]
        [Produces("application/json")]
        public IEnumerable<GlobalDayOffForList> GetAll([FromServices] DefinitionsService definitions, [FromQuery] SortAndFilterModel sortAndFilter)
        {
            var entities = this.GetAllCore(definitions, sortAndFilter)
                .ToList()
                .Select(e => this.Mapper.Map<GlobalDayOffForList>(e));

            return entities;
        }

        /// <summary>
        /// Retrieves a specific day-off.
        /// </summary>
        /// <param name="identifier">The identifier of the day-off.</param>
        /// <returns>The detailed data about the day-off identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No day-off is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override GlobalDayOffForList GetOne(Guid identifier)
        {
            return base.GetOne(identifier);
        }

        /// <summary>
        /// Creates a new global day-off.
        /// </summary>
        /// <param name="category">The day-off data.</param>
        /// <returns>The day-off data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        public override GlobalDayOffForList Post(GlobalDayOffForEdit category)
        {
            return base.Post(category);
        }

        /// <summary>
        /// Updates a specific day-off.
        /// </summary>
        /// <param name="identifier">The identifier of the day-off to update.</param>
        /// <param name="category">The day-off data.</param>
        /// <returns>The day-off data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">No day-off is associated to the provided <paramref name="identifier"/>.</response>
        [HttpPut]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override GlobalDayOffForList Put(Guid identifier, GlobalDayOffForEdit category)
        {
            return base.Put(identifier, category);
        }

        /// <summary>
        /// Deletes a specific day-off.
        /// </summary>
        /// <param name="identifier">The identifier of the day-off to delete.</param>
        /// <response code="404">No day-off is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete(Guid identifier)
        {
            base.Delete(identifier);
        }
    }
}
