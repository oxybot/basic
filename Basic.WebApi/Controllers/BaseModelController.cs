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
    public abstract class BaseModelController<TModel, TForList, TForView, TForEdit> : ControllerBase
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
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        /// Retrieves a specific entity.
        /// </summary>
        /// <param name="identifier">The identifier of the entity.</param>
        /// <returns>The detailed data about the entity identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No entity is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [Produces("application/json")]
        [Route("{identifier}")]
        public virtual TForView GetOne(Guid identifier)
        {
            var entity = AddIncludesForView(Context.Set<TModel>()).SingleOrDefault(c => c.Identifier == identifier);
            if (entity == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            return Mapper.Map<TForView>(entity);
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="entity">The entity data.</param>
        /// <returns>The entity data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [Produces("application/json")]
        public virtual TForList Post(TForEdit entity)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            TModel model = Mapper.Map<TModel>(entity);
            CheckDependencies(entity, model);

            Context.Set<TModel>().Add(model);
            Context.SaveChanges();

            return Mapper.Map<TForList>(model);
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
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            var model = AddIncludesForView(Context.Set<TModel>()).SingleOrDefault(e => e.Identifier == identifier);
            if (model == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            Mapper.Map(entity, model);
            CheckDependencies(entity, model);

            Context.SaveChanges();

            return Mapper.Map<TForList>(model);
        }

        /// <summary>
        /// Deletes a specific entity.
        /// </summary>
        /// <param name="identifier">The identifier of the entity to delete.</param>
        /// <response code="404">No entity is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [Produces("application/json")]
        [Route("{identifier}")]
        public virtual void Delete(Guid identifier)
        {
            var entity = Context.Set<TModel>().SingleOrDefault(e => e.Identifier == identifier);
            if (entity == null)
            {
                throw new NotFoundException($"Not existing entity");
            }

            Context.Set<TModel>().Remove(entity);
            Context.SaveChanges();
        }

        /// <summary>
        /// Overriden to add <see cref="EntityFrameworkQueryableExtensions.Include{TEntity, TProperty}(IQueryable{TEntity}, Expression{Func{TEntity, TProperty}})"/>
        /// calls when calling <c>GetAll</c>.
        /// </summary>
        /// <param name="query">The current GetAll query.</param>
        /// <returns>The updated query.</returns>
        protected virtual IQueryable<TModel> AddIncludesForList(IQueryable<TModel> query)
        {
            return query;
        }

        /// <summary>
        /// Overriden to add <see cref="EntityFrameworkQueryableExtensions.Include{TEntity, TProperty}(IQueryable{TEntity}, Expression{Func{TEntity, TProperty}})"/>
        /// calls when calling <see cref="GetOne(Guid)"/>.
        /// </summary>
        /// <param name="query">The current GetOne query.</param>
        /// <returns>The updated query.</returns>
        protected virtual IQueryable<TModel> AddIncludesForView(IQueryable<TModel> query)
        {
            return query;
        }

        /// <summary>
        /// Overriden to check and map the dependencies of the entity.
        /// </summary>
        /// <param name="entity">The entity data.</param>
        /// <param name="model">THe associated model instance.</param>
        /// <exception cref="BadRequestException">Thrown if one of the dependencies is invalid.</exception>
        protected virtual void CheckDependencies(TForEdit entity, TModel model)
        {
        }
    }
}
