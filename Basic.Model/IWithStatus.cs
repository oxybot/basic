using System.Collections.Generic;

namespace Basic.Model
{
    /// <summary>
    /// Indicates that the associated model instances are linked to a workflow.
    /// </summary>
    public interface IWithStatus<TStatus>
        where TStatus : BaseModelStatus
    {
        /// <summary>
        /// Gets the history of status associated with a model instance.
        /// </summary>
        public ICollection<TStatus> Statuses { get; }
    }
}
