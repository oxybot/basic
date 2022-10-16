// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Swashbuckle.AspNetCore.Annotations;

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the time-off consumption of a user.
    /// </summary>
    public class ConsumptionForList : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the associated event category.
        /// </summary>
        [SwaggerSchema(Format = "ref/category")]
        public EntityReference Category { get; set; }

        /// <summary>
        /// Gets or sets the allowed hours of the associated balance, if any.
        /// </summary>
        [SwaggerSchema(Format = "hours")]
        public decimal? Allowed { get; set; }

        /// <summary>
        /// Gets or sets the transfered hours of the associated balance, if any.
        /// </summary>
        [SwaggerSchema(Format = "hours")]
        public decimal? Transfered { get; set; }

        /// <summary>
        /// Gets or sets the number of hours of taken time-off (past and approved).
        /// </summary>
        [SwaggerSchema(Format = "hours")]
        public decimal Taken { get; set; }

        /// <summary>
        /// Gets or sets the number of hours of planned time-off (future and approved).
        /// </summary>
        [SwaggerSchema(Format = "hours")]
        public decimal Planned { get; set; }

        /// <summary>
        /// Gets or sets the number of hours of requested time-off (requested, past or future).
        /// </summary>
        [SwaggerSchema(Format = "hours")]
        public decimal Requested { get; set; }
    }
}
