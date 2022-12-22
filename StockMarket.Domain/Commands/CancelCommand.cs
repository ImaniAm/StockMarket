namespace StockMarket.Domain.Commands
{
    public class CancelCommand : BaseCommand<bool>
    {
        private readonly IStockMarketProcessor stockMarket;
        private readonly long orderId;

        public CancelCommand(IStockMarketProcessor stockMarket, long orderId)
        {
            this.stockMarket = stockMarket;
            this.orderId = orderId;
        }

        protected override bool SpecificExecute()
        {
            stockMarket.Cancel(orderId);
            return true;
        }
    }
}