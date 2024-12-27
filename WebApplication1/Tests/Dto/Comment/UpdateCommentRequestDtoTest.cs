using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApplication1.Dto.Comment;
using Xunit;

namespace WebApplication1.Tests.Dto.Comment
{
    public class UpdateCommentRequestDtoTest
    {
        [Fact]
        public void UpdateCommentRequestDto_ValidData_ShouldBeValid()
        {
            var dto = new UpdateCommentRequestDto
            {
                Title = "Title",
                Content = "Content"
            };

            var validationResults = ValidateModel(dto);

            Assert.Empty(validationResults);
        }

        [Fact]
        public void UpdateCommentRequestDto_InvalidTitle_ShouldBeInvalid()
        {
            var dto = new UpdateCommentRequestDto
            {
                Title = "T",
                Content = "Content"
            };

            var validationResults = ValidateModel(dto);

            Assert.Contains(validationResults, v => v.MemberNames.Contains("Title"));
        }

        [Fact]
        public void UpdateCommentRequestDto_InvalidContent_ShouldBeInvalid()
        {
            var dto = new UpdateCommentRequestDto
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