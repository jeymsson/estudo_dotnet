using System;
using System.Collections.Generic;
using Xunit;
using WebApplication1.Models;
using Moq;

namespace WebApplication1.Tests.Models
{
    public class StockTest
    {
        [Fact]
        public void Stock_Initialization_Test()
        {
            // Arrange
            var stock = new Stock();

            // Act & Assert
            Assert.Equal(0, stock.Id);
            Assert.Equal(string.Empty, stock.Symbol);
            Assert.Equal(string.Empty, stock.CompanyName);
            Assert.Equal(0m, stock.Purchase);
            Assert.Equal(0m, stock.LastDiv);
            Assert.Equal(string.Empty, stock.Industry);
            Assert.Equal(0L, stock.MarketCap);
            Assert.NotNull(stock.Comments);
            Assert.Empty(stock.Comments);
            Assert.NotNull(stock.Portfolios);
            Assert.Empty(stock.Portfolios);
        }

        [Fact]
        public void Stock_SetProperties_Test()
        {
            var mockComment = Mock.Of<Comment>(
                s =>
                s.Id == 1 &&
                s.Title == "Great stock!" &&
                s.Content == "Great stock!"
            );

            var mockPortfolio = Mock.Of<Portfolio>(
                s =>
                s.AppUserId == "user123" &&
                s.StockId == 1
            );

            // Arrange
            var stock = new Stock
            {
                Id = 1,
                Symbol = "AAPL",
                CompanyName = "Apple Inc.",
                Purchase = 150.25m,
                LastDiv = 0.82m,
                Industry = "Technology",
                MarketCap = 2000000000000,
                Comments = new List<Comment> { mockComment },
                Portfolios = new List<Portfolio> { mockPortfolio }
            };

            // Act & Assert
            Assert.Equal(1, stock.Id);
            Assert.Equal("AAPL", stock.Symbol);
            Assert.Equal("Apple Inc.", stock.CompanyName);
            Assert.Equal(150.25m, stock.Purchase);
            Assert.Equal(0.82m, stock.LastDiv);
            Assert.Equal("Technology", stock.Industry);
            Assert.Equal(2000000000000, stock.MarketCap);
            Assert.NotNull(stock.Comments);
            Assert.Single(stock.Comments);
            Assert.Equal("Great stock!", stock.Comments[0].Title);
            Assert.NotNull(stock.Portfolios);
            Assert.Single(stock.Portfolios);
            Assert.Equal("user123", stock.Portfolios[0].AppUserId);
            Assert.Equal(1, stock.Portfolios[0].StockId);
        }
    }
}