using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Tests.Repository
{
    public class PortfolioRepositoryTest
    {
        private readonly ApplicationDbContext _context;
        private readonly PortfolioRepository _repository;

        public PortfolioRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new PortfolioRepository(_context);
        }

        public void truncate()
        {
            _context.Portfolios.RemoveRange(_context.Portfolios);
            _context.Stock.RemoveRange(_context.Stock);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetUserPortfolio_ReturnsUserPortfolio()
        {
            truncate();
            // Arrange
            var username = "user1";
            var email = "email@admin.com";
            var user = new AppUser { UserName = username, Email = email };
            var stock1 = new Stock { Id = 1, Symbol = "AAPL" };
            var stock2 = new Stock { Id = 2, Symbol = "GOOGL" };
            var portfolio1 = new Portfolio { AppUserId = user.Id, StockId = 1 };
            var portfolio2 = new Portfolio { AppUserId = user.Id, StockId = 2 };

            // Act
            _context.Stock.AddRange(stock1, stock2);
            _context.Portfolios.AddRange(portfolio1, portfolio2);
            _context.SaveChanges();

            var result = await _repository.getUserPortfolio(user);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(portfolio1.StockId, result[0].Id);
            Assert.Equal(portfolio2.StockId, result[1].Id);
        }

        [Fact]
        public async Task CreateAsync_AddsPortfolio()
        {
            truncate();
            // Arrange
            var username = "user1";
            var email = "email@admin.com";
            var user = new AppUser { UserName = username, Email = email };
            var stock = new Stock { Id = 1, Symbol = "AAPL" };
            var portfolio = new Portfolio { AppUserId = user.Id, StockId = 1 };

            // Act
            _context.Stock.Add(stock);
            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();

            // Assert
            Assert.Equal(1, _context.Portfolios.Count());
        }

        [Fact]
        public async Task FindPortfolioByStockAsync_ReturnsPortfolio()
        {
            truncate();
            // Arrange
            var username = "user1";
            var email = "email@admin.com";
            var user = new AppUser { UserName = username, Email = email };
            var stock = new Stock { Id = 1, Symbol = "AAPL" };
            var portfolio = new Portfolio { AppUserId = user.Id, StockId = 1 };

            // Act
            _context.Stock.Add(stock);
            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();

            var result = await _repository.findPortfolioByStockAsync(stock);

            // Assert
            Assert.Equal(portfolio, result);
        }

        [Fact]
        public async Task FindPortfolioByIdAsync_ReturnsPortfolio()
        {
            truncate();
            // Arrange
            var username = "user1";
            var email = "email@admin.com";
            var user = new AppUser { UserName = username, Email = email };
            var stock = new Stock { Id = 1, Symbol = "AAPL" };
            var portfolio = new Portfolio { AppUserId = user.Id, StockId = 1 };

            // Act
            _context.Stock.Add(stock);
            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();

            var result = await _repository.findPortfolioByIdAsync(user.Id, stock.Id);

            // Assert
            Assert.Equal(portfolio, result);
        }

        [Fact]
        public async Task DeletePortfolioAsync_RemovesPortfolio()
        {
            truncate();
            // Arrange
            var username = "user1";
            var email = "email@admin.com";
            var user = new AppUser { UserName = username, Email = email };
            var stock = new Stock { Id = 1, Symbol = "AAPL" };
            var portfolio = new Portfolio { AppUserId = user.Id, StockId = 1 };

            // Act
            _context.Stock.Add(stock);
            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();

            await _repository.deletePortfolioAsync(portfolio);

            var result = await _repository.findPortfolioByStockAsync(stock);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task createAsync_ShouldAddPortfolio()
        {
            truncate();
            // Arrange
            var stock = new Stock { Id = 1, Symbol = "AAPL" };
            var portfolio = new Portfolio { AppUserId = "user1", StockId = 1 };

            // Act
            await _context.Stock.AddAsync(stock);
            await _context.SaveChangesAsync();

            var result = await _repository.createAsync(portfolio);

            // Assert
            Assert.Equal(portfolio, result);
        }
        
    }
}