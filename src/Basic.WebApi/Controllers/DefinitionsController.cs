// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using AutoMapper;
using Basic.DataAccess;
using Basic.WebApi.Framework;
using Basic.WebApi.Models;
using Basic.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides the API that describes the various DTO for the UI.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DefinitionsController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionsController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="mapper">The configured automapper.</param>
        /// <param name="definitions">The DTO definitions service.</param>
        /// <param name="logger">The associated logger.</param>
        public DefinitionsController(Context context, IMapper mapper, DefinitionsService definitions, ILogger<DefinitionsController> logger)
            : base(context, mapper, logger)
        {
            this.Definitions = definitions ?? throw new ArgumentNullException(nameof(definitions));
        }

        /// <summary>
        /// Gets the DTO definitions service.
        /// </summary>
        public DefinitionsService Definitions { get; }

        /// <summary>
        /// Retrieves the list of available entities.
        /// </summary>
        /// <returns>The list of available entities.</returns>
        [HttpGet]
        public IEnumerable<string> GetAll()
        {
            return this.Definitions.GetAll();
        }

        /// <summary>
        /// Retrieves one detailed entity definition.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        /// <returns>The associated definition.</returns>
        /// <exception cref="NotFoundException">The <paramref name="name"/> is invalid.</exception>
        [HttpGet]
        [Route("{name}")]
        public Definition GetOne([Required] string name)
        {
            return this.Definitions.GetOne(name);
        }
    }
}
