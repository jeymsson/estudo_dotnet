using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Dto.Stock;
using WebApplication1.Models;
using WebApplication1.Mappers;
using Xunit;
using Moq;
using WebApplication1.Dto.Comment;

namespace WebApplication1.Tests.Mappers
{
    public class StockMappersTest
    {
        [Fact]
        public void toStockDto_ShouldMapStockToStockDto()
        {
            // Arrange
            var commentMock1 = Mock.Of<Comment>(
                c =>
                c.Id == 1 &&
                c.Title == "Great stock!"
            );
            var commentMock2 = Mock.Of<Comment>(
                c =>
                c.Id == 2 &&
                c.Title == "Long term hold."
            );
            var stockMock = Mock.Of<Stock>(
                s =>
                s.Id == 1 &&
                s.Symbol == "AAPL" &&
                s.CompanyName == "Apple Inc." &&
                s.LastDiv == 0.82m &&
                s.Industry == "Technology" &&
                s.MarketCap == 2000000000m &&
                s.Comments == new List<Comment>
                {
                    commentMock1,
                    commentMock2
                }
            );

            // Act
            var stockDto = stockMock.toStockDto();

            // Assert
            Assert.Equal(stockMock.Id, stockDto.Id);
            Assert.Equal(stockMock.Symbol, stockDto.Symbol);
            Assert.Equal(stockMock.CompanyName, stockDto.CompanyName);
            Assert.Equal(stockMock.Purchase, stockDto.Purchase);
            Assert.Equal(stockMock.LastDiv, stockDto.LastDiv);
            Assert.Equal(stockMock.Industry, stockDto.Industry);
            Assert.Equal(stockMock.MarketCap, stockDto.MarketCap);
            Assert.Equal(stockMock.Comments.Count, stockDto.Comments.Count);
            for (int i = 0; i < stockMock.Comments.Count; i++)
            {
                Assert.Equal(stockMock.Comments[i].Id, stockDto.Comments[i].Id);
                Assert.Equal(stockMock.Comments[i].Title, stockDto.Comments[i].Title);
            }
        }

        [Fact]
        public void ToCommentDto_ValidComment_ReturnsCommentDto()
        {
            // Arrange
            var comment = new Comment
            {
                Id = 1,
                Title = "Test Title",
                Content = "Test Content",
                StockId = 100
            };

            // Act
            var result = comment.toCommentDto();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(comment.Id, result.Id);
            Assert.Equal(comment.Title, result.Title);
            Assert.Equal(comment.Content, result.Content);
            Assert.Equal(comment.StockId, result.StockId);
        }

        [Fact]
        public void ToCommentDto_NullComment_ThrowsArgumentNullException()
        {
            // Arrange
            Comment comment = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => comment.toCommentDto());
        }

        [Fact]
        public void ToCommentFromCreate_ValidCreateCommentRequestDto_ReturnsComment()
        {
            // Arrange
            var createCommentRequestDto = new CreateCommentRequestDto
            {
                Title = "Test Title",
                Content = "Test Content"
            };
            int stockId = 100;

            // Act
            var result = createCommentRequestDto.toCommentFromCreate(stockId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createCommentRequestDto.Title, result.Title);
            Assert.Equal(createCommentRequestDto.Content, result.Content);
            Assert.Equal(stockId, result.StockId);
        }

        [Fact]
        public void ToCommentFromUpdate_ValidUpdateCommentRequestDto_ReturnsComment()
        {
            // Arrange
            var updateCommentRequestDto = new UpdateCommentRequestDto
            {
                Title = "Updated Title",
                Content = "Updated Content"
            };
            int stockId = 100;

            // Act
            var result = updateCommentRequestDto.toCommentFromUpdate(stockId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateCommentRequestDto.Title, result.Title);
            Assert.Equal(updateCommentRequestDto.Content, result.Content);
            Assert.Equal(stockId, result.StockId);
        }

        [Fact]
        public void toCreateStockRequestDto_ShouldMapCreateStockRequestDtoToStock()
        {
            // Arrange
            var createStockRequestDto = new CreateStockRequestDto
            {
                Symbol = "AAPL",
                CompanyName = "Apple Inc.",
                Purchase = 150.00m,
                LastDiv = 0.82m,
                Industry = "Technology"
            };

            // Act
            var stock = createStockRequestDto.toCreateStockRequestDto();

            // Assert
            Assert.NotNull(stock);
            Assert.Equal(createStockRequestDto.Symbol, stock.Symbol);
            Assert.Equal(createStockRequestDto.CompanyName, stock.CompanyName);
            Assert.Equal(createStockRequestDto.Purchase, stock.Purchase);
            Assert.Equal(createStockRequestDto.LastDiv, stock.LastDiv);
            Assert.Equal(createStockRequestDto.Industry, stock.Industry);
            Assert.Equal(createStockRequestDto.MarketCap, stock.MarketCap);
        }

        [Fact]
        public void toUpdateStockRequestDto_ShouldMapUpdateStockRequestDtoToStock()
        {
            // Arrange
            var updateStockRequestDto = new UpdateStockRequestDto
            {
                Symbol = "AAPL",
                CompanyName = "Apple Inc.",
                Purchase = 150.00m,
                LastDiv = 0.82m,
                Industry = "Technology"
            };

            // Act
            var stock = updateStockRequestDto.toUpdateStockRequestDto();

            // Assert
            Assert.NotNull(stock);
            Assert.Equal(updateStockRequestDto.Symbol, stock.Symbol);
            Assert.Equal(updateStockRequestDto.CompanyName, stock.CompanyName);
            Assert.Equal(updateStockRequestDto.Purchase, stock.Purchase);
            Assert.Equal(updateStockRequestDto.LastDiv, stock.LastDiv);
            Assert.Equal(updateStockRequestDto.Industry, stock.Industry);
            Assert.Equal(updateStockRequestDto.MarketCap, stock.MarketCap);
        }
    }
}