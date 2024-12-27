using System;
using Xunit;
using WebApplication1.Models;
using Moq;

namespace WebApplication1.Tests.Models
{
    public class CommentTest
    {
        [Fact]
        public void Comment_Should_Have_Default_Values()
        {
            // Arrange
            var comment = new Comment();

            // Act & Assert
            Assert.Equal(0, comment.Id);
            Assert.Equal(string.Empty, comment.Title);
            Assert.Equal(string.Empty, comment.Content);
            Assert.Null(comment.StockId);
            Assert.Null(comment.Stock);
        }

        [Fact]
        public void Comment_Should_Set_And_Get_Properties()
        {
            // Arrange
            var comment = new Comment();
            var mockStock = Mock.Of<Stock>(
                s =>
                s.Id == 1 &&
                s.CompanyName == "Test Stock"
            );
            var stock = mockStock; // Objeto "mockado"

            // Act
            comment.Id = 1;
            comment.Title = "Test Title";
            comment.Content = "Test Content";
            comment.StockId = 1;
            comment.Stock = stock;

            // Assert
            Assert.Equal(1, comment.Id);
            Assert.Equal("Test Title", comment.Title);
            Assert.Equal("Test Content", comment.Content);
            Assert.Equal(1, comment.StockId);
            Assert.Equal(stock, comment.Stock);
        }
    }
}