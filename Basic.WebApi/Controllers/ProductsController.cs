using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage products data.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductsController
        : BaseModelController<Product, ProductForList, ProductForView, ProductForEdit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="logger">The associated logger.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ProductsController(Context context, IMapper mapper, ILogger<ClientsController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>The list of products.</returns>
        [HttpGet]
        [Produces("application/json")]
        public IEnumerable<ProductForList> GetAll()
        {
            return AddIncludesForList(Context.Set<Product>())
                .ToList()
                .Select(e => Mapper.Map<ProductForList>(e));
        }

        /// <summary>
        /// Retrieves a specific product.
        /// </summary>
        /// <param name="identifier">The identifier of the product.</param>
        /// <returns>The detailed data about the product identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No product is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override ProductForView GetOne(Guid identifier)
        {
            return base.GetOne(identifier);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">The product data.</param>
        /// <returns>The product data after creation.</returns>
        /// <response code="400">The provided data are invalid.</response>
        [HttpPost]
        [Produces("application/json")]
        public override ProductForList Post(ProductForEdit product)
        {
            return base.Post(product);
        }

        /// <summary>
        /// Updates a specific product.
        /// </summary>
        /// <param name="identifier">The identifier of the product to update.</param>
        /// <param name="product">The product data.</param>
        /// <returns>The product data after update.</returns>
        /// <response code="400">The provided data are invalid.</response>
        /// <response code="404">No product is associated to the provided <paramref name="identifier"/>.</response>
        [HttpPut]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override ProductForList Put(Guid identifier, ProductForEdit product)
        {
            return base.Put(identifier, product);
        }

        /// <summary>
        /// Deletes a specific product.
        /// </summary>
        /// <param name="identifier">The identifier of the product to delete.</param>
        /// <response code="404">No product is associated to the provided <paramref name="identifier"/>.</response>
        [HttpDelete]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete(Guid identifier)
        {
            base.Delete(identifier);
        }
    }
}
