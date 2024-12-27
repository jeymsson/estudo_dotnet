using System;
using System.Collections.Generic;
using Moq;
using WebApplication1.Dto.Comment;
using WebApplication1.Dto.Stock;
using Xunit;

namespace WebApplication1.Tests.Dto.Stock
{
    public class StockDtoTest
    {
        [Fact]
        public void StockDto_DefaultValues_ShouldBeSetCorrectly()
        {
            // Arrange & Act
            var stockDto = new StockDto();

            // Assert
            Assert.Equal(0, stockDto.Id);
            Assert.Equal(string.Empty, stockDto.Symbol);
            Assert.Equal(string.Empty, stockDto.CompanyName);
            Assert.Equal(0m, stockDto.Purchase);
            Assert.Equal(0m, stockDto.LastDiv);
            Assert.Equal(string.Empty, stockDto.Industry);
            Assert.Equal(0L, stockDto.MarketCap);
            Assert.NotNull(stockDto.Comments);
            Assert.Empty(stockDto.Comments);
        }

        [Fact]
        public void StockDto_SetValues_ShouldBeSetCorrectly()
        {
            var comment1 = Mock.Of<CommentDto>(
                c => c.Id == 1 && c.Title == "Great stock!"
            );
            var comment2 = Mock.Of<CommentDto>(
                c => c.Id == 2 && c.Title == "Not so great."
            );


            // Arrange
            var comments = new List<CommentDto>
            {
                comment1,
                comment2
            };

            // Act
            var stockDto = new StockDto
            {
                Id = 1,
                Symbol = "AAPL",
                CompanyName = "Apple Inc.",
                Purchase = 150.25m,
                LastDiv = 0.82m,
                Industry = "Technology",
                MarketCap = 2000000000L,
                Comments = comments
            };

            // Assert
            Assert.Equal(1, stockDto.Id);
            Assert.Equal("AAPL", stockDto.Symbol);
            Assert.Equal("Apple Inc.", stockDto.CompanyName);
            Assert.Equal(150.25m, stockDto.Purchase);
            Assert.Equal(0.82m, stockDto.LastDiv);
            Assert.Equal("Technology", stockDto.Industry);
            Assert.Equal(2000000000L, stockDto.MarketCap);
            Assert.Equal(comments, stockDto.Comments);
        }
    }
}