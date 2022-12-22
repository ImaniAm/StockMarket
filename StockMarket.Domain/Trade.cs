namespace StockMarket.Domain
{
    public class Trade
    {
        public long Id { get; }
        public long SellOrderId { get; }
        public long BuyOrderId { get; }
        public decimal Price { get; }
        public decimal Quantity { get; }

        internal Trade(long id, long sellOrderId, long buyOrderId, decimal price, decimal quantity)
        {
            Id = id;
            SellOrderId = sellOrderId;
            BuyOrderId = buyOrderId;
            Price = price;
            Quantity = quantity;
        }

    }
}