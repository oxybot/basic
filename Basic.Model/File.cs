using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    [Owned]
    public class File
    {
        [Required]
        public byte[] Data { get; set; }

        [Required]
        public string MimeType { get; set; }
    }
}
