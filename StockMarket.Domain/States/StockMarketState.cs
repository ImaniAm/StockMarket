namespace StockMarket.Domain
{
    internal abstract class StockMarketState : IStockMarketProcessor
    {
        protected readonly StockMarketProcessor stockMarket;

        public StockMarketState(StockMarketProcessor stockMarket)
        {
            this.stockMarket = stockMarket;
        }
        public virtual void Open()
        {
            throw new NotImplementedException();
        }
        public virtual void Close()
        {
            throw new NotImplementedException();
        }
        public virtual long EnqueueOrder(TradeSide side, decimal price, decimal quantity)
        {
            throw new NotImplementedException();
        }
        public virtual void Cancel(long orderId)
        {
            throw new NotImplementedException();
        }
        public virtual long Modify(long orderId, TradeSide side, decimal price, decimal quantity)
        {
            throw new NotImplementedException();
        }
    }
}