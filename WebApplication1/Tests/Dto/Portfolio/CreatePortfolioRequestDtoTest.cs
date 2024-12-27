using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebApplication1.Dto.Portfolio;
using Xunit;

namespace WebApplication1.Tests.Dto.Portfolio
{
    public class CreatePortfolioRequestDtoTest
    {
        [Fact]
        public void Symbol_Should_Have_Required_Attribute()
        {
            // Arrange
            var property = typeof(CreatePortfolioRequestDto).GetProperty("Symbol");

            // Act
            var attribute = property.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

            // Assert
            Assert.NotNull(attribute);
        }

        [Fact]
        public void Symbol_Should_Have_MinLength_Attribute()
        {
            // Arrange
            var property = typeof(CreatePortfolioRequestDto).GetProperty("Symbol");

            // Act
            var attribute = property.GetCustomAttributes(typeof(MinLengthAttribute), false).FirstOrDefault() as MinLengthAttribute;

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal(2, attribute.Length);
        }

        [Fact]
        public void Symbol_Should_Have_MaxLength_Attribute()
        {
            // Arrange
            var property = typeof(CreatePortfolioRequestDto).GetProperty("Symbol");

            // Act
            var attribute = property.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault() as MaxLengthAttribute;

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal(100, attribute.Length);
        }

        [Fact]
        public void Symbol_Should_Validate_Correctly()
        {
            // Arrange
            var dto = new CreatePortfolioRequestDto { Symbol = "ValidSymbol" };
            var context = new ValidationContext(dto, null, null);
            var results = new System.Collections.Generic.List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Symbol_Should_Fail_Validation_When_Too_Short()
        {
            // Arrange
            var dto = new CreatePortfolioRequestDto { Symbol = "A" };
            var context = new ValidationContext(dto, null, null);
            var results = new System.Collections.Generic.List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage.Contains("Symbol must be at least 5 characters"));
        }

        [Fact]
        public void Symbol_Should_Fail_Validation_When_Too_Long()
        {
            // Arrange
            var dto = new CreatePortfolioRequestDto { Symbol = new string('A', 101) };
            var context = new ValidationContext(dto, null, null);
            var results = new System.Collections.Generic.List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(dto, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage.Contains("Symbol must be at most 100 characters"));
        }
    }
}