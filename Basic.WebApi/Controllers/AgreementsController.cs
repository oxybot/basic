using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage agreements data.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AgreementsController : BaseModelController<Agreement, SimpleAgreementDTO, AgreementDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientsController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        /// <exception cref="ArgumentNullException"></exception>
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
        public IEnumerable<SimpleAgreementDTO> GetAll(Guid? clientId)
        {
            var entities = SimpleAddIncludes(Context.Set<Agreement>());
            if (clientId.HasValue)
            {
                entities = entities.Where(c => c.Client.Identifier == clientId.Value);
            }

            return entities
                .ToList()
                .Select(e => Mapper.Map<SimpleAgreementDTO>(e));
        }

        protected override IQueryable<Agreement> SimpleAddIncludes(IQueryable<Agreement> query)
        {
            return query.Include(c => c.Client);
        }

        protected override IQueryable<Agreement> StandardAddIncludes(IQueryable<Agreement> query)
        {
            return query.Include(c => c.Client);
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
        public override AgreementDTO GetOne(Guid identifier)
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
        public override SimpleAgreementDTO Post(AgreementDTO agreement)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            var client = Context.Set<Client>().SingleOrDefault(c => c.Identifier == agreement.ClientIdentifier);
            if (client == null)
            {
                throw new BadRequestException("Invalid client identifier");
            }

            Agreement model = Mapper.Map<Agreement>(agreement);
            model.Client = client;

            return base.PostCore(model);
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
        public override SimpleAgreementDTO Put(Guid identifier, AgreementDTO agreement)
        {
            return base.Put(identifier, agreement);
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
    }
}
