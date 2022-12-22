namespace StockMarket.Domain
{
    internal class OpenState : StockMarketState
    {
        public OpenState(StockMarketProcessor stockMarket) : base(stockMarket)
        {
        }
        public override void Open()
        {
        }
        public override void Close()
        {
            stockMarket.close();
        }
        public override long EnqueueOrder(TradeSide side, decimal price, decimal quantity)
        {
            return stockMarket.enqueueOrder(side, price, quantity);
        }
        public override void Cancel(long orderId)
        {
            stockMarket.cancel(orderId);
        }
        public override long Modify(long orderId, TradeSide side, decimal price, decimal quantity)
        {
            return stockMarket.modify(orderId, side, price, quantity);
        }
    }
}