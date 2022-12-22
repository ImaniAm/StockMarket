namespace StockMarket.Domain
{
    public class MinComparer : BaseComparer
    {
        protected override int SpecificCompare(Order x, Order y)
        {
            if (y.Price < x.Price) return 1;
            if (y.Price > x.Price) return -1;
            return 0;
        }
    }
}