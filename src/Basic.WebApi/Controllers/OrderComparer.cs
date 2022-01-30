namespace Basic.WebApi.Controllers
{
    internal class OrderComparer : IComparer<int?>
    {
        public static readonly OrderComparer Default = new OrderComparer();

        public int Compare(int? x, int? y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            else if (x == null && y != null)
            {
                return 1;
            }
            else if (y == null)
            {
                return -1;
            }
            else
            {
                return x.Value - y.Value;
            }
        }
    }
}
