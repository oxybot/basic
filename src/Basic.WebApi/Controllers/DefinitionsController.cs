using Basic.DataAccess;
using Basic.WebApi.DTOs;
using Basic.WebApi.Framework;
using Basic.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Basic.WebApi.Controllers
{
    /// <summary>
    /// Provides the API that describes the various DTO for the UI.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DefinitionsController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientsController"/> class.
        /// </summary>
        /// <param name="context">The datasource context.</param>
        /// <param name="logger">The associated logger.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DefinitionsController(Context context, ILogger<DefinitionsController> logger)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the datasource context.
        /// </summary>
        protected Context Context { get; }

        /// <summary>
        /// Gets the associated logger.
        /// </summary>
        protected ILogger<DefinitionsController> Logger { get; }

        /// <summary>
        /// Retrieves the list of available entities.
        /// </summary>
        /// <returns>The list of available entities.</returns>
        [HttpGet]
        public IEnumerable<string> GetAll()
        {
            var types = ExtractEntityTypes();
            return types.Keys.OrderBy(k => k);
        }

        /// <summary>
        /// Retrieves one detailed entity definition.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        /// <param name="provider">The current metadata provider.</param>
        /// <returns>The associated definition.</returns>
        /// <exception cref="NotFoundException">The <paramref name="name"/> is invalid.</exception>
        [HttpGet]
        [Route("{name}")]
        public Definition GetOne([Required] string name, [FromServices] IModelMetadataProvider provider)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            var types = ExtractEntityTypes();
            if (!types.ContainsKey(name))
            {
                throw new NotFoundException("Not existing entity");
            }

            ModelMetadata metadata = provider.GetMetadataForType(types[name]);
            return Build(metadata);
        }

        private static IDictionary<string, Type> ExtractEntityTypes()
        {
            return typeof(BaseEntityDTO).Assembly.GetTypes()
                .Where(t => typeof(BaseEntityDTO).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract)
                .ToDictionary(t =>
                {
                    var schemaAttribute = t.GetCustomAttribute<SwaggerSchemaAttribute>();
                    return schemaAttribute?.Title ?? t.Name;
                });
        }

        private static Definition Build(ModelMetadata metadata)
        {
            var definition = new Definition
            {
                Name = metadata.Name,
            };

            foreach (DefaultModelMetadata property in metadata.Properties)
            {
                definition.Fields.Add(BuildProperty(property));
            }

            return definition;
        }

        private static DefinitionField BuildProperty(DefaultModelMetadata property)
        {
            var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
            return new DefinitionField
            {
                Name = property.Name.ToJsonFieldName(),
                DisplayName = property.GetDisplayName(),
                Required = property.IsRequired,
                Placeholder = property.Placeholder,
                Group = displayAttribute?.GetGroupName(),
                Type = BuildFieldType(property),
            };
        }

        private static string BuildFieldType(DefaultModelMetadata property)
        {

            var type = property.ModelType;
            var swaggerAttribute = property.GetCustomAttribute<SwaggerSchemaAttribute>();
            if (swaggerAttribute != null && !string.IsNullOrEmpty(swaggerAttribute.Format))
            {
                return swaggerAttribute.Format;
            }

            var keyAttribute = property.GetCustomAttribute<KeyAttribute>();
            if (keyAttribute != null)
            {
                return "key";
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return "datetime";
            }
            else if (type == typeof(DateOnly) || type == typeof(DateOnly?))
            {
                return "date";
            }
            else if (type == typeof(bool))
            {
                return "boolean";
            }
            else if (type == typeof(EntityReference))
            {
                return "reference";
            }
            else if (type == typeof(Base64File))
            {
                return "file";
            }
            else
            {
                return "string";
            }
        }
    }
}
