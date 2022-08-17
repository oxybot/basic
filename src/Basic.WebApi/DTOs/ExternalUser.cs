namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents an importable user as retrieved from the external authenticator.
    /// </summary>
    public class ExternalUser : UserForEdit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalUser"/> class.
        /// </summary>
        public ExternalUser()
        {
            this.Importable = true;
        }

        /// <summary>
        /// Gets or sets importable status of the external user.
        /// </summary>
        public bool Importable { get; set; }
    }
}
