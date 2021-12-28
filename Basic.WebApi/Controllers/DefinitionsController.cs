using Basic.DataAccess;
using Basic.WebApi.DTOs;
using Basic.WebApi.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
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
        /// <returns>The associated definition.</returns>
        /// <exception cref="NotFoundException">The <paramref name="name"/> is invalid.</exception>
        [HttpGet]
        [Route("{name}")]
        public Definition GetOne(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new BadRequestException("Name of the entity required");
            }

            var types = ExtractEntityTypes();
            if (!types.ContainsKey(name))
            {
                throw new NotFoundException("Not existing entity");
            }

            return Build(types[name]);
        }

        private IDictionary<string, Type> ExtractEntityTypes()
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

        private Definition Build(Type entityType)
        {
            var schemaAttribute = entityType.GetCustomAttribute<SwaggerSchemaAttribute>();
            var definition = new Definition
            {
                Name = schemaAttribute?.Title ?? entityType.Name,
            };

            var types = GetInheritance(entityType);
            foreach (Type inheritance in types)
            {
                var properties = inheritance.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    definition.Fields.Add(Build(property));
                }
            }

            return definition;
        }

        private IEnumerable<Type> GetInheritance(Type reference)
        {
            if (typeof(BaseEntityDTO).IsAssignableFrom(reference.BaseType))
            {
                foreach (Type type in GetInheritance(reference.BaseType))
                {
                    yield return type;
                }
            }

            yield return reference;
        }

        private DefinitionField Build(PropertyInfo property)
        {
            var schemaAttribute = property.GetCustomAttribute<SwaggerSchemaAttribute>();
            var requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();
            var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();

            return new DefinitionField
            {
                Name = schemaAttribute?.Title ?? property.Name.ToJsonFieldName(),
                DisplayName = property.Name.Humanize(LetterCasing.Title),
                Required = requiredAttribute != null,
                Placeholder = displayAttribute?.Prompt,
                Group = displayAttribute?.GroupName,
                Type = BuildFieldType(property),
            };
        }

        private string BuildFieldType(PropertyInfo property)
        {
            var type = property.PropertyType;
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
