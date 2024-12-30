using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.InMemory;
using WebApplication1.Data.Seeds;
using WebApplication1.Data;
using Xunit;

namespace WebApplication1.Tests.Data.Seeds
{
    public class BuildSeedsTest
    {
        [Fact]
        public void BuildSeeds_ShouldInitializeSeedsInMemory()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BuildSeeds_ShouldInitializeSeedsInMemory")
                .Options;
            var context = new ApplicationDbContext(options);

            // Ensure the database is created and seeds are applied
            // Act
            context.Database.EnsureCreated();

            // Assert
            Assert.Equal(1, context.Users.CountAsync().Result);
            Assert.Equal(2, context.Roles.CountAsync().Result);
            Assert.Equal(0, context.UserRoles.CountAsync().Result);
            Assert.Equal(2, context.Stock.CountAsync().Result);
            Assert.Equal(2, context.Comment.CountAsync().Result);
            Assert.Equal(1, context.Portfolios.CountAsync().Result);
            Assert.Equal(1, context.AppUser.CountAsync().Result);
        }
    }
}