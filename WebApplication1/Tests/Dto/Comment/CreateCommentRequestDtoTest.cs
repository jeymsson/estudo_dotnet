using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Dto.Comment;
using Xunit;

namespace WebApplication1.Tests.Dto.Comment
{
    public class CreateCommentRequestDtoTest
    {
        [Fact]
        public void CreateCommentRequestDto_ValidData_ShouldBeValid()
        {
            var dto = new CreateCommentRequestDto
            {
                Title = "Title",
                Content = "Content"
            };

            var validationResults = ValidateModel(dto);

            Assert.Empty(validationResults);
        }

        [Fact]
        public void CreateCommentRequestDto_InvalidTitle_ShouldBeInvalid()
        {
            var dto = new CreateCommentRequestDto
            {
                Title = "T",
                Content = "Content"
            };

            var validationResults = ValidateModel(dto);

            Assert.Contains(validationResults, v => v.MemberNames.Contains("Title"));
        }

        [Fact]
        public void CreateCommentRequestDto_InvalidContent_ShouldBeInvalid()
        {
            var dto = new CreateCommentRequestDto
            {
                Title = "Title",
                Content = ""
            };

            var validationResults = ValidateModel(dto);

            Assert.Contains(validationResults, v => v.MemberNames.Contains("Content"));
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