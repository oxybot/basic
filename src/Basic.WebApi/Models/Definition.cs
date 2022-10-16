// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace Basic.WebApi.Models
{
    /// <summary>
    /// Represents the definition associated to a DTO class.
    /// </summary>
    public class Definition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Definition"/> class.
        /// </summary>
        public Definition()
        {
            this.Fields = new List<DefinitionField>();
        }

        /// <summary>
        /// Gets or sets the name of the DTO.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the field descriptors of the DTO class.
        /// </summary>
        public ICollection<DefinitionField> Fields { get; }
    }
}
