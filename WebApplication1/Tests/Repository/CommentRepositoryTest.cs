using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.Repository;
using Xunit;

namespace WebApplication1.Tests.Repository
{
    public class CommentRepositoryTest
    {
        private readonly ApplicationDbContext _context;
        private readonly CommentRepository _repository;

        public CommentRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;
            _context = new ApplicationDbContext(options);
            _repository = new CommentRepository(_context);
        }

        public void truncate()
        {
            _context.Comment.RemoveRange(_context.Comment);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllComments()
        {
            truncate();
            // Arrange
            var query = new QueryObject { PageNumber = 1, PageSize = 10 };
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Title = "Title1", Content = "Content1", StockId = 1 },
                new Comment { Id = 2, Title = "Title2", Content = "Content2", StockId = 1 }
            };
            await _context.Comment.AddRangeAsync(comments);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.getAllAsync(); // Assuming the second parameter is optional and can be null

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Title1", result[0].Title);
            Assert.Equal("Title2", result[1].Title);
        }

        [Fact]
        public async Task AddAsync_ShouldAddComment()
        {
            truncate();
            // Arrange
            var comment = new Comment { Id = 1, Title = "Title1", Content = "Content1", StockId = 1 };

            // Act
            var result = await _repository.AddAsync(comment);

            // Assert
            Assert.Equal(comment, result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateComment()
        {
            truncate();
            // Arrange
            var comment = new Comment { Id = 1, Title = "Title1", Content = "Content1", StockId = 1 };

            // Act
            await _context.Comment.AddAsync(comment);
            await _context.SaveChangesAsync();

            var result = await _repository.UpdateAsync(comment.Id, comment);

            // Assert
            Assert.Equal(comment, result);
        }

        [Fact]
        public async Task FindAsync_ShouldReturnComment()
        {
            truncate();
            // Arrange
            var comment = new Comment { Id = 1, Title = "Title1", Content = "Content1", StockId = 1 };

            // Act
            await _context.Comment.AddAsync(comment);
            var result = await _repository.FindAsync(1);

            // Assert
            Assert.Equal(comment, result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteComment()
        {
            truncate();
            // Arrange
            var comment = new Comment { Id = 1, Title = "Title1", Content = "Content1", StockId = 1 };
            await _context.Comment.AddAsync(comment);
            await _context.SaveChangesAsync();

            // Act
            var commentToDelete = await _context.Comment.FindAsync(1);
            await _repository.DeleteAsync(commentToDelete);

            // Assert
            Assert.Equal(0, await _context.Comment.CountAsync());
        }

        [Fact]
        public async Task stockExists_ShouldReturnTrue()
        {
            truncate();
            // Arrange
            var stock = new Stock { Id = 1, Symbol = "Symbol1", CompanyName = "Company1" };

            // Act
            await _context.Stock.AddAsync(stock);
            await _context.SaveChangesAsync();

            var result = await _repository.stockExists(1);

            // Assert
            Assert.True(result);
        }
    }
}
