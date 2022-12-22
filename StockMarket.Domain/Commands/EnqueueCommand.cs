namespace StockMarket.Domain.Commands
{
    public class EnqueueCommand : BaseCommand<long>
    {
        private readonly IStockMarketProcessor stockMarket;
        private readonly TradeSide side;
        private readonly decimal price;
        private readonly decimal quantity;
        public EnqueueCommand(IStockMarketProcessor stockMarket, TradeSide side, decimal price, decimal quantity)
        {
            this.stockMarket = stockMarket;
            this.side = side;
            this.price = price;
            this.quantity = quantity;
        }
        protected override long SpecificExecute()
        {
            return stockMarket.EnqueueOrder(side, price, quantity);
        }
    }
}