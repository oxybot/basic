namespace Basic.Model
{
    /// <summary>
    /// Represents the various possibility to compute 
    /// </summary>
    public enum TypeOfEntitie
    {
        /// <summary>
        /// Indicates that the type of entitie the attachment is related is a client
        /// normal activities.
        /// </summary>
        Client,

        /// <summary>
        /// Indicates that the type of entitie the attachment is related is an event
        /// </summary>
        Event,
        /// <summary>
        /// Indicates that the type of entitie the attachment is related is an user
        /// </summary>
        User,

        /// <summary>
        /// Indicates that the type of entitie the attachment is related is an agreement
        /// </summary>
        Agreement,
    }
}
