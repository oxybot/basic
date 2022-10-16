// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

namespace Basic.WebApi.DTOs
{
    /// <summary>
    /// Represents the calendar of a specific user.
    /// </summary>
    public class UserCalendar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserCalendar"/> class.
        /// </summary>
        public UserCalendar()
        {
            this.Lines = new List<Line>();
            this.DaysOff = new List<int>();
        }

        /// <summary>
        /// Gets or sets the associated users.
        /// </summary>
        public EntityReference User { get; set; }

        /// <summary>
        /// Gets or sets the standard days off for this user.
        /// </summary>
        public ICollection<int> DaysOff { get; set; }

        /// <summary>
        /// Gets or sets the lines for this user.
        /// </summary>
        public ICollection<Line> Lines { get; set; }

        /// <summary>
        /// Represents a calendar line of a specific user and category.
        /// </summary>
        public class Line
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Line"/> class.
            /// </summary>
            public Line()
            {
                this.Days = new List<int>();
            }

            /// <summary>
            /// Gets or sets the category name for this line.
            /// </summary>
            public string Category { get; set; }

            /// <summary>
            /// Gets or sets the css class for this line.
            /// </summary>
            public string ColorClass { get; set; }

            /// <summary>
            /// Gets or sets the days part of this calendar line.
            /// </summary>
            public ICollection<int> Days { get; set; }
        }
    }
}
