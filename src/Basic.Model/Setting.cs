// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Basic.Model
{
    /// <summary>
    /// Represents one of the application settings, as defined in the database.
    /// </summary>
    public class Setting : BaseModel
    {
        /// <summary>
        /// Gets or sets the name of the setting section.
        /// </summary>
        [Required]
        public string Section { get; set; }

        /// <summary>
        /// Gets or sets the unique key of the settings within the section.
        /// </summary>
        [Required]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value for this specific setting.
        /// </summary>
        public string Value { get; set; }
    }
}
