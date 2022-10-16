// Copyright (c) oxybot. All rights reserved.
// Licensed under the MIT license.

using Basic.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Basic.DataAccess
{
    /// <summary>
    /// Compares two <see cref="Schedule.WorkingSchedule"/> based on their values.
    /// </summary>
    [SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Part of a converter")]
    internal class ScheduleComparer : ValueComparer<decimal[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleComparer"/> class.
        /// </summary>
        public ScheduleComparer()
            : base(false)
        {
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="left">The first schedule to compare.</param>
        /// <param name="right">The second schedule to compare.</param>
        /// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
        public override bool Equals(decimal[] left, decimal[] right)
        {
            if (left == null && right == null)
            {
                return true;
            }
            else if (left == null || right == null || left.Length != right.Length)
            {
                return false;
            }

            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="instance">The schedule for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified schedule.</returns>
        public override int GetHashCode(decimal[] instance)
        {
            if (instance == null)
            {
                return -1;
            }

            return instance.Aggregate(0, (a, c) => a << (int)(c * 100));
        }

        /// <summary>
        /// Creates a snapshot of the given instance.
        /// </summary>
        /// <param name="instance">The schedule instance.</param>
        /// <returns>The schedule snapshot.</returns>
        /// <remarks>
        /// Snapshotting is the process of creating a copy of the value into a snapshot so
        /// it can later be compared to determine if it has changed. For some types, such
        /// as collections, this needs to be a deep copy of the collection rather than just
        /// a shallow copy of the reference.
        /// </remarks>
        public override decimal[] Snapshot(decimal[] instance)
        {
            if (instance == null)
            {
                return null;
            }

            decimal[] destination = new decimal[instance.Length];
            for (int i = 0; i < instance.Length; i++)
            {
                destination[i] = instance[i];
            }

            return destination;
        }
    }
}
