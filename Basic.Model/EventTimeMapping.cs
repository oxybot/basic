namespace Basic.Model
{
    /// <summary>
    /// Represents the various possibility to compute 
    /// </summary>
    public enum EventTimeMapping
    {
        /// <summary>
        /// Indicates that the associated time booking is not to be considered as time-off but as
        /// normal activities.
        /// </summary>
        /// <remarks>
        /// Can be used to flag special events like travel, on-calls....
        /// </remarks>
        Active = 0,

        /// <summary>
        /// Indicates that the associated time booking is standard / legal time-off.
        /// </summary>
        StandardTimeOff,

        /// <summary>
        /// Indicates that the associated time booking is extra time-off.
        /// </summary>
        /// <remarks>
        /// Can be used to flag special leaves like sickness, birth...</remarks>
        ExtraTimeOff,
    }
}
