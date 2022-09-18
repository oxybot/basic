using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage event category data.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class EventCategoriesController : BaseModelController<EventCategory, EventCategoryForList, EventCategoryForList, EventCategoryForEdit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventCategoriesController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public EventCategoriesController(Context context, IMapper mapper, ILogger<EventCategoriesController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>The list of categories.</returns>
        [HttpGet]
        [Authorize]
        [Produces("application/json")]
        public IEnumerable<EventCategoryForList> GetAll()
        {
            return AddIncludesForList(Context.Set<EventCategory>())
                .ToList()
                .Select(e => Mapper.Map<EventCategoryForList>(e));
        }

        /// <summary>
        /// Retrieves a specific category.
        /// </summary>
        /// <param name="identifier">The identifier of the category.</param>
        /// <returns>The detailed data about the category identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No category is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override EventCategoryForList GetOne(Guid identifier)
        {
            return base.GetOne(identifier);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="category">The category data.</param>
        /// <returns>The category data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        public override EventCategoryForList Post(EventCategoryForEdit category)
        {
            return base.Post(category);
        }

        /// <summary>
        /// Updates a specific category.
        /// </summary>
        /// <param name="identifier">The identifier of the category to update.</param>
        /// <param name="category">The category data.</param>
        /// <returns>The category data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">No category is associated to the provided <paramref name="identifier"/>.</response>
        [HttpPut]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override EventCategoryForList Put(Guid identifier, EventCategoryForEdit category)
        {
            return base.Put(identifier, category);
        }

        /// <summary>
        /// Deletes a specific category.
        /// </summary>
        /// <param name="identifier">The identifier of the category to delete.</param>
        /// <response code="404">No category is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete(Guid identifier)
        {
            base.Delete(identifier);
        }

        /// <summary>
        /// Overriden to manage the value of ColorClass.
        /// </summary>
        /// <param name="entity">The received entity.</param>
        /// <param name="model">The associated modei.</param>
        protected override void CheckDependencies(EventCategoryForEdit entity, EventCategory model)
        {
            base.CheckDependencies(entity, model);

            if (model.Mapping == EventTimeMapping.TimeOff)
            {
                model.ColorClass = String.Empty;
            }
        }
    }
}
