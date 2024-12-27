using System;
using System.Collections.Generic;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests.Models
{
    public class AppUserTest
    {
        [Fact]
        public void AppUser_ShouldInitializeWithEmptyPortfolios()
        {
            // Arrange
            var user = new AppUser();

            // Act
            var portfolios = user.Portfolios;

            // Assert
            Assert.NotNull(portfolios);
            Assert.Empty(portfolios);
        }

        [Fact]
        public void AppUser_ShouldAllowAddingPortfolios()
        {
            // Arrange
            var user = new AppUser();
            var portfolio = new Portfolio();

            // Act
            user.Portfolios.Add(portfolio);

            // Assert
            Assert.Single(user.Portfolios);
            Assert.Contains(portfolio, user.Portfolios);
        }
    }
}