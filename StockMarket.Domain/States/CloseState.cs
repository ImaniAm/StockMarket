namespace StockMarket.Domain
{
    internal class CloseState : StockMarketState
    {
        public CloseState(StockMarketProcessor stockMarket) : base(stockMarket)
        {
        }

        public override void Open()
        {
            stockMarket.open();
        }

        public override void Close()
        {
        }
    }
}