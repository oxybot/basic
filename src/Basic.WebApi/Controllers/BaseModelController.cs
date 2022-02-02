using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage data associated to a specific entity.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseModelController<TModel, TForList, TForView, TForEdit>
        : BaseImmutableModelController<TModel, TForList, TForView, TForEdit>
        where TModel : BaseModel
        where TForList : BaseEntityDTO
        where TForView : BaseEntityDTO
        where TForEdit : BaseEntityDTO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseModelController{TModel, TForList, TForView, TForEdit}"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BaseModelController(Context context, IMapper mapper, ILogger logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Updates a specific entity.
        /// </summary>
        /// <param name="identifier">The identifier of the entity to update.</param>
        /// <param name="entity">The entity data.</param>
        /// <returns>The entity data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">No entity is associated to the provided <paramref name="identifier"/>.</response>
        [HttpPut]
        [Produces("application/json")]
        [Route("{identifier}")]
        public virtual TForList Put(Guid identifier, TForEdit entity)
        {
            var model = AddIncludesForView(Context.Set<TModel>()).SingleOrDefault(e => e.Identifier == identifier);
            if (model == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            Mapper.Map(entity, model);

            CheckDependencies(entity, model);
            if (!ModelState.IsValid)
            {
                throw new InvalidModelStateException(ModelState);
            }

            Context.SaveChanges();

            return Mapper.Map<TForList>(model);
        }
    }
}
