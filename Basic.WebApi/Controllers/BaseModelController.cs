using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage data associated to a specific entity.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseModelController<TModel, TSimpleDTO, TStandardDTO> : ControllerBase
        where TModel : BaseModel
        where TSimpleDTO : BaseEntityDTO
        where TStandardDTO : BaseEntityDTO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseModelController{TModel, TItemDTO, TStandardDTO}"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BaseModelController(Context context, IMapper mapper, ILogger<BaseModelController<TModel, TSimpleDTO, TStandardDTO>> logger)
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
        protected ILogger<BaseModelController<TModel, TSimpleDTO, TStandardDTO>> Logger { get; }

        protected virtual IQueryable<TModel> SimpleAddIncludes(IQueryable<TModel> query)
        {
            return query;
        }

        /// <summary>
        /// Retrieves a specific entity.
        /// </summary>
        /// <param name="identifier">The identifier of the entity.</param>
        /// <returns>The detailed data about the entity identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No entity is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [Produces("application/json")]
        [Route("{identifier}")]
        public virtual TStandardDTO GetOne(Guid identifier)
        {
            var entity = StandardAddIncludes(Context.Set<TModel>()).SingleOrDefault(c => c.Identifier == identifier);
            if (entity == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            return Mapper.Map<TStandardDTO>(entity);
        }

        protected virtual IQueryable<TModel> StandardAddIncludes(IQueryable<TModel> query)
        {
            return query;
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="entity">The entity data.</param>
        /// <returns>The entity data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [Produces("application/json")]
        public virtual TSimpleDTO Post(TStandardDTO entity)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            TModel model = Mapper.Map<TModel>(entity);
            return PostCore(model);
        }

        protected TSimpleDTO PostCore(TModel model)
        {
            Context.Set<TModel>().Add(model);
            Context.SaveChanges();

            return Mapper.Map<TSimpleDTO>(model);
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
        public virtual TSimpleDTO Put(Guid identifier, TStandardDTO entity)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            var model = SimpleAddIncludes(Context.Set<TModel>()).SingleOrDefault(e => e.Identifier == identifier);
            if (model == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            Mapper.Map(entity, model);
            Context.SaveChanges();

            return Mapper.Map<TSimpleDTO>(model);
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
    }
}
