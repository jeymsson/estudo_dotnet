using System;
using WebApplication1.Dto.Account;
using Xunit;

namespace WebApplication1.Tests.Dto.Account
{
    public class NewUserDtoTest
    {
        [Fact]
        public void NewUserDto_Should_Set_Username()
        {
            // Arrange
            var newUserDto = new NewUserDto();
            var expectedUsername = "testuser";

            // Act
            newUserDto.Username = expectedUsername;

            // Assert
            Assert.Equal(expectedUsername, newUserDto.Username);
        }

        [Fact]
        public void NewUserDto_Should_Set_Email()
        {
            // Arrange
            var newUserDto = new NewUserDto();
            var expectedEmail = "testuser@example.com";

            // Act
            newUserDto.Email = expectedEmail;

            // Assert
            Assert.Equal(expectedEmail, newUserDto.Email);
        }

        [Fact]
        public void NewUserDto_Should_Set_Token()
        {
            // Arrange
            var newUserDto = new NewUserDto();
            var expectedToken = "sampletoken";

            // Act
            newUserDto.Token = expectedToken;

            // Assert
            Assert.Equal(expectedToken, newUserDto.Token);
        }
    }
}