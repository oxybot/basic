namespace Basic.WebApi.Models
{
    public class EntityDefinition
    {
        public EntityDefinition()
        {
            this.Fields = new List<EntityFieldDefinition>();
        }
        public string Name { get; set; }

        public ICollection<EntityFieldDefinition> Fields { get; }
    }
}
