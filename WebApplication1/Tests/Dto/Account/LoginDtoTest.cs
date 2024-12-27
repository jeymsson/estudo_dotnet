using System;
using System.ComponentModel.DataAnnotations;
using Xunit;
using WebApplication1.Dto.Account;

namespace WebApplication1.Tests.Dto.Account
{
    public class LoginDtoTest
    {
        [Fact]
        public void Username_IsRequired()
        {
            // Arrange
            var loginDto = new LoginDto { Password = "password" };
            var validationContext = new ValidationContext(loginDto);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(loginDto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Username"));
        }

        [Fact]
        public void Password_IsRequired()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "username" };
            var validationContext = new ValidationContext(loginDto);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(loginDto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, v => v.MemberNames.Contains("Password"));
        }

        [Fact]
        public void LoginDto_IsValid_WhenAllRequiredFieldsAreProvided()
        {
            // Arrange
            var loginDto = new LoginDto { Username = "username", Password = "password" };
            var validationContext = new ValidationContext(loginDto);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(loginDto, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
        }
    }
}