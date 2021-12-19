using System.ComponentModel.DataAnnotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Contains the count of elements linked to a specific client.
    /// </summary>
    public class ClientLinks
    {
        /// <summary>
        /// Gets or sets the number of agreements associated to a specific client.
        /// </summary>
        public int Agreements { get; set; } 
   }
}
