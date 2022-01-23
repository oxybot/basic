using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage balance data.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class BalancesController : BaseModelController<Balance, BalanceForList, BalanceForList, BalanceForEdit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BalancesController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        public BalancesController(Context context, IMapper mapper, ILogger<BalancesController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all balances.
        /// </summary>
        /// <returns>The list of balances.</returns>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time)]
        [Produces("application/json")]
        public IEnumerable<BalanceForList> GetAll()
        {
            return AddIncludesForList(Context.Set<Balance>())
                .ToList()
                .Select(e => Mapper.Map<BalanceForList>(e));
        }

        /// <summary>
        /// Retrieves a specific balance.
        /// </summary>
        /// <param name="identifier">The identifier of the balance.</param>
        /// <returns>The detailed data about the balance identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No balance is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.TimeRO, Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override BalanceForList GetOne(Guid identifier)
        {
            return base.GetOne(identifier);
        }

        /// <summary>
        /// Creates a new balance.
        /// </summary>
        /// <param name="balance">The balance data.</param>
        /// <returns>The balance data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        public override BalanceForList Post(BalanceForEdit balance)
        {
            return base.Post(balance);
        }

        /// <summary>
        /// Updates a specific balance.
        /// </summary>
        /// <param name="identifier">The identifier of the balance to update.</param>
        /// <param name="balance">The balance data.</param>
        /// <returns>The balance data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">No balance is associated to the provided <paramref name="identifier"/>.</response>
        [HttpPut]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override BalanceForList Put(Guid identifier, BalanceForEdit balance)
        {
            return base.Put(identifier, balance);
        }

        /// <summary>
        /// Deletes a specific balance.
        /// </summary>
        /// <param name="identifier">The identifier of the balance to delete.</param>
        /// <response code="404">No balance is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [AuthorizeRoles(Role.Time)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete(Guid identifier)
        {
            base.Delete(identifier);
        }

        /// <summary>
        /// Checks and maps <see cref="Balance.User"/> and <see cref="Balance.Category"/> info.
        /// </summary>
        /// <param name="balance">The event data.</param>
        /// <param name="model">The event model instance.</param>
        protected override void CheckDependencies(BalanceForEdit balance, Balance model)
        {
            model.User = Context.Set<User>().SingleOrDefault(u => u.Identifier == balance.UserIdentifier);
            if (model.User == null)
            {
                ModelState.AddModelError("UserIdentifier", "Invalid User");
            }

            model.Category = Context.Set<EventCategory>().SingleOrDefault(c => c.Identifier == balance.CategoryIdentifier);
            if (model.Category == null)
            {
                ModelState.AddModelError("CategoryIdentifier", "Invalid Category");
            }

            if (!ModelState.IsValid)
            {
                throw new BadRequestException(ModelState);
            }
        }

        /// <summary>
        /// Adds the <see cref="Balance.User"/> and <see cref="Balance.Category"/> values.
        /// </summary>
        /// <param name="query">The current query.</param>
        /// <returns>The updated query.</returns>
        protected override IQueryable<Balance> AddIncludesForList(IQueryable<Balance> query)
        {
            return query.Include(c => c.User).Include(c => c.Category);
        }

        /// <summary>
        /// Adds the <see cref="Balance.User"/> and <see cref="Balance.Category"/> values.
        /// </summary>
        /// <param name="query">The current query.</param>
        /// <returns>The updated query.</returns>
        protected override IQueryable<Balance> AddIncludesForView(IQueryable<Balance> query)
        {
            return query.Include(c => c.User).Include(c => c.Category);
        }
    }
}
