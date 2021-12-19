namespace Basic.WebApi.Models
{
    public class Definition
    {
        public Definition()
        {
            this.Fields = new List<DefinitionField>();
        }
        public string Name { get; set; }

        public ICollection<DefinitionField> Fields { get; }
    }
}
