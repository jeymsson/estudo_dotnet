using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using WebApplication1.Models;
using WebApplication1.Service;
using Xunit;

namespace WebApplication1.Tests.Service
{
    public class TokenServiceTest
    {
        private readonly Mock<IConfiguration> _configMock;
        private readonly TokenService _tokenService;

        public TokenServiceTest()
        {
            _configMock = new Mock<IConfiguration>();
            _configMock.Setup(c => c["Jwt:SigningKey"]).Returns("supersecretkey12345");
            _configMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _configMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

            _tokenService = new TokenService(_configMock.Object);
        }

        [Fact]
        public void CreateToken_ShouldReturnValidToken()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string> {
                {"JWT:Issuer", "your-issuer"},
                {"JWT:Audience", "your-audience"},
                {"JWT:SigningKey", "your-very-long-secure-signing-key-that-is-at-least-64-bytes-long"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var tokenService = new TokenService(configuration);
            var user = new AppUser { Email = "email@email.com", UserName = "username" };

            // Act
            var token = tokenService.CreateToken(user);

            // Assert
            Assert.NotNull(token);
        }
    }
}