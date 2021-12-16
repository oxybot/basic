namespace Basic.WebApi.Models
{
    public class EntityFieldDefinition
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Required { get; set; }

        public string Placeholder { get; set; }
        public string Group { get; set; }
    }
}