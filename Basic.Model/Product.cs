using System;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    public class Product : BaseModel
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string DefaultPrice { get; set; }

        [Required]
        public string DefaultUnitPrice { get; set; }

        [Required]
        public string DefaultQuantity { get; set; }
    }
}
