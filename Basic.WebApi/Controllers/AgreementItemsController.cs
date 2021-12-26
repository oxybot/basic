using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage agreement items data.
    /// </summary>
    [Route("Agreements/{agreementId}/items")]
    [ApiController]
    public class AgreementItemsController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgreementItemsController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public AgreementItemsController(Context context, IMapper mapper, ILogger<AgreementItemsController> logger)
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
        protected ILogger<AgreementItemsController> Logger { get; }

        /// <summary>
        /// Retrieves all agreement items for a specific agreement.
        /// </summary>
        /// <param name="agreementId">The identifier of the agreement.</param>
        /// <returns>The list of associated agreement items.</returns>
        /// <response code="404">The <paramref name="agreementId"/> is not associated to any agreement.</response>
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<AgreementItemForList> GetAll([FromRoute] Guid agreementId)
        {
            var agreement = Context.Set<Agreement>().SingleOrDefault(e => e.Identifier == agreementId);
            if (agreement == null)
            {
                throw new NotFoundException("Unknown agreement");
            }

            var items = Context.Set<AgreementItem>().Where(i => i.Agreement == agreement);

            return items
                .Select(e => Mapper.Map<AgreementItemForList>(e));
        }

        /// <summary>
        /// Creates a new agreement item.
        /// </summary>
        /// <param name="agreementId">The identifier of the agreement.</param>
        /// <param name="item">The agreement item data.</param>
        /// <returns>The agreement item data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">The <paramref name="agreementId"/> is not associated to any agreement.</response>
        [HttpPost]
        [Produces("application/json")]
        public AgreementItemForList Post([FromRoute] Guid agreementId, AgreementItemForEdit item)
        {
            var agreement = Context.Set<Agreement>().SingleOrDefault(e => e.Identifier == agreementId);
            if (agreement == null)
            {
                throw new NotFoundException("Unknown agreement");
            }

            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            AgreementItem model = Mapper.Map<AgreementItem>(item);
            model.Agreement = agreement;
            if (item.ProductIdentifier.HasValue)
            {
                model.Product = Context.Set<Product>()
                    .SingleOrDefault(p => p.Identifier == item.ProductIdentifier.Value);
                if (model.Product == null)
                {
                    throw new BadRequestException("Invalid product identifier");
                }
            }

            Context.Set<AgreementItem>().Add(model);
            Context.SaveChanges();

            return Mapper.Map<AgreementItemForList>(model);
        }

        /// <summary>
        /// Updates an existing agreement item.
        /// </summary>
        /// <param name="agreementId">The identifier of the agreement.</param>
        /// <param name="itemId">The identifier of the updated item.</param>
        /// <param name="item">The agreement item data.</param>
        /// <returns>The agreement item data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">The <paramref name="agreementId"/> is not associated to any agreement.</response>
        [HttpPut]
        [Produces("application/json")]
        [Route("{itemId}")]
        public AgreementItemForList Put([FromRoute] Guid agreementId, Guid itemId, AgreementItemForEdit item)
        {
            var agreement = Context.Set<Agreement>().SingleOrDefault(e => e.Identifier == agreementId);
            if (agreement == null)
            {
                throw new NotFoundException("Unknown agreement");
            }

            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            var model = Context.Set<AgreementItem>().SingleOrDefault(e => e.Identifier == itemId && e.Agreement == agreement);
            if (model == null)
            {
                throw new BadRequestException("Invalid item identifier");
            }

            Mapper.Map(item, model);
            if (item.ProductIdentifier.HasValue)
            {
                model.Product = Context.Set<Product>()
                    .SingleOrDefault(p => p.Identifier == item.ProductIdentifier.Value);
                if (model.Product == null)
                {
                    throw new BadRequestException("Invalid product identifier");
                }
            }

            Context.SaveChanges();

            return Mapper.Map<AgreementItemForList>(model);
        }

        /// <summary>
        /// Deletes a specific agreement item.
        /// </summary>
        /// <param name="agreementId">The identifier of the agreement.</param>
        /// <param name="itemId">The identifier of the item to delete.</param>
        /// <response code="404">No item is associated to the provided <paramref name="itemId"/>.</response>
        [HttpDelete]
        [Produces("application/json")]
        [Route("{itemId}")]
        public void Delete([FromRoute] Guid agreementId, Guid itemId)
        {
            var agreement = Context.Set<Agreement>()
                .SingleOrDefault(e => e.Identifier == agreementId);
            if (agreement == null)
            {
                throw new NotFoundException("Unknown agreement");
            }

            var entity = Context.Set<AgreementItem>()
                .SingleOrDefault(e => e.Identifier == itemId && e.Agreement == agreement);
            if (entity == null)
            {
                throw new NotFoundException($"Not existing entity");
            }

            Context.Set<AgreementItem>().Remove(entity);
            Context.SaveChanges();
        }
    }
}
