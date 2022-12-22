using FluentAssertions;
using StockMarket.Domain;

namespace StockMarket.Tests
{
    public class StockMarketTests
    {
        [Fact]
        public void EnqueueOrderShouldProcessSellOrderWhenBuyOrderIsAlreadyEnqueuedTest()
        {
            // Arrange
            var sut = new StockMarketProcessor();
            sut.Open();
            // Act
            var buyOrderId = sut.EnqueueOrder(TradeSide.Buy, 1500M, 1M);
            var sellOrderId = sut.EnqueueOrder(TradeSide.Sell, 1400M, 2M);
            // Assert
            Assert.Equal(1, sut.Trades.Count());
            sut.Trades.First().Should().BeEquivalentTo(new
            {
                BuyOrderId = buyOrderId,
                SellOrderId = sellOrderId,
                Price = 1400M,
                Quantity = 1M
            });
        }
        [Fact]
        public void EnqueueOrderShouldProcessBuyOrderWhenSellOrderIsAlreadyEnqueuedTest()
        {
            //Arrange
            var sut = new StockMarketProcessor();
            sut.Open();
            //Act
            var sellOrderId = sut.EnqueueOrder(TradeSide.Sell, 1400M, 2M);
            var buyOrderId = sut.EnqueueOrder(TradeSide.Buy, 1500M, 1M);
            //Assert
            Assert.Equal(1, sut.Trades.Count());
            sut.Trades.First().Should().BeEquivalentTo(new
            {
                BuyOrderId = buyOrderId,
                SellOrderId = sellOrderId,
                Price = 1400M,
                Quantity = 1M
            });
        }
        [Fact]
        public void EnqueueOrderShouldProcessSellOrderWhenMultipleBuyOrdersAreAlreadyEnqueuedTest()
        {
            //Arrange
            var sut = new StockMarketProcessor();
            sut.Open();
            //Act
            var buyOrderId1 = sut.EnqueueOrder(TradeSide.Buy, 1500M, 1M);
            var buyOrderId2 = sut.EnqueueOrder(TradeSide.Buy, 1450M, 1M);
            var sellOrderId = sut.EnqueueOrder(TradeSide.Sell, 1400M, 2M);
            //Assert
            Assert.Equal(2, sut.Trades.Count());
            sut.Trades.First().Should().BeEquivalentTo(new
            {
                BuyOrderId = buyOrderId1,
                SellOrderId = sellOrderId,
                Price = 1400M,
                Quantity = 1M
            });
            sut.Trades.Skip(1).First().Should().BeEquivalentTo(new
            {
                BuyOrderId = buyOrderId2,
                SellOrderId = sellOrderId,
                Price = 1400M,
                Quantity = 1M
            });
        }
        [Fact]
        public void EnqueueOrderShouldProcessBuyOrderWhenMultipleSellOrdersAreAlreadyEnqueuedTest()
        {
            //Arrange
            var sut = new StockMarketProcessor();
            sut.Open();
            //Act
            var sellOrderId1 = sut.EnqueueOrder(TradeSide.Sell, 1400M, 1M);
            var sellOrderId2 = sut.EnqueueOrder(TradeSide.Sell, 1400M, 1M);
            var buyOrderId = sut.EnqueueOrder(TradeSide.Buy, 1500M, 2M);
            //Assert
            Assert.Equal(2, sut.Trades.Count());
            sut.Trades.First().Should().BeEquivalentTo(new
            {
                BuyOrderId = buyOrderId,
                SellOrderId = sellOrderId1,
                Price = 1400M,
                Quantity = 1M
            });
            sut.Trades.Skip(1).First().Should().BeEquivalentTo(new
            {
                BuyOrderId = buyOrderId,
                SellOrderId = sellOrderId2,
                Price = 1400M,
                Quantity = 1M
            });
        }
        [Fact]
        public void EnqueueOrderShouldNotWorkWhenStockMarketIsClosedTest()
        {
            //Arrange
            var sut = new StockMarketProcessor();
            //Act
            Action act = () => sut.EnqueueOrder(TradeSide.Buy, 1500M, 1M);
            //Assert
            Assert.Throws(typeof(NotImplementedException), act);
        }
        [Fact]
        public void CancelShouldDequeueTheOrderTest()
        {
            //Arrange
            var sut = new StockMarketProcessor();
            sut.Open();
            var buyOrderId = sut.EnqueueOrder(TradeSide.Buy, 1500M, 1M);
            //Act
            sut.Cancel(buyOrderId);
            sut.EnqueueOrder(TradeSide.Sell, 1400M, 2M);
            //Assert
            Assert.Equal(0, sut.Trades.Count());
        }
        [Fact]
        public void ModifyShouldUpdateOrderTest()
        {
            //Arrange
            var sut = new StockMarketProcessor();
            sut.Open();
            var buyOrderId = sut.EnqueueOrder(TradeSide.Buy, 1500M, 1M);
            var sellOrderId = sut.EnqueueOrder(TradeSide.Sell, 1600M, 2M);
            //Act
            var modifiedSellOrderId = sut.Modify(sellOrderId, TradeSide.Sell, 1400M, 1M);
            //Assert
            Assert.Equal(1, sut.Trades.Count());
            sut.Trades.First().Should().BeEquivalentTo(new
            {
                BuyOrderId = buyOrderId,
                SellOrderId = modifiedSellOrderId,
                Price = 1400M,
                Quantity = 1M
            });
        }
    }
}