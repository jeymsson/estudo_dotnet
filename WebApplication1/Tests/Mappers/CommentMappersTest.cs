using System;
using Xunit;
using WebApplication1.Dto.Comment;
using WebApplication1.Models;
using WebApplication1.Mappers;

namespace WebApplication1.Tests.Mappers
{
    public class CommentMappersTest
    {
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
    }
}

