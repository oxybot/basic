// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage schedule data.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class SchedulesController : BaseModelController<Schedule, ScheduleForList, ScheduleForView, ScheduleForEdit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulesController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public SchedulesController(Context context, IMapper mapper, ILogger<SchedulesController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all schedules.
        /// </summary>
        /// <param name="filter">The search filter value, if any.</param>
        /// <param name="sortKey">The property to sort on, if any.</param>
        /// <param name="sortValue">The order of the sort (asc or desc), if any.</param>
        /// <returns>The list of schedules.</returns>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time)]
        [Produces("application/json")]
        public IEnumerable<ScheduleForList> GetAll(string filter = "", string sortKey = "", string sortValue = "")
        {
            var entities = this.AddIncludesForList(this.Context.Set<Schedule>())
                .ToList()
                .Select(e => this.Mapper.Map<ScheduleForList>(e));

            switch (sortKey)
            {
                case "User":
                    if (sortValue.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.User.DisplayName);
                    }
                    else if (sortValue.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.User.DisplayName).Reverse();
                    }

                    break;

                case "Active From":
                    if (sortValue.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.ActiveFrom);
                    }
                    else if (sortValue.Equals("desc", StringComparison.OrdinalIgnoreCase))
                    {
                        entities = entities.OrderBy(o => o.ActiveFrom).Reverse();
                    }

                    break;
            }

            return entities;
        }

        /// <summary>
        /// Retrieves a specific schedule.
        /// </summary>
        /// <param name="identifier">The identifier of the schedule.</param>
        /// <returns>The detailed data about the schedule identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No schedule is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override ScheduleForView GetOne(Guid identifier)
        {
            return base.GetOne(identifier);
        }

        /// <summary>
        /// Creates a new schedule.
        /// </summary>
        /// <param name="schedule">The schedule data.</param>
        /// <returns>The schedule data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        public override ScheduleForList Post(ScheduleForEdit schedule)
        {
            return base.Post(schedule);
        }

        /// <summary>
        /// Updates a specific schedule.
        /// </summary>
        /// <param name="identifier">The identifier of the schedule to update.</param>
        /// <param name="schedule">The schedule data.</param>
        /// <returns>The schedule data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">No schedule is associated to the provided <paramref name="identifier"/>.</response>
        [HttpPut]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override ScheduleForList Put(Guid identifier, ScheduleForEdit schedule)
        {
            return base.Put(identifier, schedule);
        }

        /// <summary>
        /// Deletes a specific schedule.
        /// </summary>
        /// <param name="identifier">The identifier of the schedule to delete.</param>
        /// <response code="404">No schedule is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete(Guid identifier)
        {
            base.Delete(identifier);
        }

        /// <summary>
        /// Checks user information.
        /// </summary>
        /// <param name="entity">The form data.</param>
        /// <param name="model">The datasource instance.</param>
        protected override void CheckDependencies(ScheduleForEdit entity, Schedule model)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            else if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            model.User = this.Context.Set<User>().SingleOrDefault(u => u.Identifier == entity.UserIdentifier);
            if (model.User == null || model.WorkingSchedule.ToList().Any(n => n < 0) || model.WorkingSchedule.ToList().All(n => n == 0))
            {
                this.ModelState.AddModelError("UserIdentifier", "Invalid User");
            }

            if (model.WorkingSchedule.Length == 7 || model.WorkingSchedule.Length == 14)
            {
                // Do nothing - the data are already correct
            }
            else if (model.WorkingSchedule.Length < 7)
            {
                model.WorkingSchedule = model.WorkingSchedule.Concat(new decimal[7 - model.WorkingSchedule.Length]).ToArray();
            }
            else if (model.WorkingSchedule.Length < 14)
            {
                model.WorkingSchedule = model.WorkingSchedule.Concat(new decimal[14 - model.WorkingSchedule.Length]).ToArray();
            }
            else if (model.WorkingSchedule.Length > 14)
            {
                this.ModelState.AddModelError("WorkingSchedule", "Invalid schedule data");
            }
        }

        /// <summary>
        /// Ensures that user data are available.
        /// </summary>
        /// <param name="query">The current query.</param>
        /// <returns>The updated query.</returns>
        protected override IQueryable<Schedule> AddIncludesForList(IQueryable<Schedule> query)
        {
            return query.Include(s => s.User);
        }

        /// <summary>
        /// Ensures that user data are available.
        /// </summary>
        /// <param name="query">The current query.</param>
        /// <returns>The updated query.</returns>
        protected override IQueryable<Schedule> AddIncludesForView(IQueryable<Schedule> query)
        {
            return query.Include(s => s.User);
        }
    }
}
