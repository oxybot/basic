using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage client data.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientsController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ClientsController(Context context, IMapper mapper, ILogger<ClientsController> logger)
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
        protected ILogger<ClientsController> Logger { get; }

        /// <summary>
        /// Retrieves all clients.
        /// </summary>
        /// <returns>The list of clients.</returns>
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<SimpleClientDTO> GetAll()
        {
            return Context.Set<Client>().ToList().Select(e => Mapper.Map<SimpleClientDTO>(e));
        }

        /// <summary>
        /// Retrieves a specific client.
        /// </summary>
        /// <param name="identifier">The identifier of the client.</param>
        /// <returns>The detailed data about the client identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No client is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [Produces("application/json")]
        [Route("{identifier}")]
        public ClientDTO GetOne(Guid identifier)
        {
            var entity = Context.Set<Client>().SingleOrDefault(c => c.Identifier == identifier);
            if (entity == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            return Mapper.Map<ClientDTO>(entity);
        }

        /// <summary>
        /// Creates a new client.
        /// </summary>
        /// <param name="client">The client data.</param>
        /// <returns>The client data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [Produces("application/json")]
        public ClientDTO Post(ClientDTO client)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            Client entity = Mapper.Map<Client>(client);
            Context.Set<Client>().Add(entity);
            Context.SaveChanges();

            return Mapper.Map<ClientDTO>(entity);
        }

        /// <summary>
        /// Updates an existing client.
        /// </summary>
        /// <param name="identifier">The identifier of the client to update.</param>
        /// <param name="client">The client data.</param>
        /// <returns>The client data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">No client is associated to the provided <paramref name="identifier"/>.</response>
        [HttpPut]
        [Produces("application/json")]
        [Route("{identifier}")]
        public ClientDTO Put(Guid identifier, ClientDTO client)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequestException("Invalid data");
            }

            var entity = Context.Set<Client>().SingleOrDefault(e => e.Identifier == identifier);
            if (entity == null)
            {
                throw new NotFoundException("Not existing entity");
            }

            Mapper.Map(client, entity);
            Context.SaveChanges();

            return Mapper.Map<ClientDTO>(entity);
        }

        /// <summary>
        /// Deletes a specific client.
        /// </summary>
        /// <param name="identifier">The identifier of the client to delete.</param>
        /// <response code="404">No client is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [Produces("application/json")]
        [Route("{identifier}")]
        public void Delete(Guid identifier)
        {
            var entity = Context.Set<Client>().SingleOrDefault(e => e.Identifier == identifier);
            if (entity == null)
            {
                throw new NotFoundException($"Not existing entity");
            }

            Context.Set<Client>().Remove(entity);
            Context.SaveChanges();
        }
    }
}
