using System;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    public class User : BaseModel
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
