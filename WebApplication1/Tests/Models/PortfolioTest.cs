using System;
using Xunit;
using WebApplication1.Models;
using Moq;

namespace WebApplication1.Tests.Models
{
    public class PortfolioTest
    {
        [Fact]
        public void Portfolio_CanBeInitialized()
        {
            // Arrange & Act
            var portfolio = new Portfolio();

            // Assert
            Assert.NotNull(portfolio);
        }

        [Fact]
        public void Portfolio_CanSetAndGetAppUserId()
        {
            // Arrange
            var portfolio = new Portfolio();
            var appUserId = "user123";

            // Act
            portfolio.AppUserId = appUserId;

            // Assert
            Assert.Equal(appUserId, portfolio.AppUserId);
        }

        [Fact]
        public void Portfolio_CanSetAndGetStockId()
        {
            // Arrange
            var portfolio = new Portfolio();
            var stockId = 1;

            // Act
            portfolio.StockId = stockId;

            // Assert
            Assert.Equal(stockId, portfolio.StockId);
        }

        [Fact]
        public void Portfolio_CanSetAndGetAppUser()
        {
            // Arrange
            var portfolio = new Portfolio();
            var appUser = new AppUser { Id = "user123", UserName = "testuser" };

            // Act
            portfolio.AppUser = appUser;

            // Assert
            Assert.Equal(appUser, portfolio.AppUser);
        }

        [Fact]
        public void Portfolio_CanSetAndGetStock()
        {
            // Arrange
            var portfolio = new Portfolio();
            var stockMock = Mock.Of<Stock>(
                s =>
                s.Id == 1 &&
                s.CompanyName == "Test Stock"
            );

            // Act
            portfolio.Stock = stockMock;

            // Assert
            Assert.Equal(stockMock, portfolio.Stock);
        }
    }
}