using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage client's contract data.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ClientContractsController : BaseModelController<ClientContract, SimpleClientContractDTO, ClientContractDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientsController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ClientContractsController(Context context, IMapper mapper, ILogger<ClientContractsController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all clients' contracts.
        /// </summary>
        /// <param name="clientId">The identifier of the client to filter the result list, if any.</param>
        /// <returns>The list of clients' contracts.</returns>
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<SimpleClientContractDTO> GetAll(Guid? clientId)
        {
            var entities = SimpleAddIncludes(Context.Set<ClientContract>());
            if (clientId.HasValue)
            {
                entities = entities.Where(c => c.Client.Identifier == clientId.Value);
            }

            return entities
                .ToList()
                .Select(e => Mapper.Map<SimpleClientContractDTO>(e));
        }

        protected override IQueryable<ClientContract> SimpleAddIncludes(IQueryable<ClientContract> query)
        {
            return query.Include(c => c.Client);
        }

        protected override IQueryable<ClientContract> StandardAddIncludes(IQueryable<ClientContract> query)
        {
            return query.Include(c => c.Client);
        }

        /// <summary>
        /// Retrieves a specific client's contract.
        /// </summary>
        /// <param name="identifier">The identifier of the client's contract.</param>
        /// <returns>The detailed data about the client's contract identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No contract is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override ClientContractDTO GetOne(Guid identifier)
        {
            return base.GetOne(identifier);
        }

        /// <summary>
        /// Creates a new client's contract.
        /// </summary>
        /// <param name="contract">The client's contract data.</param>
        /// <returns>The client's contract data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [Produces("application/json")]
        public override SimpleClientContractDTO Post(ClientContractDTO contract)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            var client = Context.Set<Client>().SingleOrDefault(c => c.Identifier == contract.ClientIdentifier);
            if (client == null)
            {
                throw new BadRequestException("Invalid client identifier");
            }

            ClientContract model = Mapper.Map<ClientContract>(contract);
            model.Client = client;

            return base.PostCore(model);
        }

        /// <summary>
        /// Updates a specific client.
        /// </summary>
        /// <param name="identifier">The identifier of the client to update.</param>
        /// <param name="contract">The client data.</param>
        /// <returns>The client data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">No client is associated to the provided <paramref name="identifier"/>.</response>
        [HttpPut]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override SimpleClientContractDTO Put(Guid identifier, ClientContractDTO contract)
        {
            return base.Put(identifier, contract);
        }

        /// <summary>
        /// Deletes a specific client.
        /// </summary>
        /// <param name="identifier">The identifier of the client to delete.</param>
        /// <response code="404">No client is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete(Guid identifier)
        {
            base.Delete(identifier);
        }
    }
}
