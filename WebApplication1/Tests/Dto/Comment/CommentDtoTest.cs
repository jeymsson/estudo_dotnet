using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApplication1.Dto.Comment;
using Xunit;

namespace WebApplication1.Tests.Dto.Comment
{
    public class CommentDtoTest
    {
        [Fact]
        public void CommentDto_ValidData_ShouldBeValid()
        {
            // Arrange
            var commentDto = new CommentDto
            {
                Id = 1,
                Title = "Valid Title",
                Content = "Valid Content",
                StockId = 1
            };

            // Act
            var validationResults = ValidateModel(commentDto);

            // Assert
            Assert.Empty(validationResults);
        }

        [Fact]
        public void CommentDto_TitleTooShort_ShouldBeInvalid()
        {
            // Arrange
            var commentDto = new CommentDto
            {
                Id = 1,
                Title = "1234",
                Content = "Valid Content",
                StockId = 1
            };

            // Act
            var validationResults = ValidateModel(commentDto);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Title") && v.ErrorMessage.Contains("at least 5 characters"));
        }

        [Fact]
        public void CommentDto_TitleTooLong_ShouldBeInvalid()
        {
            // Arrange
            var commentDto = new CommentDto
            {
                Id = 1,
                Title = new string('a', 101),
                Content = "Valid Content",
                StockId = 1
            };

            // Act
            var validationResults = ValidateModel(commentDto);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Title") && v.ErrorMessage.Contains("at most 100 characters"));
        }

        [Fact]
        public void CommentDto_ContentTooShort_ShouldBeInvalid()
        {
            // Arrange
            var commentDto = new CommentDto
            {
                Id = 1,
                Title = "Valid Title",
                Content = "1234",
                StockId = 1
            };

            // Act
            var validationResults = ValidateModel(commentDto);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Content") && v.ErrorMessage.Contains("at least 5 characters"));
        }

        [Fact]
        public void CommentDto_ContentTooLong_ShouldBeInvalid()
        {
            // Arrange
            var commentDto = new CommentDto
            {
                Id = 1,
                Title = "Valid Title",
                Content = new string('a', 101),
                StockId = 1
            };

            // Act
            var validationResults = ValidateModel(commentDto);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Content") && v.ErrorMessage.Contains("at most 100 characters"));
        }

        [Fact]
        public void CommentDto_StockIdIsNull_ShouldBeInvalid()
        {
            // Arrange
            var commentDto = new CommentDto
            {
                Id = 1,
                Title = "Valid Title",
                Content = "Valid Content",
                StockId = null
            };

            // Act
            var validationResults = ValidateModel(commentDto);

            // Assert
            Assert.Contains(validationResults, v => v.MemberNames.Contains("StockId"));
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}