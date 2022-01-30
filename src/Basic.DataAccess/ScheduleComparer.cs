using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Basic.DataAccess
{
    internal class ScheduleComparer : ValueComparer<decimal[]>
    {
        public ScheduleComparer()
            : base(false)
        {
        }

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

        public override int GetHashCode(decimal[] instance)
        {
            if (instance == null)
            {
                return -1;
            }

            return instance.Aggregate(0, (a, c) => a << (int)(c * 100));
        }

        [return: NotNullIfNotNull("instance")]
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
