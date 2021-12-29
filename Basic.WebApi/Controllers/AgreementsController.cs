using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage agreements data.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AgreementsController : BaseModelController<Agreement, AgreementForList, AgreementForView, AgreementForEdit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgreementsController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public AgreementsController(Context context, IMapper mapper, ILogger<AgreementsController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all agreements.
        /// </summary>
        /// <param name="clientId">The identifier of the client to filter the result list, if any.</param>
        /// <returns>The list of agreements.</returns>
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<AgreementForList> GetAll(Guid? clientId)
        {
            var entities = AddIncludesForList(Context.Set<Agreement>());
            if (clientId.HasValue)
            {
                entities = entities.Where(c => c.Client.Identifier == clientId.Value);
            }

            return entities
                .ToList()
                .Select(e => Mapper.Map<AgreementForList>(e));
        }

        /// <summary>
        /// Retrieves a specific agreement.
        /// </summary>
        /// <param name="identifier">The identifier of the agreement.</param>
        /// <returns>The detailed data about the agreement identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No agreement is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override AgreementForView GetOne(Guid identifier)
        {
            return base.GetOne(identifier);
        }

        /// <summary>
        /// Creates a new agreement.
        /// </summary>
        /// <param name="agreement">The agreement data.</param>
        /// <returns>The agreement data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [Produces("application/json")]
        public override AgreementForList Post(AgreementForEdit agreement)
        {
            return base.Post(agreement);
        }

        /// <summary>
        /// Updates a specific agreement.
        /// </summary>
        /// <param name="identifier">The identifier of the agreement to update.</param>
        /// <param name="agreement">The agreement data.</param>
        /// <returns>The agreement data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">No agreement is associated to the provided <paramref name="identifier"/>.</response>
        [HttpPut]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override AgreementForList Put(Guid identifier, AgreementForEdit agreement)
        {
            return base.Put(identifier, agreement);
        }

        /// <summary>
        /// Checks and maps Client info.
        /// </summary>
        /// <param name="agreement">The agreement data.</param>
        /// <param name="model">The agreement model instance.</param>
        /// <exception cref="BadRequestException"></exception>
        protected override void CheckDependencies(AgreementForEdit agreement, Agreement model)
        {
            model.Client = Context.Set<Client>().SingleOrDefault(c => c.Identifier == agreement.ClientIdentifier);
            if (model.Client == null)
            {
                throw new BadRequestException("Invalid client identifier");
            }
        }

        /// <summary>
        /// Deletes a specific agreement.
        /// </summary>
        /// <param name="identifier">The identifier of the agreement to delete.</param>
        /// <response code="404">No agreement is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete(Guid identifier)
        {
            base.Delete(identifier);
        }

        /// <summary>
        /// Retrieves all agreement items for a specific agreement.
        /// </summary>
        /// <param name="agreementId">The identifier of the agreement.</param>
        /// <returns>The list of associated agreement items.</returns>
        /// <response code="404">The <paramref name="agreementId"/> is not associated to any agreement.</response>
        [HttpGet]
        [Route("{agreementId}/items")]
        [Produces("application/json")]
        public IEnumerable<AgreementItemForList> GetAllItems([FromRoute] Guid agreementId)
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
        [Route("{agreementId}/items")]
        [Produces("application/json")]
        public AgreementItemForList PostItem([FromRoute] Guid agreementId, AgreementItemForEdit item)
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
        [Route("{agreementId}/items/{itemId}")]
        [Produces("application/json")]
        public AgreementItemForList PutItem([FromRoute] Guid agreementId, Guid itemId, AgreementItemForEdit item)
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
        [Route("{agreementId}/items/{itemId}")]
        [Produces("application/json")]
        public void DeleteItem([FromRoute] Guid agreementId, Guid itemId)
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

        /// <summary>
        /// Adds the <see cref="Client"/> values.
        /// </summary>
        /// <param name="query">The current query.</param>
        /// <returns>The updated query.</returns>
        protected override IQueryable<Agreement> AddIncludesForList(IQueryable<Agreement> query)
        {
            return query.Include(c => c.Client);
        }

        /// <summary>
        /// Adds the <see cref="Client"/> values.
        /// </summary>
        /// <param name="query">The current query.</param>
        /// <returns>The updated query.</returns>
        protected override IQueryable<Agreement> AddIncludesForView(IQueryable<Agreement> query)
        {
            return query
                .Include(c => c.Client)
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product);
        }
    }
}
