using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.Models
{
    /// <summary>
    /// Contains the count of elements linked to a specific client.
    /// </summary>
    public class ClientLinksDTO
    {
        /// <summary>
        /// Gets or sets the number of agreements associated to a specific client.
        /// </summary>
        public int Agreements { get; set; } 
   }
}
