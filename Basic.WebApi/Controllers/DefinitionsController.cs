using Basic.DataAccess;
using Basic.WebApi.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Basic.WebApi.Controllers
{
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

        [HttpGet]
        [Route("{name}")]
        public EntityDefinition GetOne(string name)
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

        private EntityDefinition Build(Type entityType)
        {
            var schemaAttribute = entityType.GetCustomAttribute<SwaggerSchemaAttribute>();
            var definition = new EntityDefinition
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

        private EntityFieldDefinition Build(PropertyInfo property)
        {
            var schemaAttribute = property.GetCustomAttribute<SwaggerSchemaAttribute>();
            var requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();
            var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();

            return new EntityFieldDefinition
            {
                Name = schemaAttribute?.Title ?? property.Name.ToJsonFieldName(),
                DisplayName = property.Name.Humanize(LetterCasing.Title),
                Required = requiredAttribute != null,
                Placeholder = displayAttribute?.Prompt,
                Group = displayAttribute?.GroupName,
            };
        }
    }
}
