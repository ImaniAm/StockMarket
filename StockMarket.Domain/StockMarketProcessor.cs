namespace StockMarket.Domain
{
    public class StockMarketProcessor : IStockMarketProcessor
    {
        private MarketState stateCode;
        private IStockMarketProcessor state;
        private long lastOrderNumber;
        private long lastTradeNumber;
        private List<Trade> trades;
        private List<Order> allOrders;
        private PriorityQueue<Order, Order> buyOrders;
        private PriorityQueue<Order, Order> sellOrders;
        public IEnumerable<Trade> Trades => trades;
        public IEnumerable<Order> Orders => allOrders;

        public StockMarketProcessor(long lastOrderNumber = 0, long lastTradeNumber = 0, IEnumerable<Order>? orders = null)
        {
            state = new CloseState(this);
            this.lastOrderNumber = lastOrderNumber;
            this.lastTradeNumber = lastOrderNumber;
            allOrders = new List<Order>();
            trades = new List<Trade>();
            buyOrders = new PriorityQueue<Order, Order>(new MaxComparer());
            sellOrders = new PriorityQueue<Order, Order>(new MinComparer());
            foreach (var order in orders?? new List<Order>()) 
            {
                allOrders.Add(order);
                enqueueOrder(order);
            }
        }
        public void Open()
        {
            state.Open();
        }
        internal void open()
        {
            this.stateCode = MarketState.Open;
            state = new OpenState(this);
        }
        public void Close()
        {
            state.Close();
        }
        internal void close()
        {
            this.stateCode = MarketState.Closed;
            state = new CloseState(this);
        }
        public long EnqueueOrder(TradeSide side, decimal price, decimal quantity)
        {
            return state.EnqueueOrder(side, price, quantity);
        }
        internal long enqueueOrder(TradeSide side, decimal price, decimal quantity)
        {
            var order = makeOrder(side, price, quantity);
            return enqueueOrder(order);
        }
        internal long enqueueOrder(Order order)
        {
            if (order.Side == TradeSide.Buy)
            {
                return matchOrder(
                order: order,
                orders: buyOrders,
                matchingOrders: sellOrders,
                comparePriceDelegate: (decimal price1, decimal price2) => price1 <= price2
                );
            }

            return matchOrder(
                order: order,
                orders: sellOrders,
                matchingOrders: buyOrders,
                comparePriceDelegate: (decimal price1, decimal price2) => price1 >= price2
                );
        }

        private Order makeOrder(TradeSide side, decimal price, decimal quantity)
        {
            Interlocked.Increment(ref lastOrderNumber);
            var order = new Order(lastOrderNumber, side, price, quantity);
            allOrders.Add(order);
            return order;
        }
        private long matchOrder(Order order, PriorityQueue<Order, Order> orders, PriorityQueue<Order, Order> matchingOrders, Func<decimal, decimal, bool> comparePriceDelegate)
        {
            while ((matchingOrders.Count > 0) && (order.Quantity > 0) && comparePriceDelegate(matchingOrders.Peek().Price, order.Price))
            {
                var peekedOrder = matchingOrders.Peek();

                if (peekedOrder.IsCanceled)
                {
                    matchingOrders.Dequeue();
                    continue;
                }

                makeTrade(peekedOrder, order);
                if (peekedOrder.Quantity == 0) matchingOrders.Dequeue();
            }

            if (order.Quantity > 0) orders.Enqueue(order, order);

            return order.Id;
        }
        private void makeTrade(Order order1, Order order2)
        {
            var matchingOrders = findOrders(order1, order2);
            var minQuantity = Math.Min(matchingOrders.SellOrder.Quantity, matchingOrders.BuyOrder.Quantity);
            Interlocked.Increment(ref lastTradeNumber);

            trades.Add(new Trade(
                id: lastTradeNumber,
                sellOrderId: matchingOrders.SellOrder.Id,
                buyOrderId: matchingOrders.BuyOrder.Id,
                price: matchingOrders.SellOrder.Price,
                quantity: minQuantity
                ));

            matchingOrders.SellOrder.DecreaseQuantity(minQuantity);
            matchingOrders.BuyOrder.DecreaseQuantity(minQuantity);
        }
        private static (Order SellOrder, Order BuyOrder) findOrders(Order order1, Order order2)
        {
            if (order1.Side == TradeSide.Sell) return (SellOrder: order1, BuyOrder: order2);
            return (SellOrder: order2, BuyOrder: order1);

        }
        public void Cancel(long orderId)
        {
            state.Cancel(orderId);
        }
        internal void cancel(long orderId)
        {
            allOrders.Single(order => order.Id == orderId).Cancel();
        }
        public long Modify(long orderId, TradeSide side, decimal price, decimal quantity)
        {
            return state.Modify(orderId, side, price, quantity);
        }
        internal long modify(long orderId, TradeSide side, decimal price, decimal quantity)
        {
            state.Cancel(orderId);
            return state.EnqueueOrder(side, price, quantity);
        }
    }
}