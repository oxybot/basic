// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper;
using Basic.DataAccess;
using Basic.Model;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Basic.WebApi.Models;
using Basic.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides API to retrieve and manage products data.
    /// </summary>
    [ApiController]
    [Authorize]
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
        public ProductsController(Context context, IMapper mapper, ILogger<ClientsController> logger)
            : base(context, mapper, logger)
        {
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <param name="definitions">The service providing the entity definitions.</param>
        /// <param name="sortAndFilter">The sort and filter options, is any.</param>
        /// <returns>The list of products.</returns>
        [HttpGet]
        [AuthorizeRoles(Role.ClientRO, Role.Client)]
        [Produces("application/json")]
        public IEnumerable<ProductForList> GetAll([FromServices] DefinitionsService definitions, [FromQuery] SortAndFilterModel sortAndFilter)
        {
            var entities = this.GetAllCore(definitions, sortAndFilter)
                .ToList()
                .Select(e => this.Mapper.Map<ProductForList>(e));

            return entities;
        }

        /// <summary>
        /// Retrieves a specific product.
        /// </summary>
        /// <param name="identifier">The identifier of the product.</param>
        /// <returns>The detailed data about the product identified by <paramref name="identifier"/>.</returns>
        /// <response code="404">No product is associated to the provided <paramref name="identifier"/>.</response>
        [HttpGet]
        [AuthorizeRoles(Role.ClientRO, Role.Client)]
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
        [AuthorizeRoles(Role.Client)]
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
        [AuthorizeRoles(Role.Client)]
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
        [AuthorizeRoles(Role.Client)]
        [Produces("application/json")]
        [Route("{identifier}")]
        public override void Delete(Guid identifier)
        {
            base.Delete(identifier);
        }
    }
}
