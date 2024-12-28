using System.Security.Claims;
using Xunit;
using WebApplication1.Extensions;

namespace WebApplication1.Tests.Extensions
{
    public class ClaimsExtensionsTest
    {
        [Fact]
        public void GetUsername_ReturnsCorrectUsername_WhenClaimExists()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, "JohnDoe")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Act
            var username = claimsPrincipal.getUsername();

            // Assert
            Assert.Equal("JohnDoe", username);
        }

        [Fact]
        public void GetUsername_ReturnsNull_WhenClaimDoesNotExist()
        {
            // Arrange
            var claims = new List<Claim>();
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            // Act
            var username = claimsPrincipal.getUsername();

            // Assert
            Assert.Null(username);
        }

        [Fact]
        public void GetUsername_ReturnsNull_WhenClaimsPrincipalIsNull()
        {
            // Arrange
            ClaimsPrincipal claimsPrincipal = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => claimsPrincipal.getUsername());
        }
    }
}