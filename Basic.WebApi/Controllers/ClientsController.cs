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
    public class ClientsController : BaseModelController<Client, SimpleClientDTO, ClientDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientsController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ClientsController(Context context, IMapper mapper, ILogger<ClientsController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all clients.
        /// </summary>
        /// <returns>The list of clients.</returns>
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<SimpleClientDTO> GetAll()
        {
            return SimpleAddIncludes(Context.Set<Client>())
                .ToList()
                .Select(e => Mapper.Map<SimpleClientDTO>(e));
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
        public override ClientDTO GetOne(Guid identifier)
        {
            return base.GetOne(identifier);
        }

        /// <summary>
        /// Creates a new client.
        /// </summary>
        /// <param name="client">The client data.</param>
        /// <returns>The client data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [Produces("application/json")]
        public override SimpleClientDTO Post(ClientDTO client)
        {
            return base.Post(client);
        }

        /// <summary>
        /// Updates a specific client.
        /// </summary>
        /// <param name="identifier">The identifier of the client to update.</param>
        /// <param name="client">The client data.</param>
        /// <returns>The client data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">No client is associated to the provided <paramref name="identifier"/>.</response>
        [HttpPut]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override SimpleClientDTO Put(Guid identifier, ClientDTO client)
        {
            return base.Put(identifier, client);
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
