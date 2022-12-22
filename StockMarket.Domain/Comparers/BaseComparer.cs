namespace StockMarket.Domain
{
    public abstract class BaseComparer : IComparer<Order>
    {
        public int Compare(Order? x, Order? y)
        {
            var result = SpecificCompare(x, y);
            if (result != 0) return result;

            if (y.Id < x.Id) return 1;
            if (y.Id > x.Id) return -1;

            return 0;
        }

        protected abstract int SpecificCompare(Order x, Order y);
    }
}