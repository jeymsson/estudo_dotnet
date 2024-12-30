using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.Repository;
using Xunit;

namespace WebApplication1.Tests.Repository
{
    public class StockRepositoryTest
    {
        private readonly ApplicationDbContext _context;
        private readonly StockRepository _repository;

        public StockRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new StockRepository(_context);
        }

        public void truncate()
        {
            _context.Portfolios.RemoveRange(_context.Portfolios);
            _context.Stock.RemoveRange(_context.Stock);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsStocks()
        {
            truncate();
            // Arrange
            var query = new QueryObject { PageNumber = 1, PageSize = 10 };
            var stocks = new List<Stock>
            {
                new Stock { Id = 1, Symbol = "AAPL", CompanyName = "Apple Inc." },
                new Stock { Id = 2, Symbol = "MSFT", CompanyName = "Microsoft Corp." }
            };
            await _context.Stock.AddRangeAsync(stocks);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.getAllAsync(query);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("AAPL", result[0].Symbol);
            Assert.Equal("MSFT", result[1].Symbol);
        }

        [Fact]
        public async Task AddAsync_AddsStock()
        {
            truncate();
            // Arrange
            var stock = new Stock { Id = 1, Symbol = "AAPL", CompanyName = "Apple Inc." };

            // Act
            var result = await _repository.AddAsync(stock);

            // Assert
            Assert.Equal(stock, result);
            Assert.Equal(1, await _context.Stock.CountAsync());
        }

        [Fact]
        public async Task UpdateAsync_UpdatesStock()
        {
            truncate();
            // Arrange
            var stock = new Stock { Id = 1, Symbol = "AAPL", CompanyName = "Apple Inc." };
            await _context.Stock.AddAsync(stock);
            await _context.SaveChangesAsync();

            var updatedStock = new Stock { Symbol = "AAPL", CompanyName = "Apple Corporation" };

            // Act
            var result = await _repository.UpdateAsync(1, updatedStock);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Apple Corporation", result.CompanyName);
        }

        [Fact]
        public async Task FindAsync_ReturnsStock()
        {
            truncate();
            // Arrange
            var stock = new Stock { Id = 1, Symbol = "AAPL", CompanyName = "Apple Inc." };
            await _context.Stock.AddAsync(stock);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.FindAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(stock, result);
        }

        [Fact]
        public async Task DeleteAsync_DeletesStock()
        {
            truncate();
            // Arrange
            var stock = new Stock { Id = 1, Symbol = "AAPL", CompanyName = "Apple Inc." };
            await _context.Stock.AddAsync(stock);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.DeleteAsync(stock);

            // Assert
            Assert.Equal(stock, result);
            Assert.Equal(0, await _context.Stock.CountAsync());
        }

        [Fact]
        public async Task StockExists_ReturnsTrueIfExists()
        {
            truncate();
            // Arrange
            var stock = new Stock { Id = 1, Symbol = "AAPL", CompanyName = "Apple Inc." };
            await _context.Stock.AddAsync(stock);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.stockExists(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task FindStockByName_ReturnsStock()
        {
            truncate();
            // Arrange
            var stock = new Stock { Id = 1, Symbol = "AAPL", CompanyName = "Apple Inc." };
            await _context.Stock.AddAsync(stock);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.findStockByName("AAPL");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(stock, result);
        }

        [Fact]
        public async Task GetAllAsync_SortsStocks()
        {
            truncate();
            // Arrange
            var query = new QueryObject { PageNumber = 1, PageSize = 10, SortBy = "symbol", IsDescending = true };
            var stocks = new List<Stock>
            {
                new Stock { Id = 1, Symbol = "AAPL", CompanyName = "Apple Inc." },
                new Stock { Id = 2, Symbol = "MSFT", CompanyName = "Microsoft Corp." }
            };
            await _context.Stock.AddRangeAsync(stocks);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.getAllAsync(query);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("MSFT", result[0].Symbol);
            Assert.Equal("AAPL", result[1].Symbol);
        }

        [Fact]
        public async Task GetAllAsync_FiltersStocks()
        {
            truncate();
            // Arrange
            var stocks = new List<Stock>
            {
                new Stock { Id = 1, Symbol = "AAPL", CompanyName = "Apple Inc." },
                new Stock { Id = 2, Symbol = "MSFT", CompanyName = "Microsoft Corp." }
            };
            await _context.Stock.AddRangeAsync(stocks);
            await _context.SaveChangesAsync();

            // Act
            var result1 = await _repository.getAllAsync(new QueryObject { PageNumber = 1, PageSize = 10, Symbol = "AAPL", CompanyName = "Apple Inc.", SortBy = "symbol", IsDescending = true });
            var result2 = await _repository.getAllAsync(new QueryObject { PageNumber = 1, PageSize = 10, Symbol = "AAPL", CompanyName = "Apple Inc.", SortBy = "symbol", IsDescending = false });
            var result3 = await _repository.getAllAsync(new QueryObject { PageNumber = 1, PageSize = 10, Symbol = "AAPL", CompanyName = "Apple Inc.", SortBy = "companyName", IsDescending = true });
            var result4 = await _repository.getAllAsync(new QueryObject { PageNumber = 1, PageSize = 10, Symbol = "AAPL", CompanyName = "Apple Inc.", SortBy = "companyName", IsDescending = false });

            // Assert
            Assert.Single(result1);
            Assert.Equal("AAPL", result1[0].Symbol);
            Assert.Single(result2);
            Assert.Equal("AAPL", result2[0].Symbol);
            Assert.Single(result3);
            Assert.Equal("AAPL", result3[0].Symbol);
            Assert.Single(result4);
            Assert.Equal("AAPL", result4[0].Symbol);
        }
    }
}
