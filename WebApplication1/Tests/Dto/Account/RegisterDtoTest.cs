using System;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Dto.Account;
using Xunit;

namespace WebApplication1.Tests.Dto.Account
{
    public class RegisterDtoTest
    {
        [Fact]
        public void Username_ShouldBeRequired()
        {
            // Arrange
            var dto = new RegisterDto();

            // Act
            var context = new ValidationContext(dto) { MemberName = "Username" };
            var result = new List<ValidationResult>();

            // Assert
            Assert.False(Validator.TryValidateProperty(dto.Username, context, result));
            Assert.Contains(result, v => v.ErrorMessage.Contains("The Username field is required."));
        }

        [Fact]
        public void Email_ShouldBeRequired()
        {
            // Arrange
            var dto = new RegisterDto();

            // Act
            var context = new ValidationContext(dto) { MemberName = "Email" };
            var result = new List<ValidationResult>();

            // Assert
            Assert.False(Validator.TryValidateProperty(dto.Email, context, result));
            Assert.Contains(result, v => v.ErrorMessage.Contains("The Email field is required."));
        }

        [Fact]
        public void Email_ShouldBeValidEmailAddress()
        {
            // Arrange
            var dto = new RegisterDto { Email = "invalid-email" };

            // Act
            var context = new ValidationContext(dto) { MemberName = "Email" };
            var result = new List<ValidationResult>();

            // Assert
            Assert.False(Validator.TryValidateProperty(dto.Email, context, result));
            Assert.Contains(result, v => v.ErrorMessage.Contains("The Email field is not a valid e-mail address."));
        }

        [Fact]
        public void Password_ShouldBeRequired()
        {
            // Arrange
            var dto = new RegisterDto();

            // Act
            var context = new ValidationContext(dto) { MemberName = "Password" };
            var result = new List<ValidationResult>();

            // Assert
            Assert.False(Validator.TryValidateProperty(dto.Password, context, result));
            Assert.Contains(result, v => v.ErrorMessage.Contains("The Password field is required."));
        }
    }
}